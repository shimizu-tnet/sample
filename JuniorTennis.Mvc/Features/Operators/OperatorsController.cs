using JuniorTennis.Domain.UseCases.Identity.Accounts;
using JuniorTennis.Domain.UseCases.Operators;
using JuniorTennis.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Operators
{
    /// <summary>
    /// 管理ユーザーのコントローラー。
    /// </summary>
    public class OperatorsController : Controller
    {
        private readonly OperatorService operatorService;

        public OperatorsController(
            IOperatorUseCase operatorUseCase,
            IAuthorizationUseCase authorizationUseCase,
            UserManager<ApplicationUser> userManager)
        {
            this.operatorService = new OperatorService(operatorUseCase, authorizationUseCase, userManager);
        }

        /// <summary>
        /// 管理ユーザー一覧を表示します。
        /// </summary>
        /// <returns>管理ユーザー一覧画面。</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var viewModel = await this.operatorService.CreateIndexViewModel();
            return this.View(viewModel);
        }

        /// <summary>
        /// 管理ユーザー新規登録画面を表示します。
        /// </summary>
        /// <returns>管理ユーザー新規登録画面。</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return this.View(new RegisterViewModel());
        }

        /// <summary>
        /// 管理ユーザーの新規登録します。
        /// </summary>
        /// <param name="model">管理ユーザー登録のビューモデル。</param>
        /// <returns>管理ユーザー一覧画面。</returns>
        [HttpPost]
        public async Task<IActionResult> Register([Bind(
            "SelectedRoleName",
            "Name",
            "EmailAddress",
            "LoginId")]
            RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var url = this.Url.ActionLink(
                controller: "Accounts",
                action: "SetupPassword",
                values: new { area = "Identity" });
            await this.operatorService.RegisterOperator(
                model.SelectedRoleName,
                model.Name,
                model.EmailAddress,
                model.LoginId,
                url);

            return this.RedirectToAction(nameof(this.Index));
        }

        /// <summary>
        /// 管理ユーザーの編集画面を表示します。
        /// </summary>
        /// <param name="id">管理ユーザーID。</param>
        /// <returns>管理ユーザー編集画面。</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await this.operatorService.CreateEditViewModel(id);
            return this.View(viewModel);
        }

        /// <summary>
        /// 管理ユーザーを更新します。
        /// </summary>
        /// <param name="model">管理ユーザー編集のビューモデル。</param>
        /// <returns>管理ユーザー一覧画面。</returns>
        [HttpPost]
        public async Task<IActionResult> Edit([Bind(
            "OperatorId",
            "SelectedRoleName",
            "Name",
            "EmailAddress")]
            EditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            await this.operatorService.UpdateOperator(
                model.OperatorId,
                model.SelectedRoleName,
                model.Name,
                model.EmailAddress);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
