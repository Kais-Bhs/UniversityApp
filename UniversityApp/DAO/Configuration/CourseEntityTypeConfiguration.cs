// ---------------------------------------------------------------
// Copyright (c) Kais Bhh. All rights reserved.
// Licensed under the MIT License.
// See License.txt in the project root for license information.
// ---------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAO.Configuration
{
    internal class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Code).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(1000);
            builder.Property(c => c.Credits).IsRequired();
            builder.Property(c => c.CreatedDate).IsRequired();
            builder.Property(c => c.UpdatedDate).IsRequired();

            builder.HasIndex(c => new { c.Code, c.DepartmentId }).IsUnique();

            builder.HasMany(c => c.Classes)
                .WithOne(cl => cl.Course)
                .HasForeignKey(cl => cl.CourseId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
