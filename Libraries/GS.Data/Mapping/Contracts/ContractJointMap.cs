using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class ContractJointMap : GSEntityTypeConfiguration<ContractJoint>
    {
        public override void Configure(EntityTypeBuilder<ContractJoint> builder)
        {
            builder.ToTable(nameof(ContractJoint));
            builder.HasKey(t => t.Id);


            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.contract)
             .WithMany()
             .HasForeignKey(t => t.ContractId)
             .IsRequired();

            builder.HasOne(t => t.curProcuringAgency)
            .WithMany()
            .HasForeignKey(t => t.ProcuringAgencyId)
            .IsRequired();

            base.Configure(builder);
        }
    }
}
