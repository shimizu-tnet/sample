using Microsoft.AspNetCore.Identity;

namespace JuniorTennis.Infrastructure.Identity
{
    /// <summary>
    /// 認証ロール。
    /// </summary>
    public class ApplicationRole : IdentityRole
    {
        /// <summary>
        /// 認証ロールの新しいインスタンスを生成します。
        /// </summary>
        public ApplicationRole() : base() { }

        /// <summary>
        /// 認証ロールの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="roleName">ロール名。</param>
        public ApplicationRole(string roleName) : base(roleName) { }
    }
}
