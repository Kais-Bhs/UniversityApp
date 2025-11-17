// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Common;
using DTOs.Course;
using Entities;
using Microsoft.Extensions.Caching.Memory;
using NLog;

namespace BL.Managers
{
    public class CourseManager : ICourseManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private const string CoursesCacheKey = "AllCourses";
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public CourseManager(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<PagedResult<CourseDto>> GetAllCoursesAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null)
        {
            try
            {
                _logger.Info("Getting courses, page {PageNumber}, search term: {SearchTerm}", pageNumber, searchTerm ?? "None");

                var (courses, totalCount) = await _unitOfWork.RepoCourse.GetCoursesWithDepartmentPaginatedAsync(pageNumber, pageSize, searchTerm);

                var courseDtos = _mapper.Map<List<CourseDto>>(courses);

                _logger.Info("Retrieved {Count} courses", courseDtos.Count);

                return new PagedResult<CourseDto>(courseDtos, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting courses");
                throw;
            }
        }

        public async Task<CourseDto> GetCourseByIdAsync(Guid id)
        {
            try
            {
                _logger.Info("Getting course {CourseId}", id);

                var course = await _unitOfWork.RepoCourse.GetCourseByIdWithDepartmentAsync(id);

                if (course == null)
                {
                    _logger.Warn("Course {CourseId} not found", id);
                    throw new KeyNotFoundException($"Course with ID {id} not found");
                }

                _logger.Info("Course {CourseId} retrieved successfully", id);

                return _mapper.Map<CourseDto>(course);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting course {CourseId}", id);
                throw;
            }
        }

        public async Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto)
        {
            try
            {
                _logger.Info("Creating course with code {Code} in department {DepartmentId}", createCourseDto.Code, createCourseDto.DepartmentId);

                var codeExists = await _unitOfWork.RepoCourse.CheckCourseCodeExistsInDepartmentAsync(createCourseDto.Code, createCourseDto.DepartmentId);

                if (codeExists)
                {
                    _logger.Warn("Course code {Code} already exists in department {DepartmentId}", createCourseDto.Code, createCourseDto.DepartmentId);
                    throw new InvalidOperationException("Course code must be unique per department");
                }

                var departmentExists = await _unitOfWork.RepoDepartment.DoesDepartmentExistAsync(createCourseDto.DepartmentId);

                if (!departmentExists)
                {
                    _logger.Warn("Department {DepartmentId} not found", createCourseDto.DepartmentId);
                    throw new KeyNotFoundException("Department not found");
                }

                var course = _mapper.Map<Course>(createCourseDto);
                course.Id = Guid.NewGuid();

                await _unitOfWork.RepoCourse.Add(course);
                await _unitOfWork.SaveAsync();

                _cache.Remove(CoursesCacheKey);

                _logger.Info("Course {CourseId} created successfully", course.Id);

                return await GetCourseByIdAsync(course.Id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating course with code {Code}", createCourseDto.Code);
                throw;
            }
        }

        public async Task<CourseDto> UpdateCourseAsync(Guid id, UpdateCourseDto updateCourseDto)
        {
            try
            {
                _logger.Info("Updating course {CourseId}", id);

                var course = await _unitOfWork.RepoCourse.GetCourseByIdWithDepartmentAsync(id);

                if (course == null)
                {
                    _logger.Warn("Course {CourseId} not found", id);
                    throw new KeyNotFoundException($"Course with ID {id} not found");
                }

                var codeExists = await _unitOfWork.RepoCourse.CheckCourseCodeExistsInDepartmentAsync(updateCourseDto.Code, updateCourseDto.DepartmentId, id);

                if (codeExists)
                {
                    _logger.Warn("Course code {Code} already exists in department {DepartmentId}", updateCourseDto.Code, updateCourseDto.DepartmentId);
                    throw new InvalidOperationException("Course code must be unique per department");
                }

                var departmentExists = await _unitOfWork.RepoDepartment.DoesDepartmentExistAsync(updateCourseDto.DepartmentId);

                if (!departmentExists)
                {
                    _logger.Warn("Department {DepartmentId} not found", updateCourseDto.DepartmentId);
                    throw new KeyNotFoundException("Department not found");
                }

                _mapper.Map(updateCourseDto, course);
                course.UpdatedDate = DateTimeOffset.UtcNow;

                await _unitOfWork.RepoCourse.Update(course);
                await _unitOfWork.SaveAsync();

                _cache.Remove(CoursesCacheKey);

                _logger.Info("Course {CourseId} updated successfully", id);

                return await GetCourseByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating course {CourseId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteCourseAsync(Guid id)
        {
            try
            {
                _logger.Info("Deleting course {CourseId}", id);

                var course = await _unitOfWork.RepoCourse.GetCourseByIdWithDepartmentAsync(id);

                if (course == null)
                {
                    _logger.Warn("Course {CourseId} not found", id);
                    throw new KeyNotFoundException($"Course with ID {id} not found");
                }

                await _unitOfWork.RepoCourse.Delete(course);
                await _unitOfWork.SaveAsync();

                _cache.Remove(CoursesCacheKey);

                _logger.Info("Course {CourseId} deleted successfully", id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting course {CourseId}", id);
                throw;
            }
        }
    }
}
