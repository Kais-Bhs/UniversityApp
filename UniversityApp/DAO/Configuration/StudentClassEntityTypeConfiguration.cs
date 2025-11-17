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
    internal class StudentClassEntityTypeConfiguration : IEntityTypeConfiguration<StudentClass>
    {
        public void Configure(EntityTypeBuilder<StudentClass> builder)
        {
            builder.HasKey(sc => sc.Id);
            builder.Property(sc => sc.Id).ValueGeneratedOnAdd();
            builder.Property(sc => sc.EnrollmentDate).IsRequired();

            builder.HasIndex(sc => new { sc.StudentId, sc.ClassId }).IsUnique();
        }
    }
}
