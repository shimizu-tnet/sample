using JuniorTennis.Domain.Announcements;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Announcements
{
    public class AnnouncementTitleTests
    {
        [Fact]
        public void お知らせタイトルを正しく取得()
        {
            var act = new AnnouncementTitle("大会のお知らせ");

            Assert.Equal("大会のお知らせ", act.Value);
        }

        [Fact]
        public void お知らせタイトルがNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new AnnouncementTitle(null));
            Assert.Equal("お知らせタイトル", exception.ParamName);
        }

        [Fact]
        public void お知らせタイトルが空文字の場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new AnnouncementTitle(""));
            Assert.Equal("お知らせタイトル", exception.ParamName);
        }
    }
}
