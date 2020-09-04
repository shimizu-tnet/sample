using JuniorTennis.Domain.DrawTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class DrawSettingsConfiguration : IEntityTypeConfiguration<DrawSettings>
    {
        public void Configure(EntityTypeBuilder<DrawSettings> builder)
        {
            builder.ToTable("draw_settings");
            builder.Property(o => o.NumberOfBlocks)
                .HasConversion(o => o.Value, o => new NumberOfBlocks(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.NumberOfDraws)
                .HasConversion(o => o.Value, o => new NumberOfDraws(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.NumberOfEntries)
                .HasConversion(o => o.Value, o => new NumberOfEntries(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.NumberOfWinners)
                .HasConversion(o => o.Value, o => new NumberOfWinners(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TournamentGrade)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
        }
    }
}
