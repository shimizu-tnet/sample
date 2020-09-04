using System.Linq;
using JuniorTennis.Domain.RequestTeams;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.Domain.Teams;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class RequestTeamRepository : IRequestTeamRepository
    {
        private readonly JuniorTennisDbContext context;

        public RequestTeamRepository(JuniorTennisDbContext context) => this.context = context;

        public async Task<List<RequestTeam>> FindAllAsync() => 
            await this.context.RequestTeams
            .Include(o => o.Team)
            .Include(o => o.Season)
            .ToListAsync();

        public async Task<RequestTeam> FindByRequestTeamIdAsync(int requestTeamId) =>
            await this.context.RequestTeams
                .Where(o => o.Id == requestTeamId)
                .Include(o => o.Team)
                .FirstOrDefaultAsync();

        public async Task<List<RequestTeam>> FindBySeasonIdAsync(int seasonId) =>
            await this.context.RequestTeams
                .Where(o => o.SeasonId == seasonId)
                .Include(o => o.Team)
                .ToListAsync();

        public async Task<RequestTeam> FindByTeamIdAndSeasonId(int teamId, int seasonId) =>
            await this.context.RequestTeams
                .Where(o => o.TeamId == teamId)
                .Where(o => o.SeasonId == seasonId)
                .Include(o => o.Team)
                .FirstOrDefaultAsync();

        public async Task<RequestTeam> Update(RequestTeam entity)
        {
            this.context.Update(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public async Task<RequestTeam> Add(RequestTeam entity)
        {
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public async Task<Pagable<RequestTeam>> SearchAsync(RequestTeamSearchCondition condition)
        {
            var query = this.context.RequestTeams.AsQueryable();
            var totalCount = await condition.ApplyWithoutPagination(query).CountAsync();
            var requestTeams = await condition.Apply(query.Include(o => o.Team)).ToListAsync();
            return new Pagable<RequestTeam>(requestTeams, condition.PageIndex, totalCount, condition.DisplayCount);
        }
    }
}
