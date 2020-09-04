using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Domain.UseCases.DrawTables;
using JuniorTennis.Domain.UseCases.Tournaments;
using JuniorTennis.Domain.Utils;
using JuniorTennis.Mvc.Features.DrawTables;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace JuniorTennis.DomainTests.DrawTables
{
    public class DrawTableTests
    {
        [Fact]
        public async void 大会一覧が開催開始日の降順かつIDの昇順で取得されること()
        {
            var initialData = new List<Tournament>()
            {
                new Tournament(
                    tournamentName: new TournamentName("大会名 001"),
                    tournamentType: TournamentType.WithDraw,
                    registrationYear: new RegistrationYear(new DateTime(2020, 4, 1)),
                    typeOfYear: TypeOfYear.Odd,
                    aggregationMonth: new AggregationMonth(new DateTime(2020, 8, 1)),
                    tennisEvents: TennisEvent.GetAllEvents().Select(o => (o.Value)).ToList(),
                    holdingPeriod: new HoldingPeriod(new DateTime(2020, 8, 2), new DateTime(2020, 8, 31)),
                    holdingDates: new List<HoldingDate>(){  new HoldingDate(new DateTime(2020, 8, 2)) },
                    venue: new Venue($"会場名"),
                    entryFee: new EntryFee(1000),
                    methodOfPayment: MethodOfPayment.PrePayment,
                    applicationPeriod: new ApplicationPeriod(new DateTime(2020, 7, 25), new DateTime(2020, 7, 31)),
                    outline: new Outline($"大会要領"),
                    tournamentEntryReceptionMailSubject: "メール件名",
                    tournamentEntryReceptionMailBody: "メール本文"
                ),
                new Tournament(
                    tournamentName: new TournamentName("大会名 004"),
                    tournamentType: TournamentType.WithDraw,
                    registrationYear: new RegistrationYear(new DateTime(2020, 4, 1)),
                    typeOfYear: TypeOfYear.Odd,
                    aggregationMonth: new AggregationMonth(new DateTime(2020, 8, 1)),
                    tennisEvents: TennisEvent.GetAllEvents().Select(o => (o.Value)).ToList(),
                    holdingPeriod: new HoldingPeriod(new DateTime(2020, 8, 3), new DateTime(2020, 8, 31)),
                    holdingDates: new List<HoldingDate>(){  new HoldingDate(new DateTime(2020, 8,3)) },
                    venue: new Venue($"会場名"),
                    entryFee: new EntryFee(1000),
                    methodOfPayment: MethodOfPayment.PrePayment,
                    applicationPeriod: new ApplicationPeriod(new DateTime(2020, 7, 25), new DateTime(2020, 7, 31)),
                    outline: new Outline($"大会要領"),
                    tournamentEntryReceptionMailSubject: "メール件名",
                    tournamentEntryReceptionMailBody: "メール本文"
                ),
                new Tournament(
                    tournamentName: new TournamentName("大会名 003"),
                    tournamentType: TournamentType.WithDraw,
                    registrationYear: new RegistrationYear(new DateTime(2020, 4, 1)),
                    typeOfYear: TypeOfYear.Odd,
                    aggregationMonth: new AggregationMonth(new DateTime(2020, 8, 1)),
                    tennisEvents: TennisEvent.GetAllEvents().Select(o => (o.Value)).ToList(),
                    holdingPeriod: new HoldingPeriod(new DateTime(2020, 8, 1), new DateTime(2020, 8, 31)),
                    holdingDates: new List<HoldingDate>(){  new HoldingDate(new DateTime(2020, 8,1)) },
                    venue: new Venue($"会場名"),
                    entryFee: new EntryFee(1000),
                    methodOfPayment: MethodOfPayment.PrePayment,
                    applicationPeriod: new ApplicationPeriod(new DateTime(2020, 7, 25), new DateTime(2020, 7, 31)),
                    outline: new Outline($"大会要領"),
                    tournamentEntryReceptionMailSubject: "メール件名",
                    tournamentEntryReceptionMailBody: "メール本文"
                ),
                new Tournament(
                    tournamentName: new TournamentName("大会名 002"),
                    tournamentType: TournamentType.WithDraw,
                    registrationYear: new RegistrationYear(new DateTime(2020, 4, 1)),
                    typeOfYear: TypeOfYear.Odd,
                    aggregationMonth: new AggregationMonth(new DateTime(2020, 8, 1)),
                    tennisEvents: TennisEvent.GetAllEvents().Select(o => (o.Value)).ToList(),
                    holdingPeriod: new HoldingPeriod(new DateTime(2020, 8, 3), new DateTime(2020, 8, 31)),
                    holdingDates: new List<HoldingDate>(){  new HoldingDate(new DateTime(2020, 8,3)) },
                    venue: new Venue($"会場名"),
                    entryFee: new EntryFee(1000),
                    methodOfPayment: MethodOfPayment.PrePayment,
                    applicationPeriod: new ApplicationPeriod(new DateTime(2020, 7, 25), new DateTime(2020, 7, 31)),
                    outline: new Outline($"大会要領"),
                    tournamentEntryReceptionMailSubject: "メール件名",
                    tournamentEntryReceptionMailBody: "メール本文"
                ),
            };
            initialData[0].Id = 1;
            initialData[1].Id = 4;
            initialData[2].Id = 3;
            initialData[3].Id = 2;

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(o => o.Find())
                .ReturnsAsync(initialData)
                .Verifiable();

            var tournamentUseCase = new TournamentUseCase(mockRepository.Object);
            var mockDrawTableUseCase = new Mock<IDrawTableUseCase>();

            var controller = new DrawTablesController(tournamentUseCase, mockDrawTableUseCase.Object);
            var jsonString = await controller.GetTournaments();

            var json = JsonSerializer
                .Deserialize<JsonElement>(jsonString)
                .EnumerateArray()
                .ToList();

            mockRepository.Verify();
            Assert.Equal("大会名 002", JsonConverter.ToString(json[0].GetProperty("name")));
            Assert.Equal("大会名 004", JsonConverter.ToString(json[1].GetProperty("name")));
            Assert.Equal("大会名 001", JsonConverter.ToString(json[2].GetProperty("name")));
            Assert.Equal("大会名 003", JsonConverter.ToString(json[3].GetProperty("name")));
        }
    }
}
