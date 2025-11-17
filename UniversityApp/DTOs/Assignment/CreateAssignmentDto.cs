// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace DTOs.Assignment
{
    public class CreateAssignmentDto
    {
        [Required(ErrorMessage = "Class ID is required")]
        public Guid ClassId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, MinimumLength = 2)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Due date is required")]
        public DateTimeOffset DueDate { get; set; }
    }
}
