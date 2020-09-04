using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Announcements
{
    /// <summary>
    /// お知らせ管理。
    /// </summary>
    public interface IAnnouncementRepository
    {
        /// <summary>
        /// お知らせ一覧を取得します。
        /// </summary>
        /// <returns>お知らせ一覧。</returns>
        Task<List<Announcement>> Find();

        /// <summary>
        /// お知らせを取得します。
        /// </summary>
        /// <param name="id">お知らせID。</param>
        /// <returns>お知らせ。</returns>
        Task<Announcement> FindById(int id);

        /// <summary>
        /// お知らせを更新します。
        /// </summary>
        /// <param name="announcement">お知らせ。</param>
        Task<Announcement> Update(Announcement announcement);

        /// <summary>
        /// 新規のお知らせを登録します。
        /// </summary>
        /// <param name="announcement">お知らせ。</param>
        /// <returns>お知らせ。</returns>
        Task<Announcement> Add(Announcement announcement);
    }
}
