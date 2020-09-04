using JuniorTennis.Domain.Teams;
using Xunit;

namespace JuniorTennis.DomainTests.Teams
{
    public class TeamTests
    {
        [Fact]
        public void JPINの団体番号を変更する()
        {
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
                teamJpin: "");
            team.ChangeTeamJpin("OS9999");
            Assert.Equal("OS9999", team.TeamJpin);
        }
    }
}
