using JuniorTennis.Domain.Announcements;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Announcements
{
    public class AttachedFilePathTests
    {
        [Fact]
        public void 添付ファイルパスを正しく取得()
        {
            var act = new AttachedFilePath("/attached/filePath");

            Assert.Equal("/attached/filePath", act.Value);
        }

        [Fact]
        public void 添付ファイルパスがnullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new AttachedFilePath(null));
            Assert.Equal("添付ファイルパス", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("　")]
        public void 添付ファイルパスが未入力の場合例外(string value)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new AttachedFilePath(value));
            Assert.Equal("未入力です。 (Parameter '添付ファイルパス')", exception.Message);
        }
    }
}
