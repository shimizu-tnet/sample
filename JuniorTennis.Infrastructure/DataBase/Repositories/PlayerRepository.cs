using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.RequestPlayers;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly JuniorTennisDbContext context;

        public PlayerRepository(JuniorTennisDbContext context) => this.context = context;

        public async Task<Player> FindByPlayerCodeAsync(PlayerCode playerCode) =>
            await this.context.Players.FirstOrDefaultAsync(o => o.PlayerCode == playerCode);

        public async Task<List<Player>> FindUnrequestedAllByTeamIdWithoutPlayerCode(int teamId)
        {
            // EFCoreでLeft Joinを行う場合、GroupJoin→DefaultIfEmptyだとInvalidOperationExceptionが発生する
            // そのためGroupJoin→SelectMany→DefaultIfEmptyで結合を行う必要がある
            return await this.context.Set<Player>()
                .Where(o => o.TeamId == teamId)
                .Where(o => o.PlayerCode == null)
                .GroupJoin(
                    this.context.Set<RequestPlayer>(),
                    player => player.Id,
                    request => request.PlayerId,
                    (player, request) => new
                    {
                        Player = player,
                        RequestPlayers = request
                    })
                .SelectMany(
                    o => o.RequestPlayers.DefaultIfEmpty(),
                    (player, request) => new { player.Player, Request = request }
                )
                .Where(o => o.Request == null)
                .Select(o => o.Player)
                .ToListAsync();
        }

        public async Task<Player> FindByIdAsync(int playerId) =>
            await this.context.Players.FirstOrDefaultAsync(o => o.Id == playerId);

        public async Task<List<Player>> FindAllByIdsAsync(List<int> playerIds) =>
            await this.context.Players
                .Where(o => playerIds.Contains(o.Id))
                .ToListAsync();

        public async Task<bool> ExistsByNameAndBirtDateAsync(PlayerFamilyName playerFamilyName, PlayerFirstName playerFirstName, BirthDate birthDate) =>
            await this.context.Players
                .Where(o => o.PlayerFamilyName == playerFamilyName)
                .Where(o => o.PlayerFirstName == playerFirstName)
                .Where(o => o.BirthDate == birthDate)
                .AnyAsync();

        public async Task<Player> UpdateAsync(Player player)
        {
            this.context.Update(player);
            await this.context.SaveChangesAsync();
            return player;
        }

        public async Task<Player> AddAsync(Player player)
        {
            await this.context.AddAsync(player);
            await this.context.SaveChangesAsync();
            return player;
        }

        public async Task DeleteAsync(Player player)
        {
            this.context.Remove(player);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<Player>> SearchAsync(PlayerSearchCondition condition)
        {
            var query = this.context.Players.AsQueryable();
            return await condition.Apply(query).ToListAsync();
        }

        public async Task<Pagable<Player>> SearchPagedListAsync(PlayerSearchCondition condition, int seasonId)
        {
            var query = this.context.Players.AsQueryable();
            var totalCount = 0;
            var players = new List<Player>();
            if (seasonId == 0)
            {
                totalCount = await condition.ApplyWithoutPagination(query).CountAsync();
                players = await condition.Apply(query.Include(o => o.Team)).ToListAsync();
                return new Pagable<Player>(players, condition.PageIndex, totalCount, condition.DisplayCount);
            }

            query = query.GroupJoin(
                this.context.Set<RequestPlayer>(),
                player => player.Id,
                request => request.PlayerId,
                (player, request) => new
                {
                    Player = player,
                    RequestPlayers = request
                })
            .SelectMany(
                o => o.RequestPlayers.DefaultIfEmpty(),
                (player, request) => new { player.Player, Request = request }
            )
            .Where(o => o.Request.SeasonId == seasonId)
            .Select(o => o.Player);

            totalCount = await condition.ApplyWithoutPagination(query).CountAsync();
            players = await condition.Apply(query.Include(o => o.Team)).ToListAsync();
            return new Pagable<Player>(players, condition.PageIndex, totalCount, condition.DisplayCount);

        }

        public async Task<List<Player>> SearchListAsync(PlayerSearchCondition condition, int seasonId)
        {
            var query = this.context.Players.AsQueryable();
            if (seasonId == 0)
            {
                return await condition.Apply(query.Include(o => o.Team)).ToListAsync();
            }
            query = query.GroupJoin(
                this.context.Set<RequestPlayer>(),
                player => player.Id,
                request => request.PlayerId,
                (player, request) => new
                {
                    Player = player,
                    RequestPlayers = request
                })
            .SelectMany(
                o => o.RequestPlayers.DefaultIfEmpty(),
                (player, request) => new { player.Player, Request = request }
            )
            .Where(o => o.Request.SeasonId == seasonId)
            .Select(o => o.Player);

            return await condition.Apply(query.Include(o => o.Team)).ToListAsync();
        }
    }
}
