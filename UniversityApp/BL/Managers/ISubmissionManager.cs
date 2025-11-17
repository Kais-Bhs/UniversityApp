// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DTOs.Submission;

namespace BL.Managers
{
    public interface ISubmissionManager
    {
        Task<SubmissionDto> SubmitAssignmentAsync(SubmitAssignmentDto submitAssignmentDto, Guid studentId);
        Task<SubmissionDto> GradeSubmissionAsync(Guid submissionId, GradeSubmissionDto gradeSubmissionDto, Guid teacherId);
        Task<List<SubmissionDto>> GetSubmissionsByAssignmentAsync(Guid assignmentId);
        Task<List<SubmissionDto>> GetStudentGradesAsync(Guid studentId);
    }
}
