using JuniorTennis.Domain.UseCases.Announcements;
using JuniorTennis.Domain.UseCases.DrawTables;
using JuniorTennis.Domain.UseCases.Identity.Accounts;
using JuniorTennis.Domain.UseCases.Operators;
using JuniorTennis.Domain.UseCases.Players;
using JuniorTennis.Domain.UseCases.Seasons;
using JuniorTennis.Domain.UseCases.Teams;
using JuniorTennis.Domain.UseCases.TournamentEntries;
using JuniorTennis.Domain.UseCases.Tournaments;
using Microsoft.Extensions.DependencyInjection;

namespace JuniorTennis.Mvc.Configurations
{
    public static class ApplicationModule
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITournamentUseCase, TournamentUseCase>();
            services.AddScoped<ITeamUseCase, TeamUseCase>();
            services.AddScoped<IDrawTableUseCase, DrawTableUseCase>();
            services.AddScoped<IAnnouncementUseCase, AnnouncementUseCase>();
            services.AddScoped<IAccountsUseCase, AccountsUseCase>();
            services.AddScoped<ISeasonUseCase, SeasonUseCase>();
            services.AddScoped<IOperatorUseCase, OperatorUseCase>();
            services.AddScoped<IPlayerUseCase, PlayerUseCase>();
            services.AddScoped<IAuthorizationUseCase, AuthorizationUseCase>();
            services.AddScoped<ITournamentEntryUseCase, TournamentEntryUseCase>();
        }
    }
}
