using JuniorTennis.Domain.Accounts;
using JuniorTennis.SeedWork;
using System;

namespace JuniorTennis.Domain.Operators
{
    /// <summary>
    /// 管理ユーザー。
    /// </summary>
    public class Operator : EntityBase
    {
        /// <summary>
        /// 氏名を取得します。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// メールアドレスを取得します。
        /// </summary>
        public EmailAddress EmailAddress { get; private set; }

        /// <summary>
        /// ログインIdを取得します。
        /// </summary>
        public LoginId LoginId { get; private set;}

        /// <summary>
        /// 管理ユーザーの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="name">氏名。</param>
        /// <param name="emailAddress">メールアドレス。</param>
        /// <param name="loginId">ログインId。</param>
        public Operator(string name, EmailAddress emailAddress, LoginId loginId)
        {
            this.Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException("名前") : name;
            this.EmailAddress = emailAddress ?? throw new ArgumentNullException("メールアドレス");
            this.LoginId = loginId ?? throw new ArgumentNullException("ログインId");
        }

        /// <summary>
        /// 管理ユーザーを更新します。
        /// </summary>
        /// <param name="name">氏名。</param>
        /// <param name="emailAddress">メールアドレス。</param>
        public void Change(string name, EmailAddress emailAddress)
        {
            this.Name = name;
            this.EmailAddress = emailAddress;
        }
    }
}
