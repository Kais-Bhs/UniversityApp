// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using AutoMapper;
using DTOs.Assignment;
using DTOs.Attendance;
using DTOs.Auth;
using DTOs.Class;
using DTOs.Course;
using DTOs.Department;
using DTOs.Notification;
using DTOs.Submission;
using DTOs.User;
using Entities;

namespace BL.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));

            // Department mappings
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.HeadOfDepartmentName, opt => opt.MapFrom(src => src.HeadOfDepartment.Name));
            CreateMap<CreateDepartmentDto, Department>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));
            CreateMap<UpdateDepartmentDto, Department>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));

            // Course mappings
            CreateMap<Course, CourseDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
            CreateMap<CreateCourseDto, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));
            CreateMap<UpdateCourseDto, Course>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));

            // Class mappings
            CreateMap<Class, ClassDto>()
                .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
                .ForMember(dest => dest.CourseCode, opt => opt.MapFrom(src => src.Course.Code))
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name))
                .ForMember(dest => dest.EnrolledStudentsCount, opt => opt.MapFrom(src => src.StudentClasses.Count));
            CreateMap<CreateClassDto, Class>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TeacherId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));
            CreateMap<UpdateClassDto, Class>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CourseId, opt => opt.Ignore())
                .ForMember(dest => dest.TeacherId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));

            // Attendance mappings
            CreateMap<Entities.Attendance, AttendanceDto>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.MarkedByTeacherName, opt => opt.MapFrom(src => src.MarkedByTeacher.Name));
            CreateMap<MarkAttendanceDto, Entities.Attendance>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MarkedByTeacherId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));

            // Assignment mappings
            CreateMap<Entities.Assignment, AssignmentDto>()
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name))
                .ForMember(dest => dest.CreatedByTeacherName, opt => opt.MapFrom(src => src.CreatedByTeacher.Name))
                .ForMember(dest => dest.SubmissionsCount, opt => opt.MapFrom(src => src.Submissions.Count));
            CreateMap<CreateAssignmentDto, Entities.Assignment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedByTeacherId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow));

            // Submission mappings
            CreateMap<Entities.Submission, SubmissionDto>()
                .ForMember(dest => dest.AssignmentTitle, opt => opt.MapFrom(src => src.Assignment.Title))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Name))
                .ForMember(dest => dest.GradedByTeacherName, opt => opt.MapFrom(src => src.GradedByTeacher != null ? src.GradedByTeacher.Name : null));
            CreateMap<SubmitAssignmentDto, Entities.Submission>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StudentId, opt => opt.Ignore())
                .ForMember(dest => dest.SubmittedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.Grade, opt => opt.Ignore())
                .ForMember(dest => dest.GradedByTeacherId, opt => opt.Ignore())
                .ForMember(dest => dest.Remarks, opt => opt.Ignore());

            // Notification mappings
            CreateMap<Entities.Notification, NotificationDto>();
            CreateMap<CreateNotificationDto, Entities.Notification>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTimeOffset.UtcNow))
                .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => false));
        }
    }
}
