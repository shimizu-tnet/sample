using JuniorTennis.Domain.Seasons;
using JuniorTennis.SeedWork;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Seasons
{
    public class TeamRegistrationFeeTests
    {
        [Fact]
        public void 団体登録料にマイナスの金額は設定不可()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new TeamRegistrationFee(-1000));
            Assert.Equal("団体登録料がマイナスです。 (Parameter '団体登録料')", exception.Message);
        }

        [Fact]
        public void 正しい団体登録料を設定()
        {
            var act = new TeamRegistrationFee(5000);
            Assert.Equal(5000, act.Value);
            Assert.Equal("5,000円", act.DisplayValue);
        }

        [Fact]
        public void 団体登録料が3桁以下の場合カンマ区切りしない()
        {
            var act = new TeamRegistrationFee(500);
            Assert.Equal("500円", act.DisplayValue);
        }
    }
}
