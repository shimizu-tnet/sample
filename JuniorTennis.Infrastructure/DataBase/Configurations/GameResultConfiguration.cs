using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.TournamentEntries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class GameResultConfiguration : IEntityTypeConfiguration<GameResult>
    {
        public void Configure(EntityTypeBuilder<GameResult> builder)
        {
            builder.ToTable("game_results");
            builder.HasOne(p => p.Game)
                .WithOne(b => b.GameResult);
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.GameStatus)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.PlayerClassificationOfWinner)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.EntryNumberOfWinner)
                 .HasConversion(o => o.Value, o => new EntryNumber(o))
                 .HasSnakeCaseColumnName();
            builder.Property(o => o.GameScore)
                .HasConversion(o => o.Value, o => new GameScore(o))
                .HasSnakeCaseColumnName();
        }
    }
}
