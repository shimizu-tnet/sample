using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Domain.UseCases.Tournaments;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JuniorTennis.DomainTests.UseCases
{
    public class TournamentUseCaseTests
    {
        [Fact]
        public async Task 大会一覧取得()
        {
            // Arrange
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
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                1
                );

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.Find())
                .ReturnsAsync(Enumerable.Range(0, 10).Select(o => tournament).ToList())
                .Verifiable();
            var usecase = new TournamentUseCase(mockRepository.Object);

            // Act
            var act = await usecase.GetTournaments();

            // Assert
            mockRepository.Verify();
            Assert.Equal(10, act.Count());
        }

        [Fact]
        public async Task 大会IDを指定し大会取得()
        {
            // Arrange
            var id = 100000;
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
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                100000
                );

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.FindById(id))
                .ReturnsAsync(tournament)
                .Verifiable();
            var usecase = new TournamentUseCase(mockRepository.Object);

            // Act
            var act = await usecase.GetTournament(id);

            // Assert
            mockRepository.Verify();
            Assert.Equal(id, act.Id);
        }

        [Fact]
        public async Task 大会を登録する()
        {
            // Arrange
            var dto = new RegisterTournamentDto()
            {
                TournamentName = "ジュニア選手権",
                TournamentType = TournamentType.WithDraw.Id,
                RegistrationYear = new DateTime(2020, 4, 1),
                TypeOfYear = TypeOfYear.Odd.Id,
                AggregationMonth = new DateTime(2020, 6, 1),
                TennisEvents = new List<(int, int, int)>() { (1, 1, 1), (1, 1, 2) },
                HoldingStartDate = new DateTime(2020, 6, 10),
                HoldingEndDate = new DateTime(2020, 6, 20),
                HoldingDates = new List<DateTime>() { new DateTime(2020, 6, 12), new DateTime(2020, 6, 13) },
                Venue = "日本テニスコート",
                EntryFee = 100,
                MethodOfPayment = MethodOfPayment.PrePayment.Id,
                ApplicationStartDate = new DateTime(2020, 5, 1),
                ApplicationEndDate = new DateTime(2020, 5, 31),
                Outline = "大会名：ジュニア選手権場所：日本テニスコート",
                TournamentEntryReceptionMailSubject = "メール件名",
                TournamentEntryReceptionMailBody = "メール本文",
            };

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
                0
                );

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.Add(It.Is<Tournament>(o => o.TournamentType == TournamentType.WithDraw)))
                .ReturnsAsync(tournament)
                .Verifiable();
            var usecase = new TournamentUseCase(mockRepository.Object);

            // Act
            var act = await usecase.RegisterTournament(dto);

            // Assert
            mockRepository.Verify();
            Assert.Equal(TournamentType.WithDraw, act.TournamentType);
        }

        [Fact]
        public async Task ポイントのみ大会を登録する()
        {
            // Arrange
            var dto = new RegisterTournamentDto()
            {
                TournamentName = "ジュニア選手権",
                TournamentType = TournamentType.OnlyPoints.Id,
                RegistrationYear = new DateTime(2020, 4, 1),
                TypeOfYear = TypeOfYear.Odd.Id,
                AggregationMonth = new DateTime(2020, 6, 1),
                TennisEvents = new List<(int, int, int)>() { (1, 1, 1), (1, 1, 2) },
                HoldingStartDate = new DateTime(2020, 6, 10),
                HoldingEndDate = new DateTime(2020, 6, 20),
                HoldingDates = new List<DateTime>() { new DateTime(2020, 6, 12), new DateTime(2020, 6, 13) },
                Venue = "日本テニスコート",
                EntryFee = 100,
                MethodOfPayment = MethodOfPayment.PrePayment.Id,
                ApplicationStartDate = new DateTime(2020, 5, 1),
                ApplicationEndDate = new DateTime(2020, 5, 31),
                Outline = "大会名：ジュニア選手権場所：日本テニスコート",
                TournamentEntryReceptionMailSubject = "メール件名",
                TournamentEntryReceptionMailBody = "メール本文",
            };

            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.OnlyPoints,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                0
                );

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.Add(It.Is<Tournament>(o => o.TournamentType == TournamentType.OnlyPoints)))
                .ReturnsAsync(tournament)
                .Verifiable();
            var usecase = new TournamentUseCase(mockRepository.Object);

            // Act
            var act = await usecase.RegisterTournament(dto);

            // Assert
            mockRepository.Verify();
            Assert.Equal(TournamentType.OnlyPoints, act.TournamentType);
        }

        [Fact]
        public async Task 大会を更新する()
        {
            // Arrange
            var dto = new UpdateTournamentDto()
            {
                TournamentId = 1,
                TournamentName = "ジュニア選手権",
                TournamentType = TournamentType.WithDraw.Id,
                RegistrationYear = new DateTime(2020, 4, 1),
                TypeOfYear = TypeOfYear.Odd.Id,
                AggregationMonth = new DateTime(2020, 6, 1),
                TennisEvents = new List<(int, int, int)>() { (1, 1, 1), (1, 1, 2) },
                HoldingStartDate = new DateTime(2020, 6, 10),
                HoldingEndDate = new DateTime(2020, 6, 20),
                HoldingDates = new List<DateTime>() { new DateTime(2020, 6, 12), new DateTime(2020, 6, 13) },
                Venue = "日本テニスコート",
                EntryFee = 100,
                MethodOfPayment = MethodOfPayment.PrePayment.Id,
                ApplicationStartDate = new DateTime(2020, 5, 1),
                ApplicationEndDate = new DateTime(2020, 5, 31),
                Outline = "大会名：ジュニア選手権場所：日本テニスコート",
                TournamentEntryReceptionMailSubject = "メール件名",
                TournamentEntryReceptionMailBody = "メール本文",
            };

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
                1
                );

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.Update(It.Is<Tournament>(o => o.TournamentType == TournamentType.WithDraw)))
                .ReturnsAsync(tournament)
                .Verifiable();
            var usecase = new TournamentUseCase(mockRepository.Object);

            // Act
            var act = await usecase.UpdateTournament(dto);

            // Assert
            mockRepository.Verify();
            Assert.Equal(TournamentType.WithDraw, act.TournamentType);
            Assert.Equal(1, act.Id);
        }

        [Fact]
        public async Task ポイントのみ大会を更新する()
        {
            // Arrange
            var dto = new UpdateTournamentDto()
            {
                TournamentId = 1,
                TournamentName = "ジュニア選手権",
                TournamentType = TournamentType.OnlyPoints.Id,
                RegistrationYear = new DateTime(2020, 4, 1),
                TypeOfYear = TypeOfYear.Odd.Id,
                AggregationMonth = new DateTime(2020, 6, 1),
                TennisEvents = new List<(int, int, int)>() { (1, 1, 1), (1, 1, 2) },
                HoldingStartDate = new DateTime(2020, 6, 10),
                HoldingEndDate = new DateTime(2020, 6, 20),
                HoldingDates = new List<DateTime>() { new DateTime(2020, 6, 12), new DateTime(2020, 6, 13) },
                Venue = "日本テニスコート",
                EntryFee = 100,
                MethodOfPayment = MethodOfPayment.PrePayment.Id,
                ApplicationStartDate = new DateTime(2020, 5, 1),
                ApplicationEndDate = new DateTime(2020, 5, 31),
                Outline = "大会名：ジュニア選手権場所：日本テニスコート",
                TournamentEntryReceptionMailSubject = "メール件名",
                TournamentEntryReceptionMailBody = "メール本文",
            };

            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.OnlyPoints,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                1
                );

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.Update(It.Is<Tournament>(o => o.TournamentType == TournamentType.OnlyPoints)))
                .ReturnsAsync(tournament)
                .Verifiable();
            var usecase = new TournamentUseCase(mockRepository.Object);

            // Act
            var act = await usecase.UpdateTournament(dto);

            // Assert
            mockRepository.Verify();
            Assert.Equal(TournamentType.OnlyPoints, act.TournamentType);
            Assert.Equal(1, act.Id);
        }

        [Fact]
        public async Task 大会を削除する()
        {
            // Arrange
            var id = 100000;
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
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                100000
                );

            var mockRepository = new Mock<ITournamentRepository>();
            mockRepository.Setup(r => r.FindById(id))
                .ReturnsAsync(tournament);
            var usecase = new TournamentUseCase(mockRepository.Object);

            // Act
            await usecase.DeleteTournament(id);

            // Assert
            mockRepository.Verify(m => m.Delete(It.IsAny<Tournament>()), Times.Once);
        }

        [Fact]
        public void 終了日より開催日の方が新しい場合中身のない一覧を返す()
        {
            // Arrange
            var holdingStartDate = new DateTime(2020, 4, 10);
            var holdingEndDate = new DateTime(2020, 4, 1);
            var mockRepository = new Mock<ITournamentRepository>();
            var usecase = new TournamentUseCase(mockRepository.Object);

            // Act
            var act = usecase.CreateHoldingDates(holdingStartDate, holdingEndDate);

            //Assert
            Assert.Equal(new List<JsonHoldingDate>(), act);
        }

        [Fact]
        public void 開催日の一覧を作成()
        {
            // Arrange
            var holdingStartDate = new DateTime(2020, 4, 1);
            var holdingEndDate = new DateTime(2020, 4, 8);
            var values = new List<string>()
            {
                "2020/03/30",
                "2020/03/31",
                "2020/04/01",
                "2020/04/02",
                "2020/04/03",
                "2020/04/04",
                "2020/04/05",
                "2020/04/06",
                "2020/04/07",
                "2020/04/08",
                "2020/04/09",
                "2020/04/10",
                "2020/04/11",
                "2020/04/12",
            };

            var mockRepository = new Mock<ITournamentRepository>();
            var usecase = new TournamentUseCase(mockRepository.Object);

            // Act
            var act = usecase.CreateHoldingDates(holdingStartDate, holdingEndDate);

            //Assert
            Assert.Equal(values, act.Select(o => o.Value).ToList());
            Assert.Equal(14, act.Count());
        }
    }
}
