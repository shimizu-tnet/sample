using JuniorTennis.SeedWork;
using System;

namespace JuniorTennis.Domain.Announcements
{
    /// <summary>
    /// お知らせ。
    /// </summary>
    public class Announcement : EntityBase
    {
        /// <summary>
        /// お知らせタイトルを取得します。
        /// </summary>
        public AnnouncementTitle AnnounceTitle { get; private set; }

        /// <summary>
        /// お知らせ本文を取得します。
        /// </summary>
        public string Body { get; private set; }

        /// <summary>
        /// お知らせ種別を取得します。
        /// </summary>
        public AnnouncementGenre AnnouncementGenre { get; private set; }

        /// <summary>
        /// 登録日を取得します。
        /// </summary>
        public RegisteredDate RegisteredDate { get; private set; }

        /// <summary>
        /// 終了日を取得します。
        /// </summary>
        public EndDate EndDate { get; private set; }

        /// <summary>
        /// 削除日時を取得します。
        /// </summary>
        public DateTime? DeletedDateTime { get; private set; }

        /// <summary>
        /// 添付ファイルパスを取得します。
        /// </summary>
        public AttachedFilePath AttachedFilePath { get; private set; }

        /// <summary>
        /// 添付ファイルパスが存在するかどうかを判定します。
        /// </summary>
        public bool HasAttachedFile => this.AttachedFilePath != null;

        /// <summary>
        /// お知らせの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="announceTitle">お知らせタイトル。</param>
        /// <param name="body">お知らせ本文。</param>
        /// <param name="announcementGenre">お知らせ種別。</param>
        /// <param name="registeredDate">登録日。</param>
        /// <param name="endDate">終了日。</param>
        /// <param name="attachedFilePath">添付ファイルパス。</param>
        public Announcement(AnnouncementTitle announceTitle, string body, AnnouncementGenre announcementGenre, RegisteredDate registeredDate, EndDate endDate, AttachedFilePath attachedFilePath)
        {
            this.AnnounceTitle = announceTitle ?? throw new ArgumentNullException("お知らせタイトル");
            this.Body = body;
            this.AnnouncementGenre = announcementGenre = announcementGenre ?? throw new ArgumentNullException("お知らせ種別"); ;
            this.RegisteredDate = registeredDate ?? throw new ArgumentNullException("登録日");
            if (endDate != null)
            {
                this.EnsureValidEndDates(this.RegisteredDate, endDate);
            }

            this.EndDate = endDate;
            this.AttachedFilePath = attachedFilePath;
        }

        /// <summary>
        /// お知らせを変更します。
        /// </summary>
        /// <param name="title">お知らせタイトル。</param>
        /// <param name="body">お知らせ本文。</param>
        /// <param name="announcementGenre">お知らせ種別。</param>
        /// <param name="enddate">終了日。</param>
        public void Change(AnnouncementTitle title, string body, AnnouncementGenre announcementGenre, EndDate enddate)
        {
            this.AnnounceTitle = title;
            this.Body = body;
            this.AnnouncementGenre = announcementGenre;
            this.EndDate = enddate;
        }

        /// <summary>
        /// お知らせを削除します。
        /// </summary>
        public void Delete()
        {
            this.DeletedDateTime = DateTime.Now;
        }

        /// <summary>
        /// 添付ファイルパスを削除します。
        /// </summary>
        public void DeleteAttachedFilePath()
        {
            this.AttachedFilePath = null;
        }

        /// <summary>
        /// 添付ファイルパスを変更します。
        /// </summary>
        /// <param name="attachedFilePath">添付ファイルパス。</param>
        public void ChangeAttachedFilePath(AttachedFilePath attachedFilePath)
        {
            this.AttachedFilePath = attachedFilePath;
        }

        private Announcement()
        {

        }

        /// <summary>
        /// 登録日が終了日以前に設定されているか判定します。
        /// </summary>
        /// <param name="registeredDate">登録日。</param>
        /// <param name="endDate">終了日。</param>
        private void EnsureValidEndDates(RegisteredDate registeredDate, EndDate endDate)
        {
            if (registeredDate.Value > endDate.Value)
            {
                throw new ArgumentException("終了日が登録日より前に設定されています。");
            }
        }
    }
}
