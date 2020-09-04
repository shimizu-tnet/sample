using JuniorTennis.Domain.Tournaments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {
            builder.ToTable("tournaments");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.AggregationMonth)
                .HasConversion(o => o.Value, o => new AggregationMonth(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ApplicationPeriod)
               .HasConversion(o => o.ToJson(), o => ApplicationPeriod.FromJson(o))
               .HasSnakeCaseColumnName();
            builder.Property(o => o.EntryFee)
                .HasConversion(o => o.Value, o => new EntryFee(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.HoldingPeriod)
               .HasConversion(o => o.ToJson(), o => HoldingPeriod.FromJson(o))
               .HasSnakeCaseColumnName();
            builder.Property(o => o.Outline)
                .HasConversion(o => o.Value, o => new Outline(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.RegistrationYear)
                .HasConversion(o => o.Value, o => new RegistrationYear(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TournamentName)
                .HasConversion(o => o.Value, o => new TournamentName(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.Venue)
                .HasConversion(o => o.Value, o => new Venue(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.MethodOfPayment)
                .HasEnumerationConversion()
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TournamentType)
                .HasEnumerationConversion()
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TypeOfYear)
                .HasEnumerationConversion()
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TournamentEntryReceptionMailSubject).HasSnakeCaseColumnName();
            builder.Property(o => o.TournamentEntryReceptionMailBody).HasSnakeCaseColumnName();
        }
    }
}
