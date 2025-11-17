// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Class;
using DTOs.Common;
using Entities;

namespace BL.Managers
{
    public class ClassManager : IClassManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClassManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<ClassDto>> GetAllClassesAsync(Guid? teacherId, int pageNumber = 1, int pageSize = 10)
        {
            var (classes, totalCount) = await _unitOfWork.RepoClass.GetClassesWithRelationsPaginatedAsync(teacherId, pageNumber, pageSize);

            var classDtos = _mapper.Map<List<ClassDto>>(classes);

            return new PagedResult<ClassDto>(classDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<ClassDto> GetClassByIdAsync(Guid id)
        {
            var classEntity = await _unitOfWork.RepoClass.GetClassByIdWithRelationsAsync(id);

            if (classEntity == null)
            {
                throw new KeyNotFoundException($"Class with ID {id} not found");
            }

            return _mapper.Map<ClassDto>(classEntity);
        }

        public async Task<ClassDto> CreateClassAsync(CreateClassDto createClassDto, Guid teacherId)
        {
            var course = await _unitOfWork.RepoCourse.GetCourseByIdWithDepartmentAsync(createClassDto.CourseId);

            if (course == null)
            {
                throw new KeyNotFoundException("Course not found");
            }

            var teacher = await _unitOfWork.RepoUser.GetTeacherByIdAsync(teacherId);

            if (teacher == null)
            {
                throw new UnauthorizedAccessException("Only teachers can create classes");
            }

            if (createClassDto.EndDate <= createClassDto.StartDate)
            {
                throw new InvalidOperationException("End date must be after start date");
            }

            var classEntity = _mapper.Map<Class>(createClassDto);
            classEntity.Id = Guid.NewGuid();
            classEntity.TeacherId = teacherId;

            await _unitOfWork.RepoClass.Add(classEntity);
            await _unitOfWork.SaveAsync();

            return await GetClassByIdAsync(classEntity.Id);
        }

        public async Task<ClassDto> UpdateClassAsync(Guid id, UpdateClassDto updateClassDto, Guid teacherId)
        {
            var classEntity = await _unitOfWork.RepoClass.GetClassByIdForTeacherAsync(id, teacherId);

            if (classEntity == null)
            {
                throw new KeyNotFoundException($"Class with ID {id} not found or you don't have permission to update it");
            }

            if (updateClassDto.EndDate <= updateClassDto.StartDate)
            {
                throw new InvalidOperationException("End date must be after start date");
            }

            _mapper.Map(updateClassDto, classEntity);
            classEntity.UpdatedDate = DateTimeOffset.UtcNow;

            await _unitOfWork.RepoClass.Update(classEntity);
            await _unitOfWork.SaveAsync();

            return await GetClassByIdAsync(id);
        }

        public async Task<bool> AssignStudentToClassAsync(AssignStudentDto assignStudentDto, Guid teacherId)
        {
            var classEntity = await _unitOfWork.RepoClass.GetClassByIdForTeacherAsync(assignStudentDto.ClassId, teacherId);

            if (classEntity == null)
            {
                throw new KeyNotFoundException("Class not found or you don't have permission to assign students");
            }

            var student = await _unitOfWork.RepoUser.GetStudentByIdAsync(assignStudentDto.StudentId);

            if (student == null)
            {
                throw new KeyNotFoundException("Student not found");
            }

            var isAlreadyEnrolled = await _unitOfWork.RepoStudentClass.IsStudentEnrolledInClassAsync(assignStudentDto.StudentId, assignStudentDto.ClassId);

            if (isAlreadyEnrolled)
            {
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

            return true;
        }

        public async Task<List<DTOs.User.UserDto>> GetStudentsInClassAsync(Guid classId)
        {
            var classEntity = await _unitOfWork.RepoClass.GetClassByIdWithRelationsAsync(classId);

            if (classEntity == null)
            {
                throw new KeyNotFoundException("Class not found");
            }

            var students = await _unitOfWork.RepoStudentClass.GetStudentsWithDetailsByClassIdAsync(classId);

            return _mapper.Map<List<DTOs.User.UserDto>>(students);
        }
    }
}
