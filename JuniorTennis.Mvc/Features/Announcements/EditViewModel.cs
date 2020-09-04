using JuniorTennis.Domain.Announcements;
using JuniorTennis.Mvc.Features.Shared;
using JuniorTennis.Mvc.Validations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace JuniorTennis.Mvc.Features.Announcements
{
    /// <summary>
    /// お知らせ編集のビューモデル。
    /// </summary>
    public class EditViewModel
    {
        /// <summary>
        /// お知らせIdを取得または設定します。
        /// </summary>
        public int AnnouncementId { get; set; }

        /// <summary>
        /// お知らせタイトルを取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "タイトルを入力してください。")]
        [Display(Name = "タイトル")]
        public string AnnouncementTitle { get; set; }

        /// <summary>
        /// お知らせ本文を取得または設定します。
        /// </summary>
        [Display(Name = "本文")]
        public string Body { get; set; }

        /// <summary>
        /// お知らせ種別を取得または設定します。
        /// </summary>
        [Display(Name ="お知らせ種別")]
        public List<SelectListItem> AnnouncementGenres { get; set; }

        /// <summary>
        /// 選択されたお知らせ種別を取得または設定します。
        /// </summary>
        public string SelectedAnnouncementGenre { get; set; }

        /// <summary>
        /// 登録日を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "登録日を設定してください。")]
        [DataType(DataType.Date)]
        public DateTime RegisteredDate { get; set; }

        /// <summary>
        /// 終了日を取得または設定します。
        /// </summary>
        [Display(Name = "終了日")]
        [DateTimeAfter("RegisteredDate", "終了日が登録日より前に設定されています。")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 添付ファイル名を取得または設定します。
        /// </summary>
        public string AttachedFilePath { get; set; }

        /// <summary>
        /// 添付ファイルを取得または設定します。
        /// </summary>
        [Display(Name = "添付ファイル")]
        [DataType(DataType.Upload)]
        public IFormFile UploadFile { get; set; }

        /// <summary>
        /// 添付ファイルの名前を取得します。
        /// </summary>
        public string AttachedFileName => this.UploadFile?.FileName;

        /// <summary>
        /// 添付ファイルのストリームを取得します。
        /// </summary>
        public Stream AttachedFileStream => this.UploadFile?.OpenReadStream();

        /// <summary>
        /// 添付ファイル名を取得します。
        /// </summary>
        public string GetAttachedFileName() => Path.GetFileName(this.AttachedFilePath);

        /// <summary>
        /// 添付ファイルが存在するかどうかを判定します。
        /// </summary>
        public bool HasAttachedFile => this.AttachedFilePath != null;

        /// <summary>
        /// お知らせ編集のビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="announcementId">お知らせId。</param>
        /// <param name="announcementTitle">お知らせタイトル。</param>
        /// <param name="body">お知らせ本文。</param>
        /// <param name="announcementGenre">お知らせ種別。</param>
        /// <param name="registeredDate">登録日。</param>
        /// <param name="endDate">終了日。</param>
        /// <param name="attachedFilePath">添付ファイルパス。</param>
        public EditViewModel(
            int announcementId,
            string announcementTitle,
            string body,
            int announcementGenre,
            DateTime registeredDate,
            DateTime? endDate,
            string attachedFilePath
            )
        {
            this.AnnouncementId = announcementId;
            this.AnnouncementTitle = announcementTitle;
            this.Body = body;
            this.AnnouncementGenres = MvcViewHelper.CreateSelectListItem<AnnouncementGenre>();
            this.SelectedAnnouncementGenre = announcementGenre.ToString();
            this.RegisteredDate = registeredDate;
            this.EndDate = endDate;
            this.AttachedFilePath = attachedFilePath;
        }

        public EditViewModel() { }
    }
}
