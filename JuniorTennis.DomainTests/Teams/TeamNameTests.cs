using JuniorTennis.Domain.Teams;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace JuniorTennis.DomainTests.Teams
{
    public class TeamNameTests
    {
        [Fact]
        public void 団体名称は50文字まで有効()
        {
            var act = new TeamName("あいうえおかきくけこあいうえおかきくけこあいうえおかきくけこあいうえおかきくけこあいうえおかきくけこ");
            Assert.Equal("あいうえおかきくけこあいうえおかきくけこあいうえおかきくけこあいうえおかきくけこあいうえおかきくけこ", act.Value);
        }

        [Fact]
        public void 団体名称がnullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new TeamName(null));
            Assert.Equal("団体名称", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("　")]
        public void 団体名称が未入力の場合例外(string value)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new TeamName(value));
            Assert.Equal("未入力です。 (Parameter '団体名称')", exception.Message);
        }

        [Fact]
        public void 団体名称が50文字を超える場合例外()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new TeamName("あいうえおかきくけこあいうえおかきくけこあいうえおかきくけこあいうえおかきくけこあいうえおかきくけこあ"));
            Assert.Equal("50 文字を超えています。 (Parameter '団体名称')", exception.Message);
        }
    }
}
