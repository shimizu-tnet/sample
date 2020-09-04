namespace JuniorTennis.Mvc.Features.Identity.Roles
{
    /// <summary>
    /// DeleteRole用のViewModel
    /// </summary>
    public class DeleteRoleViewModel
    {
        /// <summary>
        /// RoleName
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// RoleId
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// Roleに紐づくユーザー数
        /// </summary>
        public int MemberCount { get; set; }
    }
}
