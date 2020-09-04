using JuniorTennis.Domain.Players;
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

        public Task<List<TournamentEntry>> FindAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<TournamentEntry>> FindByIdAsync(int tournamentId, string tennisEventId)
        {
            var tournaments = await this.context.Tournaments
                    .Include(o => o.TennisEvents)
                    .Include(o => o.HoldingDates)
                    .ToListAsync();
            var tournament = tournaments.FirstOrDefault(o => o.Id == tournamentId);
            var random = new Random();

            var TournamentEntries = new List<TournamentEntry>();
            Enumerable.Range(1, 30).ToList()
                .ForEach(o =>
                {
                    TournamentEntries.AddRange(Enumerable.Range(1, 10).Select(p =>
                    {
                        return new TournamentEntry(
                            reservationNumber: $"{DateTime.Now:yyyyMMdd}{o:0000}{p:00}",
                            reservationDate: tournament.ApplicationPeriod.StartDate,
                            teams:
                            TennisEvent.FromId(tennisEventId).IsSingles
                                ? new Team[] {
                                    new Team(
                                        teamCode: new TeamCode($"C{o:0000}"),
                                        teamType: TeamType.Club,
                                        teamName: new TeamName($"クラブ {o:0000}"),
                                        teamAbbreviatedName: new TeamAbbreviatedName($"クラブ {o:0000}"),
                                        representativeName:"",
                                        representativeEmailAddress:"",
                                        telephoneNumber: "",
                                        address: "",
                                        coachName:"",
                                        coachEmailAddress: "",
                                        teamJpin:"")
                                }
                                : new Team[] {
                                    new Team(
                                        teamCode: new TeamCode($"C{o:0000}"),
                                        teamType: TeamType.Club,
                                        teamName: new TeamName($"クラブ {o:0000}"),
                                        teamAbbreviatedName: new TeamAbbreviatedName($"クラブ {o:0000}"),
                                        representativeName:"",
                                        representativeEmailAddress:"",
                                        telephoneNumber: "",
                                        address: "",
                                        coachName:"",
                                        coachEmailAddress: "",
                                        teamJpin:"")
                                    , new Team(
                                        teamCode: new TeamCode($"C{o:0000}"),
                                        teamType: TeamType.Club,
                                        teamName: new TeamName($"クラブ {o:0000}"),
                                        teamAbbreviatedName: new TeamAbbreviatedName($"クラブ {o:0000}"),
                                        representativeName:"",
                                        representativeEmailAddress:"",
                                        telephoneNumber: "",
                                        address: "",
                                        coachName:"",
                                        coachEmailAddress: "",
                                        teamJpin:"")
                                },
                            tournamentName: tournament.TournamentName,
                            tennisEvent: tournament.TennisEvents.First(o => o == TennisEvent.FromId(tennisEventId)),
                            players:
                            TennisEvent.FromId(tennisEventId).IsSingles
                                ? new Player[] {
                                    new Player(
                                        teamId: 1,
                                        playerCode: new PlayerCode($"{o:000}{p:000}A"),
                                        playerFamilyName:  new PlayerFamilyName($"名前 {o:000}"),
                                        playerFirstName:new PlayerFirstName($"{p:000}A"),
                                        playerFamilyNameKana: new PlayerFamilyNameKana("テスト"),
                                        playerFirstNameKana: new PlayerFirstNameKana("ナマエ"),
                                        playerJpin: null,
                                        category: null,
                                        gender: null,
                                        birthDate: new BirthDate(new DateTime()),
                                        telephoneNumber: null)
                                }
                                : new Player[] {
                                    new Player(
                                        teamId: 1,
                                        playerCode: new PlayerCode($"{o:000}{p:000}A"),
                                        playerFamilyName:  new PlayerFamilyName($"名前 {o:000}"),
                                        playerFirstName:new PlayerFirstName($"{p:000}A"),
                                        playerFamilyNameKana: new PlayerFamilyNameKana("テスト"),
                                        playerFirstNameKana: new PlayerFirstNameKana("ナマエ"),
                                        playerJpin: null,
                                        category: null,
                                        gender: null,
                                        birthDate: new BirthDate(new DateTime()),
                                        telephoneNumber: null)
                                    , new Player(
                                        teamId: 1,
                                        playerCode: new PlayerCode($"{o:000}{p:000}B"),
                                        playerFamilyName:  new PlayerFamilyName($"名前 {o:000}"),
                                        playerFirstName:new PlayerFirstName($"{p:000}B"),
                                        playerFamilyNameKana: new PlayerFamilyNameKana("テスト"),
                                        playerFirstNameKana: new PlayerFirstNameKana("ナマエ"),
                                        playerJpin: null,
                                        category: null,
                                        gender: null,
                                        birthDate: new BirthDate(new DateTime()),
                                        telephoneNumber: null)
                                },
                            entryFee: tournament.EntryFee,
                            receiptStatus: Enumeration.FromValue<ReceiptStatus>(random.Next(1, 3)),
                            receivedDate: null,
                            applicant: Applicant.Team
                            ); ;
                    }));
                });

            return TournamentEntries;
        }
    }
}
