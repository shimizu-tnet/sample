using JuniorTennis.Domain.QueryConditions;
using JuniorTennis.Domain.Tournaments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    /// <summary>
    /// 大会管理。
    /// </summary>
    public class TournamentRepository : ITournamentRepository
    {
        private readonly JuniorTennisDbContext context;
        public TournamentRepository(JuniorTennisDbContext context) => this.context = context;

        /// <summary>
        /// 大会一覧を取得します。
        /// </summary>
        /// <returns>大会一覧。</returns>
        public async Task<List<Tournament>> Find() =>
            await this.context.Tournaments
                    .Include(o => o.TennisEvents)
                    .Include(o => o.HoldingDates)
                    .ToListAsync();

        /// <summary>
        /// 新規に大会を登録します。
        /// </summary>
        /// <param name="entity">大会。</param>
        /// <returns>大会。</returns>
        public async Task<Tournament> Add(Tournament entity)
        {
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// 大会を取得します。
        /// </summary>
        /// <param name="id">大会 ID。</param>
        /// <returns>大会。</returns>
        public async Task<Tournament> FindById(int id)
        {
            var tournaments = await this.Find();
            return tournaments.FirstOrDefault(o => o.Id == id);
        }

        /// <summary>
        /// 大会を更新します。
        /// </summary>
        /// <param name="entity">大会。</param>
        /// <returns>大会。</returns>
        public async Task<Tournament> Update(Tournament entity)
        {
            var removeHoldingDates = this.context.HoldingDates
                .Where(o => o.TournamentId == entity.Id)
                .AsEnumerable();
            this.context.HoldingDates
                .RemoveRange(removeHoldingDates);

            var removeTennisEvents = this.context.TennisEvents
                .Where(o => o.TournamentId == entity.Id)
                .AsEnumerable();
            this.context.TennisEvents
                .RemoveRange(removeTennisEvents);

            this.context.Update(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// 大会を削除します。
        /// </summary>
        /// <param name="entity">大会。</param>
        public async Task Delete(Tournament entity)
        {
            this.context.Tournaments.Remove(entity);
            await this.context.SaveChangesAsync();
        }

        public Task<Tournament> FindByRegistrationYear(DateTime registrationYear) => throw new NotImplementedException();

        public async Task<List<Tournament>> SearchAsync(SearchCondition<Tournament> condition)
        {
            var query = this.context.Tournaments
                    .Include(o => o.TennisEvents)
                    .Include(o => o.HoldingDates);
            return await condition.Apply(query).ToListAsync();
        }
    }
}
