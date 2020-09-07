using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Domain.UseCases.TournamentEntries;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace JuniorTennis.DomainTests.UseCases.TournamentEntries
{
    public class TournamentEntryUseCaseTests
    {
        [Fact]
        public async void 申込期間中の大会名と大会IDを取得する()
        {
            // Arrange
            var tournaments = new List<Tournament>()
            {
                new Tournament(
                new TournamentName("ジュニア選手権1"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(DateTime.Today.AddMonths(2), DateTime.Today.AddMonths(3)),
                new List<HoldingDate>() { new HoldingDate(DateTime.Today.AddMonths(2).AddDays(1)), new HoldingDate(DateTime.Today.AddMonths(2).AddDays(2)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(DateTime.Today, DateTime.Today.AddMonths(1)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                1
                ),

                new Tournament(
                new TournamentName("ジュニア選手権1"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(DateTime.Today.AddMonths(2).AddDays(1), DateTime.Today.AddMonths(3)),
                new List<HoldingDate>() { new HoldingDate(DateTime.Today.AddMonths(2).AddDays(2)), new HoldingDate(DateTime.Today.AddMonths(2).AddDays(3)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(DateTime.Today.AddMonths(-1), DateTime.Today),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                2
                ),

                new Tournament(
                new TournamentName("ジュニア選手権1"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(DateTime.Today.AddMonths(2), DateTime.Today.AddMonths(3)),
                new List<HoldingDate>() { new HoldingDate(DateTime.Today.AddMonths(2).AddDays(1)), new HoldingDate(DateTime.Today.AddMonths(2).AddDays(2)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(DateTime.Today.AddMonths(-2), DateTime.Today.AddMonths(-1)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                3
                )
            };

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.Find())
                .ReturnsAsync(tournaments)
                .Verifiable();
            var useCase = new TournamentEntryUseCase(mockRepository.Object);

            // Act
            var act = await useCase.GetApplicationTournaments();

            // Assert
            mockRepository.Verify();
            Assert.Equal(2, act.Count);
            Assert.Equal("2", act[0].Id);
            Assert.Equal("1", act[1].Id);
        }

        [Fact]
        public async void 大会開催期間と申込期間と種目一覧を取得する()
        {
            // Arrange
            var tournamentId = 1;
            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(new DateTime(2020, 6, 10), new DateTime(2020, 6, 20)),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                tournamentId
                );

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.FindById(tournamentId))
                .ReturnsAsync(tournament)
                .Verifiable();
            var useCase = new TournamentEntryUseCase(mockRepository.Object);

            // Act
            var act = await useCase.GetSelectedTournament(tournamentId);

            // Assert
            mockRepository.Verify();
            Assert.Equal("17/18歳以下 男子 シングルス", act.TennisEvents[0].Name);
            Assert.Equal("17/18歳以下 男子 ダブルス", act.TennisEvents[1].Name);
            Assert.Equal("2020年6月10日 ～ 2020年6月20日", act.HoldingPeriod);
            Assert.Equal("2020年5月1日 ～ 2020年5月31日", act.ApplicationPeriod);
        }

        [Fact]
        public async void 大会開催期間申込期間取得時大会Idが0だった場合空データを返す()
        {
            // Arrange
            var tournamentId = 0;
            var mockRepository = new Mock<ITournamentRepository>();
            var useCase = new TournamentEntryUseCase(mockRepository.Object);

            // Act
            var act = await useCase.GetSelectedTournament(tournamentId);

            // Assert
            mockRepository.Verify(r => r.FindById(It.IsAny<int>()), Times.Never);
            Assert.Null(act.TennisEvents);
            Assert.Null(act.ApplicationPeriod);
            Assert.Null(act.HoldingPeriod);
        }
    }
}
