// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Assignment;
using DTOs.Common;
using NLog;

namespace BL.Managers
{
    public class AssignmentManager : IAssignmentManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public AssignmentManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AssignmentDto> CreateAssignmentAsync(CreateAssignmentDto createAssignmentDto, Guid teacherId)
        {
            try
            {
                _logger.Info("Creating assignment for class {ClassId} by teacher {TeacherId}", createAssignmentDto.ClassId, teacherId);

                var classEntity = await _unitOfWork.RepoClass.GetClassByIdForTeacherAsync(createAssignmentDto.ClassId, teacherId);

                if (classEntity == null)
                {
                    _logger.Warn("Class {ClassId} not found or teacher {TeacherId} has no permission", createAssignmentDto.ClassId, teacherId);
                    throw new KeyNotFoundException("Class not found or you can only create assignments for your own classes");
                }

                if (createAssignmentDto.DueDate <= DateTimeOffset.UtcNow)
                {
                    _logger.Warn("Invalid due date {DueDate} for assignment", createAssignmentDto.DueDate);
                    throw new InvalidOperationException("Assignment due date cannot be in the past");
                }

                var assignment = _mapper.Map<Entities.Assignment>(createAssignmentDto);
                assignment.Id = Guid.NewGuid();
                assignment.CreatedByTeacherId = teacherId;

                await _unitOfWork.RepoAssignment.Add(assignment);
                await _unitOfWork.SaveAsync();

                _logger.Info("Assignment {AssignmentId} created successfully", assignment.Id);

                var savedAssignment = await _unitOfWork.RepoAssignment.GetAssignmentByIdWithRelationsAsync(assignment.Id);

                return _mapper.Map<AssignmentDto>(savedAssignment);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating assignment for class {ClassId} by teacher {TeacherId}", createAssignmentDto.ClassId, teacherId);
                throw;
            }
        }

        public async Task<PagedResult<AssignmentDto>> GetAssignmentsByClassAsync(Guid classId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                _logger.Info("Getting assignments for class {ClassId}, page {PageNumber}", classId, pageNumber);

                var (assignments, totalCount) = await _unitOfWork.RepoAssignment.GetAssignmentsByClassPaginatedAsync(classId, pageNumber, pageSize);

                var assignmentDtos = _mapper.Map<List<AssignmentDto>>(assignments);

                _logger.Info("Retrieved {Count} assignments for class {ClassId}", assignmentDtos.Count, classId);

                return new PagedResult<AssignmentDto>(assignmentDtos, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting assignments for class {ClassId}", classId);
                throw;
            }
        }

        public async Task<AssignmentDto> GetAssignmentByIdAsync(Guid id)
        {
            try
            {
                _logger.Info("Getting assignment {AssignmentId}", id);

                var assignment = await _unitOfWork.RepoAssignment.GetAssignmentByIdWithRelationsAsync(id);

                if (assignment == null)
                {
                    _logger.Warn("Assignment {AssignmentId} not found", id);
                    throw new KeyNotFoundException($"Assignment with ID {id} not found");
                }

                _logger.Info("Assignment {AssignmentId} retrieved successfully", id);

                return _mapper.Map<AssignmentDto>(assignment);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting assignment {AssignmentId}", id);
                throw;
            }
        }

        public async Task<List<AssignmentDto>> GetStudentAssignmentsAsync(Guid studentId)
        {
            try
            {
                _logger.Info("Getting assignments for student {StudentId}", studentId);

                var enrolledClassIds = await _unitOfWork.RepoStudentClass.GetEnrolledClassIdsByStudentAsync(studentId);

                var assignments = await _unitOfWork.RepoAssignment.GetAssignmentsByEnrolledClassesAsync(enrolledClassIds);

                var assignmentDtos = _mapper.Map<List<AssignmentDto>>(assignments);

                _logger.Info("Retrieved {Count} assignments for student {StudentId}", assignmentDtos.Count, studentId);

                return assignmentDtos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting assignments for student {StudentId}", studentId);
                throw;
            }
        }
    }
}
