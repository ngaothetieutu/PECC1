﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Forums;

namespace GS.Data.Mapping.Forums
{
    /// <summary>
    /// Represents a forum topic mapping configuration
    /// </summary>
    public partial class ForumTopicMap : GSEntityTypeConfiguration<ForumTopic>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ForumTopic> builder)
        {
            builder.ToTable(GSMappingDefaults.ForumsTopicTable);
            builder.HasKey(topic => topic.Id);

            builder.Property(topic => topic.Subject).HasMaxLength(450).IsRequired();

            builder.HasOne(topic => topic.Forum)
                .WithMany()
                .HasForeignKey(topic => topic.ForumId)
                .IsRequired();

            builder.HasOne(topic => topic.Customer)
               .WithMany()
               .HasForeignKey(topic => topic.CustomerId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore(topic => topic.ForumTopicType);

            base.Configure(builder);
        }

        #endregion
    }
}