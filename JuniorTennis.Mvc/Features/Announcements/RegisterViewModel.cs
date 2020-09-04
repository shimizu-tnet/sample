using JuniorTennis.Domain.Announcements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using JuniorTennis.Mvc.Features.Shared;

namespace JuniorTennis.Mvc.Features.Announcements
{
    /// <summary>
    /// お知らせ新規登録のビューモデル。
    /// </summary>
    public class RegisterViewModel
    {
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
        [Display(Name = "お知らせ種別")]
        public List<SelectListItem> AnnouncementGenres { get; set; }

        /// <summary>
        /// 選択されたお知らせ種別を取得または設定します。
        /// </summary>
        public string SelectedAnnouncementGenre { get; set; }

        /// <summary>
        /// 終了日を取得または設定します。
        /// </summary>
        [Display(Name = "終了日")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

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
        /// 登録画面の新しインスタンスを生成します。
        /// </summary>
        public RegisterViewModel()
        {
            this.AnnouncementGenres = MvcViewHelper.CreateSelectListItem<AnnouncementGenre>(AnnouncementGenre.News.Id);
        }
    }
}
