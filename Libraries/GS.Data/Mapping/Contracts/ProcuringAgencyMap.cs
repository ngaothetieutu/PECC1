using GS.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GS.Data.Mapping.Contracts
{
    public class ProcuringAgencyMap : GSEntityTypeConfiguration<ProcuringAgency>
    {
        public override void Configure(EntityTypeBuilder<ProcuringAgency> builder)
        {
            builder.ToTable(nameof(ProcuringAgency));
            builder.HasKey(t => t.Id);
            //dinh nghia lien ket foreikey
            builder.Ignore(t => t.procuringAgencyType);
            base.Configure(builder);
        }
    }
}
