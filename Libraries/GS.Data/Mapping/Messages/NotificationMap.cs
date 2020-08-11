using GS.Core.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Messages
{
    public class NotificationMap : GSEntityTypeConfiguration<Notification>
    {
        public override void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable(nameof(Notification));
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.receiver)
             .WithMany()
             .HasForeignKey(t => t.ReceiverId)
             .IsRequired();
            builder.HasOne(t => t.creator)
             .WithMany()
             .HasForeignKey(t => t.CreatorId)
             .IsRequired();

            builder.Ignore(t => t.notificationStatus);
            builder.Ignore(t => t.notificationType);
            base.Configure(builder);
        }
    }
}
