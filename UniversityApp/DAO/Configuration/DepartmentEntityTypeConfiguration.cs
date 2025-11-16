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
    internal class DepartmentEntityTypeConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).ValueGeneratedOnAdd();
            builder.Property(d => d.Name).HasMaxLength(200).IsRequired();
            builder.Property(d => d.Description).HasMaxLength(1000);
            builder.Property(d => d.CreatedDate).IsRequired();
            builder.Property(d => d.UpdatedDate).IsRequired();

            builder.HasIndex(d => d.Name).IsUnique();

            builder.HasMany(d => d.Courses)
                .WithOne(c => c.Department)
                .HasForeignKey(c => c.DepartmentId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
