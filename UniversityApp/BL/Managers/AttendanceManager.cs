// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DAL;
using DTOs.Attendance;
using Entities;

namespace BL.Managers
{
    public class AttendanceManager : IAttendanceManager
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AttendanceManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AttendanceDto> MarkAttendanceAsync(MarkAttendanceDto markAttendanceDto, Guid teacherId)
        {
            var classEntity = await _unitOfWork.RepoClass.GetClassByIdForTeacherAsync(markAttendanceDto.ClassId, teacherId);

            if (classEntity == null)
            {
                throw new KeyNotFoundException("Class not found or you don't have permission to mark attendance");
            }

            var isEnrolled = await _unitOfWork.RepoStudentClass.IsStudentEnrolledInClassAsync(markAttendanceDto.StudentId, markAttendanceDto.ClassId);

            if (!isEnrolled)
            {
                throw new InvalidOperationException("Student is not enrolled in this class");
            }

            var attendance = _mapper.Map<Entities.Attendance>(markAttendanceDto);
            attendance.Id = Guid.NewGuid();
            attendance.MarkedByTeacherId = teacherId;

            await _unitOfWork.RepoAttendance.Add(attendance);
            await _unitOfWork.SaveAsync();

            var savedAttendance = await _unitOfWork.RepoAttendance.GetAttendanceByIdWithRelationsAsync(attendance.Id);

            return _mapper.Map<AttendanceDto>(savedAttendance);
        }

        public async Task<List<AttendanceDto>> GetAttendanceByClassAsync(Guid classId, Guid? studentId = null)
        {
            var attendances = await _unitOfWork.RepoAttendance.GetAttendanceByClassWithRelationsAsync(classId, studentId);

            return _mapper.Map<List<AttendanceDto>>(attendances);
        }

        public async Task<List<AttendanceDto>> GetStudentAttendanceAsync(Guid studentId)
        {
            var attendances = await _unitOfWork.RepoAttendance.GetAttendanceByStudentWithRelationsAsync(studentId);

            return _mapper.Map<List<AttendanceDto>>(attendances);
        }
    }
}
