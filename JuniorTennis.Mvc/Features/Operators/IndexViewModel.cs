using JuniorTennis.Domain.Operators;
using JuniorTennis.Mvc.Features.Identity;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Operators
{
    /// <summary>
    /// 管理ユーザー一覧のビューモデル。
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// 管理ユーザー一覧を取得します。
        /// </summary>
        public IReadOnlyList<DisplayOperator> Operators { get; }

        /// <summary>
        /// 管理ユーザー一覧のビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="operators">管理ユーザー一覧。</param>
        /// <param name="userAppRoleNames">ユーザーに紐づく権限名一覧。</param>
        public IndexViewModel(List<Operator> operators, Dictionary<string, AppRoleName> userAppRoleNames)
        {
            this.Operators = operators.Select(o => new DisplayOperator(
                    o.Id,
                    userAppRoleNames[o.LoginId.Value].DisplayName,
                    o.Name,
                    o.EmailAddress.Value,
                    o.LoginId.Value))
                .ToList();
        }
    }
}
