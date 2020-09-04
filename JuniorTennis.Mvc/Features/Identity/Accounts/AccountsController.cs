using JuniorTennis.Domain.UseCases.Identity.Accounts;
using JuniorTennis.Infrastructure.Identity;
using JuniorTennis.Mvc.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Identity.Accounts
{
    [Area("Identity")]
    public class AccountsController : Controller
    {
        private readonly IAccountsUseCase accountsUseCase;
        private readonly IAuthorizationUseCase authorizationUseCase;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger logger;
        private readonly AccountService accountService;

        public UrlSettings Options { get; }

        public AccountsController(
            IAccountsUseCase accountsUseCase,
            IAuthorizationUseCase authorizationUseCase,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory,
            IOptions<UrlSettings> optionsAccessor)
        {
            this.accountsUseCase = accountsUseCase;
            this.authorizationUseCase = authorizationUseCase;
            this.userManager = userManager;
            this.logger = loggerFactory.CreateLogger<AccountsController>();
            this.signInManager = signInManager;
            this.Options = optionsAccessor.Value;
            this.accountService = new AccountService(
                this.signInManager,
                accountsUseCase,
                authorizationUseCase);
        }

        /// <summary>
        /// 団体ログイン画面を表示します。
        /// </summary>
        /// <returns>団体ログイン画面</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// 確認メールアドレス入力画面を表示します。
        /// </summary>
        /// <returns>確認メールアドレス入力画面を。</returns>
        [HttpGet]
        public ActionResult ConfirmMail() => this.View(new ConfirmMailViewModel());

        /// <summary>
        /// 確認メールアドレスを認証テーブルに登録します。
        /// </summary>
        /// <param name="viewModel">確認メールアドレス入力画面ViewModel。</param>
        /// <returns>トップ画面。</returns>
        [HttpPost]
        public async Task<ActionResult> ConfirmMail([Bind(
            "MailAddress")]
         ConfirmMailViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var normalizedMailAddress = userManager.NormalizeEmail(viewModel.MailAddress);
            // 既にuserManagerにアドレスがあればそのuserを取得する
            var duplicatedMailAddress = await userManager.FindByEmailAsync(normalizedMailAddress);
            if (duplicatedMailAddress != null)
            {
                viewModel.IsDuplicated = true;
                return this.View(viewModel);
            }

            var authorizationLink = await this.accountsUseCase.AddAuthorizationLink(viewModel.MailAddress);
            await this.accountsUseCase.SendConfirmEmail(viewModel.MailAddress, authorizationLink.AuthorizationCode, this.Options.DomainUrl);

            return this.RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// パスワード再設定画面を表示します。
        /// </summary>
        /// <param name="authorizationCode">認証コード。</param>
        /// <returns>パスワード再設定画面</returns>
        [HttpGet]
        public ActionResult ChangeMailAddress([FromQuery] string authorizationCode)
        {
            var viewModel = new ChangeMailAddressViewModel
            {
                AuthorizationCode = authorizationCode
            };
            return this.View(viewModel);
        }

        /// <summary>
        /// パスワードを再設定します。
        /// </summary>
        /// <param name="viewModel">パスワード再設定画面ViewModel。</param>
        /// <returns>トップ画面。</returns>
        [HttpPost]
        public async Task<ActionResult> ChangeMailAddress([Bind("AuthorizationCode","MailAddress")]
            ChangeMailAddressViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var normalizedMailAddress = userManager.NormalizeEmail(viewModel.MailAddress);
            var duplicatedMailAddress = await userManager.FindByEmailAsync(normalizedMailAddress);
            if (duplicatedMailAddress != null)
            {
                viewModel.IsDuplicated = true;
                return this.View(viewModel);
            }

            // teamsの更新
            var authorizationLink = await this.accountsUseCase.GetAuthorizationLinkByCode(viewModel.AuthorizationCode);
            var deserializedTeamCode = authorizationLink.UniqueKey;
            var originalMailAddress = await this.accountsUseCase.UpdateTeamMailAddress(deserializedTeamCode, viewModel.MailAddress);

            // AspNetUserの更新
            var user = await userManager.FindByEmailAsync(userManager.NormalizeEmail(originalMailAddress));
            user.Email = viewModel.MailAddress;
            await userManager.UpdateAsync(user);

            // 登録完了通知メールの送信
            await this.accountsUseCase.SendChangeMailAddressMail(viewModel.MailAddress);

            return this.Redirect("/");
        }

        /// <summary>
        /// ログイン画面を表示します。
        /// </summary>
        /// <returns>ログイン画面。</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return this.View();
        }

        /// <summary>
        /// ログインを行います。
        /// </summary>
        /// <param name="model">ログインのビューモデル。</param>
        /// <returns>ログイン成功：お知らせ一覧。ロックアウト：ロックアウト画面。ログイン失敗：エラーメッセージ。</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind(
            "LoginId",
            "Password")]
            LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var loginResult = await this.accountService.Login(model.LoginId, model.Password);
            if (loginResult.Succeeded)
            {
                this.logger.LogInformation(1, "User logged in.");
                return this.RedirectToAction("Index", "Announcements", new { area = string.Empty });
            }

            if (loginResult.IsLockedOut)
            {
                this.logger.LogWarning(2, "User account locked out.");
                return this.View("Lockout");
            }
            else
            {
                this.ModelState.AddModelError("LoginError", "IDまたはパスワードが一致しません。");
                return this.View(model);
            }
        }

        /// <summary>
        /// パスワード変更問合せ画面を表示します。
        /// </summary>
        /// <returns>パスワード変更問合せ画面。</returns>
        [HttpGet]
        public IActionResult ChangePasswordInquiry()
        {
            return this.View(new ChangePasswordInquiryViewModel());
        }

        /// <summary>
        /// パスワード変更のメールを送信します。
        /// </summary>
        /// <param name="viewModel">POSTデータ。</param>
        /// <returns>パスワード変更問合せ画面。</returns>
        [HttpPost]
        public async Task<IActionResult> ChangePasswordInquiry(
            [Bind("MailAddress")] ChangePasswordInquiryViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var url = this.Url.ActionLink(nameof(this.SetupPassword));
            await this.accountService.SendSetupPasswordVerifyMail(
                viewModel.MailAddress,
                url);
            return this.RedirectToAction(nameof(this.ChangePasswordInquiry));
        }

        /// <summary>
        /// パスワード設定画面を表示します。
        /// </summary>
        /// <param name="authorizationCode">認証コード。</param>
        /// <param name="token">メール認証トークン。</param>
        /// <returns>パスワード設定画面。</returns>
        [HttpGet]
        public async Task<IActionResult> SetupPassword(
            [FromQuery] string authorizationCode,
            [FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                return this.NotFound();
            }

            var authorizationLink = await this.accountsUseCase.GetAuthorizationLinkByCode(authorizationCode);
            if (authorizationLink == null)
            {
                return this.NotFound();
            }

            return this.View(new SetupPasswordViewModel(authorizationCode, token));
        }

        /// <summary>
        /// パスワードを設定します。
        /// </summary>
        /// <param name="authorizationCode">認証コード。</param>
        /// <param name="viewModel">POSTデータ。</param>
        /// <returns>ログイン画面。</returns>
        [HttpPost]
        public async Task<IActionResult> SetupPassword(
            [FromQuery] string authorizationCode,
            [FromQuery] string token,
            [Bind("Password")] SetupPasswordViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(new SetupPasswordViewModel(authorizationCode, token, viewModel.Password));
            }

            var authorizationLink = await this.accountsUseCase.GetAuthorizationLinkByCode(authorizationCode);
            await this.accountService.SetupPassword(
                authorizationLink.UniqueKey,
                viewModel.Password,
                token);
            return this.RedirectToAction(nameof(this.Login));
        }
    }
}
