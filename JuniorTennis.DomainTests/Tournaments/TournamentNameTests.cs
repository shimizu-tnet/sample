using JuniorTennis.Domain.Tournaments;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class TournamentNameTests
    {
        [Fact]
        public void 大会名が50文字以下の場合正しく取得()
        {
            var act = new TournamentName("ジュニアカップNo.0000000000000000000000000000000000000001");

            Assert.Equal("ジュニアカップNo.0000000000000000000000000000000000000001", act.Value);
        }

        [Fact]
        public void 大会名が50文字を超えている場合例外()
        {
            var tournamentName = "ジュニアカップNo.00000000000000000000000000000000000000001";

            var exception = Assert.Throws<ArgumentException>(
                () => new TournamentName(tournamentName));
            Assert.Equal("50 文字を超えています。 (Parameter '大会名')", exception.Message);
        }

        [Fact]
        public void 大会名がNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new TournamentName(null));
            Assert.Equal("大会名", exception.ParamName);
        }
    }
}
