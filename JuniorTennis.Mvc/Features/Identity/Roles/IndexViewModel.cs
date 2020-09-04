using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Identity.Roles
{
    public class IndexViewModel
    {
        /// <summary>
        /// DBから取得したRoleNameの一覧を取得します。
        /// </summary>
        public IReadOnlyList<string> RoleNames { get; }

        /// <summary>
        /// 新規登録を行うRoleNameの一覧を取得します。
        /// </summary>
        public IReadOnlyList<string> AddRoleNames { get; }

        /// <summary>
        /// 削除を行うRoleの一覧を取得します。
        /// </summary>
        public IReadOnlyList<DeleteRoleViewModel> DeleteRoles { get; }

        /// <summary>
        /// 削除を行うRoleNameの一覧を取得します。
        /// </summary>
        public IReadOnlyList<string> DeleteRoleNames { get; }

        /// <summary>
        /// Roleの新規登録を行うか取得します。
        /// </summary>
        public bool HasUnregistered => this.AddRoleNames.Any();

        /// <summary>
        /// Roleの削除を行うか取得します。
        /// </summary>
        public bool HasUndeleted => this.DeleteRoles.Any();

        public IndexViewModel(List<string> roleNames, List<string> addRoleNames, List<DeleteRoleViewModel> deleteRoles)
        {
            this.RoleNames = roleNames;
            this.AddRoleNames = addRoleNames;
            this.DeleteRoles = deleteRoles;
        }
    }
}
