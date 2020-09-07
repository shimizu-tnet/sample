using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Ranking;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class TournamentEntryRepository : ITournamentEntryRepository
    {
        private readonly JuniorTennisDbContext context;
        public TournamentEntryRepository(JuniorTennisDbContext context) => this.context = context;

        public Task<List<TournamentEntry>> FindAllAsync()
        {
            return this.context.TournamentEntries
                .Include(o => o.EntryDetail)
                .ThenInclude(o => o.EntryPlayers)
                .Where(o => o.EntryDetail.UsageFeatures == UsageFeatures.TournamentEntry)
                .ToListAsync();
        }

        public async Task<List<TournamentEntry>> FindByIdAsync(int tournamentId, string tennisEventId)
        {
            #region Variables
            var tournament = await this.context.Tournaments
                .Where(o => o.Id == tournamentId)
                .Include(o => o.TennisEvents)
                .Include(o => o.HoldingDates)
                .FirstOrDefaultAsync();
            var isSingled = TennisEvent.FromId(tennisEventId).IsSingles;
            var r = new Random();
            #endregion Variables

            #region Create EarnedPoints
            var earnedPoints = new List<EarnedPoints>();
            Enumerable.Range(1, 30).ToList().ForEach(o =>
            {
                var earnedPointsA = Enumerable
                    .Range(1, 10)
                    .Select(p => new EarnedPoints(
                        tournamentId,
                        tennisEventId,
                        playerCode: new PlayerCode($"{o:00}{p:00}A"),
                        point: new Point(r.Next(500, 9999))));
                earnedPoints.AddRange(earnedPointsA);

                var earnedPointsB = Enumerable
                    .Range(1, 10)
                    .Select(p => new EarnedPoints(
                        tournamentId,
                        tennisEventId,
                        playerCode: new PlayerCode($"{o:00}{p:00}B"),
                        point: new Point(r.Next(500, 9999))));
                earnedPoints.AddRange(earnedPointsB);
            });
            #endregion Create EarnedPoints

            #region Create TournamentEntry
            var tournamentEntries = new List<TournamentEntry>();
            for (var i = 1; i <= 30; i++)
            {
                for (var j = 1; j <= 10; j++)
                {
                    #region Create EntryPlayer
                    var entryPlayers = new List<EntryPlayer>();
                    for (var k = 0; k < (isSingled ? 1 : 2); k++)
                    {
                        var point = earnedPoints
                            .Where(p => p.TennisEventId == tennisEventId)
                            .Where(p => p.PlayerCode.Value == $"{i:00}{j:00}{(k == 0 ? "A" : "B")}")
                            .Sum(o => o.Point.Value);

                        var entryPlayer = new EntryPlayer(
                            teamCode: new TeamCode($"TEAM{i:00}{j:00}"),
                            teamName: new TeamName($"チーム{i:00}{j:00}"),
                            teamAbbreviatedName: new TeamAbbreviatedName($"略称{i:00}{j:00}"),
                            playerCode: new PlayerCode($"{i:00}{j:00}{(k == 0 ? "A" : "B")}"),
                            playerFamilyName: new PlayerFamilyName($"姓{i:00}{j:00}"),
                            playerFirstName: new PlayerFirstName($"名{i:00}{j:00}"),
                            point: new Point(point));
                        entryPlayers.Add(entryPlayer);
                    }
                    #endregion Create EntryPlayer

                    #region Create EntryDetail
                    var entryDetail = new EntryDetail(
                        entryNumber: new EntryNumber(j),
                        participationClassification: ParticipationClassification.Main,
                        seedNumber: new SeedNumber(0),
                        entryPlayers: entryPlayers,
                        canParticipationDates: tournament.HoldingDates
                            .Where(o => r.Next(0, 4) != 1)
                            .Select(o => new CanParticipationDate(o.Value)),
                        receiptStatus: Enumeration.FromValue<ReceiptStatus>(r.Next(1, 4)),
                        usageFeatures: UsageFeatures.TournamentEntry,
                        fromQualifying: false,
                        blockNumber: null);
                    #endregion Create EntryDetail

                    var tournamentEntry = new TournamentEntry(
                        reservationNumber: $"{DateTime.Now:yyyyMMdd}{i:0000}{j:00}",
                        reservationDate: tournament.ApplicationPeriod.StartDate,
                        entryDetail: entryDetail,
                        entryFee: tournament.EntryFee,
                        receiptStatus: ReceiptStatus.Received,
                        receivedDate: DateTime.Today,
                        applicant: Enumeration.FromValue<Applicant>(r.Next(1, 3)));
                    tournamentEntries.Add(tournamentEntry);
                }
            }
            #endregion Create TournamentEntry

            return tournamentEntries;
        }
    }
}
