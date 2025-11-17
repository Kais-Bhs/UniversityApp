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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UniversityApp.Controllers
{
    [Route("api/student")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class StudentController : ControllerBase
    {
        private readonly IClassManager _classManager;
        private readonly IAttendanceManager _attendanceManager;
        private readonly IAssignmentManager _assignmentManager;
        private readonly ISubmissionManager _submissionManager;
        private readonly INotificationManager _notificationManager;

        public StudentController(
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

        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value;
        }

        #region Classes

        [HttpGet("classes")]
        public async Task<ActionResult<ApiResponse<PagedResult<ClassDto>>>> GetEnrolledClasses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                // This would need to be implemented in ClassManager to filter by enrolled students
                var classes = await _classManager.GetAllClassesAsync(null, pageNumber, pageSize);
                return Ok(ApiResponse<PagedResult<ClassDto>>.SuccessResponse(classes));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<PagedResult<ClassDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        #endregion

        #region Attendance

        [HttpGet("attendance")]
        public async Task<ActionResult<ApiResponse<List<AttendanceDto>>>> GetMyAttendance()
        {
            try
            {
                var studentId = GetCurrentUserId();
                var attendances = await _attendanceManager.GetStudentAttendanceAsync(studentId);
                return Ok(ApiResponse<List<AttendanceDto>>.SuccessResponse(attendances));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AttendanceDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        #endregion

        #region Assignments

        [HttpGet("assignments")]
        public async Task<ActionResult<ApiResponse<List<AssignmentDto>>>> GetMyAssignments()
        {
            try
            {
                var studentId = GetCurrentUserId();
                var assignments = await _assignmentManager.GetStudentAssignmentsAsync(studentId);
                return Ok(ApiResponse<List<AssignmentDto>>.SuccessResponse(assignments));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<AssignmentDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpGet("assignments/{id}")]
        public async Task<ActionResult<ApiResponse<AssignmentDto>>> GetAssignmentById(Guid id)
        {
            try
            {
                var assignment = await _assignmentManager.GetAssignmentByIdAsync(id);
                return Ok(ApiResponse<AssignmentDto>.SuccessResponse(assignment));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<AssignmentDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<AssignmentDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpPost("assignments/{id}/submit")]
        public async Task<ActionResult<ApiResponse<SubmissionDto>>> SubmitAssignment(Guid id, [FromBody] SubmitAssignmentDto submitAssignmentDto)
        {
            try
            {
                submitAssignmentDto.AssignmentId = id;
                var studentId = GetCurrentUserId();
                var submission = await _submissionManager.SubmitAssignmentAsync(submitAssignmentDto, studentId);
                return Ok(ApiResponse<SubmissionDto>.SuccessResponse(submission, "Assignment submitted successfully"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<SubmissionDto>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<SubmissionDto>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<SubmissionDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SubmissionDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }
        [HttpPost("assignments/{id}/submit-with-file")]
        public async Task<ActionResult<ApiResponse<SubmissionDto>>> SubmitAssignmentWithFile(Guid id, [FromForm] SubmitAssignmentWithFileDto submitDto)
        {
            try
            {
                submitDto.AssignmentId = id;
                var studentId = GetCurrentUserId();
                var submission = await _submissionManager.SubmitAssignmentWithFileAsync(submitDto, studentId);
                return Ok(ApiResponse<SubmissionDto>.SuccessResponse(submission, "Assignment submitted successfully with file"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<SubmissionDto>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<SubmissionDto>.ErrorResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<SubmissionDto>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SubmissionDto>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpGet("submissions/{submissionId}/download")]
        public async Task<IActionResult> DownloadSubmissionFile(Guid submissionId)
        {
            try
            {
                var studentId = GetCurrentUserId();
                var submissions = await _submissionManager.GetStudentGradesAsync(studentId);
                var submission = submissions.FirstOrDefault(s => s.Id == submissionId);

                if (submission == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Submission not found or you don't have access"));
                }

                if (string.IsNullOrEmpty(submission.FileUrl))
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("No file associated with this submission"));
                }

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", submission.FileUrl.Replace("/", "\\"));

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("File not found on server"));
                }

                var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                var fileName = Path.GetFileName(filePath);

                return File(fileBytes, "application/octet-stream", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }
        #endregion

        #region Grades

        [HttpGet("grades")]
        public async Task<ActionResult<ApiResponse<List<SubmissionDto>>>> GetMyGrades()
        {
            try
            {
                var studentId = GetCurrentUserId();
                var grades = await _submissionManager.GetStudentGradesAsync(studentId);
                return Ok(ApiResponse<List<SubmissionDto>>.SuccessResponse(grades));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<SubmissionDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        #endregion

        #region Notifications

        [HttpGet("notifications")]
        public async Task<ActionResult<ApiResponse<List<NotificationDto>>>> GetMyNotifications()
        {
            try
            {
                var userId = GetCurrentUserId();
                var userRole = GetCurrentUserRole();
                var notifications = await _notificationManager.GetUserNotificationsAsync(userId, userRole);
                return Ok(ApiResponse<List<NotificationDto>>.SuccessResponse(notifications));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<NotificationDto>>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        [HttpPut("notifications/{id}/read")]
        public async Task<ActionResult<ApiResponse<bool>>> MarkNotificationAsRead(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _notificationManager.MarkNotificationAsReadAsync(id, userId);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Notification marked as read"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ApiResponse<bool>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred", new List<string> { ex.Message }));
            }
        }

        #endregion
    }
}
