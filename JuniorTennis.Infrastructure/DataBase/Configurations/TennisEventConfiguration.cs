using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class TennisEventConfiguration : IEntityTypeConfiguration<TennisEvent>
    {
        public void Configure(EntityTypeBuilder<TennisEvent> builder)
        {
            builder.ToTable("tennis_events");
            builder.HasKey(c => new { c.TournamentId, c.Category, c.Gender, c.Format });
            builder.HasOne(o => o.Tournament)
                .WithMany(o => o.TennisEvents)
                .HasForeignKey(o => o.TournamentId)
                .HasPrincipalKey(o => o.Id);
            builder.Property(o => o.TournamentId).HasSnakeCaseColumnName();
            builder.Property(o => o.Category)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.Gender)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.Format)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
        }
    }
}
