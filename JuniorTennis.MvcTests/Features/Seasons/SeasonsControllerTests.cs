using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.UseCases.Seasons;
using JuniorTennis.Mvc.Features.Seasons;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace JuniorTennis.MvcTests.Features.Seasons
{
    public class SeasonsControllerTests
    {
        [Fact]
        public async void 年度一覧の表示()
        {
            // Arrange
            var mockUseCase = new Mock<ISeasonUseCase>();
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
                    ),
                new Season(
                    "2021年度",
                    new DateTime(2021, 4, 1),
                    new DateTime(2022, 3, 31),
                    new DateTime(2021, 3, 31),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    ),
                new Season(
                    "2022年度",
                    new DateTime(2022, 4, 1),
                    new DateTime(2023, 3, 31),
                    new DateTime(2022, 3, 31),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    ),
                new Season(
                    "2023年度",
                    new DateTime(2023, 4, 1),
                    new DateTime(2024, 3, 31),
                    new DateTime(2023, 3, 31),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    )
            };

            mockUseCase.Setup(m => m.GetSeasons())
                .ReturnsAsync(seasons)
                .Verifiable();
            var controller = new SeasonsController(mockUseCase.Object);

            // Act
            var result = await controller.Index();

            // Assert
            mockUseCase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            Assert.Equal(4, model.Seasons.Count);
        }

        [Fact]
        public void 年度新規登録画面を表示()
        {
            // Arrange
            var mockUseCase = new Mock<ISeasonUseCase>();
            var controller = new SeasonsController(mockUseCase.Object);

            // Act
            var result = controller.Register();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<RegisterViewModel>(viewResult.ViewData.Model);
        }

        [Fact]
        public async void 年度を登録()
        {
            // Arrange
            var viewModel = new RegisterViewModel()
            {
                SeasonName = "2020年度",
                FromDate = new DateTime(2020, 4, 1),
                ToDate = new DateTime(2021, 3, 31),
                RegistrationFromDate = new DateTime(2020, 3, 31),
                TeamRegistrationFee = 5000,
                PlayerRegistrationFee = 500,
                PlayerTradeFee = 200
            };

            var season = new Season(
                "2020年度",
                new DateTime(2020, 4, 1),
                new DateTime(2021, 3, 31),
                new DateTime(2020, 3, 31),
                new TeamRegistrationFee(5000),
                new PlayerRegistrationFee(500),
                new PlayerTradeFee(200));

            var mockUseCase = new Mock<ISeasonUseCase>();
            mockUseCase.Setup(m => m.RegisterSeason("2020年度", new DateTime(2020, 4, 1), new DateTime(2021, 3, 31), new DateTime(2020, 3, 31), 5000, 500, 200))
                .ReturnsAsync(season)
                .Verifiable();
            var controller = new SeasonsController(mockUseCase.Object);

            // Act
            var result = await controller.Register(viewModel);

            // Assert
            mockUseCase.Verify();
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Edit), redirectToActionResult.ActionName);
        }

        [Fact]
        public async void 年度登録時無効な値の場合再表示()
        {
            // Arrange
            var viewModel = new RegisterViewModel()
            {
                SeasonName = "2020年度",
                FromDate = new DateTime(2020, 4, 1),
                ToDate = new DateTime(2021, 3, 31),
                RegistrationFromDate = new DateTime(2020, 3, 31),
                TeamRegistrationFee = 5000,
                PlayerRegistrationFee = 500,
                PlayerTradeFee = 200
            };

            var mockUseCase = new Mock<ISeasonUseCase>();
            var controller = new SeasonsController(mockUseCase.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Register(viewModel);

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsType<RegisterViewModel>(viewResult.ViewData.Model);
            Assert.Equal(viewModel.SeasonName, model.SeasonName);
            Assert.Equal(viewModel.FromDate, model.FromDate);
            Assert.Equal(viewModel.ToDate, model.ToDate);
            Assert.Equal(viewModel.RegistrationFromDate, model.RegistrationFromDate);
            Assert.Equal(viewModel.TeamRegistrationFee, model.TeamRegistrationFee);
            Assert.Equal(viewModel.PlayerRegistrationFee, model.PlayerRegistrationFee);
            Assert.Equal(viewModel.PlayerTradeFee, model.PlayerTradeFee);
        }

        [Fact]
        public async void 年度登録画面を表示する()
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
                new PlayerTradeFee(200))
                { Id = id };

            var mockUseCase = new Mock<ISeasonUseCase>();
            mockUseCase.Setup(m => m.GetSeason(id))
                .ReturnsAsync(season)
                .Verifiable();
            var controller = new SeasonsController(mockUseCase.Object);

            // Act
            var result = await controller.Edit(id);

            // Assert
            mockUseCase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsType<EditViewModel>(viewResult.ViewData.Model);
            Assert.Equal(id, model.SeasonId);
            Assert.Equal("2020年度", model.SeasonName);
            Assert.Equal(new DateTime(2020, 4, 1), model.FromDate);
            Assert.Equal(new DateTime(2021, 3, 31), model.ToDate);
            Assert.Equal(new DateTime(2020, 3, 31), model.RegistrationFromDate);
            Assert.Equal(5000, model.TeamRegistrationFee);
            Assert.Equal(500, model.PlayerRegistrationFee);
            Assert.Equal(200, model.PlayerTradeFee);
        }

        [Fact]
        public async void 年度を登録する()
        {
            // Arrange
            var id = 100000;
            var viewModel = new EditViewModel(
                id,
                "2020年度",
                new DateTime(2020, 4, 1),
                new DateTime(2021, 3, 31),
                new DateTime(2020, 3, 31),
                5000,
                500,
                200);

            var season = new Season(
                "2020年度",
                new DateTime(2020, 4, 1),
                new DateTime(2021, 3, 31),
                new DateTime(2020, 3, 31),
                new TeamRegistrationFee(5000),
                new PlayerRegistrationFee(500),
                new PlayerTradeFee(200))
                { Id = id };

            var mockUseCase = new Mock<ISeasonUseCase>();
            mockUseCase.Setup(m => m.UpdateSeason(id, new DateTime(2020, 4, 1), new DateTime(2021, 3, 31), new DateTime(2020, 3, 31), 5000, 500, 200))
                .ReturnsAsync(season)
                .Verifiable();
            var controller = new SeasonsController(mockUseCase.Object);

            // Act
            var result = await controller.Edit(viewModel);

            // Assert
            mockUseCase.Verify();
            var viewResulut = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResulut.ViewName);
            var model = Assert.IsType<EditViewModel>(viewResulut.Model);
            Assert.Equal(id, model.SeasonId);
            Assert.Equal(viewModel.SeasonName, model.SeasonName);
            Assert.Equal(viewModel.FromDate, model.FromDate);
            Assert.Equal(viewModel.ToDate, model.ToDate);
            Assert.Equal(viewModel.RegistrationFromDate, model.RegistrationFromDate);
            Assert.Equal(viewModel.TeamRegistrationFee, model.TeamRegistrationFee);
            Assert.Equal(viewModel.PlayerRegistrationFee, model.PlayerRegistrationFee);
            Assert.Equal(viewModel.PlayerTradeFee, model.PlayerTradeFee);
        }

        [Fact]
        public async void 年度更新時無効な値の場合再表示する()
        {
            // Arrange
            var viewModel = new EditViewModel(
                100000,
                "2020年度",
                new DateTime(2020, 4, 1),
                new DateTime(2021, 3, 31),
                new DateTime(2020, 3, 31),
                5000,
                500,
                200);

            var mockUseCase = new Mock<ISeasonUseCase>();
            var controller = new SeasonsController(mockUseCase.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Edit(viewModel);

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsType<EditViewModel>(viewResult.ViewData.Model);
            Assert.Equal(viewModel.SeasonId, model.SeasonId);
            Assert.Equal(viewModel.SeasonName, model.SeasonName);
            Assert.Equal(viewModel.FromDate, model.FromDate);
            Assert.Equal(viewModel.ToDate, model.ToDate);
            Assert.Equal(viewModel.RegistrationFromDate, model.RegistrationFromDate);
            Assert.Equal(viewModel.TeamRegistrationFee, model.TeamRegistrationFee);
            Assert.Equal(viewModel.PlayerRegistrationFee, model.PlayerRegistrationFee);
            Assert.Equal(viewModel.PlayerTradeFee, model.PlayerTradeFee);
        }
    }
}
