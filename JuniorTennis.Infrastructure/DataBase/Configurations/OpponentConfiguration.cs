using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.TournamentEntries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class OpponentConfiguration : IEntityTypeConfiguration<Opponent>
    {
        public void Configure(EntityTypeBuilder<Opponent> builder)
        {
            builder.ToTable("opponents");
            builder.HasOne(p => p.Game)
                .WithMany(b => b.Opponents)
                .HasForeignKey(o => o.GameId)
                .HasPrincipalKey(o => o.Id);
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.GameId).HasSnakeCaseColumnName();
            builder.Property(o => o.BlockNumber)
                .HasConversion(o => o.Value, o => new BlockNumber(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.GameNumber)
                .HasConversion(o => o.Value, o => new GameNumber(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.DrawNumber)
                .HasConversion(o => o.Value, o => new DrawNumber(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerClassification)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.SeedLevel)
                .HasConversion(o => o.Value, o => new SeedLevel(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.AssignOrder)
                .HasConversion(o => o.Value, o => new AssignOrder(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.FramePlayerClassification)
                .HasEnumerationConversion()
                .HasSnakeCaseColumnName();
            builder.Property(o => o.IsManuallySettingFrame).HasSnakeCaseColumnName();
            builder.Property(o => o.IsManuallyAssigned).HasSnakeCaseColumnName();
            builder.Property(o => o.EntryNumber)
                    .HasConversion(o => o.Value, o => new EntryNumber(o))
                    .HasSnakeCaseColumnName();
            builder.Property(o => o.SeedNumber)
                .HasConversion(o => o.Value, o => new SeedNumber(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TeamCodes)
                .HasConversion(o => o.ToJson(), o => TeamCodes.FromJson(o))
                .HasSnakeCaseColumnName()
                .Metadata
                .SetValueComparer(ValueComparerFactory.CreateListComparer<TeamCodes, TeamCode>());
            builder.Property(o => o.TeamAbbreviatedNames)
                .HasConversion(o => o.ToJson(), o => TeamAbbreviatedNames.FromJson(o))
                .HasSnakeCaseColumnName()
                .Metadata
                .SetValueComparer(ValueComparerFactory.CreateListComparer<TeamAbbreviatedNames, TeamAbbreviatedName>());
            builder.Property(o => o.PlayerCodes)
                .HasConversion(o => o.ToJson(), o => PlayerCodes.FromJson(o))
                .HasSnakeCaseColumnName()
                .Metadata
                .SetValueComparer(ValueComparerFactory.CreateListComparer<PlayerCodes, PlayerCode>());
            builder.Property(o => o.PlayerNames)
                .HasConversion(o => o.ToJson(), o => PlayerNames.FromJson(o))
                .HasSnakeCaseColumnName()
                .Metadata
                .SetValueComparer(ValueComparerFactory.CreateListComparer<PlayerNames, PlayerName>());
            builder.Property(o => o.FromGameNumber)
                .HasConversion(o => o.Value, o => new GameNumber(o))
                .HasSnakeCaseColumnName();
        }
    }
}
