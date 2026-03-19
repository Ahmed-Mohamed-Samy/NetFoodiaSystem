using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.NotificationModule;
using NetFoodia.Services.Specifications.NotificationSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.NotificationDTOs;
using NotificationTypeDomain = NetFoodia.Domain.Entities.NotificationModule.NotificationType;

namespace NetFoodia.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRealtimeNotificationService _realtimeNotificationService;

        public NotificationService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IRealtimeNotificationService realtimeNotificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _realtimeNotificationService = realtimeNotificationService;
        }

        public async Task<Result> CreateNotificationAsync(
            string userId,
            string title,
            string message,
            int type,
            int? relatedTaskId = null,
            int? relatedDonationId = null)
        {
            var repo = _unitOfWork.GetRepository<Notification>();

            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = (NotificationTypeDomain)type,
                RelatedTaskId = relatedTaskId,
                RelatedDonationId = relatedDonationId,
                IsRead = false
            };

            await repo.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await _realtimeNotificationService.SendToUserAsync(userId, new
            {
                notificationId = notification.Id,
                title,
                message,
                type = type.ToString(),
                isRead = false,
                relatedTaskId,
                relatedDonationId,
                createdAt = notification.CreatedAt
            });

            return Result.OK();
        }

        public async Task<Result<IEnumerable<NotificationDTO>>> GetMyNotificationsAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var notifications = await repo.GetAllAsync(new MyNotificationsSpecification(userId));

            var dto = _mapper.Map<IEnumerable<NotificationDTO>>(notifications);
            return Result<IEnumerable<NotificationDTO>>.OK(dto);
        }

        public async Task<Result<bool>> MarkAsReadAsync(string userId, int notificationId)
        {
            var repo = _unitOfWork.GetRepository<Notification>();
            var notification = await repo.FirstOrDefaultAsync(new NotificationByUserSpecification(userId, notificationId));

            if (notification is null)
                return Error.NotFound("Notification.NotFound", "Notification not found");

            notification.IsRead = true;
            repo.Update(notification);

            var result = await _unitOfWork.SaveChangesAsync() > 0;
            return result;
        }
    }
}