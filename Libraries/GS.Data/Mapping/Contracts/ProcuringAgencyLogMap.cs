using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ProcuringAgencyLogMap : GSEntityTypeConfiguration<ProcuringAgencyLog>
    {
        public override void Configure(EntityTypeBuilder<ProcuringAgencyLog> builder)
        {
            builder.ToTable(nameof(ProcuringAgencyLog));
            builder.HasKey(t => t.Id);


            //dinh nghia lien ket foreikey
            builder.HasOne(t => t.creator)
              .WithMany()
              .HasForeignKey(t => t.CreatorId)
              .IsRequired();

            base.Configure(builder);
        }

    }
}
