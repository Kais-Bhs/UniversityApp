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
    internal class ClassEntityTypeConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
            builder.Property(c => c.Semester).HasMaxLength(50).IsRequired();
            builder.Property(c => c.StartDate).IsRequired();
            builder.Property(c => c.EndDate).IsRequired();
            builder.Property(c => c.IsActive).IsRequired();
            builder.Property(c => c.CreatedDate).IsRequired();
            builder.Property(c => c.UpdatedDate).IsRequired();

            builder.HasMany(c => c.StudentClasses)
                .WithOne(sc => sc.Class)
                .HasForeignKey(sc => sc.ClassId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Attendances)
                .WithOne(a => a.Class)
                .HasForeignKey(a => a.ClassId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Assignments)
                .WithOne(a => a.Class)
                .HasForeignKey(a => a.ClassId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
