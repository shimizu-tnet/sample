using JuniorTennis.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using JuniorTennis.Domain.UseCases.Identity.Accounts;

namespace JuniorTennis.Mvc.Features.Identity.Accounts
{
    /// <summary>
    /// アカウントのサービス。
    /// </summary>
    public class AccountService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IAccountsUseCase accountsUseCase;
        private readonly IAuthorizationUseCase authorizationUseCase;

        public AccountService(
            SignInManager<ApplicationUser> signInManager,
            IAccountsUseCase accountsUseCase,
            IAuthorizationUseCase authorizationUseCase)
        {
            this.signInManager = signInManager;
            this.accountsUseCase = accountsUseCase;
            this.authorizationUseCase = authorizationUseCase;
        }

        /// <summary>
        /// ログインを行います。
        /// </summary>
        /// <param name="loginId">ログインID。</param>
        /// <param name="password">パスワード。</param>
        /// <returns>ログインの結果。</returns>
        public async Task<SignInResult> Login(string loginId, string password)
        {
            return await this.signInManager.PasswordSignInAsync(loginId, password, isPersistent: false, lockoutOnFailure: false);
        }

        /// <summary>
        /// パスワードを設定します。
        /// </summary>
        /// <param name="userName">ユーザーネーム。</param>
        /// <param name="password">パスワード。</param>
        /// <param name="emailConfirmationToken">メール確認トークン。</param>
        /// <returns>Task。</returns>
        public async Task SetupPassword(string userName, string password, string emailConfirmationToken)
        {
            var userManager = this.signInManager.UserManager;
            var user = await userManager.FindByNameAsync(userName);
            if (emailConfirmationToken != null)
            {
                await userManager.ConfirmEmailAsync(user, emailConfirmationToken);
            }

            await userManager.RemovePasswordAsync(user);
            await userManager.AddPasswordAsync(user, password);
        }

        /// <summary>
        /// メールアドレスに紐づくユーザーが存在するかどうか確認します。
        /// </summary>
        /// <param name="mailAddress">メールアドレス。</param>
        /// <returns>trueの場合存在します。</returns>
        public async Task<bool> ExistApplicationUser(string mailAddress)
        {
            var user = await this.signInManager.UserManager.FindByEmailAsync(mailAddress);
            return user != null;
        }

        /// <summary>
        /// パスワード設定の認証メールを送信します。
        /// メールアドレスに一致する認証ユーザーが存在しない場合何もしません。
        /// </summary>
        /// <param name="mailAddress">メールアドレス。</param>
        /// <param name="url">送付するURL。</param>
        /// <returns>Task。</returns>
        public async Task SendSetupPasswordVerifyMail(string mailAddress, string url)
        {
            var user = await this.signInManager.UserManager.FindByEmailAsync(mailAddress);
            if (user == null)
            {
                return;
            }

            var authorizationLink = await this.authorizationUseCase.AddAuthorizationLink(user.UserName);
            var linkUrl = $"{url}?authorizationCode={authorizationLink.AuthorizationCode.Value}";
            await this.accountsUseCase.SendSetupPasswordVerifyMail(mailAddress, linkUrl);
        }
    }
}
