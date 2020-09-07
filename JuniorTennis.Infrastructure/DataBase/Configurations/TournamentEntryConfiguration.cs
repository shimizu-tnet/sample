using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Tournaments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class TournamentEntryConfiguration : IEntityTypeConfiguration<TournamentEntry>
    {
        public void Configure(EntityTypeBuilder<TournamentEntry> builder)
        {
            builder.ToTable("tournament_entry");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.ReservationNumber)
                .HasConversion(o => o.Value, o => new ReservationNumber(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ReservationDate)
                .HasConversion(o => o.Value, o => new ReservationDate(o))
                .HasSnakeCaseColumnName();
            builder.HasOne(o => o.EntryDetail);
            builder.Property(o => o.EntryFee)
                .HasConversion(o => o.Value, o => new EntryFee(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ReceiptStatus)
                .HasEnumerationConversion()
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ReceivedDate)
                .HasConversion(o => o.Value, o => new ReceivedDate(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ReceivedDate)
                .HasConversion(o => o.Value, o => new ReceivedDate(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.Applicant)
                .HasEnumerationConversion()
                .HasSnakeCaseColumnName();
        }
    }
}
