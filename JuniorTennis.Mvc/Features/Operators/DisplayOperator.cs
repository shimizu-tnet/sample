namespace JuniorTennis.Mvc.Features.Operators
{
    /// <summary>
    /// 管理ユーザーの表示用モデル。
    /// </summary>
    public class DisplayOperator
    {
        /// <summary>
        /// 管理ユーザーIDを取得します。
        /// </summary>
        public int OperatorId { get; }

        /// <summary>
        /// 権限名を取得します。
        /// </summary>
        public string RoleName { get; }

        /// <summary>
        /// 氏名を取得します。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// メールアドレスを取得します。
        /// </summary>
        public string EmailAddress { get; }

        /// <summary>
        /// ログインIDを取得します。
        /// </summary>
        public string LoginId { get; }

        /// <summary>
        /// 管理ユーザーの表示用モデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="operatorId">管理ユーザーID。</param>
        /// <param name="roleName">権限名。</param>
        /// <param name="name">氏名。</param>
        /// <param name="emailAddress">メールアドレス。</param>
        /// <param name="loginId">ログインID。</param>
        public DisplayOperator(int operatorId, string roleName, string name, string emailAddress, string loginId)
        {
            this.OperatorId = operatorId;
            this.RoleName = roleName;
            this.Name = name;
            this.EmailAddress = emailAddress;
            this.LoginId = loginId;
        }
    }
}
