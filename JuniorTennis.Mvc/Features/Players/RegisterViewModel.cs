using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JuniorTennis.Domain.UseCases.Players;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Mvc.Features.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JuniorTennis.Mvc.Features.Players
{
    /// <summary>
    /// 選手登録画面ViewModel。
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// 姓を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "姓を入力してください。")]
        [MaxLength(20)]
        [RegularExpression(@"^[\S]*$", ErrorMessage = "姓に空白文字が入力されています。")]
        public string PlayerFamilyName { get; set; }

        /// <summary>
        /// 名を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "名を入力してください。")]
        [MaxLength(20)]
        [RegularExpression(@"^[\S]*$", ErrorMessage = "名に空白文字が入力されています。")]
        public string PlayerFirstName { get; set; }

        /// <summary>
        /// 姓(カナ)を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "姓(カナ)を入力してください。")]
        [MaxLength(30)]
        [RegularExpression(@"^[ァ-ヶー]*$", ErrorMessage = "姓(カナ)はカタカナで入力してください。")]
        public string PlayerFamilyNameKana { get; set; }

        /// <summary>
        /// 名(カナ)を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "名(カナ)を入力してください。")]
        [MaxLength(30)]
        [RegularExpression(@"^[ァ-ヶー]*$", ErrorMessage = "名(カナ)はカタカナで入力してください。")]
        public string PlayerFirstNameKana { get; set; }

        /// <summary>
        /// 性別を取得または設定します。
        /// </summary>
        [Display(Name = "性別")]
        public int SelectedGender { get; set; }

        /// <summary>
        /// カテゴリーを取得または設定します。
        /// </summary>
        [Display(Name = "カテゴリー")]
        public int SelectedCategory { get; set; }

        /// <summary>
        /// 誕生日(年)を取得または設定します。
        /// </summary>
        public int BirthYear { get; set; }

        /// <summary>
        /// 誕生日(月)を取得または設定します。
        /// </summary>
        public int BirthMonth { get; set; }

        /// <summary>
        /// 誕生日(日)を取得または設定します。
        /// </summary>
        public int BirthDate { get; set; }

        /// <summary>
        /// 電話番号を取得または設定します。
        /// </summary>
        [Phone(ErrorMessage = "電話番号の形式で入力してください。")]
        [Display(Name = "電話番号")]
        public string TelephoneNumber { get; set; }

        /// <summary>
        /// 現在年度を取得または設定します。
        /// </summary>
        public int CurrentYear { get; set; }

        /// <summary>
        /// カテゴリーボタンを取得または設定します。
        /// </summary>
        public List<SelectListItem> CategoryButton { get; set; }

        /// <summary>
        /// 性別ボタンを取得または設定します。
        /// </summary>
        public List<SelectListItem> GenderButton { get; set; }

        /// <summary>
        /// 選手重複を取得または設定します。
        /// </summary>
        public bool IsDuplicated { get; set; }

        /// <summary>
        /// 不正誕生日かどうかを取得または設定します。
        /// </summary>
        public bool IsIllegalBirthDate { get; set; }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public RegisterViewModel()
        {
            this.CategoryButton = MvcViewHelper.CreateSelectListItem<Category>(Category.Under17Or18.Id);
            this.GenderButton = MvcViewHelper.CreateSelectListItem<Gender>(Gender.Boys.Id);
            this.CurrentYear = DateTime.Today.Year;
        }

        /// <summary>
        /// 不正誕生日かどうかを判定します。
        /// </summary>
        public bool ValidateBirthDate()
        {
            try
            {
                var birthDay = new DateTime(this.BirthYear, this.BirthMonth, this.BirthDate);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 選手登録用 DTO に変換します。
        /// </summary>
        /// <returns>選手登録用 DTO。</returns>
        public AddPlayerDto ToDto()
        {
            return new AddPlayerDto()
            {
                PlayerFamilyName = this.PlayerFamilyName,
                PlayerFirstName = this.PlayerFirstName,
                PlayerFamilyNameKana = this.PlayerFamilyNameKana,
                PlayerFirstNameKana = this.PlayerFirstNameKana,
                Gender = this.SelectedGender,
                Category = this.SelectedCategory,
                BirthDate = new DateTime(this.BirthYear, this.BirthMonth, this.BirthDate),
                TelephoneNumber = this.TelephoneNumber
            };
        }
    }
}
