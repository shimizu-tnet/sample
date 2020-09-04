using JuniorTennis.Domain.Teams;
using JuniorTennis.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("teams");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.TeamCode)
                .HasConversion(o => o.Value, o => new TeamCode(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TeamType)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.TeamName)
                .HasConversion(o => o.Value, o => new TeamName(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.TeamAbbreviatedName)
                .HasConversion(o => o.Value, o => new TeamAbbreviatedName(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.RepresentativeName).HasSnakeCaseColumnName();
            builder.Property(o => o.RepresentativeEmailAddress).HasSnakeCaseColumnName();
            builder.Property(o => o.TelephoneNumber).HasSnakeCaseColumnName();
            builder.Property(o => o.Address).HasSnakeCaseColumnName();
            builder.Property(o => o.CoachName).HasSnakeCaseColumnName();
            builder.Property(o => o.CoachEmailAddress).HasSnakeCaseColumnName();
            builder.Property(o => o.TeamJpin).HasSnakeCaseColumnName();
        }
    }
}
