// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using DAO.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAO
{
    public class UniversityContext : DbContext
    {
        public UniversityContext(DbContextOptions<UniversityContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new DepartmentEntityTypeConfiguration().Configure(modelBuilder.Entity<Department>());
            new CourseEntityTypeConfiguration().Configure(modelBuilder.Entity<Course>());
            new ClassEntityTypeConfiguration().Configure(modelBuilder.Entity<Class>());
            new StudentClassEntityTypeConfiguration().Configure(modelBuilder.Entity<StudentClass>());
            new AttendanceEntityTypeConfiguration().Configure(modelBuilder.Entity<Attendance>());
            new AssignmentEntityTypeConfiguration().Configure(modelBuilder.Entity<Assignment>());
            new SubmissionEntityTypeConfiguration().Configure(modelBuilder.Entity<Submission>());
            new NotificationEntityTypeConfiguration().Configure(modelBuilder.Entity<Notification>());

        }
    }
}