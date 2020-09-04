using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.Operators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class OperatorConfiguration : IEntityTypeConfiguration<Operator>
    {
        public void Configure(EntityTypeBuilder<Operator> builder)
        {
            builder.ToTable("operators");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.Name).HasSnakeCaseColumnName();
            builder.Property(o => o.EmailAddress)
                .HasConversion(o => o.Value, o => new EmailAddress(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.LoginId)
                .HasConversion(o => o.Value, o => new LoginId(o))
                .HasSnakeCaseColumnName();
        }
    }
}
