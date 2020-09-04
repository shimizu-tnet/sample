using JuniorTennis.Domain.Teams;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Teams
{
    public class TeamAbbreviatedNameTests
    {
        [Fact]
        public void 団体名略称は13文字まで有効()
        {
            var act = new TeamAbbreviatedName("あいうえおかきくけこあいう");
            Assert.Equal("あいうえおかきくけこあいう", act.Value);
        }

        [Fact]
        public void 団体名略称がnullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new TeamAbbreviatedName(null));
            Assert.Equal("団体名略称", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("　")]
        public void 団体名略称が未入力の場合例外(string value)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new TeamAbbreviatedName(value));
            Assert.Equal("未入力です。 (Parameter '団体名略称')", exception.Message);
        }

        [Fact]
        public void 団体名略称が13文字を超える場合例外()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new TeamAbbreviatedName("あいうえおかきくけこあいうえ"));
            Assert.Equal("13 文字を超えています。 (Parameter '団体名略称')", exception.Message);
        }

        [Theory]
        [InlineData("０")]
        [InlineData("Ａ")]
        [InlineData("ａ")]
        public void 団体名略称が全角英数字の場合例外(string value)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new TeamAbbreviatedName(value));
            Assert.Equal("全角英数字が入力されています。 (Parameter '団体名略称')", exception.Message);
        }

        [Theory]
        [InlineData("ｱ")]
        public void 団体名略称が半角カナの場合例外(string value)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new TeamAbbreviatedName(value));
            Assert.Equal("半角カナが入力されています。 (Parameter '団体名略称')", exception.Message);
        }

        [Fact]
        public void 団体名略称にParseする際に全角英数字と半角カナを変換()
        {
            var act = TeamAbbreviatedName.Parse("0Aa０Ａａアｱ");
            Assert.Equal("0Aa0Aaアア", act.Value);
        }
    }
}
