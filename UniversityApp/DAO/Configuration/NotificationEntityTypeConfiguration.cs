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
    internal class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Id).ValueGeneratedOnAdd();
            builder.Property(n => n.Title).HasMaxLength(200).IsRequired();
            builder.Property(n => n.Message).HasMaxLength(2000).IsRequired();
            builder.Property(n => n.RecipientRole).HasMaxLength(50);
            builder.Property(n => n.CreatedDate).IsRequired();
            builder.Property(n => n.IsRead).IsRequired();
        }
    }
}
