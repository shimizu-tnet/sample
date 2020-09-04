using JuniorTennis.Mvc.Features.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Operators
{
    /// <summary>
    /// 管理ユーザー編集のビューモデル。
    /// </summary>
    public class EditViewModel
    {
        /// <summary>
        /// 管理ユーザーIDを取得または設定します。
        /// </summary>
        public int OperatorId { get; set; }

        /// <summary>
        /// 氏名を取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "氏名を入力してください。")]
        [Display(Name = "氏名")]
        public string Name { get; set; }

        /// <summary>
        /// メールアドレスを取得または設定します。
        /// </summary>
        [Required(ErrorMessage = "メールアドレスを入力してください。")]
        [EmailAddress(ErrorMessage = "メールアドレスを正しく入力してください。")]
        [Display(Name = "メールアドレス")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// ログインIDを取得または設定します。
        /// </summary>
        [Display(Name = "ログインID")]
        public string LoginId { get; set; }

        /// <summary>
        /// 権限名一覧を取得します。
        /// </summary>
        public List<SelectListItem> RoleNames { get; set; }

        /// <summary>
        /// 選択された権限名を取得または設定します。
        /// </summary>
        [Display(Name = "権限")]
        public string SelectedRoleName { get; set; }

        /// <summary>
        /// 編集画面のビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="operatorId">管理ユーザーID。</param>
        /// <param name="name">氏名。</param>
        /// <param name="emaliAddress">メールアドレス。</param>
        /// <param name="roleName">権限名。</param>
        public EditViewModel(int operatorId, string name, string emaliAddress, string loginId, string roleName)
        {
            this.OperatorId = operatorId;
            this.Name = name;
            this.EmailAddress = emaliAddress;
            this.LoginId = loginId;
            this.RoleNames = AppRoleName.GetOperatorRoles
            .Select(o => new SelectListItem(o.DisplayName, $"{o.Name}", o.Name == roleName))
            .ToList();
        }

        public EditViewModel() { } 
    }
}
