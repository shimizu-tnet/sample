using JuniorTennis.Domain.Tournaments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class HoldingDatesConfiguration : IEntityTypeConfiguration<HoldingDate>
    {
        public void Configure(EntityTypeBuilder<HoldingDate> builder)
        {
            builder.ToTable("holding_dates");
            builder.HasKey(o => new { o.TournamentId, o.Value });
            builder.HasOne(o => o.Tournament)
                .WithMany(o => o.HoldingDates)
                .HasForeignKey(o => o.TournamentId)
                .HasPrincipalKey(o => o.Id);
            builder.Property(o => o.TournamentId).HasSnakeCaseColumnName();
            builder.Property(o => o.Value).HasSnakeCaseColumnName();
        }
    }
}
