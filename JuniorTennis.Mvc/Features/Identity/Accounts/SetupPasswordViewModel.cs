using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Features.Identity.Accounts
{
    public class SetupPasswordViewModel
    {
        /// <summary>
        /// アクション用パラメーター一覧を取得します。
        /// </summary>
        public Dictionary<string, string> ActionParameters { get; }

        /// <summary>
        /// パスワードを取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "パスワードを入力してください。")]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; }

        /// <summary>
        /// パスワード設定ViewModelの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="authorizationCode">認証コード。</param>
        /// <param name="token">メール確認トークン。</param>
        public SetupPasswordViewModel(string authorizationCode, string token)
        {
            this.ActionParameters = new Dictionary<string, string>()
            {
                { "authorizationCode", authorizationCode },
                { "token", token }
            };
        }

        /// <summary>
        /// パスワード設定ViewModelの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="authorizationCode">認証コード。</param>
        /// <param name="token">メール確認トークン。</param>
        /// <param name="password">パスワード。</param>
        public SetupPasswordViewModel(string authorizationCode, string token, string password)
            :this(authorizationCode, token)
        {
            this.Password = password;
        }

        /// <summary>
        /// パスワード設定ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public SetupPasswordViewModel()
        {
        }
    }
}
