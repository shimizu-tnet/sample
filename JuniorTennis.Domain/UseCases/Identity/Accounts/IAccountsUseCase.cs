using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JuniorTennis.Domain.Accounts;

namespace JuniorTennis.Domain.UseCases.Identity.Accounts
{
    public interface IAccountsUseCase
    {
        /// <summary>
        /// メールアドレス確認メールを送信します
        /// </summary>
        /// <param name="mailAddress">送信先emailアドレス。</param>
        /// <param name="authorizationCode">認証コード。</param>
        /// <param name="domainUrl">ドメインURL。</param>
        /// <returns>Task。</returns>
        Task SendConfirmEmail(string mailAddress, AuthorizationCode authorizationCode, string domainUrl);

        /// <summary>
        /// 認証情報を認証テーブルに登録します
        /// </summary>
        /// <param name="uniqueKey">ユニークキー</param>
        /// <returns>認証情報。</returns>
        Task<AuthorizationLink> AddAuthorizationLink(string uniqueKey);

        /// <summary>
        /// 認証コードと紐づく認証情報を取得します
        /// </summary>
        /// <param name="authorizationCode">認証コード</param>
        /// <returns>認証情報。</returns>
        Task<AuthorizationLink> GetAuthorizationLinkByCode(string authorizationCode);

        /// <summary>
        /// 団体メールアドレスを再設定します
        /// </summary>
        /// <param name="teamCode">団体コード</param>
        /// <param name="mailAddress">メールアドレス</param>
        /// <returns>変更前のメールアドレス。</returns>
        Task<string> UpdateTeamMailAddress(string teamCode, string mailAddress);

        /// <summary>
        /// メールアドレス再設定後の通知メールを送信します。
        /// </summary>
        /// <param name="mailAddress">メールアドレス</param>
        /// <returns>Task</returns>
        Task SendChangeMailAddressMail(string mailAddress);

        /// <summary>
        /// パスワード設定の認証メールを送信
        /// </summary>
        /// <param name="mailAddress">送信先メールアドレス。</param>
        /// <param name="linkUrl">リンクURL。</param>
        /// <returns>Task。</returns>
        Task SendSetupPasswordVerifyMail(string mailAddress, string linkUrl);
    }
}
