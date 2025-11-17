// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;


namespace DTOs.Submission
{
    public class SubmitAssignmentWithFileDto
    {
        [Required(ErrorMessage = "Assignment ID is required")]
        public Guid AssignmentId { get; set; }

        [Required(ErrorMessage = "File is required")]
        public IFormFile File { get; set; }
    }
}