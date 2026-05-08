using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.DeliveryModule;
using NetFoodia.Domain.Entities.MembershipModule;
using NetFoodia.Services.Specifications.DeliverySpecifications;
using NetFoodia.Services.Specifications.MembershipSpecifications;
using NetFoodia.Services_Abstraction;
using AttemptOutcome = NetFoodia.Domain.Entities.DeliveryModule.AttemptOutcome;
using AttemptResponse = NetFoodia.Domain.Entities.DeliveryModule.AttemptResponse;
using TaskStatus = NetFoodia.Domain.Entities.DeliveryModule.TaskStatus;

namespace NetFoodia.Services
{
    /// <summary>
    /// Hosted background worker that runs every 5 minutes and escalates "orphaned" pickup tasks.
    /// A task is considered orphaned when it has remained in <see cref="TaskStatus.Open"/>
    /// for more than 15 minutes — meaning the AI matching either failed or produced no offers.
    ///
    /// Escalation strategy: bypass AI entirely and offer the task to ALL available approved
    /// volunteers for that charity, guaranteeing the task eventually gets picked up.
    /// </summary>
    public class TaskEscalationWorker : BackgroundService
    {
        private static readonly TimeSpan _interval = TimeSpan.FromMinutes(5);
        private const int OfferTimeoutMinutes = 10;

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TaskEscalationWorker> _logger;

        public TaskEscalationWorker(
            IServiceScopeFactory scopeFactory,
            ILogger<TaskEscalationWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "TaskEscalationWorker started. Interval: {Interval} min | Escalating tasks past SlaDueAt.",
                _interval.TotalMinutes);

            using var timer = new PeriodicTimer(_interval);

            // Run immediately on startup, then on each tick
            do
            {
                try
                {
                    await RunEscalationCycleAsync(stoppingToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    // Never crash the host — log and continue to the next cycle
                    _logger.LogError(ex, "Unhandled error in TaskEscalationWorker cycle.");
                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));

            _logger.LogInformation("TaskEscalationWorker stopped.");
        }

        // ────────────────────────────────────────────────────────────────────────────
        private async Task RunEscalationCycleAsync(CancellationToken ct)
        {
            // Scope 0: Actively expire pending offers so tasks revert to 'Open' if ignored.
            // This prevents the system from getting stuck if no volunteers open the app.
            await using (var expirationScope = _scopeFactory.CreateAsyncScope())
            {
                var attemptService = expirationScope.ServiceProvider.GetRequiredService<IAssignmentAttemptService>();
                await attemptService.ExpirePendingOffersAsync();
            }

            List<PickupTask> orphanedTasks;

            // Scope 1: Just query the orphaned tasks
            await using (var scope = _scopeFactory.CreateAsyncScope())
            {
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var taskRepo = unitOfWork.GetRepository<PickupTask>();

                orphanedTasks = (await taskRepo.GetAllAsync(
                    new OrphanedOpenTasksSpecification())).ToList();
            }

            if (orphanedTasks.Count == 0)
            {
                _logger.LogDebug("Escalation cycle: no orphaned tasks found.");
                return;
            }

            _logger.LogInformation(
                "Escalation cycle: found {Count} orphaned task(s) that have exceeded their SlaDueAt deadline.",
                orphanedTasks.Count);

            foreach (var task in orphanedTasks)
            {
                if (ct.IsCancellationRequested) break;

                // Scope 2..N: Process each task in its own isolated scope.
                // This guarantees that if one task fails (e.g. DB error), it won't poison
                // the DbContext and block the other orphaned tasks from being escalated.
                await ProcessSingleTaskAsync(task.Id, ct);
            }
        }

        // ────────────────────────────────────────────────────────────────────────────
        private async Task ProcessSingleTaskAsync(int taskId, CancellationToken ct)
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                
                var taskRepo       = unitOfWork.GetRepository<PickupTask>();
                var membershipRepo = unitOfWork.GetRepository<VolunteerMembership>();
                var attemptRepo    = unitOfWork.GetRepository<AssignmentAttempt>();

                // Re-fetch task in current scope to ensure it's tracked by this DbContext
                var task = await taskRepo.GetByIdAsync(taskId);
                
                // Safety check: ensure it wasn't picked up or canceled since we queried
                if (task is null || task.Status != TaskStatus.Open) return;

                await EscalateTaskAsync(task, membershipRepo, attemptRepo, taskRepo);

                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to escalate Task {TaskId} due to an unexpected error.", taskId);
            }
        }

        // ────────────────────────────────────────────────────────────────────────────
        private async Task EscalateTaskAsync(
            PickupTask task,
            IGenericRepository<VolunteerMembership> membershipRepo,
            IGenericRepository<AssignmentAttempt> attemptRepo,
            IGenericRepository<PickupTask> taskRepo)
        {
            // Fetch ALL approved + available volunteers for this charity (bypasses AI)
            var memberships = (await membershipRepo.GetAllAsync(
                new VolunteersApprovedForCharitySpecification(task.CharityId))).ToList();

            if (memberships.Count == 0)
            {
                _logger.LogWarning(
                    "Escalation: Task {TaskId} (Charity {CharityId}) has no available approved volunteers. Skipping.",
                    task.Id, task.CharityId);
                return;
            }

            _logger.LogInformation(
                "Escalating Task {TaskId} to all {Count} available volunteers for Charity {CharityId}.",
                task.Id, memberships.Count, task.CharityId);

            var now = DateTime.UtcNow;

            foreach (var membership in memberships)
            {
                // Skip if the volunteer has EVER received an offer for this task (avoid spamming)
                var alreadyOffered = await attemptRepo.AnyAsync(
                    new AnyAttemptForVolunteerAndTaskSpecification(membership.VolunteerId, task.Id));

                if (alreadyOffered)
                {
                    _logger.LogDebug(
                        "  ↳ Volunteer {VolunteerId} already received an offer for Task {TaskId} previously. Skipping.",
                        membership.VolunteerId, task.Id);
                    continue;
                }

                var attempt = new AssignmentAttempt
                {
                    PickupTaskId  = task.Id,
                    DonationId    = task.DonationId,
                    VolunteerId   = membership.VolunteerId,
                    OfferedAt     = now,
                    ExpiresAt     = now.AddMinutes(OfferTimeoutMinutes),
                    Response      = AttemptResponse.Pending,
                    Outcome       = null,
                    DistanceKm    = 0,
                    EtaMinutes    = 0,
                    CandidateLoad = 0,
                    ScoreAtOffer  = null
                };

                await attemptRepo.AddAsync(attempt);

                _logger.LogDebug(
                    "  ↳ Created escalation offer for Volunteer {VolunteerId} (expires {ExpiresAt:HH:mm:ss} UTC).",
                    membership.VolunteerId, attempt.ExpiresAt);
            }

            task.Status = TaskStatus.Offered;
            taskRepo.Update(task);
        }
    }
}
