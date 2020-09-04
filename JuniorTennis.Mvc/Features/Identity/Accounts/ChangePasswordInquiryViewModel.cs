using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Features.Identity.Accounts
{
    public class ChangePasswordInquiryViewModel
    {
        /// <summary>
        /// メールアドレスを取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "メールアドレスを入力してください。")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "メールアドレス")]
        public string MailAddress { get; set; }
    }
}
