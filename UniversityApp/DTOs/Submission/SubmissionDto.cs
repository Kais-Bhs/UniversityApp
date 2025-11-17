// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
namespace DTOs.Submission
{
    public class SubmissionDto
    {
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public string AssignmentTitle { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTimeOffset SubmittedDate { get; set; }
        public string FileUrl { get; set; }
        public double? Grade { get; set; }
        public Guid? GradedByTeacherId { get; set; }
        public string GradedByTeacherName { get; set; }
        public string Remarks { get; set; }
    }
}
