// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System.Security.Claims;
using BL.Managers;
using DTOs.Assignment;
using DTOs.Attendance;
using DTOs.Class;
using DTOs.Common;
using DTOs.Notification;
using DTOs.Submission;
using DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UniversityApp.Controllers
{
    [Route("api/teacher")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly IClassManager _classManager;
        private readonly IAttendanceManager _attendanceManager;
        private readonly IAssignmentManager _assignmentManager;
        private readonly ISubmissionManager _submissionManager;
        private readonly INotificationManager _notificationManager;

        public TeacherController(
            IClassManager classManager,
            IAttendanceManager attendanceManager,
            IAssignmentManager assignmentManager,
            ISubmissionManager submissionManager,
            INotificationManager notificationManager)
        {
            _classManager = classManager;
            _attendanceManager = attendanceManager;
            _assignmentManager = assignmentManager;
            _submissionManager = submissionManager;
            _notificationManager = notificationManager;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim);
        }

        #region Classes

        [HttpGet("classes")]
        public async Task<ActionResult<ApiResponse<PagedResult<ClassDto>>>> GetMyClasses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var teacherId = GetCurrentUserId();
                var classes = await _classManager.GetAllClassesAsync(teacherId, pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<ClassDto>>.SuccessResponse(classes));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<ClassDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpGet("classes/{id}")]
        public async Task<ActionResult<ApiResponse<ClassDto>>> GetClassById(Guid id)
        {
            try
            {
                var classDto = await _classManager.GetClassByIdAsync(id);
                return Ok(ApiResponse<ClassDto>.SuccessResponse(classDto));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<ClassDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ClassDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpPost("classes")]
        public async Task<ActionResult<ApiResponse<ClassDto>>> CreateClass([FromBody] CreateClassDto createClassDto)
        {
            try
            {
                var teacherId = GetCurrentUserId();
                var classDto = await _classManager.CreateClassAsync(createClassDto, teacherId);
                return CreatedAtAction(nameof(GetClassById), new { id = classDto.Id }, ApiResponse<ClassDto>.SuccessResponse(classDto, "Class created successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<ClassDto>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<ClassDto>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ClassDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ClassDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpPut("classes/{id}")]
        public async Task<ActionResult<ApiResponse<ClassDto>>> UpdateClass(Guid id, [FromBody] UpdateClassDto updateClassDto)
        {
            try
            {
                var teacherId = GetCurrentUserId();
                var classDto = await _classManager.UpdateClassAsync(id, updateClassDto, teacherId);
                return Ok(ApiResponse<ClassDto>.SuccessResponse(classDto, "Class updated successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<ClassDto>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<ClassDto>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<ClassDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ClassDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpPost("classes/assign-student")]
        public async Task<ActionResult<ApiResponse<bool>>> AssignStudentToClass([FromBody] AssignStudentDto assignStudentDto)
        {
            try
            {
                var teacherId = GetCurrentUserId();
                var result = await _classManager.AssignStudentToClassAsync(assignStudentDto, teacherId);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Student assigned to class successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpGet("classes/{classId}/students")]
        public async Task<ActionResult<ApiResponse<List<UserDto>>>> GetStudentsInClass(Guid classId)
        {
            try
            {
                var students = await _classManager.GetStudentsInClassAsync(classId);
                return Ok(ApiResponse<List<UserDto>>.SuccessResponse(students));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<List<UserDto>>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<UserDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        #endregion

        #region Attendance

        [HttpPost("attendance")]
        public async Task<ActionResult<ApiResponse<AttendanceDto>>> MarkAttendance([FromBody] MarkAttendanceDto markAttendanceDto)
        {
            try
            {
                var teacherId = GetCurrentUserId();
                var attendance = await _attendanceManager.MarkAttendanceAsync(markAttendanceDto, teacherId);
                return Ok(ApiResponse<AttendanceDto>.SuccessResponse(attendance, "Attendance marked successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<AttendanceDto>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<AttendanceDto>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<AttendanceDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AttendanceDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpGet("attendance/{classId}")]
        public async Task<ActionResult<ApiResponse<List<AttendanceDto>>>> GetAttendanceByClass(Guid classId, [FromQuery] Guid? studentId = null)
        {
            try
            {
                var attendances = await _attendanceManager.GetAttendanceByClassAsync(classId, studentId);
                return Ok(ApiResponse<List<AttendanceDto>>.SuccessResponse(attendances));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AttendanceDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        #endregion

        #region Assignments

        [HttpPost("assignments")]
        public async Task<ActionResult<ApiResponse<AssignmentDto>>> CreateAssignment([FromBody] CreateAssignmentDto createAssignmentDto)
        {
            try
            {
                var teacherId = GetCurrentUserId();
                var assignment = await _assignmentManager.CreateAssignmentAsync(createAssignmentDto, teacherId);
                return Ok(ApiResponse<AssignmentDto>.SuccessResponse(assignment, "Assignment created successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<AssignmentDto>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<AssignmentDto>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<AssignmentDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AssignmentDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpGet("assignments/{classId}")]
        public async Task<ActionResult<ApiResponse<PagedResult<AssignmentDto>>>> GetAssignmentsByClass(Guid classId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var assignments = await _assignmentManager.GetAssignmentsByClassAsync(classId, pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<AssignmentDto>>.SuccessResponse(assignments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<AssignmentDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpPost("assignments/{submissionId}/grade")]
        public async Task<ActionResult<ApiResponse<SubmissionDto>>> GradeSubmission(Guid submissionId, [FromBody] GradeSubmissionDto gradeSubmissionDto)
        {
            try
            {
                var teacherId = GetCurrentUserId();
                var submission = await _submissionManager.GradeSubmissionAsync(submissionId, gradeSubmissionDto, teacherId);
                return Ok(ApiResponse<SubmissionDto>.SuccessResponse(submission, "Submission graded successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<SubmissionDto>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<SubmissionDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SubmissionDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpGet("assignments/{assignmentId}/submissions")]
        public async Task<ActionResult<ApiResponse<List<SubmissionDto>>>> GetSubmissionsByAssignment(Guid assignmentId)
        {
            try
            {
                var submissions = await _submissionManager.GetSubmissionsByAssignmentAsync(assignmentId);
                return Ok(ApiResponse<List<SubmissionDto>>.SuccessResponse(submissions));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<SubmissionDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        #endregion

        #region Notifications

        [HttpPost("notifications")]
        public async Task<ActionResult<ApiResponse<NotificationDto>>> SendNotification([FromBody] CreateNotificationDto createNotificationDto)
        {
            try
            {
                var notification = await _notificationManager.CreateNotificationAsync(createNotificationDto);
                return Ok(ApiResponse<NotificationDto>.SuccessResponse(notification, "Notification sent successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<NotificationDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        #endregion
    }
}
