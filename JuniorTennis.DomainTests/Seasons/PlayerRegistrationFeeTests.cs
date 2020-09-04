using JuniorTennis.Domain.Seasons;
using JuniorTennis.SeedWork;
using Newtonsoft.Json.Bson;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Seasons
{
    public class PlayerRegistrationFeeTests
    {
        [Fact]
        public void 選手登録料にマイナスの金額は設定不可()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PlayerRegistrationFee(-1000));
            Assert.Equal("選手登録料がマイナスです。 (Parameter '選手登録料')", exception.Message);
        }

        [Fact]
        public void 正しい選手登録料を設定()
        {
            var act = new PlayerRegistrationFee(1000);
            Assert.Equal(1000, act.Value);
            Assert.Equal("1,000円", act.DisplayValue);
        }

        [Fact]
        public void 選手登録料が3桁以下の場合カンマ区切りしない()
        {
            var act = new PlayerRegistrationFee(500);
            Assert.Equal("500円", act.DisplayValue);
        }
    }
}
