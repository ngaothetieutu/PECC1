﻿using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace GS.Data.Mapping.Contracts
{
    public class ContractAcceptanceSubMap : GSEntityTypeConfiguration<ContractAcceptanceSub>
    {
        public override void Configure(EntityTypeBuilder<ContractAcceptanceSub> builder)
        {
            builder.ToTable(nameof(ContractAcceptanceSub));
            builder.HasKey(t => t.Id);                      
            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.creator)
              .WithMany()
              .HasForeignKey(t => t.CreatorId)
              .IsRequired();            
            builder.HasOne(t => t.currency)
               .WithMany()
               .HasForeignKey(t => t.CurrencyId);            
            builder.HasOne(t => t.contract)
             .WithMany()
             .HasForeignKey(t => t.ContractId);
            builder.HasOne(t => t.contractAcceptance)
            .WithMany()
            .HasForeignKey(t => t.AcceptanceId);
            builder.HasOne(t => t.unit)
            .WithMany()
            .HasForeignKey(t => t.UnitId);
            builder.HasOne(t => t.task)
           .WithMany()
           .HasForeignKey(t => t.TaskId);
            //cac truong bo qua trong dinh nghia csdl  
            base.Configure(builder);
        }
    }
}
