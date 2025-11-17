// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using BL.Managers;
using DTOs.Common;
using DTOs.Course;
using DTOs.Department;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UniversityApp.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IDepartmentManager _departmentManager;
        private readonly ICourseManager _courseManager;

        public AdminController(IDepartmentManager departmentManager, ICourseManager courseManager)
        {
            _departmentManager = departmentManager;
            _courseManager = courseManager;
        }

        #region Departments

        [HttpGet("departments")]
        public async Task<ActionResult<ApiResponse<List<DepartmentDto>>>> GetAllDepartments()
        {
            try
            {
                var departments = await _departmentManager.GetAllDepartmentsAsync();
                return Ok(ApiResponse<List<DepartmentDto>>.SuccessResponse(departments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<DepartmentDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpGet("departments/{id}")]
        public async Task<ActionResult<ApiResponse<DepartmentDto>>> GetDepartmentById(Guid id)
        {
            try
            {
                var department = await _departmentManager.GetDepartmentByIdAsync(id);
                return Ok(ApiResponse<DepartmentDto>.SuccessResponse(department));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<DepartmentDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DepartmentDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpPost("departments")]
        public async Task<ActionResult<ApiResponse<DepartmentDto>>> CreateDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
        {
            try
            {
                var department = await _departmentManager.CreateDepartmentAsync(createDepartmentDto);
                return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, ApiResponse<DepartmentDto>.SuccessResponse(department, "Department created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<DepartmentDto>.ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<DepartmentDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DepartmentDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpPut("departments/{id}")]
        public async Task<ActionResult<ApiResponse<DepartmentDto>>> UpdateDepartment(Guid id, [FromBody] UpdateDepartmentDto updateDepartmentDto)
        {
            try
            {
                var department = await _departmentManager.UpdateDepartmentAsync(id, updateDepartmentDto);
                return Ok(ApiResponse<DepartmentDto>.SuccessResponse(department, "Department updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<DepartmentDto>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<DepartmentDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<DepartmentDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpDelete("departments/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteDepartment(Guid id)
        {
            try
            {
                var result = await _departmentManager.DeleteDepartmentAsync(id);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Department deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        #endregion

        #region Courses

        [HttpGet("courses")]
        public async Task<ActionResult<ApiResponse<PagedResult<CourseDto>>>> GetAllCourses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = null)
        {
            try
            {
                var courses = await _courseManager.GetAllCoursesAsync(pageNumber, pageSize, searchTerm);
                return Ok(ApiResponse<PagedResult<CourseDto>>.SuccessResponse(courses));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<CourseDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpGet("courses/{id}")]
        public async Task<ActionResult<ApiResponse<CourseDto>>> GetCourseById(Guid id)
        {
            try
            {
                var course = await _courseManager.GetCourseByIdAsync(id);
                return Ok(ApiResponse<CourseDto>.SuccessResponse(course));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<CourseDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CourseDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpPost("courses")]
        public async Task<ActionResult<ApiResponse<CourseDto>>> CreateCourse([FromBody] CreateCourseDto createCourseDto)
        {
            try
            {
                var course = await _courseManager.CreateCourseAsync(createCourseDto);
                return CreatedAtAction(nameof(GetCourseById), new { id = course.Id }, ApiResponse<CourseDto>.SuccessResponse(course, "Course created successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<CourseDto>.ErrorResponse(ex.Message));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<CourseDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CourseDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpPut("courses/{id}")]
        public async Task<ActionResult<ApiResponse<CourseDto>>> UpdateCourse(Guid id, [FromBody] UpdateCourseDto updateCourseDto)
        {
            try
            {
                var course = await _courseManager.UpdateCourseAsync(id, updateCourseDto);
                return Ok(ApiResponse<CourseDto>.SuccessResponse(course, "Course updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<CourseDto>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<CourseDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CourseDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpDelete("courses/{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteCourse(Guid id)
        {
            try
            {
                var result = await _courseManager.DeleteCourseAsync(id);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Course deleted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        #endregion
    }
}
