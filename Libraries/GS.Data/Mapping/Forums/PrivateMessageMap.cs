﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Forums;

namespace GS.Data.Mapping.Forums
{
    /// <summary>
    /// Represents a private message mapping configuration
    /// </summary>
    public partial class PrivateMessageMap : GSEntityTypeConfiguration<PrivateMessage>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<PrivateMessage> builder)
        {
            builder.ToTable(GSMappingDefaults.PrivateMessageTable);
            builder.HasKey(message => message.Id);

            builder.Property(message => message.Subject).HasMaxLength(450).IsRequired();
            builder.Property(message => message.Text).IsRequired();

            builder.HasOne(message => message.FromCustomer)
               .WithMany()
               .HasForeignKey(message => message.FromCustomerId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(message => message.ToCustomer)
               .WithMany()
               .HasForeignKey(message => message.ToCustomerId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }

        #endregion
    }
}