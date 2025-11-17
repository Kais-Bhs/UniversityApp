// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace DTOs.Submission
{
    public class SubmitAssignmentDto
    {
        [Required(ErrorMessage = "Assignment ID is required")]
        public Guid AssignmentId { get; set; }

        [StringLength(500)]
        public string FileUrl { get; set; }
    }
}
