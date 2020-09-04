using JuniorTennis.SeedWork;
using System;

namespace JuniorTennis.Domain.Accounts
{
    /// <summary>
    /// 認証情報。
    /// </summary>
    public class AuthorizationLink : EntityBase
    {
        /// <summary>
        /// 認証コードを取得します。
        /// </summary>
        public AuthorizationCode AuthorizationCode { get; private set; }

        /// <summary>
        /// ユニークキーを取得します。
        /// </summary>
        public string UniqueKey { get; private set; }

        /// <summary>
        /// 登録日時を取得します。
        /// </summary>
        public DateTime RegistrationDate { get; private set; }

        /// <summary>
        /// 認証情報の新しいインスタンスを生成します。
        /// <param name="uniqueKey">ユニークキー。</param>
        /// <param name="registrationDate">登録日時。</param>
        /// </summary>
        public AuthorizationLink(
            string uniqueKey,
            DateTime registrationDate
            )
        {
            this.AuthorizationCode = new AuthorizationCode();
            this.UniqueKey = uniqueKey;
            this.RegistrationDate = registrationDate;
        }
    }
}
