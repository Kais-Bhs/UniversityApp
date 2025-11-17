// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace DTOs.Submission
{
    public class GradeSubmissionDto
    {
        [Required(ErrorMessage = "Grade is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        public double Grade { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
