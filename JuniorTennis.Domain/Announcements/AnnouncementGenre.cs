using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Announcements
{
    /// <summary>
    /// お知らせ種別を定義します。
    /// </summary>
    public class AnnouncementGenre : Enumeration
    {
        /// <summary>
        /// お知らせ。
        /// </summary>
        public static readonly AnnouncementGenre News = new AnnouncementGenre(1, "お知らせ");

        /// <summary>
        /// その他ご案内。
        /// </summary>
        public static readonly AnnouncementGenre Guide = new AnnouncementGenre(2, "その他ご案内");

        public AnnouncementGenre(int id, string name) : base(id, name) { }
    }
}
