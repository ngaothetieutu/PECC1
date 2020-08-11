using System;
using System.Collections.Generic;
using System.Text;
using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace GS.Data.Mapping.Contracts
{
    public class ContractRelateMap : GSEntityTypeConfiguration<ContractRelate>
    {
        public override void Configure(EntityTypeBuilder<ContractRelate> builder)
        {
            builder.ToTable(nameof(ContractRelate));
            builder.HasKey(t => t.Id);


            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.contract1)
            .WithMany()
            .HasForeignKey(t => t.Contract1Id)
            .IsRequired();
            builder.HasOne(t => t.contract2)
            .WithMany()
            .HasForeignKey(t => t.Contract2Id)
            .IsRequired();

            base.Configure(builder);
        }
    }
}
