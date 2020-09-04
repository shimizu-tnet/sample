using System;
using System.Collections.Generic;
using System.Text;
using JuniorTennis.Domain.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class AuthorizationLinkConfiguration : IEntityTypeConfiguration<AuthorizationLink>
    {
        public void Configure(EntityTypeBuilder<AuthorizationLink> builder)
        {
            builder.ToTable("authorization_links");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.AuthorizationCode)
                .HasConversion(o => o.Value, o => new AuthorizationCode(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.UniqueKey).HasSnakeCaseColumnName();
            builder.Property(o => o.RegistrationDate).HasSnakeCaseColumnName();
        }
    }
    
}
