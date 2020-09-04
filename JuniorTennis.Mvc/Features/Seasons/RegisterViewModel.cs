using JuniorTennis.Mvc.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Features.Seasons
{
    /// <summary>
    /// 年度新規登録のビューモデル。
    /// </summary>
    public class RegisterViewModel
    {
        /// <summary>
        /// 年度を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "年度を入力してください。")]
        [Display(Name = "年度")]
        public string SeasonName { get; set; }

        /// <summary>
        /// 年度開始日を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "年度開始日を入力してください。")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        /// <summary>
        /// 年度終了日を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "年度終了日を入力してください。")]
        [DateTimeAfter("FromDate", false, "開始日よりも前の日付が入力されています。")]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        /// <summary>
        /// 登録受付開始日を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "登録受付開始日を入力してください。")]
        [DataType(DataType.Date)]
        [Display(Name = "登録受付開始日")]
        public DateTime RegistrationFromDate { get; set; }

        /// <summary>
        /// 団体登録料を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "団体登録料を入力してください。")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "半角数字のみ入力できます。")]
        [Display(Name = "団体登録料")]
        public int TeamRegistrationFee { get; set; }

        /// <summary>
        /// 選手登録料を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "選手登録料を入力してください。")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "半角数字のみ入力できます。")]
        [Display(Name = "選手登録料")]
        public int PlayerRegistrationFee { get; set; }

        /// <summary>
        /// 選手移籍料を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "選手移籍料を入力してください。")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "半角数字のみ入力できます。")]
        [Display(Name = "選手移籍料")]
        public int PlayerTradeFee { get; set; }

        /// <summary>
        /// 登録画面の新しいインスタンスを生成します。
        /// </summary>
        public RegisterViewModel()
        {
        }
    }
}
