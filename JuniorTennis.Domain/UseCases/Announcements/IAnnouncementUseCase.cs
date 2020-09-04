using JuniorTennis.Domain.Announcements;
using JuniorTennis.Domain.UseCases.Shared;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Announcements
{
    /// <summary>
    /// お知らせ管理
    /// </summary>
    public interface IAnnouncementUseCase
    {
        /// <summary>
        /// お知らせ一覧を登録日の降順で取得します。
        /// </summary>
        /// <param name="pageIndex">現在のページ番号。</param>
        /// <param name="displayCount">ページ当たりの表示件数。</param>
        /// <returns>お知らせ一覧</returns>
        Task<Pagable<Announcement>> GetAnnouncements(int pageIndex, int displayCount);

        /// <summary>
        /// お知らせを取得します。
        /// </summary>
        /// <param name="id">お知らせId。</param>
        /// <returns>お知らせ。</returns>
        Task<Announcement> GetAnnouncement(int id);

        /// <summary>
        /// お知らせを登録します。
        /// </summary>
        /// <param name="title">お知らせタイトル。</param>
        /// <param name="body">お知らせ本文。</param>
        /// <param name="announcementGenre">お知らせ種別。</param>
        /// <param name="endDate">終了日。</param>
        /// <param name="fileName">添付ファイル名。</param>
        /// <param name="fileStream">添付ファイルストリーム。</param>
        /// <returns></returns>
        Task<Announcement> RegisterAnnouncement(string title, string body, int announcementGenre, DateTime? endDate, string fileName, Stream fileStream);

        /// <summary>
        /// お知らせの更新します。
        /// </summary>
        /// <param name="title">お知らせタイトル。</param>
        /// <param name="id">お知らせId。</param>
        /// <param name="body">お知らせ本文。</param>
        /// <param name="announcementGenre">お知らせ種別。</param>
        /// <param name="endDate">終了日。</param>
        /// <param name="fileName">添付ファイル名</param>
        /// <param name="fileStream">添付ファイルストリーム。</param>
        /// <returns></returns>
        Task<Announcement> UpdateAnnouncement(int id, string title, string body, int announcementGenre, DateTime? endDate, string fileName, Stream fileStream);

        /// <summary>
        /// お知らせを論理削除します。
        /// </summary>
        /// <param name="id">お知らせId。</param>
        Task DeleteAnnouncement(int id);

        /// <summary>
        /// 添付ファイルのアップロードを行います。
        /// </summary>
        /// <param name="uploadFile">添付ファイル。</param>
        /// <returns>添付ファイルパス。</returns>
        Task<string> UploadFile(string fileName, Stream fileStream);

        /// <summary>
        /// 添付ファイルを削除します。
        /// </summary>
        /// <param name="id">お知らせId</param>
        Task DeleteAttachedFile(int id);
    }
}
