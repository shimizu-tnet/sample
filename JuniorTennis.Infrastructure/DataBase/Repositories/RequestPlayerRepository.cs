using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.RequestPlayers;
using JuniorTennis.Domain.RequestTeams;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class RequestPlayerRepository : IRequestPlayerRepository
    {
        private readonly JuniorTennisDbContext context;

        public RequestPlayerRepository(JuniorTennisDbContext context) => this.context = context;

        public async Task<List<RequestPlayer>> FindAllByTeamIdAndSeasonId(int teamId, int seasonId) =>
            await this.context.RequestPlayers
                .Where(o => o.TeamId == teamId)
                .Where(o => o.SeasonId == seasonId)
                .Include(o => o.Team)
                .Include(o => o.Player)
                .ToListAsync();

        public async Task<List<RequestPlayer>> FindRequestingAsync(int teamId, int seasonId) =>
            await this.context.RequestPlayers
                .Where(o => o.TeamId == teamId)
                .Where(o => o.SeasonId == seasonId)
                .Where(o => o.ApproveState == ApproveState.Unapproved)
                .Include(o => o.Team)
                .Include(o => o.Player)
                .ToListAsync();

        public async Task<bool> ExistsInOtherTeamAsync(int teamId, int playerId) =>
            await this.context.RequestPlayers
                .Where(o => o.TeamId != teamId)
                .Where(o => o.PlayerId != playerId)
                .AnyAsync();

        public async Task<List<RequestPlayer>> FindAllByPlayerIdsAndSeasonId(List<int> playerIds, int seasonId) =>
            await this.context.RequestPlayers
                .Where(o => playerIds.Contains(o.PlayerId))
                .Where(o => o.SeasonId == seasonId)
                .ToListAsync();

        public async Task<RequestPlayer> UpdateAsync(RequestPlayer requestPlayer)
        {
            this.context.Update(requestPlayer);
            await this.context.SaveChangesAsync();
            return requestPlayer;
        }

        public async Task<RequestPlayer> AddAsync(RequestPlayer requestPlayer)
        {
            await this.context.AddAsync(requestPlayer);
            await this.context.SaveChangesAsync();
            return requestPlayer;
        }

        public async Task<List<RequestPlayer>> SearchAsync(RequestPlayerSearchCondition condition)
        {
            var query = this.context.RequestPlayers.AsQueryable().Include(o => o.Team).Include(o => o.Player);
            return await condition.Apply(query).ToListAsync();
        }
    }    
}
