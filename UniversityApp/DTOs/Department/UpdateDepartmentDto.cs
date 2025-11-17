// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace DTOs.Department
{
    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Head of Department is required")]
        public Guid HeadOfDepartmentId { get; set; }
    }
}
