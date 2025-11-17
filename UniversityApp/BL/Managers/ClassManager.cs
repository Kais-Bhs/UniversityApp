// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using BL.Services;
using DAL;
using DTOs.Class;
using DTOs.Common;
using Entities;
using NLog;

namespace BL.Managers
{
    public class ClassManager : IClassManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public ClassManager(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<PagedResult<ClassDto>> GetAllClassesAsync(Guid? teacherId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                _logger.Info("Getting classes, teacher filter: {TeacherId}, page {PageNumber}", teacherId?.ToString() ?? "None", pageNumber);

                var (classes, totalCount) = await _unitOfWork.RepoClass.GetClassesWithRelationsPaginatedAsync(teacherId, pageNumber, pageSize);

                var classDtos = _mapper.Map<List<ClassDto>>(classes);

                _logger.Info("Retrieved {Count} classes", classDtos.Count);

                return new PagedResult<ClassDto>(classDtos, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting classes");
                throw;
            }
        }

        public async Task<ClassDto> GetClassByIdAsync(Guid id)
        {
            try
            {
                _logger.Info("Getting class {ClassId}", id);

                var classEntity = await _unitOfWork.RepoClass.GetClassByIdWithRelationsAsync(id);

                if (classEntity == null)
                {
                    _logger.Warn("Class {ClassId} not found", id);
                    throw new KeyNotFoundException($"Class with ID {id} not found");
                }

                _logger.Info("Class {ClassId} retrieved successfully", id);

                return _mapper.Map<ClassDto>(classEntity);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting class {ClassId}", id);
                throw;
            }
        }

        public async Task<ClassDto> CreateClassAsync(CreateClassDto createClassDto, Guid teacherId)
        {
            try
            {
                _logger.Info("Creating new class for course {CourseId} by teacher {TeacherId}", createClassDto.CourseId, teacherId);

                var course = await _unitOfWork.RepoCourse.GetCourseByIdWithDepartmentAsync(createClassDto.CourseId);

                if (course == null)
                {
                    _logger.Warn("Course {CourseId} not found", createClassDto.CourseId);
                    throw new KeyNotFoundException("Course not found");
                }

                var teacher = await _unitOfWork.RepoUser.GetTeacherByIdAsync(teacherId);

                if (teacher == null)
                {
                    _logger.Warn("Teacher {TeacherId} not found or not a teacher", teacherId);
                    throw new UnauthorizedAccessException("Only teachers can create classes");
                }

                if (createClassDto.EndDate <= createClassDto.StartDate)
                {
                    _logger.Warn("Invalid date range: StartDate {StartDate}, EndDate {EndDate}", createClassDto.StartDate, createClassDto.EndDate);
                    throw new InvalidOperationException("End date must be after start date");
                }

                var classEntity = _mapper.Map<Class>(createClassDto);
                classEntity.Id = Guid.NewGuid();
                classEntity.TeacherId = teacherId;

                await _unitOfWork.RepoClass.Add(classEntity);
                await _unitOfWork.SaveAsync();

                _logger.Info("Class {ClassId} created successfully for course {CourseId}", classEntity.Id, createClassDto.CourseId);

                return await GetClassByIdAsync(classEntity.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating class for course {CourseId} by teacher {TeacherId}", createClassDto.CourseId, teacherId);
                throw;
            }
        }

        public async Task<ClassDto> UpdateClassAsync(Guid id, UpdateClassDto updateClassDto, Guid teacherId)
        {
            try
            {
                _logger.Info("Updating class {ClassId} by teacher {TeacherId}", id, teacherId);

                var classEntity = await _unitOfWork.RepoClass.GetClassByIdForTeacherAsync(id, teacherId);

                if (classEntity == null)
                {
                    _logger.Warn("Class {ClassId} not found or teacher {TeacherId} has no permission", id, teacherId);
                    throw new KeyNotFoundException($"Class with ID {id} not found or you don't have permission to update it");
                }

                if (updateClassDto.EndDate <= updateClassDto.StartDate)
                {
                    _logger.Warn("Invalid date range: StartDate {StartDate}, EndDate {EndDate}", updateClassDto.StartDate, updateClassDto.EndDate);
                    throw new InvalidOperationException("End date must be after start date");
                }

                _mapper.Map(updateClassDto, classEntity);
                classEntity.UpdatedDate = DateTimeOffset.UtcNow;

                await _unitOfWork.RepoClass.Update(classEntity);
                await _unitOfWork.SaveAsync();

                _logger.Info("Class {ClassId} updated successfully", id);

                return await GetClassByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating class {ClassId} by teacher {TeacherId}", id, teacherId);
                throw;
            }
        }

        public async Task<bool> AssignStudentToClassAsync(AssignStudentDto assignStudentDto, Guid teacherId)
        {
            try
            {
                _logger.Info("Assigning student {StudentId} to class {ClassId}", assignStudentDto.StudentId, assignStudentDto.ClassId);

                var classEntity = await _unitOfWork.RepoClass.GetClassByIdForTeacherAsync(assignStudentDto.ClassId, teacherId);

                if (classEntity == null)
                {
                    _logger.Warn("Class {ClassId} not found or teacher {TeacherId} has no permission", assignStudentDto.ClassId, teacherId);
                    throw new KeyNotFoundException("Class not found or you don't have permission to assign students");
                }

                var student = await _unitOfWork.RepoUser.GetStudentByIdAsync(assignStudentDto.StudentId);

                if (student == null)
                {
                    _logger.Warn("Student {StudentId} not found", assignStudentDto.StudentId);
                    throw new KeyNotFoundException("Student not found");
                }

                var isAlreadyEnrolled = await _unitOfWork.RepoStudentClass.IsStudentEnrolledInClassAsync(assignStudentDto.StudentId, assignStudentDto.ClassId);

                if (isAlreadyEnrolled)
                {
                    _logger.Warn("Student {StudentId} already enrolled in class {ClassId}", assignStudentDto.StudentId, assignStudentDto.ClassId);
                    throw new InvalidOperationException("Student is already enrolled in this class");
                }

                var studentClass = new StudentClass
                {
                    Id = Guid.NewGuid(),
                    StudentId = assignStudentDto.StudentId,
                    ClassId = assignStudentDto.ClassId,
                    EnrollmentDate = DateTimeOffset.UtcNow
                };

                await _unitOfWork.RepoStudentClass.Add(studentClass);
                await _unitOfWork.SaveAsync();

                _logger.Info("Student {StudentId} assigned to class {ClassId} successfully", assignStudentDto.StudentId, assignStudentDto.ClassId);

                try
                {
                    var fullClass = await _unitOfWork.RepoClass.GetClassByIdWithRelationsAsync(assignStudentDto.ClassId);

                    await _emailService.SendNewClassNotificationAsync(
                        student.Email,
                        $"{student.Name}",
                        fullClass.Name,
                        fullClass.Course.Name,
                        $"{fullClass.Teacher.Name}"
                    );
                    _logger.Info("Class enrollment notification email sent to student {StudentId}", assignStudentDto.StudentId);
                }
                catch (Exception emailEx)
                {
                    _logger.Error(emailEx, "Failed to send class enrollment notification email to student {StudentId}", assignStudentDto.StudentId);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error assigning student {StudentId} to class {ClassId}", assignStudentDto.StudentId, assignStudentDto.ClassId);
                throw;
            }
        }

        public async Task<List<DTOs.User.UserDto>> GetStudentsInClassAsync(Guid classId)
        {
            try
            {
                _logger.Info("Getting students in class {ClassId}", classId);

                var classEntity = await _unitOfWork.RepoClass.GetClassByIdWithRelationsAsync(classId);

                if (classEntity == null)
                {
                    _logger.Warn("Class {ClassId} not found", classId);
                    throw new KeyNotFoundException("Class not found");
                }

                var students = await _unitOfWork.RepoStudentClass.GetStudentsWithDetailsByClassIdAsync(classId);

                var studentDtos = _mapper.Map<List<DTOs.User.UserDto>>(students);

                _logger.Info("Retrieved {Count} students in class {ClassId}", studentDtos.Count, classId);

                return studentDtos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting students in class {ClassId}", classId);
                throw;
            }
        }
    }
}
