using System;
using System.Collections.Generic;
using System.Text;
using JuniorTennis.Domain.ReservationNumbers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class ReservationNumberConfiguration : IEntityTypeConfiguration<ReservationNumber>
    {
        public void Configure(EntityTypeBuilder<ReservationNumber> builder)
        {
            builder.ToTable("reservation_numbers");
            builder.HasKey(o => new { o.RegistratedDate, o.SerialNumber });
            builder.Ignore(o => o.Value);
            builder.Property(o => o.RegistratedDate).HasSnakeCaseColumnName();
            builder.Property(o => o.SerialNumber).HasSnakeCaseColumnName();
        }
    }
}
