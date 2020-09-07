using JuniorTennis.Infrastructure.Identity;
using JuniorTennis.Mvc.Features.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JuniorTennis.Infrastructure.DataBase.Configurations
{
    public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationRole> builder)
        {
            builder.HasData(new ApplicationRole(AppRoleName.Administrator.Name) { NormalizedName = AppRoleName.Administrator.Name.ToUpper() });
            builder.HasData(new ApplicationRole(AppRoleName.TournamentCreator.Name) { NormalizedName = AppRoleName.TournamentCreator.Name.ToUpper() });
            builder.HasData(new ApplicationRole(AppRoleName.Recorder.Name) { NormalizedName = AppRoleName.Recorder.Name.ToUpper() });
            builder.HasData(new ApplicationRole(AppRoleName.Team.Name) { NormalizedName = AppRoleName.Team.Name.ToUpper() });
            builder.HasData(new ApplicationRole(AppRoleName.Developer.Name) { NormalizedName = AppRoleName.Developer.Name.ToUpper() });
        }
    }
}
