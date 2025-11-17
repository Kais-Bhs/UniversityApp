// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Assignment;
using DTOs.Common;

namespace BL.Managers
{
    public class AssignmentManager : IAssignmentManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AssignmentManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AssignmentDto> CreateAssignmentAsync(CreateAssignmentDto createAssignmentDto, Guid teacherId)
        {
            var classEntity = await _unitOfWork.RepoClass.GetClassByIdForTeacherAsync(createAssignmentDto.ClassId, teacherId);

            if (classEntity == null)
            {
                throw new KeyNotFoundException("Class not found or you can only create assignments for your own classes");
            }

            if (createAssignmentDto.DueDate <= DateTimeOffset.UtcNow)
            {
                throw new InvalidOperationException("Assignment due date cannot be in the past");
            }

            var assignment = _mapper.Map<Entities.Assignment>(createAssignmentDto);
            assignment.Id = Guid.NewGuid();
            assignment.CreatedByTeacherId = teacherId;

            await _unitOfWork.RepoAssignment.Add(assignment);
            await _unitOfWork.SaveAsync();

            var savedAssignment = await _unitOfWork.RepoAssignment.GetAssignmentByIdWithRelationsAsync(assignment.Id);

            return _mapper.Map<AssignmentDto>(savedAssignment);
        }

        public async Task<PagedResult<AssignmentDto>> GetAssignmentsByClassAsync(Guid classId, int pageNumber = 1, int pageSize = 10)
        {
            var (assignments, totalCount) = await _unitOfWork.RepoAssignment.GetAssignmentsByClassPaginatedAsync(classId, pageNumber, pageSize);

            var assignmentDtos = _mapper.Map<List<AssignmentDto>>(assignments);

            return new PagedResult<AssignmentDto>(assignmentDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<AssignmentDto> GetAssignmentByIdAsync(Guid id)
        {
            var assignment = await _unitOfWork.RepoAssignment.GetAssignmentByIdWithRelationsAsync(id);

            if (assignment == null)
            {
                throw new KeyNotFoundException($"Assignment with ID {id} not found");
            }

            return _mapper.Map<AssignmentDto>(assignment);
        }

        public async Task<List<AssignmentDto>> GetStudentAssignmentsAsync(Guid studentId)
        {
            var enrolledClassIds = await _unitOfWork.RepoStudentClass.GetEnrolledClassIdsByStudentAsync(studentId);

            var assignments = await _unitOfWork.RepoAssignment.GetAssignmentsByEnrolledClassesAsync(enrolledClassIds);

            return _mapper.Map<List<AssignmentDto>>(assignments);
        }
    }
}
