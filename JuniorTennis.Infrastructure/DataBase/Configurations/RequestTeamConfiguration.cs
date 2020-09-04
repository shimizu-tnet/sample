using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.ReservationNumbers;
using JuniorTennis.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class RequestTeamConfiguration : IEntityTypeConfiguration<RequestTeam>
    {
        public void Configure(EntityTypeBuilder<RequestTeam> builder)
        {
            builder.ToTable("request_teams");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.TeamId).HasSnakeCaseColumnName();
            builder.Property(o => o.SeasonId).HasSnakeCaseColumnName();
            builder.Property(o => o.ReservationNumber)
                .HasConversion(o => o.Value, o => ReservationNumber.FromValue(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ApproveState)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.RequestedDateTime).HasSnakeCaseColumnName();
            builder.Property(o => o.RequestedFee)
                .HasConversion(o => o.Value, o => new RequestedFee(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ApproveDateTime).HasSnakeCaseColumnName();
            builder.Property(o => o.MailState)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.HasOne(o => o.Season);
            builder.HasOne(o => o.Team);
        }
    }
}
