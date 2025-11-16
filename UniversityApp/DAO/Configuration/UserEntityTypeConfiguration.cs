// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAO.Configuration
{
    internal class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
            builder.Property(u => u.Name).HasMaxLength(200).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(200).IsRequired();
            builder.Property(u => u.Password).HasMaxLength(500).IsRequired();
            builder.Property(u => u.Role).HasMaxLength(50).IsRequired();
            builder.Property(u => u.CreatedDate).IsRequired();
            builder.Property(u => u.UpdatedDate).IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();

            builder.HasMany(u => u.TaughtClasses)
                .WithOne(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.StudentClasses)
                .WithOne(sc => sc.Student)
                .HasForeignKey(sc => sc.StudentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Attendances)
                .WithOne(a => a.Student)
                .HasForeignKey(a => a.StudentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.MarkedAttendances)
                .WithOne(a => a.MarkedByTeacher)
                .HasForeignKey(a => a.MarkedByTeacherId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.CreatedAssignments)
                .WithOne(a => a.CreatedByTeacher)
                .HasForeignKey(a => a.CreatedByTeacherId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Submissions)
                .WithOne(s => s.Student)
                .HasForeignKey(s => s.StudentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.GradedSubmissions)
                .WithOne(s => s.GradedByTeacher)
                .HasForeignKey(s => s.GradedByTeacherId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.ManagedDepartments)
                .WithOne(d => d.HeadOfDepartment)
                .HasForeignKey(d => d.HeadOfDepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}