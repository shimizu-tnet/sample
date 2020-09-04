using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.SeedWork;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.RequestTeams
{
    public class PlayerTradeFeeTests
    {
        [Fact]
        public void 申請登録料にマイナスの金額は設定不可()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new RequestedFee(-1000));
            Assert.Equal("申請登録料がマイナスです。 (Parameter '申請登録料')", exception.Message);
        }

        [Fact]
        public void 正しい申請登録料を設定()
        {
            var act = new RequestedFee(200);
            Assert.Equal(200, act.Value);
        }
    }
}
