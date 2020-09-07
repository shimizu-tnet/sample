using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Ranking;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.TournamentEntries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class EntryPlayerConfiguration : IEntityTypeConfiguration<EntryPlayer>
    {
        public void Configure(EntityTypeBuilder<EntryPlayer> builder)
        {
            builder.ToTable("entry_players");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.HasOne(o => o.EntryDetail);
            builder.Property(o => o.EntryDetailId).HasSnakeCaseColumnName();
            builder.HasOne(o => o.EntryDetail)
                .WithMany(o => o.EntryPlayers)
                .HasForeignKey(o => o.EntryDetailId)
                .HasPrincipalKey(o => o.Id);
            builder.Property(o => o.TeamCode)
                .HasConversion(o => o.Value, o => new TeamCode(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TeamName)
                .HasConversion(o => o.Value, o => new TeamName(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TeamAbbreviatedName)
                .HasConversion(o => o.Value, o => new TeamAbbreviatedName(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerCode)
                .HasConversion(o => o.Value, o => new PlayerCode(o))
                .HasSnakeCaseColumnName();
            builder.Ignore(o => o.PlayerName);
            builder.Property(o => o.PlayerFamilyName)
                .HasConversion(o => o.Value, o => new PlayerFamilyName(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerFirstName)
                .HasConversion(o => o.Value, o => new PlayerFirstName(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.Point)
                .HasConversion(o => o.Value, o => new Point(o))
                .HasSnakeCaseColumnName();
        }
    }
}
