using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Accounts
{
    /// <summary>
    /// 認証情報管理。
    /// </summary>
    public interface IAuthorizationLinkRepository
    {
        /// <summary>
        /// 認証codeに紐づくユニークキーを取得します
        /// </summary>
        /// <param name="authorizationCode">認証コード</param>
        /// <returns>ユニークキー</returns>
        Task<AuthorizationLink> FindByAuthorizationCodeAsync(AuthorizationCode authorizationCode);

        /// <summary>
        /// ユニークキーを登録します。
        /// </summary>
        /// <param name="entity">認証情報entity。</param>
        /// <returns>Task。</returns>
        Task<AuthorizationLink> Add(AuthorizationLink entity);
    }
}
