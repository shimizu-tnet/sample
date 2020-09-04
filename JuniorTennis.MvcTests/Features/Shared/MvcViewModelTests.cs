using JuniorTennis.Domain.Announcements;
using JuniorTennis.Mvc.Features.Shared;
using Xunit;

namespace JuniorTennis.MvcTests.Features.Shared
{
    public class MvcViewModelTests
    {
        [Fact]
        public void AnnouncementGenreからSelectListItemの一覧を作成する()
        {
            var act = MvcViewHelper.CreateSelectListItem<AnnouncementGenre>();

            Assert.Equal("お知らせ", act[0].Text);
            Assert.Equal("その他ご案内", act[1].Text);
            Assert.False(act[0].Selected);
            Assert.False(act[1].Selected);
        }

        [Fact]
        public void AnnouncementGenreから選択されたIdがTrueになっているSelectListItemの一覧を作成する()
        {
            var act = MvcViewHelper.CreateSelectListItem<AnnouncementGenre>(1);

            Assert.Equal("お知らせ", act[0].Text);
            Assert.Equal("その他ご案内", act[1].Text);
            Assert.True(act[0].Selected);
            Assert.False(act[1].Selected);
        }
    }
}
