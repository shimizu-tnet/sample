using JuniorTennis.Domain.Announcements;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.ToTable("announcements");
            builder.Property(o => o.Id).HasSnakeCaseColumnName();
            builder.Property(o => o.AnnounceTitle)
                .HasConversion(o => o.Value, o => new AnnouncementTitle(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.Body).HasSnakeCaseColumnName();
            builder.Property(o => o.AnnouncementGenre)
                .HasSnakeCaseColumnName()
                .HasEnumerationConversion();
            builder.Property(o => o.RegisteredDate)
                .HasConversion(o => o.Value, o => new RegisteredDate(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.EndDate)
                .HasConversion(o => o.Value, o => new EndDate(o))
                .HasSnakeCaseColumnName();
            builder.Property(o => o.DeletedDateTime).HasSnakeCaseColumnName();
            builder.Property(o => o.AttachedFilePath)
                .HasConversion(o => o.Value, o => new AttachedFilePath(o))
                .HasSnakeCaseColumnName();
        }
    }
}
