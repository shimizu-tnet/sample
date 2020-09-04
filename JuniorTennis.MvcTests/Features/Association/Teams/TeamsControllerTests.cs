using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.Domain.UseCases.Teams;
using JuniorTennis.Mvc.Configurations;
using JuniorTennis.Mvc.Features.Association.Teams;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JuniorTennis.MvcTests.Features.Teams
{
    public class TeamsControllerTests
    {
        [Fact]
        public async Task 団体が登録されている()
        {
            // Arrange
            var target = DateTime.Today;
            var teams = Enumerable.Range(1, 10)
                .Select(o => new Team(
                    teamCode: new TeamCode($"S000{o:00}"),
                    teamType: TeamType.School,
                    teamName: new TeamName("大成ネット学園"),
                    teamAbbreviatedName: new TeamAbbreviatedName("大成ネット学園"),
                    representativeName: "山村修治",
                    representativeEmailAddress: "yamamura-shuji@taiseinet.com",
                    telephoneNumber: "03-5408-8576",
                    address: "大阪市西区京町堀2-13-1-211",
                    coachName: "横溝学",
                    coachEmailAddress: "yokomizo-manabu@taiseinet.com",
                    teamJpin: "OS12345"))
                .ToList();
            var mockUsecase = new Mock<ITeamUseCase>();
            mockUsecase.Setup(u => u.SearchTeam(0, 50, null, null))
                .ReturnsAsync(new Pagable<Team>(teams, 0, 10, 50))
                .Verifiable();
            var mockOptions = new Mock<IOptions<UrlSettings>>();
            var controller = new TeamsController(mockUsecase.Object, mockOptions.Object);

            // Act
            var result = await controller.Index(0);

            // Assert
            mockUsecase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            Assert.Equal(10, model.Teams.Count);
        }

        [Fact]
        public async Task 団体一覧の２ページ目を表示()
        {
            // Arrange
            var target = DateTime.Today;
            var teams = Enumerable.Range(51, 20)
                .Select(o => new Team(
                    teamCode: new TeamCode($"S000{o:00}"),
                    teamType: TeamType.School,
                    teamName: new TeamName("大成ネット学園"),
                    teamAbbreviatedName: new TeamAbbreviatedName("大成ネット学園"),
                    representativeName: "山村修治",
                    representativeEmailAddress: "yamamura-shuji@taiseinet.com",
                    telephoneNumber: "03-5408-8576",
                    address: "大阪市西区京町堀2-13-1-211",
                    coachName: "横溝学",
                    coachEmailAddress: "yokomizo-manabu@taiseinet.com",
                    teamJpin: "OS12345"))
                .ToList();
            var mockUsecase = new Mock<ITeamUseCase>();
            mockUsecase.Setup(u => u.SearchTeam(1, 50, null, null))
                .ReturnsAsync(new Pagable<Team>(teams, 1, 20, 50))
                .Verifiable();
            var mockOptions = new Mock<IOptions<UrlSettings>>();
            var controller = new TeamsController(mockUsecase.Object, mockOptions.Object);

            // Act
            var result = await controller.Index(1);

            // Assert
            mockUsecase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            Assert.Equal(20, model.Teams.Count);
        }

        [Fact]
        public async Task 団体が登録されていない()
        {
            // Arrange
            var target = DateTime.Today;
            var mockUsecase = new Mock<ITeamUseCase>();
            mockUsecase.Setup(u => u.SearchTeam(0, 50, null, null))
                .ReturnsAsync(new Pagable<Team>(new List<Team>(), 0, 50, 50))
                .Verifiable();
            var mockOptions = new Mock<IOptions<UrlSettings>>();
            var controller = new TeamsController(mockUsecase.Object, mockOptions.Object);

            // Act
            var result = await controller.Index(0);

            // Assert
            mockUsecase.Verify();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            Assert.Empty(model.Teams);
        }
    }
}
