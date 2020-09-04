using JuniorTennis.Domain.Teams;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Teams
{
    public class TeamCodeTests
    {
        [Fact]
        public void 未入力不可()
        {
            Assert.Throws<ArgumentNullException>(
                () => new TeamCode(string.Empty));
        }
    }
}
