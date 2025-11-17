// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Notification;
using NLog;

namespace BL.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public NotificationManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto createNotificationDto)
        {
            try
            {
                _logger.Info("Creating notification for recipient {RecipientId}", createNotificationDto.RecipientId?.ToString() ?? "All");

                var notification = _mapper.Map<Entities.Notification>(createNotificationDto);
                notification.Id = Guid.NewGuid();

                await _unitOfWork.RepoNotification.Add(notification);
                await _unitOfWork.SaveAsync();

                _logger.Info("Notification {NotificationId} created successfully", notification.Id);

                return _mapper.Map<NotificationDto>(notification);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating notification");
                throw;
            }
        }

        public async Task<List<NotificationDto>> GetUserNotificationsAsync(Guid userId, string userRole)
        {
            try
            {
                _logger.Info("Getting notifications for user {UserId} with role {Role}", userId, userRole);

                var notifications = await _unitOfWork.RepoNotification.GetUserNotificationsAsync(userId, userRole);

                var notificationDtos = _mapper.Map<List<NotificationDto>>(notifications);

                _logger.Info("Retrieved {Count} notifications for user {UserId}", notificationDtos.Count, userId);

                return notificationDtos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting notifications for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> MarkNotificationAsReadAsync(Guid notificationId, Guid userId)
        {
            try
            {
                _logger.Info("Marking notification {NotificationId} as read by user {UserId}", notificationId, userId);

                var notification = await _unitOfWork.RepoNotification.GetNotificationByIdAsync(notificationId);

                if (notification == null)
                {
                    _logger.Warn("Notification {NotificationId} not found", notificationId);
                    throw new KeyNotFoundException("Notification not found");
                }

                if (notification.RecipientId.HasValue && notification.RecipientId.Value != userId)
                {
                    _logger.Warn("User {UserId} attempted to mark notification {NotificationId} belonging to another user", userId, notificationId);
                    throw new UnauthorizedAccessException("You can only mark your own notifications as read");
                }

                notification.IsRead = true;
                await _unitOfWork.RepoNotification.Update(notification);
                await _unitOfWork.SaveAsync();

                _logger.Info("Notification {NotificationId} marked as read successfully", notificationId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error marking notification {NotificationId} as read", notificationId);
                throw;
            }
        }
    }
}
