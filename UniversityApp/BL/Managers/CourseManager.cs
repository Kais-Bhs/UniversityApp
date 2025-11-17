// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Course;
using DTOs.Common;
using Entities;
using Microsoft.Extensions.Caching.Memory;

namespace BL.Managers
{
    public class CourseManager : ICourseManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string CoursesCacheKey = "AllCourses";

        public CourseManager(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<PagedResult<CourseDto>> GetAllCoursesAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null)
        {
            var (courses, totalCount) = await _unitOfWork.RepoCourse.GetCoursesWithDepartmentPaginatedAsync(pageNumber, pageSize, searchTerm);

            var courseDtos = _mapper.Map<List<CourseDto>>(courses);

            return new PagedResult<CourseDto>(courseDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<CourseDto> GetCourseByIdAsync(Guid id)
        {
            var course = await _unitOfWork.RepoCourse.GetCourseByIdWithDepartmentAsync(id);

            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {id} not found");
            }

            return _mapper.Map<CourseDto>(course);
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
        {
            var codeExists = await _unitOfWork.RepoCourse.CheckCourseCodeExistsInDepartmentAsync(createCourseDto.Code, createCourseDto.DepartmentId);

            if (codeExists)
            {
                throw new InvalidOperationException("Course code must be unique per department");
            }

            var departmentExists = await _unitOfWork.RepoDepartment.DoesDepartmentExistAsync(createCourseDto.DepartmentId);

            if (!departmentExists)
            {
                throw new KeyNotFoundException("Department not found");
            }

            var course = _mapper.Map<Course>(createCourseDto);
            course.Id = Guid.NewGuid();

            await _unitOfWork.RepoCourse.Add(course);
            await _unitOfWork.SaveAsync();

            _cache.Remove(CoursesCacheKey);

            return await GetCourseByIdAsync(course.Id);
        }

        public async Task<CourseDto> UpdateCourseAsync(Guid id, UpdateCourseDto updateCourseDto)
        {
            var course = await _unitOfWork.RepoCourse.GetCourseByIdWithDepartmentAsync(id);

            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {id} not found");
            }

            var codeExists = await _unitOfWork.RepoCourse.CheckCourseCodeExistsInDepartmentAsync(updateCourseDto.Code, updateCourseDto.DepartmentId, id);

            if (codeExists)
            {
                throw new InvalidOperationException("Course code must be unique per department");
            }

            var departmentExists = await _unitOfWork.RepoDepartment.DoesDepartmentExistAsync(updateCourseDto.DepartmentId);

            if (!departmentExists)
            {
                throw new KeyNotFoundException("Department not found");
            }

            _mapper.Map(updateCourseDto, course);
            course.UpdatedDate = DateTimeOffset.UtcNow;

            await _unitOfWork.RepoCourse.Update(course);
            await _unitOfWork.SaveAsync();

            _cache.Remove(CoursesCacheKey);

            return await GetCourseByIdAsync(id);
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            var course = await _unitOfWork.RepoCourse.GetCourseByIdWithDepartmentAsync(id);

            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {id} not found");
            }

            await _unitOfWork.RepoCourse.Delete(course);
            await _unitOfWork.SaveAsync();

            _cache.Remove(CoursesCacheKey);

            return true;
        }
    }
}
