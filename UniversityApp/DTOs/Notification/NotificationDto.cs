// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
namespace DTOs.Notification
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string RecipientRole { get; set; }
        public Guid? RecipientId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsRead { get; set; }
    }
}
