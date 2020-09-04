using System;
using System.Collections.Generic;
using System.Text;
using JuniorTennis.Domain.Seasons;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> builder)
        {
            builder.ToTable("seasons");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.Name).HasSnakeCaseColumnName();
            builder.Property(o => o.FromDate).HasSnakeCaseColumnName();
            builder.Property(o => o.ToDate).HasSnakeCaseColumnName();
            builder.Property(o => o.RegistrationFromDate).HasSnakeCaseColumnName();
            builder.Property(o => o.TeamRegistrationFee)
                .HasConversion(o => o.Value, o => new TeamRegistrationFee(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerRegistrationFee)
                .HasConversion(o => o.Value, o => new PlayerRegistrationFee(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.PlayerTradeFee)
                .HasConversion(o => o.Value, o => new PlayerTradeFee(o))
                .HasSnakeCaseColumnName();
        }
    }
}
