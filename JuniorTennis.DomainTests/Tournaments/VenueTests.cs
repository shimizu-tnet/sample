using JuniorTennis.Domain.Tournaments;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class VenueTests
    {
        [Fact]
        public void 大会名が50文字以下の場合正しく取得()
        {
            var act = new Venue("日本テニスコート第10000000000000000000000000000000000000コート");

            Assert.Equal("日本テニスコート第10000000000000000000000000000000000000コート", act.Value);
        }

        [Fact]
        public void 会場名が50文字を超えている場合例外()
        {
            var tournamentName = "日本テニスコート第100000000000000000000000000000000000000コート";

            var exception = Assert.Throws<ArgumentException>(
                () => new Venue(tournamentName));
            Assert.Equal("50 文字を超えています。 (Parameter '会場')", exception.Message);
        }

        [Fact]
        public void 会場名がNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Venue(null));
            Assert.Equal("会場", exception.ParamName);
        }
    }
}
