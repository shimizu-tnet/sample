using JuniorTennis.Domain.Announcements;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.Mvc.Features.Shared.Pagination;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Announcements
{
    /// <summary>
    /// お知らせ一覧のビューモデル。
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// お知らせ一覧を取得します。
        /// </summary>
        public PagedList<Announcement> Annoucements;

        /// <summary>
        /// お知らせ一覧新しいビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="annoucements">お知らせ一覧。</param>
        public IndexViewModel(Pagable<Announcement> announcements)
        {
            this.Annoucements = new PagedList<Announcement>(
                 announcements.List.ToList(),
                 announcements.PageIndex,
                 announcements.TotalCount,
                 announcements.DisplayCount);
        }
    }
}
