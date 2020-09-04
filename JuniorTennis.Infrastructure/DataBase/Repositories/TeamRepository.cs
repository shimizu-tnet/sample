using System.Linq;
using JuniorTennis.Domain.Teams;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using JuniorTennis.Domain.UseCases.Shared;
using System.Collections.ObjectModel;
using JuniorTennis.Domain.QueryConditions;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly JuniorTennisDbContext context;

        public TeamRepository(JuniorTennisDbContext context) => this.context = context;

        public async Task<List<Team>> FindAsync() => await this.context.Teams.ToListAsync();

        public async Task<Team> FindByCodeAsync(TeamCode teamCode) =>
            await this.context.Teams
                .Where(o => o.TeamCode == teamCode)
                .FirstOrDefaultAsync();

        public async Task<Team> FindByIdAsync(int teamId) =>
            await this.context.Teams
                .Where(o => o.Id == teamId)
                .FirstOrDefaultAsync();

        public async Task<Team> Update(Team team)
        {
            this.context.Update(team);
            await this.context.SaveChangesAsync();
            return team;
        }

        /// <summary>
        /// 団体を新規登録申請します。
        /// </summary>
        /// <param name="entity">団体</param>
        /// <returns>団体</returns>
        public async Task<Team> Add(Team entity)
        {
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public async Task<Pagable<Team>> SearchAsync(TeamSearchCondition condition)
        {
            var query = this.context.Teams.AsQueryable();
            var totalCount = await condition.ApplyWithoutPagination(query).CountAsync();
            var teams = await condition.Apply(query).ToListAsync();
            return new Pagable<Team>(teams, condition.PageIndex, totalCount, condition.DisplayCount);
        }
    }
}
