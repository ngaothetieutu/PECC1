﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GS.Core.Domain.Forums;

namespace GS.Data.Mapping.Forums
{
    /// <summary>
    /// Represents a forum mapping configuration
    /// </summary>
    public partial class ForumMap : GSEntityTypeConfiguration<Forum>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Forum> builder)
        {
            builder.ToTable(GSMappingDefaults.ForumTable);
            builder.HasKey(forum => forum.Id);

            builder.Property(forum => forum.Name).HasMaxLength(200).IsRequired();

            builder.HasOne(forum => forum.ForumGroup)
                .WithMany(forumGroup => forumGroup.Forums)
                .HasForeignKey(forum => forum.ForumGroupId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}