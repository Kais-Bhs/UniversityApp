// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DTOs.Common;
using DTOs.Course;

namespace BL.Managers
{
    public interface ICourseManager
    {
        Task<PagedResult<CourseDto>> GetAllCoursesAsync(int pageNumber = 1, int pageSize = 10, string searchTerm = null);
        Task<CourseDto> GetCourseByIdAsync(Guid id);
        Task<CourseDto> CreateCourseAsync(CreateCourseDto createCourseDto);
        Task<CourseDto> UpdateCourseAsync(Guid id, UpdateCourseDto updateCourseDto);
        Task<bool> DeleteCourseAsync(Guid id);
    }
}
