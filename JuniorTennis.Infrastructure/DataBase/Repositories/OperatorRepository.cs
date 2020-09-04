using JuniorTennis.Domain.Operators;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class OperatorRepository : IOperatorRepository
    {
        private readonly JuniorTennisDbContext context;

        public OperatorRepository(JuniorTennisDbContext context) => this.context = context;

        public async Task<List<Operator>> FindAllAsync() => await this.context.Operators.ToListAsync();

        public async Task<Operator> AddAsync(Operator entity)
        {
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public async Task<Operator> FindByIdAsync(int id) => await this.context.Operators.FirstOrDefaultAsync(o => o.Id == id);

        public async Task<Operator> UpdateAsync(Operator entity)
        {
            this.context.Update(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }
    }
}
