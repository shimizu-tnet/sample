using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Ranking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class RankingRepository : IRankingRepository
    {
        private readonly JuniorTennisDbContext context;
        public RankingRepository(JuniorTennisDbContext context) => this.context = context;

        public async Task<List<EarnedPoints>> FindByTournamentEvent(int tournamentId, string tennisEventId)
        {
            var tournaments = await this.context.Tournaments.ToListAsync();
            var tournament = tournaments.FirstOrDefault(o => o.Id == tournamentId);
            var random = new Random();
            var earnedPoints = new List<EarnedPoints>();
            Enumerable.Range(1, 30).ToList().ForEach(o =>
            {
                earnedPoints.AddRange(Enumerable.Range(1, 10).Select(p =>
                {
                    return new EarnedPoints(
                        tournamentId: tournamentId,
                        tennisEventId: tennisEventId,
                        playerCode: new PlayerCode($"{o:000}{p:000}A"),
                        point: new Point(random.Next(500, 9999)));
                }));
            });
            Enumerable.Range(1, 30).ToList().ForEach(o =>
            {
                earnedPoints.AddRange(Enumerable.Range(1, 10).Select(p =>
                {
                    return new EarnedPoints(
                        tournamentId: tournamentId,
                        tennisEventId: tennisEventId,
                        playerCode: new PlayerCode($"{o:000}{p:000}B"),
                        point: new Point(random.Next(500, 9999)));
                }));
            });
            return earnedPoints;
        }
    }
}
