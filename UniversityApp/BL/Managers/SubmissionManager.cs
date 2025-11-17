// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using BL.Services;
using DAL;
using DTOs.Submission;
using NLog;

namespace BL.Managers
{
    public class SubmissionManager : ISubmissionManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IFileStorageService _fileStorageService;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public SubmissionManager(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IFileStorageService fileStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
            _fileStorageService = fileStorageService;
        }

        public async Task<SubmissionDto> SubmitAssignmentAsync(SubmitAssignmentDto submitAssignmentDto, Guid studentId)
        {
            try
            {
                _logger.Info("Student {StudentId} submitting assignment {AssignmentId}", studentId, submitAssignmentDto.AssignmentId);

                var assignment = await _unitOfWork.RepoAssignment.GetAssignmentByIdWithRelationsAsync(submitAssignmentDto.AssignmentId);

                if (assignment == null)
                {
                    _logger.Warn("Assignment {AssignmentId} not found", submitAssignmentDto.AssignmentId);
                    throw new KeyNotFoundException("Assignment not found");
                }

                var isEnrolled = await _unitOfWork.RepoStudentClass.IsStudentEnrolledInClassAsync(studentId, assignment.ClassId);

                if (!isEnrolled)
                {
                    _logger.Warn("Student {StudentId} not enrolled in class {ClassId}", studentId, assignment.ClassId);
                    throw new UnauthorizedAccessException("You are not enrolled in this class");
                }

                var hasSubmitted = await _unitOfWork.RepoSubmission.HasStudentSubmittedAsync(submitAssignmentDto.AssignmentId, studentId);

                if (hasSubmitted)
                {
                    _logger.Warn("Student {StudentId} already submitted assignment {AssignmentId}", studentId, submitAssignmentDto.AssignmentId);
                    throw new InvalidOperationException("You have already submitted this assignment");
                }

                var submission = _mapper.Map<Entities.Submission>(submitAssignmentDto);
                submission.Id = Guid.NewGuid();
                submission.StudentId = studentId;

                await _unitOfWork.RepoSubmission.Add(submission);
                await _unitOfWork.SaveAsync();

                _logger.Info("Assignment {AssignmentId} submitted successfully by student {StudentId}", submitAssignmentDto.AssignmentId, studentId);

                var savedSubmission = await _unitOfWork.RepoSubmission.GetSubmissionByIdWithRelationsAsync(submission.Id);

                return _mapper.Map<SubmissionDto>(savedSubmission);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error submitting assignment {AssignmentId} by student {StudentId}", submitAssignmentDto.AssignmentId, studentId);
                throw;
            }
        }

        public async Task<SubmissionDto> SubmitAssignmentWithFileAsync(SubmitAssignmentWithFileDto submitDto, Guid studentId)
        {
            try
            {
                _logger.Info("Student {StudentId} submitting assignment {AssignmentId} with file upload", studentId, submitDto.AssignmentId);

                var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".txt", ".zip", ".rar" };
                if (!_fileStorageService.IsValidFileType(submitDto.File, allowedExtensions))
                {
                    _logger.Warn("Invalid file type attempted: {FileName}", submitDto.File.FileName);
                    throw new InvalidOperationException($"Invalid file type. Allowed types: {string.Join(", ", allowedExtensions)}");
                }

                var maxFileSize = 10 * 1024 * 1024;
                if (!_fileStorageService.IsValidFileSize(submitDto.File, maxFileSize))
                {
                    _logger.Warn("File size exceeds limit: {FileSize} bytes", submitDto.File.Length);
                    throw new InvalidOperationException($"File size exceeds the maximum allowed size of {maxFileSize / (1024 * 1024)} MB");
                }

                var assignment = await _unitOfWork.RepoAssignment.GetAssignmentByIdWithRelationsAsync(submitDto.AssignmentId);

                if (assignment == null)
                {
                    _logger.Warn("Assignment {AssignmentId} not found", submitDto.AssignmentId);
                    throw new KeyNotFoundException("Assignment not found");
                }

                var isEnrolled = await _unitOfWork.RepoStudentClass.IsStudentEnrolledInClassAsync(studentId, assignment.ClassId);

                if (!isEnrolled)
                {
                    _logger.Warn("Student {StudentId} not enrolled in class {ClassId}", studentId, assignment.ClassId);
                    throw new UnauthorizedAccessException("You are not enrolled in this class");
                }

                var hasSubmitted = await _unitOfWork.RepoSubmission.HasStudentSubmittedAsync(submitDto.AssignmentId, studentId);

                if (hasSubmitted)
                {
                    _logger.Warn("Student {StudentId} already submitted assignment {AssignmentId}", studentId, submitDto.AssignmentId);
                    throw new InvalidOperationException("You have already submitted this assignment");
                }

                var fileUrl = await _fileStorageService.SaveFileAsync(submitDto.File, $"submissions/{submitDto.AssignmentId}");

                var submission = new Entities.Submission
                {
                    Id = Guid.NewGuid(),
                    AssignmentId = submitDto.AssignmentId,
                    StudentId = studentId,
                    FileUrl = fileUrl,
                    SubmittedDate = DateTimeOffset.UtcNow
                };

                await _unitOfWork.RepoSubmission.Add(submission);
                await _unitOfWork.SaveAsync();

                _logger.Info("Assignment {AssignmentId} submitted successfully by student {StudentId} with file {FileUrl}",
                    submitDto.AssignmentId, studentId, fileUrl);

                var savedSubmission = await _unitOfWork.RepoSubmission.GetSubmissionByIdWithRelationsAsync(submission.Id);

                return _mapper.Map<SubmissionDto>(savedSubmission);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error submitting assignment {AssignmentId} with file by student {StudentId}", submitDto.AssignmentId, studentId);
                throw;
            }
        }

        public async Task<SubmissionDto> GradeSubmissionAsync(Guid submissionId, GradeSubmissionDto gradeSubmissionDto, Guid teacherId)
        {
            try
            {
                _logger.Info("Grading submission {SubmissionId} by teacher {TeacherId}", submissionId, teacherId);

                var submission = await _unitOfWork.RepoSubmission.GetSubmissionWithAssignmentAndClassAsync(submissionId);

                if (submission == null)
                {
                    _logger.Warn("Submission {SubmissionId} not found", submissionId);
                    throw new KeyNotFoundException("Submission not found");
                }

                if (submission.Assignment.CreatedByTeacherId != teacherId)
                {
                    _logger.Warn("Teacher {TeacherId} attempted to grade submission {SubmissionId} without permission", teacherId, submissionId);
                    throw new UnauthorizedAccessException("Only the teacher who created the assignment can grade submissions");
                }

                submission.Grade = (decimal)gradeSubmissionDto.Grade;
                submission.Remarks = gradeSubmissionDto.Remarks;
                submission.GradedByTeacherId = teacherId;

                await _unitOfWork.RepoSubmission.Update(submission);
                await _unitOfWork.SaveAsync();

                _logger.Info("Submission {SubmissionId} graded successfully with grade {Grade}", submissionId, gradeSubmissionDto.Grade);

                try
                {
                    var student = submission.Student;
                    await _emailService.SendGradeNotificationAsync(
                        student.Email,
                        $"{student.Name}",
                        submission.Assignment.Title,
                        (decimal)gradeSubmissionDto.Grade,
                        gradeSubmissionDto.Remarks
                    );
                    _logger.Info("Grade notification email sent to student {StudentId}", submission.StudentId);
                }
                catch (Exception emailEx)
                {
                    _logger.Error(emailEx, "Failed to send grade notification email to student {StudentId}", submission.StudentId);
                }

                var gradedSubmission = await _unitOfWork.RepoSubmission.GetSubmissionByIdWithRelationsAsync(submissionId);

                return _mapper.Map<SubmissionDto>(gradedSubmission);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error grading submission {SubmissionId} by teacher {TeacherId}", submissionId, teacherId);
                throw;
            }
        }

        public async Task<List<SubmissionDto>> GetSubmissionsByAssignmentAsync(Guid assignmentId)
        {
            try
            {
                _logger.Info("Getting submissions for assignment {AssignmentId}", assignmentId);

                var submissions = await _unitOfWork.RepoSubmission.GetSubmissionsByAssignmentWithRelationsAsync(assignmentId);

                var submissionDtos = _mapper.Map<List<SubmissionDto>>(submissions);

                _logger.Info("Retrieved {Count} submissions for assignment {AssignmentId}", submissionDtos.Count, assignmentId);

                return submissionDtos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting submissions for assignment {AssignmentId}", assignmentId);
                throw;
            }
        }

        public async Task<List<SubmissionDto>> GetStudentGradesAsync(Guid studentId)
        {
            try
            {
                _logger.Info("Getting grades for student {StudentId}", studentId);

                var submissions = await _unitOfWork.RepoSubmission.GetStudentGradesWithRelationsAsync(studentId);

                var submissionDtos = _mapper.Map<List<SubmissionDto>>(submissions);

                _logger.Info("Retrieved {Count} grades for student {StudentId}", submissionDtos.Count, studentId);

                return submissionDtos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting grades for student {StudentId}", studentId);
                throw;
            }
        }
    }
}
