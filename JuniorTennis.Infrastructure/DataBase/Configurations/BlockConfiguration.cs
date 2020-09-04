using JuniorTennis.Domain.DrawTables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class BlockConfiguration : IEntityTypeConfiguration<Block>
    {
        public void Configure(EntityTypeBuilder<Block> builder)
        {
            builder.ToTable("blocks");
            builder.HasOne(p => p.DrawTable)
                .WithMany(b => b.Blocks)
                .HasForeignKey(o => o.DrawTableId)
                .HasPrincipalKey(o => o.Id);
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.DrawTableId).HasSnakeCaseColumnName();
            builder.Property(o => o.BlockNumber)
                .HasConversion(o => o.Value, o => new BlockNumber(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.ParticipationClassification)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.GameDate)
                .HasConversion(o => o.Value, o => new GameDate(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.DrawSettingsId).HasSnakeCaseColumnName();
            builder.HasOne(o => o.DrawSettings);
        }
    }
}
