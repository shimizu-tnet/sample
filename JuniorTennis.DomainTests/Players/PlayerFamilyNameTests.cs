using JuniorTennis.Domain.Players;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Players
{
    public class PlayerFamilyNameTests
    {
        [Fact]
        public void 姓は20文字まで有効()
        {
            var act = new PlayerFamilyName("あいうえおかきくけこあいうえおかきくけこ");
            Assert.Equal("あいうえおかきくけこあいうえおかきくけこ", act.Value);
        }

        [Fact]
        public void 姓が20文字を超える場合例外()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PlayerFamilyName("あいうえおかきくけこあいうえおかきくけこあ"));
            Assert.Equal("20 文字を超えています。 (Parameter '姓')", exception.Message);
        }
    }
}
