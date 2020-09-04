using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.UseCases.Seasons;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JuniorTennis.DomainTests.UseCases.Seasons
{
    public class SeasonUseCaseTests
    {
        [Fact]
        public async void 年度一覧を年度の降順で取得()
        {
            // Arrange
            var seasons = new List<Season>
            {
                new Season(
                    "2020年度",
                    new DateTime(2020, 4, 1),
                    new DateTime(2021, 3, 31),
                    new DateTime(2020, 3, 31),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    )
                    { Id = 1 },
                new Season(
                    "2021年度",
                    new DateTime(2021, 4, 1),
                    new DateTime(2022, 3, 31),
                    new DateTime(2021, 3, 31),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    )
                    { Id = 2 },
                new Season(
                    "2022年度",
                    new DateTime(2022, 4, 1),
                    new DateTime(2023, 3, 31),
                    new DateTime(2022, 3, 31),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    )
                    { Id = 3 },
                new Season(
                    "2023年度",
                    new DateTime(2023, 4, 1),
                    new DateTime(2024, 3, 31),
                    new DateTime(2023, 3, 31),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    )
                    { Id = 4 }
            };
            var mockRepository = new Mock<ISeasonRepository>();
            mockRepository.Setup(r => r.FindAll())
                .ReturnsAsync(seasons)
                .Verifiable();
            var usecase = new SeasonUseCase(mockRepository.Object);

            // Act
            var act = await usecase.GetSeasons();

            // Assert
            mockRepository.Verify();
            Assert.Equal(4, act.Count);
            Assert.Equal("2023年度", act.First().Name);
            Assert.Equal("2022年度", act.Skip(1).First().Name);
            Assert.Equal("2021年度", act.Skip(2).First().Name);
            Assert.Equal("2020年度", act.Skip(3).First().Name);
        }

        [Fact]
        public async void 年度を登録する()
        {
            // Arrange
            var name = "2020年度";
            var fromDate = new DateTime(2020, 4, 1);
            var toDate = new DateTime(2021, 3, 31);
            var registrationFromDate = new DateTime(2020, 3, 31);
            var teamRegistrationFee = 5000;
            var playerRegistrationFee = 500;
            var playerTradeFee = 200;
            var mockRepository = new Mock<ISeasonRepository>();
            var season = new Season(
                name,
                fromDate,
                toDate,
                registrationFromDate,
                new TeamRegistrationFee(teamRegistrationFee),
                new PlayerRegistrationFee(playerRegistrationFee),
                new PlayerTradeFee(playerTradeFee)
                );

            mockRepository.Setup(m => m.Add(It.IsAny<Season>()))
                .ReturnsAsync(season)
                .Verifiable();
            var usecase = new SeasonUseCase(mockRepository.Object);

            // Act
            var result = await usecase.RegisterSeason(name, fromDate, toDate, registrationFromDate, teamRegistrationFee, playerRegistrationFee, playerTradeFee);

            // Assert
            mockRepository.Verify();
            Assert.Equal(season.Name, result.Name);
            Assert.Equal(season.FromDate, result.FromDate);
            Assert.Equal(season.ToDate, result.ToDate);
            Assert.Equal(season.RegistrationFromDate, result.RegistrationFromDate);
            Assert.Equal(season.TeamRegistrationFee, result.TeamRegistrationFee);
            Assert.Equal(season.PlayerRegistrationFee, result.PlayerRegistrationFee);
            Assert.Equal(season.PlayerTradeFee, result.PlayerTradeFee);
        }

        [Fact]
        public async void 年度を更新する()
        {
            // Arrange
            var id = 100000;
            var fromDate = new DateTime(2020, 4, 1);
            var toDate = new DateTime(2021, 3, 31);
            var registrationFromDate = new DateTime(2020, 3, 31);
            var teamRegistrationFee = 5000;
            var playerRegistrationFee = 500;
            var playerTradeFee = 200;
            var season = new Season(
                "2020年度",
                fromDate,
                toDate,
                registrationFromDate,
                new TeamRegistrationFee(teamRegistrationFee),
                new PlayerRegistrationFee(playerRegistrationFee),
                new PlayerTradeFee(playerTradeFee)
                )
            { Id = id };

            var mockRepository = new Mock<ISeasonRepository>();
            mockRepository.Setup(m => m.FindById(id))
                .ReturnsAsync(season)
                .Verifiable();
            mockRepository.Setup(m => m.Update(It.IsAny<Season>()))
                .ReturnsAsync(season)
                .Verifiable();
            var usecase = new SeasonUseCase(mockRepository.Object);

            // Act
            var result = await usecase.UpdateSeason(id, fromDate, toDate, registrationFromDate, teamRegistrationFee, playerRegistrationFee, playerTradeFee);

            // Assert
            mockRepository.Verify();
            Assert.Equal(season.Id, result.Id);
            Assert.Equal(season.FromDate, result.FromDate);
            Assert.Equal(season.ToDate, result.ToDate);
            Assert.Equal(season.RegistrationFromDate, result.RegistrationFromDate);
            Assert.Equal(season.TeamRegistrationFee, result.TeamRegistrationFee);
            Assert.Equal(season.PlayerRegistrationFee, result.PlayerRegistrationFee);
            Assert.Equal(season.PlayerTradeFee, result.PlayerTradeFee);
        }

        [Fact]
        public async void 年度を取得する()
        {
            // Arrange
            var id = 100000;
            var season = new Season(
                    "2020年度",
                    new DateTime(2020, 4, 1),
                    new DateTime(2021, 3, 31),
                    new DateTime(2020, 3, 31),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    )
                    { Id = id };

            var mockRepository = new Mock<ISeasonRepository>();
            mockRepository.Setup(m => m.FindById(id))
                .ReturnsAsync(season)
                .Verifiable();
            var usecase = new SeasonUseCase(mockRepository.Object);

            // Act
            var result = await usecase.GetSeason(id);

            // Assert
            mockRepository.Verify();
            Assert.Equal(season.Id, result.Id);
            Assert.Equal(season.Name, result.Name);
            Assert.Equal(season.FromDate, result.FromDate);
            Assert.Equal(season.ToDate, result.ToDate);
            Assert.Equal(season.RegistrationFromDate, result.RegistrationFromDate);
            Assert.Equal(season.TeamRegistrationFee, result.TeamRegistrationFee);
            Assert.Equal(season.PlayerRegistrationFee, result.PlayerRegistrationFee);
            Assert.Equal(season.PlayerTradeFee, result.PlayerTradeFee);
        }
    }
}
