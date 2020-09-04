using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.Externals;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.ReservationNumbers;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.Domain.UseCases.Teams;
using JuniorTennis.SeedWork.Exceptions;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JuniorTennis.DomainTests.UseCases.Teams
{
    public class TeamUseCaseTests
    {
        [Fact]
        public async Task 団体種別が民間クラブで団体名に1を含む団体を検索()
        {
            // Arrange
            var teams = Enumerable.Range(1, 20)
                .Select(o => new Team(
                    teamCode: new TeamCode($"S000{o:00}"),
                    teamType: o % 2 == 0 ? TeamType.School : TeamType.Club,
                    teamName: new TeamName($"大成ネット学園{o}"),
                    teamAbbreviatedName: new TeamAbbreviatedName("大成ネット学園"),
                    representativeName: "山村修治",
                    representativeEmailAddress: "yamamura-shuji@taiseinet.com",
                    telephoneNumber: "03-5408-8576",
                    address: "大阪市西区京町堀2-13-1-211",
                    coachName: "横溝学",
                    coachEmailAddress: "yokomizo-manabu@taiseinet.com",
                    teamJpin: "OS12345"))
                .ToList();
            var condition = new TeamSearchCondition(
                pageIndex: 1,
                displayCount: 5,
                teamTypes: new int[] { TeamType.Club.Id },
                teamName: "1");
            var teamRepository = new Mock<ITeamRepository>();
            teamRepository.Setup(r => r.SearchAsync(It.Is<TeamSearchCondition>(o => o.PageIndex == 1 && o.DisplayCount == 5)))
                .ReturnsAsync(It.IsAny<Pagable<Team>>())
                .Verifiable();
            var confirmationMailAddressRepository = new Mock<IAuthorizationLinkRepository>();
            var requestTeamRepository = new Mock<IRequestTeamRepository>();
            var reservationNumberRepository = new Mock<IReservationNumberRepository>();
            var seasonRepository = new Mock<ISeasonRepository>();
            var mailSender = new Mock<IMailSender>();
            var usecase = new TeamUseCase(
                teamRepository.Object,
                confirmationMailAddressRepository.Object,
                requestTeamRepository.Object,
                reservationNumberRepository.Object,
                seasonRepository.Object, mailSender.Object);

            // Act
            var act = await usecase.SearchTeam(pageIndex: 1, displayCount: 5, teamTypes: new int[] { TeamType.Club.Id }, teamName: "1");

            // Assert
            teamRepository.Verify();
            teamRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task 団体を取得()
        {
            // Arrange
            var team = new Team(
                teamCode: new TeamCode("S00001"),
                teamType: TeamType.School,
                teamName: new TeamName("大成ネット学園"),
                teamAbbreviatedName: new TeamAbbreviatedName("大成ネット学園"),
                representativeName: "山村修治",
                representativeEmailAddress: "yamamura-shuji@taiseinet.com",
                telephoneNumber: "03-5408-8576",
                address: "大阪市西区京町堀2-13-1-211",
                coachName: "横溝学",
                coachEmailAddress: "yokomizo-manabu@taiseinet.com",
                teamJpin: "OS12345");
            var teamRepository = new Mock<ITeamRepository>();
            teamRepository.Setup(r => r.FindByCodeAsync(new TeamCode("S00001")))
                .ReturnsAsync(team)
                .Verifiable();
            var confirmationMailAddressRepository = new Mock<IAuthorizationLinkRepository>();
            var requestTeamRepository = new Mock<IRequestTeamRepository>();
            var reservationNumberRepository = new Mock<IReservationNumberRepository>();
            var seasonRepository = new Mock<ISeasonRepository>();
            var mailSender = new Mock<IMailSender>();
            var usecase = new TeamUseCase(
                teamRepository.Object,
                confirmationMailAddressRepository.Object,
                requestTeamRepository.Object,
                reservationNumberRepository.Object,
                seasonRepository.Object, mailSender.Object);

            // Act
            var act = await usecase.GetTeam("S00001");

            // Assert
            teamRepository.Verify();
            teamRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task 団体が存在しない場合例外()
        {
            // Arrange
            var teamRepository = new Mock<ITeamRepository>();
            teamRepository.Setup(r => r.FindByCodeAsync(new TeamCode("S00001")))
                .ReturnsAsync(default(Team))
                .Verifiable();
            var confirmationMailAddressRepository = new Mock<IAuthorizationLinkRepository>();
            var requestTeamRepository = new Mock<IRequestTeamRepository>();
            var reservationNumberRepository = new Mock<IReservationNumberRepository>();
            var seasonRepository = new Mock<ISeasonRepository>();
            var mailSender = new Mock<IMailSender>();
            var usecase = new TeamUseCase(
                teamRepository.Object,
                confirmationMailAddressRepository.Object,
                requestTeamRepository.Object,
                reservationNumberRepository.Object,
                seasonRepository.Object, mailSender.Object);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => usecase.GetTeam("S00001"));
            teamRepository.Verify();
            teamRepository.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task 管理者が団体のJPIN番号を更新する()
        {
            //Arrange
            var teamCode = "S00001";
            var teamJpin = "OS9999";
            var team = new Team(
                teamCode: new TeamCode(teamCode),
                teamType: TeamType.School,
                teamName: new TeamName("大成ネット学園"),
                teamAbbreviatedName: new TeamAbbreviatedName("大成ネット学園"),
                representativeName: "山村修治",
                representativeEmailAddress: "yamamura-shuji@taiseinet.com",
                telephoneNumber: "03-5408-8576",
                address: "大阪市西区京町堀2-13-1-211",
                coachName: "横溝学",
                coachEmailAddress: "yokomizo-manabu@taiseinet.com",
                teamJpin: "");
            var teamRepository = new Mock<ITeamRepository>();
            teamRepository.Setup(o => o.FindByCodeAsync(new TeamCode(teamCode)))
                .ReturnsAsync(team)
                .Verifiable();
            teamRepository.Setup(o => o.Update(It.Is<Team>(o => o.TeamCode == new TeamCode(teamCode) && o.TeamJpin == teamJpin)))
                .Verifiable();
            var confirmationMailAddressRepository = new Mock<IAuthorizationLinkRepository>();
            var requestTeamRepository = new Mock<IRequestTeamRepository>();
            var reservationNumberRepository = new Mock<IReservationNumberRepository>();
            var seasonRepository = new Mock<ISeasonRepository>();
            var mailSender = new Mock<IMailSender>();
            var usecase = new TeamUseCase(
                teamRepository.Object,
                confirmationMailAddressRepository.Object,
                requestTeamRepository.Object,
                reservationNumberRepository.Object,
                seasonRepository.Object, mailSender.Object);

            // Act
            await usecase.UpdateTeamJpin(teamCode, teamJpin);

            // Assert
            teamRepository.Verify();
            teamRepository.VerifyNoOtherCalls();
            Assert.Equal(teamJpin, team.TeamJpin);
        }

        [Fact]
        public async Task 団体のJPIN番号を更新したい団体が存在しない場合例外()
        {
            //Arrange
            var teamCode = "S00001";
            var teamJpin = "OS9999";
            var team = new Team(
                teamCode: new TeamCode(teamCode),
                teamType: TeamType.School,
                teamName: new TeamName("大成ネット学園"),
                teamAbbreviatedName: new TeamAbbreviatedName("大成ネット学園"),
                representativeName: "山村修治",
                representativeEmailAddress: "yamamura-shuji@taiseinet.com",
                telephoneNumber: "03-5408-8576",
                address: "大阪市西区京町堀2-13-1-211",
                coachName: "横溝学",
                coachEmailAddress: "yokomizo-manabu@taiseinet.com",
                teamJpin: "");
            var teamRepository = new Mock<ITeamRepository>();
            teamRepository.Setup(o => o.FindByCodeAsync(new TeamCode(teamCode)))
                .ReturnsAsync(default(Team))
                .Verifiable();
            var confirmationMailAddressRepository = new Mock<IAuthorizationLinkRepository>();
            var requestTeamRepository = new Mock<IRequestTeamRepository>();
            var reservationNumberRepository = new Mock<IReservationNumberRepository>();
            var seasonRepository = new Mock<ISeasonRepository>();
            var mailSender = new Mock<IMailSender>();
            var usecase = new TeamUseCase(
                teamRepository.Object,
                confirmationMailAddressRepository.Object,
                requestTeamRepository.Object,
                reservationNumberRepository.Object,
                seasonRepository.Object, mailSender.Object);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(
                () => usecase.UpdateTeamJpin(teamCode, teamJpin));
            teamRepository.Verify();
            teamRepository.VerifyNoOtherCalls();
        }
    }
}
