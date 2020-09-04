using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Features.Identity.Accounts
{
    /// <summary>
    /// ログインのビューモデル。
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// ログインIDを取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "IDを入力してください。")]
        [Display(Name = "ID")]
        [RegularExpression(@"^[0-9a-zA-Z]*$", ErrorMessage = "ログインIDは半角英数字のみで入力してください。")]
        [MaxLength(255, ErrorMessage = "ログインIDは255文字以内で入力してください。")]
        [MinLength(6, ErrorMessage = "ログインIDは6文字以上入力してください。")]
        public string LoginId { get; set; }

        /// <summary>
        /// パスワードを取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "パスワードを入力してください。")]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; }
    }
}
