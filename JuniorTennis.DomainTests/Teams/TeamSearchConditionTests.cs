using JuniorTennis.Domain.Teams;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace JuniorTennis.DomainTests.Teams
{
    public class TeamSearchConditionTests
    {
        private List<Team> teams;

        public TeamSearchConditionTests()
        {
            this.teams = Enumerable.Range(1, 20)
                .Select(o => new Team(
                    teamCode: new TeamCode($"S{o:00000}"),
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
        }

        [Fact]
        public void 団体を条件なしで検索()
        {
            // Arrange
            var condition = new TeamSearchCondition(
                pageIndex: 0,
                displayCount: 5,
                teamTypes: null,
                teamName: null);

            // Act
            var act = condition.Apply(this.teams.AsQueryable()).ToList();

            // Assert
            Assert.Equal(5, act.Count);
            Assert.Equal(new TeamCode("S00001"), act.First().TeamCode);
        }

        [Fact]
        public void 団体をページングなしで検索()
        {
            // Arrange
            var condition = new TeamSearchCondition(
                pageIndex: 1,
                displayCount: 5,
                teamTypes: new int[] { TeamType.Club.Id },
                teamName: "1");

            // Act
            var act = condition.ApplyWithoutPagination(this.teams.AsQueryable()).ToList();

            // Assert
            Assert.Equal(6, act.Count);
            Assert.Equal(new TeamCode("S00001"), act.First().TeamCode);
        }

        [Fact]
        public void 団体をページングして検索()
        {
            // Arrange
            var condition = new TeamSearchCondition(
                pageIndex: 1,
                displayCount: 5,
                teamTypes: new int[] { TeamType.Club.Id },
                teamName: "1");

            // Act
            var act = condition.Apply(this.teams.AsQueryable()).ToList();

            // Assert
            Assert.Single(act);
            Assert.Equal(new TeamCode("S00019"), act.First().TeamCode);
        }

        [Fact]
        public void 団体種別が学校の団体を検索()
        {
            // Arrange
            var condition = new TeamSearchCondition(
                pageIndex: 0, 
                displayCount: 5, 
                teamTypes: new int[] { TeamType.School.Id }, 
                teamName: null);

            // Act
            var act = condition.Apply(this.teams.AsQueryable()).ToList();

            // Assert
            Assert.Equal(5, act.Count);
            Assert.Equal(new TeamCode("S00002"), act.First().TeamCode);
            Assert.True(act.All(o => o.TeamType == TeamType.School));
        }

        [Fact]
        public void 団体名に1を含む団体を検索()
        {
            // Arrange
            var condition = new TeamSearchCondition(
                pageIndex: 1,
                displayCount: 5,
                teamTypes: null,
                teamName: "1");

            // Act
            var act = condition.Apply(this.teams.AsQueryable()).ToList();
            
            // Assert
            Assert.Equal(5, act.Count);
            Assert.Equal(new TeamCode("S00014"), act.First().TeamCode);
        }

        [Fact]
        public void 検索条件に一致しない場合0件()
        {
            // Arrange
            var condition = new TeamSearchCondition(
                pageIndex: 1,
                displayCount: 5,
                teamTypes: new int[] { TeamType.Club.Id },
                teamName: "taisei");

            // Act
            var act = condition.Apply(this.teams.AsQueryable()).ToList();

            // Assert
            Assert.Empty(act);
        }
    }
}
