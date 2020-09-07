using JuniorTennis.Domain.DrawTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class DrawTableConfiguration : IEntityTypeConfiguration<DrawTable>
    {
        public void Configure(EntityTypeBuilder<DrawTable> builder)
        {
            builder.ToTable("draw_tables");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.TournamentId).HasSnakeCaseColumnName();
            builder.Property(o => o.TennisEventId).HasSnakeCaseColumnName();
            builder.Property(o => o.MainDrawSettingsId).HasSnakeCaseColumnName();
            builder.Property(o => o.QualifyingDrawSettingsId).HasSnakeCaseColumnName();
            builder.Property(o => o.TournamentFormat)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.EligiblePlayersType)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.HasMany(o => o.EntryDetails);
            builder.Property(o => o.EditStatus)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
        }
    }
}
