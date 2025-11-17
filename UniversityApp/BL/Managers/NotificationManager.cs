// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Notification;
using Entities;

namespace BL.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto createNotificationDto)
        {
            var notification = _mapper.Map<Entities.Notification>(createNotificationDto);
            notification.Id = Guid.NewGuid();

            await _unitOfWork.RepoNotification.Add(notification);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(Guid userId, string userRole)
        {
            var notifications = await _unitOfWork.RepoNotification.GetUserNotificationsAsync(userId, userRole);

            return _mapper.Map<List<NotificationDto>>(notifications);
        }

        public async Task<bool> MarkNotificationAsReadAsync(Guid notificationId, Guid userId)
        {
            var notification = await _unitOfWork.RepoNotification.GetNotificationByIdAsync(notificationId);

            if (notification == null)
            {
                throw new KeyNotFoundException("Notification not found");
            }
            if (notification.RecipientId.HasValue && notification.RecipientId.Value != userId)
            {
                throw new UnauthorizedAccessException("You can only mark your own notifications as read");
            }

            notification.IsRead = true;
            await _unitOfWork.RepoNotification.Update(notification);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
