using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using JuniorTennis.Domain.Seasons;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly JuniorTennisDbContext context;
        public SeasonRepository(JuniorTennisDbContext context) => this.context = context;

        public async Task<List<Season>> FindByRegistrationPeriod(DateTime date) =>
            await this.context.Seasons
                .Where(o => o.RegistrationFromDate <= date && o.ToDate >= date)
                .ToListAsync();

        public async Task<Season> FindByDate(DateTime date) =>
            await this.context.Seasons
                .Where(o => o.FromDate <= date.Date && o.ToDate >= date.Date)
                .FirstOrDefaultAsync();

        public async Task<Season> Add(Season entity)
        {
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Season>> FindAll() => await this.context.Seasons.ToListAsync();

        public async Task<Season> FindById(int id)
        {
            return await this.context.Seasons.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Season> Update(Season entity)
        {
            this.context.Update(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }
    }
}
