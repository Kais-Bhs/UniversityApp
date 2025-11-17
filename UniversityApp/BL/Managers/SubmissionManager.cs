// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Submission;
using Entities;

namespace BL.Managers
{
    public class SubmissionManager : ISubmissionManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubmissionManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SubmissionDto> SubmitAssignmentAsync(SubmitAssignmentDto submitAssignmentDto, Guid studentId)
        {
            var assignment = await _unitOfWork.RepoAssignment.GetAssignmentByIdWithRelationsAsync(submitAssignmentDto.AssignmentId);

            if (assignment == null)
            {
                throw new KeyNotFoundException("Assignment not found");
            }

            var isEnrolled = await _unitOfWork.RepoStudentClass.IsStudentEnrolledInClassAsync(studentId, assignment.ClassId);

            if (!isEnrolled)
            {
                throw new UnauthorizedAccessException("You are not enrolled in this class");
            }

            var hasSubmitted = await _unitOfWork.RepoSubmission.HasStudentSubmittedAsync(submitAssignmentDto.AssignmentId, studentId);

            if (hasSubmitted)
            {
                throw new InvalidOperationException("You have already submitted this assignment");
            }

            var submission = _mapper.Map<Entities.Submission>(submitAssignmentDto);
            submission.Id = Guid.NewGuid();
            submission.StudentId = studentId;

            await _unitOfWork.RepoSubmission.Add(submission);
            await _unitOfWork.SaveAsync();

            var savedSubmission = await _unitOfWork.RepoSubmission.GetSubmissionByIdWithRelationsAsync(submission.Id);

            return _mapper.Map<SubmissionDto>(savedSubmission);
        }

        public async Task<SubmissionDto> GradeSubmissionAsync(Guid submissionId, GradeSubmissionDto gradeSubmissionDto, Guid teacherId)
        {
            var submission = await _unitOfWork.RepoSubmission.GetSubmissionWithAssignmentAndClassAsync(submissionId);

            if (submission == null)
            {
                throw new KeyNotFoundException("Submission not found");
            }

            if (submission.Assignment.CreatedByTeacherId != teacherId)
            {
                throw new UnauthorizedAccessException("Only the teacher who created the assignment can grade submissions");
            }

            submission.Grade = (decimal)gradeSubmissionDto.Grade;
            submission.Remarks = gradeSubmissionDto.Remarks;
            submission.GradedByTeacherId = teacherId;

            await _unitOfWork.RepoSubmission.Update(submission);
            await _unitOfWork.SaveAsync();

            var gradedSubmission = await _unitOfWork.RepoSubmission.GetSubmissionByIdWithRelationsAsync(submissionId);

            return _mapper.Map<SubmissionDto>(gradedSubmission);
        }

        public async Task<List<SubmissionDto>> GetSubmissionsByAssignmentAsync(Guid assignmentId)
        {
            var submissions = await _unitOfWork.RepoSubmission.GetSubmissionsByAssignmentWithRelationsAsync(assignmentId);

            return _mapper.Map<List<SubmissionDto>>(submissions);
        }

        public async Task<List<SubmissionDto>> GetStudentGradesAsync(Guid studentId)
        {
            var submissions = await _unitOfWork.RepoSubmission.GetStudentGradesWithRelationsAsync(studentId);

            return _mapper.Map<List<SubmissionDto>>(submissions);
        }
    }
}
