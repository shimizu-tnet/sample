using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using JuniorTennis.Domain.UseCases.Identity.Accounts;

namespace JuniorTennis.Mvc.Features.Identity.Accounts
{
    public class ChangeMailAddressViewModel
    {
        /// <summary>
        /// メールアドレス
        /// </summary>
        [Required(ErrorMessage = "メールアドレスを入力してください。")]
        [EmailAddress(ErrorMessage = "メールアドレスの形式で入力してください。")]
        public string MailAddress { get; set; }

        /// <summary>
        /// 認証コード
        /// </summary>
        public string AuthorizationCode { get; set; }

        /// <summary>
        /// 入力アドレスが重複している/いない
        /// </summary>
        public bool IsDuplicated { get; set; }
    }
}
