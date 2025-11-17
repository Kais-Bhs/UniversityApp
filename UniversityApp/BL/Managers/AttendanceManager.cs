// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Attendance;
using NLog;

namespace BL.Managers
{
    public class AttendanceManager : IAttendanceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public AttendanceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AttendanceDto> MarkAttendanceAsync(MarkAttendanceDto markAttendanceDto, Guid teacherId)
        {
            try
            {
                _logger.Info("Marking attendance for student {StudentId} in class {ClassId} by teacher {TeacherId}", 
                    markAttendanceDto.StudentId, markAttendanceDto.ClassId, teacherId);

                var classEntity = await _unitOfWork.RepoClass.GetClassByIdForTeacherAsync(markAttendanceDto.ClassId, teacherId);

                if (classEntity == null)
                {
                    _logger.Warn("Class {ClassId} not found or teacher {TeacherId} has no permission", markAttendanceDto.ClassId, teacherId);
                    throw new KeyNotFoundException("Class not found or you don't have permission to mark attendance");
                }

                var isEnrolled = await _unitOfWork.RepoStudentClass.IsStudentEnrolledInClassAsync(markAttendanceDto.StudentId, markAttendanceDto.ClassId);

                if (!isEnrolled)
                {
                    _logger.Warn("Student {StudentId} not enrolled in class {ClassId}", markAttendanceDto.StudentId, markAttendanceDto.ClassId);
                    throw new InvalidOperationException("Student is not enrolled in this class");
                }

                var attendance = _mapper.Map<Entities.Attendance>(markAttendanceDto);
                attendance.Id = Guid.NewGuid();
                attendance.MarkedByTeacherId = teacherId;

                await _unitOfWork.RepoAttendance.Add(attendance);
                await _unitOfWork.SaveAsync();

                _logger.Info("Attendance {AttendanceId} marked successfully for student {StudentId}", attendance.Id, markAttendanceDto.StudentId);

                var savedAttendance = await _unitOfWork.RepoAttendance.GetAttendanceByIdWithRelationsAsync(attendance.Id);

                return _mapper.Map<AttendanceDto>(savedAttendance);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error marking attendance for student {StudentId} in class {ClassId}", 
                    markAttendanceDto.StudentId, markAttendanceDto.ClassId);
                throw;
            }
        }

        public async Task<List<AttendanceDto>> GetAttendanceByClassAsync(Guid classId, Guid? studentId = null)
        {
            try
            {
                _logger.Info("Getting attendance for class {ClassId}, student filter: {StudentId}", classId, studentId?.ToString() ?? "None");

                var attendances = await _unitOfWork.RepoAttendance.GetAttendanceByClassWithRelationsAsync(classId, studentId);

                var attendanceDtos = _mapper.Map<List<AttendanceDto>>(attendances);

                _logger.Info("Retrieved {Count} attendance records for class {ClassId}", attendanceDtos.Count, classId);

                return attendanceDtos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting attendance for class {ClassId}", classId);
                throw;
            }
        }

        public async Task<List<AttendanceDto>> GetStudentAttendanceAsync(Guid studentId)
        {
            try
            {
                _logger.Info("Getting attendance for student {StudentId}", studentId);

                var attendances = await _unitOfWork.RepoAttendance.GetAttendanceByStudentWithRelationsAsync(studentId);

                var attendanceDtos = _mapper.Map<List<AttendanceDto>>(attendances);

                _logger.Info("Retrieved {Count} attendance records for student {StudentId}", attendanceDtos.Count, studentId);

                return attendanceDtos;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting attendance for student {StudentId}", studentId);
                throw;
            }
        }
    }
}
