// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DTOs.Notification;

namespace BL.Managers
{
    public interface INotificationManager
    {
        Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto createNotificationDto);
        Task<List<NotificationDto>> GetUserNotificationsAsync(Guid userId, string userRole);
        Task<bool> MarkNotificationAsReadAsync(Guid notificationId, Guid userId);
    }
}
