using JuniorTennis.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Identity.Accounts
{
    public interface IAuthorizationUseCase
    {
        /// <summary>
        /// 認証情報を認証テーブルに登録します
        /// </summary>
        /// <param name="uniqueKey">ユニークキー</param>
        /// <returns>認証リンク。</returns>
        Task<AuthorizationLink> AddAuthorizationLink(string uniqueKey);
    }
}
