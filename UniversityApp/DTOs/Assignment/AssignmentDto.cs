// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
namespace DTOs.Assignment
{
    public class AssignmentDto
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset DueDate { get; set; }
        public Guid CreatedByTeacherId { get; set; }
        public string CreatedByTeacherName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int SubmissionsCount { get; set; }
    }
}
