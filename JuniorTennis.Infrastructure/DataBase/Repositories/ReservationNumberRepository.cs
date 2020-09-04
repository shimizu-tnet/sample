using System;
using System.Linq;
using JuniorTennis.Domain.Teams;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using JuniorTennis.Domain.ReservationNumbers;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class ReservationNumberRepository : IReservationNumberRepository
    {
        private readonly JuniorTennisDbContext context;
        public ReservationNumberRepository(JuniorTennisDbContext context) => this.context = context;

        public async Task<ReservationNumber> Max() =>
            await this.context.ReservationNumbers
                .Where(o => o.RegistratedDate == DateTime.Today)
                .OrderByDescending(o => o.SerialNumber)
                .FirstOrDefaultAsync();

        public async Task<ReservationNumber> Add(ReservationNumber entity)
        {
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }
    }
}
