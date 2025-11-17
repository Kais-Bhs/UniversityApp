// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace DTOs.Class
{
    public class UpdateClassDto
    {
        [Required(ErrorMessage = "Class name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Semester is required")]
        [StringLength(50)]
        public string Semester { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTimeOffset StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        public DateTimeOffset EndDate { get; set; }

        [Required(ErrorMessage = "IsActive is required")]
        public bool IsActive { get; set; }
    }
}
