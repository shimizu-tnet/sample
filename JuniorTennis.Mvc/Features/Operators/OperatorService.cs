using JuniorTennis.Domain.UseCases.Identity.Accounts;
using JuniorTennis.Domain.UseCases.Operators;
using JuniorTennis.Infrastructure.Identity;
using JuniorTennis.Mvc.Features.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JuniorTennis.Mvc.Features.Operators
{
    /// <summary>
    /// 管理ユーザーのサービス。
    /// </summary>
    public class OperatorService
    {
        private readonly IOperatorUseCase operatorUseCase;
        private readonly IAuthorizationUseCase authorizationUseCase;
        private readonly UserManager<ApplicationUser> userManager;

        public OperatorService(
            IOperatorUseCase operatorUseCase,
            IAuthorizationUseCase authorizationUseCase,
            UserManager<ApplicationUser> userManager)
        {
            this.operatorUseCase = operatorUseCase;
            this.authorizationUseCase = authorizationUseCase;
            this.userManager = userManager;
        }

        /// <summary>
        /// 管理ユーザー一覧のビューモデルを生成します。
        /// </summary>
        /// <returns>管理ユーザー一覧のビューモデル。</returns>
        public async Task<IndexViewModel> CreateIndexViewModel()
        {
            var operators = await this.operatorUseCase.GetOperators();
            var userAppRoleNames = new Dictionary<string, AppRoleName>();
            foreach (var appRoleName in AppRoleName.GetOperatorRoles)
            {
                var applcationUsers = await this.userManager.GetUsersInRoleAsync(appRoleName.Name);
                foreach (var applcationUser in applcationUsers)
                {
                    userAppRoleNames.Add(applcationUser.UserName, appRoleName);
                }
            }
            return new IndexViewModel(operators, userAppRoleNames);
        }

        /// <summary>
        /// 管理ユーザー新規登録、認証ユーザーの新規登録、招待メールの送信を行います。
        /// </summary>
        /// <param name="roleName">権限名。</param>
        /// <param name="name">氏名。</param>
        /// <param name="emailAddress">メールアドレス。</param>
        /// <param name="loginId">ログインID。</param>
        /// <param name="url">送付するURL。</param>
        public async Task RegisterOperator(string roleName, string name, string emailAddress, string loginId, string url)
        {
            // 管理ユーザーの登録。
            await this.operatorUseCase.RegisterOperator(name, emailAddress, loginId);

            // 認証ユーザーの登録。
            var newUser = new ApplicationUser() { UserName = loginId, Email = emailAddress };
            await this.userManager.CreateAsync(newUser);

            // 認証ユーザーにRoleを追加。
            await this.userManager.AddToRoleAsync(newUser, roleName);

            // メールの送信。
            var invitaionUrl = await this.CreateInvitationUrl(url, newUser);
            await this.operatorUseCase.SendOperatorInvitaionMail(emailAddress, invitaionUrl);
        }

        /// <summary>
        /// 管理ユーザーの編集ビューモデルを生成します。
        /// </summary>
        /// <param name="id">管理ユーザーID。</param>
        /// <returns>管理ユーザー編集のビューモデル。</returns>
        public async Task<EditViewModel> CreateEditViewModel(int id)
        {
            var editOperator = await this.operatorUseCase.GetOperator(id);
            var appUser = await this.userManager.FindByNameAsync(editOperator.LoginId.Value);
            var roleName = this.userManager.GetRolesAsync(appUser).Result.FirstOrDefault();
            var viewModel = new EditViewModel(
                id,
                editOperator.Name,
                editOperator.EmailAddress.Value,
                editOperator.LoginId.Value,
                roleName);

            return viewModel;
        }

        /// <summary>
        /// 管理ユーザーの更新認証ユーザーの更新を行います。
        /// </summary>
        /// <param name="id">管理ユーザーID。</param>
        /// <param name="newRoleName">権限名。</param>
        /// <param name="name">氏名</param>
        /// <param name="emailAddress">メールアドレス。</param>
        public async Task UpdateOperator(int id, string newRoleName, string name, string emailAddress)
        {
            // 管理ユーザーの更新。
            var updateOperator = await this.operatorUseCase.UpdateOperator(id, name, emailAddress);

            // 認証ユーザーの更新。
            var user = await this.userManager.FindByNameAsync(updateOperator.LoginId.Value);
            if (user.Email != emailAddress)
            {
                user.Email = emailAddress;
                await this.userManager.UpdateAsync(user);
            }

            var userRoles = await this.userManager.GetRolesAsync(user);
            var userRole = userRoles.FirstOrDefault();
            if (userRole != newRoleName)
            {
                await this.userManager.RemoveFromRoleAsync(user, userRole);
                await this.userManager.AddToRoleAsync(user, newRoleName);
            }
        }

        /// <summary>
        /// 招待URLを生成します。
        /// </summary>
        /// <param name="url">URL。</param>
        /// <param name="applicationUser">認証ユーザー。</param>
        /// <returns>招待URL。</returns>
        private async Task<string> CreateInvitationUrl(string url, ApplicationUser applicationUser)
        {
            var authorizationLink = await authorizationUseCase.AddAuthorizationLink(applicationUser.UserName);
            var authorizationCode = HttpUtility.UrlEncode(authorizationLink.AuthorizationCode.Value);
            var emailConfirmationToken = await this.userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
            var token = HttpUtility.UrlEncode(emailConfirmationToken);
            return $"{url}?authorizationCode={authorizationCode}&token={token}";
        }
    }
}
