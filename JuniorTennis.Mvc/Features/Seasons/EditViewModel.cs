using JuniorTennis.Mvc.Validations;
using System;
using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Features.Seasons
{
    /// <summary>
    /// 年度編集のビューモデル。
    /// </summary>
    public class EditViewModel
    {
        /// <summary>
        /// 年度Idを取得または設定します。
        /// </summary>
        public int SeasonId { get; set; }

        /// <summary>
        /// 年度を取得または設定します。
        /// </summary>
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
        /// 編集画面のビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id">年度Id。</param>
        /// <param name="seasonName">年度。</param>
        /// <param name="fromDate">年度開始日。</param>
        /// <param name="toDate">年度終了日。</param>
        /// <param name="registrationFromDate">年度登録受付開始日。</param>
        /// <param name="teamRegistrationFee">団体登録料。</param>
        /// <param name="playerRegistrationFee">選手登録料。</param>
        /// <param name="playerTradeFee">選手移籍料。</param>
        public EditViewModel(
            int id,
            string seasonName,
            DateTime fromDate,
            DateTime toDate,
            DateTime registrationFromDate,
            int teamRegistrationFee,
            int playerRegistrationFee,
            int playerTradeFee
            )
        {
            this.SeasonId = id;
            this.SeasonName = seasonName;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.RegistrationFromDate = registrationFromDate;
            this.TeamRegistrationFee = teamRegistrationFee;
            this.PlayerRegistrationFee = playerRegistrationFee;
            this.PlayerTradeFee = playerTradeFee;
        }

        /// <summary>
        /// 編集画面のビューモデルの新しいインスタンスを生成します。
        /// </summary>
        public EditViewModel() { }
    }
}
