using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.TournamentEntries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class EntryDetailConfiguration : IEntityTypeConfiguration<EntryDetail>
    {
        public void Configure(EntityTypeBuilder<EntryDetail> builder)
        {
            builder.ToTable("entry_details");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.EntryNumber)
                .HasConversion(o => o.Value, o => new EntryNumber(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ParticipationClassification)
                .HasEnumerationConversion()
                .HasSnakeCaseColumnName();
            builder.Property(o => o.SeedNumber)
                .HasConversion(o => o.Value, o => new SeedNumber(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.CanParticipationDates)
                .HasConversion(o => o.ToJson(), o => CanParticipationDates.FromJson(o))
                .HasSnakeCaseColumnName()
                .Metadata
                .SetValueComparer(ValueComparerFactory.CreateListComparer<CanParticipationDates, CanParticipationDate>());
            builder.Property(o => o.ReceiptStatus)
                .HasEnumerationConversion()
                .HasSnakeCaseColumnName();
            builder.Property(o => o.UsageFeatures)
                .HasEnumerationConversion()
                .HasSnakeCaseColumnName();
            builder.Property(o => o.FromQualifying)
                .HasSnakeCaseColumnName();
            builder.Property(o => o.BlockNumber)
                .HasConversion(o => o.Value, o => new BlockNumber(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TournamentEntryId).HasSnakeCaseColumnName();
            builder.Property(o => o.DrawTableId).HasSnakeCaseColumnName();
        }
    }
}
