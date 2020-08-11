using System;
using System.Collections.Generic;
using System.Text;
using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ContractResourceMap : GSEntityTypeConfiguration<ContractResource>
    {
        public override void Configure(EntityTypeBuilder<ContractResource> builder)
        {
            builder.ToTable(nameof(ContractResource));
            builder.HasKey(t => t.Id);


            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.contract)
               .WithMany()
               .HasForeignKey(t => t.ContractId)
               .IsRequired();

            builder.HasOne(t => t.customer)
             .WithMany()
             .HasForeignKey(t => t.CustomerId)
             .IsRequired();

            builder.HasOne(t => t.contractRole)
             .WithMany()
             .HasForeignKey(t => t.RoleId)
             .IsRequired();
            builder.HasOne(t => t.unitInfo)
              .WithMany()
              .HasForeignKey(t => t.UnitId);

            builder.HasOne(t => t.groupRole)
            .WithMany()
            .HasForeignKey(t => t.GroupId);

            base.Configure(builder);
        }
    }
}
