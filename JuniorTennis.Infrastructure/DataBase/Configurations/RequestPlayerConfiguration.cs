using JuniorTennis.Domain.RequestPlayers;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.ReservationNumbers;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class RequestPlayerConfiguration : IEntityTypeConfiguration<RequestPlayer>
    {
        public void Configure(EntityTypeBuilder<RequestPlayer> builder)
        {
            builder.ToTable("request_players");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerId).HasSnakeCaseColumnName();
            builder.Property(o => o.TeamId).HasSnakeCaseColumnName();
            builder.Property(o => o.SeasonId).HasSnakeCaseColumnName();
            builder.Property(o => o.ReservationNumber)
                .HasConversion(o => o.Value, o => ReservationNumber.FromValue(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ReservationBranchNumber).HasSnakeCaseColumnName();
            builder.Property(o => o.Category)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.RequestType)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.ApproveState)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.RequestedDateTime).HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerRegistrationFee)
                .HasConversion(o => o.Value, o => new PlayerRegistrationFee(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ApproveDateTime).HasSnakeCaseColumnName();
            builder.HasOne(o => o.Season);
            builder.HasOne(o => o.Team);
            builder.HasOne(o => o.Player);
        }
    }
}
