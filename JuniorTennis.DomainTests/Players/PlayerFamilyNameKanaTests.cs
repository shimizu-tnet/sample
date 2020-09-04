using JuniorTennis.Domain.Players;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Players
{
    public class PlayerFamilyNameKanaTests
    {
        [Fact]
        public void 姓カナは20文字まで有効()
        {
            var act = new PlayerFamilyNameKana("アイウエオカキクケコアイウエオカキクケコ");
            Assert.Equal("アイウエオカキクケコアイウエオカキクケコ", act.Value);
        }

        [Fact]
        public void 姓カナが20文字を超える場合例外()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PlayerFamilyNameKana("アイウエオカキクケコアイウエオカキクケコア"));
            Assert.Equal("20 文字を超えています。 (Parameter '姓(カナ)')", exception.Message);
        }

        [Theory]
        [InlineData("大成")]
        [InlineData("taisei")]
        public void 姓カナがカナ以外の場合例外(string value)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new PlayerFamilyNameKana(value));
            Assert.Equal("カナ以外が入力されています。 (Parameter '姓(カナ)')", exception.Message);
        }
    }
}
