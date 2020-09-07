using JuniorTennis.Domain.Players;
using JuniorTennis.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.ToTable("players");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.TeamId).HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerCode)
                .HasConversion(o => o.Value, o => new PlayerCode(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerFamilyName)
               .HasConversion(o => o.Value, o => new PlayerFamilyName(o))
               .HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerFirstName)
               .HasConversion(o => o.Value, o => new PlayerFirstName(o))
               .HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerFamilyNameKana)
               .HasConversion(o => o.Value, o => new PlayerFamilyNameKana(o))
               .HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerFirstNameKana)
               .HasConversion(o => o.Value, o => new PlayerFirstNameKana(o))
               .HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerJpin).HasSnakeCaseColumnName();
            builder.Property(o => o.Category)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.Gender)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.BirthDate)
               .HasConversion(o => o.Value, o => new BirthDate(o))
               .HasSnakeCaseColumnName();
            builder.Property(o => o.TelephoneNumber).HasSnakeCaseColumnName();
            builder.HasOne(o => o.Team);
            builder.Ignore(o => o.PlayerName);
            builder.Ignore(o => o.PlayerNameKana);
        }
    }
}
