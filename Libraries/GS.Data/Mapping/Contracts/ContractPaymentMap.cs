using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ContractPaymentMap : GSEntityTypeConfiguration<ContractPayment>
    {
        public override void Configure(EntityTypeBuilder<ContractPayment> builder)
        {
            builder.ToTable(nameof(ContractPayment));
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Code).IsRequired();

            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.contract)
             .WithMany()
             .HasForeignKey(t => t.ContractId);

            builder.HasOne(t => t.creator)
             .WithMany()
             .HasForeignKey(t => t.CreatorId)
             .IsRequired();

            builder.HasOne(t => t.approver)
               .WithMany()
               .HasForeignKey(t => t.ApproverId);


            builder.HasOne(t => t.currency)
              .WithMany()
              .HasForeignKey(t => t.CurrencyId)
              .IsRequired();

            builder.HasOne(t => t.unitInfo)
               .WithMany()
               .HasForeignKey(t => t.UnitId);

            builder.HasOne(t => t.procuringAgency)
               .WithMany()
               .HasForeignKey(t => t.ProcuringAgencyId);
            builder.HasOne(t => t.task)
              .WithMany()
              .HasForeignKey(t => t.TaskId);
            builder.HasOne(t => t.contractbb)
             .WithMany()
             .HasForeignKey(t => t.ContractIdBB);

            builder.HasOne(t => t.paymentExpenditure)
             .WithMany()
             .HasForeignKey(t => t.ExpenditureId);
            builder.Ignore(t => t.paymentStatus);
            builder.Ignore(t => t.paymentType);

            base.Configure(builder);
        }
    }
}
