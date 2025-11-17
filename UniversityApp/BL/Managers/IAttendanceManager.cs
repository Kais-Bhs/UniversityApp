// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DTOs.Attendance;

namespace BL.Managers
{
    public interface IAttendanceManager
    {
        Task<AttendanceDto> MarkAttendanceAsync(MarkAttendanceDto markAttendanceDto, Guid teacherId);
        Task<List<AttendanceDto>> GetAttendanceByClassAsync(Guid classId, Guid? studentId = null);
        Task<List<AttendanceDto>> GetStudentAttendanceAsync(Guid studentId);
    }
}
