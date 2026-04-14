using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NetFoodia.Domain.Events;
using NetFoodia.Services_Abstraction;

namespace NetFoodia.Services.EventHandlers
{
    public class DonationCreatedHandler : INotificationHandler<DonationCreatedEvent>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DonationCreatedHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public Task Handle(DonationCreatedEvent notification, CancellationToken cancellationToken)
        {
            _ = Task.Run(async () =>
            {

                using (var scope = _scopeFactory.CreateScope())
                {
                    try
                    {

                        var inspectionService = scope.ServiceProvider.GetRequiredService<IFoodInspectionService>();

                        await inspectionService.CreateOrUpdateFromAI(notification.Donation.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[AI Background Error]: {ex.Message}");
                    }
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }
    }
}
