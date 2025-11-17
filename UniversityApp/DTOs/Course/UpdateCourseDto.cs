// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace DTOs.Course
{
    public class UpdateCourseDto
    {
        [Required(ErrorMessage = "Course name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Course code is required")]
        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public Guid DepartmentId { get; set; }

        [Required(ErrorMessage = "Credits are required")]
        [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10")]
        public int Credits { get; set; }
    }
}
