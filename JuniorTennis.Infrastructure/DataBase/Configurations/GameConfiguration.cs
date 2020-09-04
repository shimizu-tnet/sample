using JuniorTennis.Domain.DrawTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.ToTable("games");
            builder.HasOne(p => p.Block)
                .WithMany(b => b.Games)
                .HasForeignKey(o => o.BlockId)
                .HasPrincipalKey(o => o.Id);
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.BlockId).HasSnakeCaseColumnName();
            builder.Property(o => o.GameNumber)
                 .HasConversion(o => o.Value, o => new GameNumber(o))
                 .HasSnakeCaseColumnName();
            builder.Property(o => o.RoundNumber)
                 .HasConversion(o => o.Value, o => new RoundNumber(o))
                 .HasSnakeCaseColumnName();
            builder.Property(o => o.DrawSettingsId).HasSnakeCaseColumnName();
            builder.HasOne(o => o.DrawSettings);
        }
    }
}
