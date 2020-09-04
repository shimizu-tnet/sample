using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Infrastructure.DataBase;
using System;
using System.Linq;

namespace JuniorTennis.Mvc.DummyData
{
    public static class TournamentsDummy
    {
        static TournamentsDummy() { }
        public static void Create(JuniorTennisDbContext context)
        {
            try
            {
                if (context.Tournaments.Any())
                {
                    return;
                }

                for (var i = 0; i < 10; i++)
                {
                    var tournament = new Tournament(
                        new TournamentName($"大阪府代表選考会 {i:000}"),
                        TournamentType.WithDraw,
                        new RegistrationYear(new DateTime(2020, 4, 1)),
                        TypeOfYear.Odd,
                        new AggregationMonth(new DateTime(2020, 8, 1)),
                        TennisEvent.GetAllEvents().Select(o => o.Value).ToList(),
                        new HoldingPeriod(new DateTime(2020, 8, 1), new DateTime(2020, 8, 31)),
                        Enumerable.Range(1, 31)
                            .Select(o => new HoldingDate(new DateTime(2020, 8, o)))
                            .Where(o => o.Value.DayOfWeek == DayOfWeek.Saturday || o.Value.DayOfWeek == DayOfWeek.Sunday)
                            .ToList(),
                        new Venue($"大阪スタジアム {i:000}"),
                        new EntryFee(1000 + i),
                        MethodOfPayment.PrePayment,
                        new ApplicationPeriod(new DateTime(2020, 7, 25), new DateTime(2020, 7, 31)),
                        new Outline($"大会要領 {i:000}"),
                        "",
                        ""
                    );

                    context.Tournaments.Add(tournament);
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
