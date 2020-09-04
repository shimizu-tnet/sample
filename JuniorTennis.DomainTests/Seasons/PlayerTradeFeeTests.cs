using JuniorTennis.Domain.Seasons;
using JuniorTennis.SeedWork;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Seasons
{
    public class PlayerTradeFeeTests
    {
        [Fact]
        public void 選手移籍料にマイナスの金額は設定不可()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PlayerTradeFee(-1000));
            Assert.Equal("選手移籍料がマイナスです。 (Parameter '選手移籍料')", exception.Message);
        }

        [Fact]
        public void 正しい選手移籍料を設定()
        {
            var act = new PlayerTradeFee(200);
            Assert.Equal(200, act.Value);
        }
    }
}
