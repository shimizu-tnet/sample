using JuniorTennis.Mvc.Features.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Operators
{
    /// <summary>
    /// 管理ユーザー登録のビューモデル。
    /// </summary>
    public class RegisterViewModel
    {
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
        [Required(ErrorMessage = "ログインIDを入力してください。")]
        [RegularExpression(@"^[0-9a-zA-Z]*$", ErrorMessage = "ログインIDは半角英数字のみで入力してください。")]
        [MaxLength(255, ErrorMessage = "ログインIDは255文字以内で入力してください。")]
        [MinLength(6, ErrorMessage = "ログインIDは6文字以上入力してください。")]
        [Display(Name = "ログインID")]
        public string LoginId { get; set; }

        /// <summary>
        /// 権限名を取得します。
        /// </summary>
        public List<SelectListItem> RoleNames => AppRoleName.GetOperatorRoles
            .Select(o => new SelectListItem(o.DisplayName, $"{o.Name}"))
            .ToList();

        /// <summary>
        /// 選択された権限名を取得または設定します。
        /// </summary>
        [Display(Name = "権限")]
        public string SelectedRoleName { get; set; }
    }
}
