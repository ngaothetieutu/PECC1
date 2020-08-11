using GS.Core.Domain.Works;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Works
{
    public class TaskContractMap : GSEntityTypeConfiguration<TaskContract>
    {
        public override void Configure(EntityTypeBuilder<TaskContract> builder)
        {
            builder.ToTable(nameof(TaskContract));
            builder.HasKey(t => t.Id);
            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.task)
               .WithMany(c => c.TaskContracts)
               .HasForeignKey(t => t.TaskId)
               .IsRequired();           
            builder.HasOne(t => t.creator)
             .WithMany()
             .HasForeignKey(t => t.CreatorId)
             .IsRequired();           
            builder.HasOne(t => t.contract)
             .WithMany()
             .HasForeignKey(t => t.ContractId)
             .IsRequired();          

            base.Configure(builder);
        }
     
    }
}
