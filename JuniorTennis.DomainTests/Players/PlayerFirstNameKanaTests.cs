using JuniorTennis.Domain.Players;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Players
{
    public class PlayerFirstNameKanaTests
    {
        [Fact]
        public void 名カナは30文字まで有効()
        {
            var act = new PlayerFirstNameKana("アイウエオカキクケコアイウエオカキクケコアイウエオカキクケコ");
            Assert.Equal("アイウエオカキクケコアイウエオカキクケコアイウエオカキクケコ", act.Value);
        }

        [Fact]
        public void 名カナが30文字を超える場合例外()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PlayerFirstNameKana("アイウエオカキクケコアイウエオカキクケコアイウエオカキクケコア"));
            Assert.Equal("30 文字を超えています。 (Parameter '名(カナ)')", exception.Message);
        }

        [Theory]
        [InlineData("太郎")]
        [InlineData("tarou")]
        public void 名カナがカナ以外の場合例外(string value)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PlayerFirstNameKana(value));
            Assert.Equal("カナ以外が入力されています。 (Parameter '名(カナ)')", exception.Message);
        }
    }
}
