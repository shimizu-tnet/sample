using JuniorTennis.Domain.Announcements;
using JuniorTennis.SeedWork;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Announcements
{
    public class AnnouncementTests
    {
        [Fact]
        public void 登録日に終了日を超える日付は設定不可()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new Announcement(
                    new AnnouncementTitle("大会のお知らせ"),
                    "<h3>○○大会のおしらせ</h3><p>エントリー料の入金期限は7/27となっております。まだの方は早急にお振込みをお願い致します。</p>",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020, 4, 1)),
                    new EndDate(new DateTime(2020, 3, 31)),
                    new AttachedFilePath("/attached/filePath")
                    ));

            Assert.Equal(
                "終了日が登録日より前に設定されています。",
                exception.Message);
        }

        [Fact]
        public void 登録日と終了日が同じ場合はセット可能()
        {
            var act = new Announcement(
                   new AnnouncementTitle("大会のお知らせ"),
                   "<h3>○○大会のおしらせ</h3><p>エントリー料の入金期限は7/27となっております。まだの方は早急にお振込みをお願い致します。</p>",
                   AnnouncementGenre.News,
                   new RegisteredDate(new DateTime(2020, 4, 1)),
                   new EndDate(new DateTime(2020, 4, 1)),
                    new AttachedFilePath("/attached/filePath")
                   );

            var sameDate = new DateTime(2020, 4, 1);
            Assert.Equal(sameDate, act.RegisteredDate.Value);
            Assert.Equal(sameDate, act.EndDate.Value);
        }

        [Fact]
        public void 終了日が登録日より後の場合セット可能()
        {
            var act = new Announcement(
                new AnnouncementTitle("大会のお知らせ"),
                "<h3>○○大会のおしらせ</h3><p>エントリー料の入金期限は7/27となっております。まだの方は早急にお振込みをお願い致します。</p>",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 4, 30)),
                new AttachedFilePath("/attached/filePath")
                );

            Assert.Equal(new DateTime(2020, 4, 1), act.RegisteredDate.Value);
            Assert.Equal(new DateTime(2020, 4, 30), act.EndDate.Value);
        }

        [Fact]
        public void 登録日がNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Announcement(
                    new AnnouncementTitle("大会のお知らせ"),
                    "<h3>○○大会のおしらせ</h3><p>エントリー料の入金期限は7/27となっております。まだの方は早急にお振込みをお願い致します。</p>",
                    AnnouncementGenre.News,
                    null,
                    new EndDate(new DateTime(2020, 3, 31)),
                    new AttachedFilePath("/attached/filePath")
                    ));

            Assert.Equal("登録日", exception.ParamName);
        }

        [Fact]
        public void お知らせタイトルがNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Announcement(
                    null,
                    "<h3>○○大会のおしらせ</h3><p>エントリー料の入金期限は7/27となっております。まだの方は早急にお振込みをお願い致します。</p>",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020, 4, 1)),
                    new EndDate(new DateTime(2020, 4, 30)),
                    new AttachedFilePath("/attached/filePath")
                    ));

            Assert.Equal("お知らせタイトル", exception.ParamName);
        }

        [Fact]
        public void お知らせ種別がNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Announcement(
                    new AnnouncementTitle("大会のお知らせ"),
                    "<h3>○○大会のおしらせ</h3><p>エントリー料の入金期限は7/27となっております。まだの方は早急にお振込みをお願い致します。</p>",
                    null,
                    new RegisteredDate(new DateTime(2020, 4, 1)),
                    new EndDate(new DateTime(2020, 4, 30)),
                    new AttachedFilePath("/attached/filePath")
                    ));

            Assert.Equal("お知らせ種別", exception.ParamName);
        }

        [Fact]
        public void 削除日時がNullの場合でもセット可能()
        {
            var act = new Announcement(
                    new AnnouncementTitle("大会のお知らせ"),
                    "<h3>○○大会のおしらせ</h3><p>エントリー料の入金期限は7/27となっております。まだの方は早急にお振込みをお願い致します。</p>",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020, 4, 1)),
                    new EndDate(new DateTime(2020, 4, 30)),
                    new AttachedFilePath("/attached/filePath")
                    );

            Assert.Null(act.DeletedDateTime);
        }

        [Fact]
        public void 添付ファイルパスをNullに設定()
        {
            var act = new Announcement(
                    new AnnouncementTitle("大会のお知らせ"),
                    "<h3>○○大会のおしらせ</h3><p>エントリー料の入金期限は7/27となっております。まだの方は早急にお振込みをお願い致します。</p>",
                    AnnouncementGenre.News,
                    new RegisteredDate(new DateTime(2020, 4, 1)),
                    new EndDate(new DateTime(2020, 4, 30)),
                    new AttachedFilePath("/attached/filePath")
                    );
            act.DeleteAttachedFilePath();

            Assert.Null(act.AttachedFilePath);
        }

        [Fact]
        public void お知らせを変更()
        {
            var act = new Announcement(
                new AnnouncementTitle("大会のお知らせ"),
                "<h3>○○大会のおしらせ</h3><p>エントリー料の入金期限は7/27となっております。まだの方は早急にお振込みをお願い致します。</p>",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 4, 30)),
                new AttachedFilePath("/attached/filePath")
                );

            act.Change(new AnnouncementTitle("大会変更のお知らせ"), "大会が変更されました。", AnnouncementGenre.News, new EndDate(new DateTime(2020, 5, 1)));

            Assert.Equal(new AnnouncementTitle("大会変更のお知らせ"), act.AnnounceTitle);
            Assert.Equal("大会が変更されました。", act.Body);
            Assert.Equal(new EndDate(new DateTime(2020, 5, 1)), act.EndDate);
        }

        [Fact]
        public void お知らせを削除()
        {
            var act = new Announcement(
                new AnnouncementTitle("大会のお知らせ"),
                "<h3>○○大会のおしらせ</h3><p>エントリー料の入金期限は7/27となっております。まだの方は早急にお振込みをお願い致します。</p>",
                AnnouncementGenre.News,
                new RegisteredDate(new DateTime(2020, 4, 1)),
                new EndDate(new DateTime(2020, 4, 30)),
                new AttachedFilePath("/attached/filePath")
                );

            act.Delete();

            Assert.NotNull(act.DeletedDateTime);
        }
    }
}
