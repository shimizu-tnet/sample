using JuniorTennis.Domain.Players;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Players
{
    public class PlayerFirstNameTests
    {
        [Fact]
        public void 名は30文字まで有効()
        {
            var act = new PlayerFirstName("あいうえおかきくけこあいうえおかきくけこあいうえおかきくけこ");
            Assert.Equal("あいうえおかきくけこあいうえおかきくけこあいうえおかきくけこ", act.Value);
        }

        [Fact]
        public void 名が30文字を超える場合例外()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PlayerFirstName("あいうえおかきくけこあいうえおかきくけこあいうえおかきくけこあ"));
            Assert.Equal("30 文字を超えています。 (Parameter '名')", exception.Message);
        }
    }
}
