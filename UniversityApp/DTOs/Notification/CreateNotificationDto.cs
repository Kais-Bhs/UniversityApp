// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace DTOs.Notification
{
    public class CreateNotificationDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000, MinimumLength = 2)]
        public string Message { get; set; }

        [Required(ErrorMessage = "Recipient role is required")]
        [RegularExpression("^(Admin|Teacher|Student)$", ErrorMessage = "Recipient role must be Admin, Teacher, or Student")]
        public string RecipientRole { get; set; }

        public Guid? RecipientId { get; set; }
    }
}
