// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace DTOs.Attendance
{
    public class MarkAttendanceDto
    {
        [Required(ErrorMessage = "Class ID is required")]
        public Guid ClassId { get; set; }

        [Required(ErrorMessage = "Student ID is required")]
        public Guid StudentId { get; set; }

        [Required(ErrorMessage = "Date is required")]
        public DateTimeOffset Date { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Present|Absent|Late)$", ErrorMessage = "Status must be Present, Absent, or Late")]
        public string Status { get; set; }
    }
}
