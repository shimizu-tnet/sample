using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Features.Association.Teams
{
    /// <summary>
    /// パスワード再設定要求メール送信画面のViewModel
    /// </summary>
    public class RequestChangeMailAddressViewModel
    {
        /// <summary>
        /// 確認用メールアドレス
        /// </summary>
        [Required(ErrorMessage = "メールアドレスを入力してください。")]
        [EmailAddress(ErrorMessage = "メールアドレスの形式で入力してください。")]
        public string MailAddress { get; set; }

        /// <summary>
        /// 変更対象の団体番号
        /// </summary>
        public string TeamCode { get; set; }
    }
}
