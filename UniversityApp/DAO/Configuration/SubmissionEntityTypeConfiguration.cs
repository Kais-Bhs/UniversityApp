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
    internal class SubmissionEntityTypeConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Property(s => s.SubmittedDate).IsRequired();
            builder.Property(s => s.FileUrl).HasMaxLength(500);
            builder.Property(s => s.Grade).HasPrecision(5, 2);
            builder.Property(s => s.Remarks).HasMaxLength(1000);

            builder.HasIndex(s => new { s.AssignmentId, s.StudentId }).IsUnique();
        }
    }
}
