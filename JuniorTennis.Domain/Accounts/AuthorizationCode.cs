using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JuniorTennis.Domain.Accounts
{
    /// <summary>
    /// 認証コード。
    /// </summary>
    public class AuthorizationCode : ValueObject
    {
        /// <summary>
        /// 認証コードを取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 認証コードの新しいインスタンスを生成します。
        /// </summary>
        public AuthorizationCode()
        {
            this.Value = Guid.NewGuid().ToString().Replace("-", String.Empty);
        }

        /// <summary>
        /// 取得した値から認証コードのインスタンスを生成します。
        /// <param name="value">取得した値。</param>
        /// </summary>
        public AuthorizationCode(string value)
        {
            this.Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
