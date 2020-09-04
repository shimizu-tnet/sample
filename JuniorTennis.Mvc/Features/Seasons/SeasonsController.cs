using System.Threading.Tasks;
using JuniorTennis.Domain.UseCases.Seasons;
using Microsoft.AspNetCore.Mvc;

namespace JuniorTennis.Mvc.Features.Seasons
{
    /// <summary>
    /// 年度のコントローラー。
    /// </summary>
    public class SeasonsController : Controller
    {
        private readonly ISeasonUseCase useCase;

        public SeasonsController(ISeasonUseCase useCase)
        {
            this.useCase = useCase;
        }

        /// <summary>
        /// 年度一覧を表示します。 
        /// </summary>
        /// <returns>年度一覧画面。</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return this.View(new IndexViewModel(await this.useCase.GetSeasons()));
        }

        /// <summary>
        /// 年度新規登録画面を表示します。
        /// </summary>
        /// <returns>年度新規登録画面。</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return this.View(new RegisterViewModel());
        }

        /// <summary>
        /// 年度を登録します。
        /// </summary>
        /// <param name="model">年度新規登録のビューモデル。</param>
        /// <returns>年度一覧画面。</returns>
        [HttpPost]
        public async Task<IActionResult> Register([Bind(
            "SeasonName",
            "FromDate",
            "ToDate",
            "RegistrationFromDate",
            "TeamRegistrationFee",
            "PlayerRegistrationFee",
            "PlayerTradeFee")]
            RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var season = await this.useCase.RegisterSeason(
                model.SeasonName,
                model.FromDate,
                model.ToDate,
                model.RegistrationFromDate,
                model.TeamRegistrationFee,
                model.PlayerRegistrationFee,
                model.PlayerTradeFee);

            return this.RedirectToAction(nameof(this.Edit), new { id = season.Id });
        }

        /// <summary>
        /// 年度編集画面を表示します。
        /// </summary>
        /// <param name="id">年度Id。</param>
        /// <returns>年度編集画面</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var season = await this.useCase.GetSeason(id);

            var viewModel = new EditViewModel(
                season.Id,
                season.Name,
                season.FromDate,
                season.ToDate,
                season.RegistrationFromDate,
                season.TeamRegistrationFee.Value,
                season.PlayerRegistrationFee.Value,
                season.PlayerTradeFee.Value
                );

            return this.View(viewModel);
        }

        /// <summary>
        /// 年度を更新します。
        /// </summary>
        /// <param name="model">年度編集のビューモデル。</param>
        /// <returns>年度編集画面。</returns>
        public async Task<IActionResult> Edit([Bind(
            "SeasonId",
            "FromDate",
            "ToDate",
            "RegistrationFromDate",
            "TeamRegistrationFee",
            "PlayerRegistrationFee",
            "PlayerTradeFee")]
            EditViewModel model
            )
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var season = await this.useCase.UpdateSeason(
                model.SeasonId,
                model.FromDate,
                model.ToDate,
                model.RegistrationFromDate,
                model.TeamRegistrationFee,
                model.PlayerRegistrationFee,
                model.PlayerTradeFee);

            var updateModel = new EditViewModel(
                season.Id,
                season.Name,
                season.FromDate,
                season.ToDate,
                season.RegistrationFromDate,
                season.TeamRegistrationFee.Value,
                season.PlayerRegistrationFee.Value,
                season.PlayerTradeFee.Value);

            return this.View(updateModel);
        }
    }
}
