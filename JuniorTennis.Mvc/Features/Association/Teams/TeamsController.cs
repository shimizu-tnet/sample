using System.Threading.Tasks;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Shared;
using System;
using JuniorTennis.Domain.UseCases.Teams;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Mvc.Configurations;
using JuniorTennis.SeedWork.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using JuniorTennis.Mvc.Features.Shared.Pagination;

namespace JuniorTennis.Mvc.Features.Association.Teams
{
    [Area("Association")]
    public class TeamsController : Controller
    {
        /// <summary>
        /// 表示件数。
        /// </summary>
        private const int DisplayCount = 50;

        private readonly ITeamUseCase useCase;
        public UrlSettings Options { get; }

        public TeamsController(
            ITeamUseCase useCase,
            IOptions<UrlSettings> optionsAccessor)
        {
            this.useCase = useCase;
            this.Options = optionsAccessor.Value;
        }

        /// <summary>
        /// 団体一覧画面を表示します。
        /// </summary>
        /// <param name="page">ページインデックス。</param>
        /// <returns>団体一覧画面。</returns>
        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            if (!page.HasValue)
            {
                return this.View(new IndexViewModel(DisplayCount));
            }

            var viewModel = await this.CreateIndexViewModel(page);
            return this.View(viewModel);
        }

        /// <summary>
        /// 団体編集画面を表示します。
        /// </summary>
        /// <param name="teamCode">団体コード。</param>
        /// <returns>団体編集画面。</returns>
        [HttpGet]
        public async Task<ActionResult> Edit(string teamCode)
        {
            try
            {
                var team = await this.useCase.GetTeam(teamCode);
                return this.View(EditViewModel.FromEntity(team));
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind("TeamCode,TeamJpin")] EditViewModel viewModel)
        {
            try
            {
                var team = await this.useCase.UpdateTeamJpin(viewModel.TeamCode, viewModel.TeamJpin);
                return this.RedirectToAction(nameof(Edit), new { teamCode = team.TeamCode.Value });
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// 団体パスワード再設定画面を表示します。
        /// </summary>
        /// <param name="teamCode">団体コード。</param>
        /// <returns>団体編集画面。</returns>
        [HttpGet]
        public ActionResult RequestChangeMailAddress(string teamCode)
        {
            try
            {
                var model = new RequestChangeMailAddressViewModel();
                model.TeamCode = teamCode;
                return this.View(model);
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// 団体パスワード再設定用の認証情報を登録し、認証メールを送信します。
        /// </summary>
        /// <param name="viewModel">パスワード再設定画面viewModel。</param>
        /// <returns>団体編集画面。</returns>
        [HttpPost]
        public async Task<ActionResult> RequestChangeMailAddress([Bind("TeamCode,MailAddress")] RequestChangeMailAddressViewModel viewModel)
        {
            try
            {
                var authorizationLink = await this.useCase.AddAuthorizationLink(viewModel.TeamCode);
                await this.useCase.SendChangeMailAddressVerifyMail(authorizationLink.AuthorizationCode, viewModel.MailAddress, Options.DomainUrl);
                return this.RedirectToAction(nameof(Edit), new { teamCode = viewModel.TeamCode });
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// 受領一覧画面を表示します。
        /// </summary>
        /// <returns>受領一覧画面。</returns>
        [HttpGet]
        public async Task<ActionResult> Receipt(int? page)
        {
            var seasons = await this.useCase.GetSeasons();
            if (!page.HasValue)
            {
                return this.View(new ReceiptViewModel(DisplayCount, seasons));
            }
            var viewModel = await this.CreateReciptViewModel(page);
            return this.View(viewModel);
        }

        /// <summary>
        /// 登録団体の受領・取消をします。
        /// </summary>
        /// <param name="viewModel">受領一覧画面のViewModel。</param>
        /// <returns>受領一覧画面。</returns>
        [HttpPost]
        public async Task<ActionResult> Receipt([Bind(
            "SelectedSeasonId",
            "TeamCodeForSearch",
            "ReservationNumberForSearch",
            "SelectedApproveState",
            "SelectedRequestTeamIds")] ReceiptViewModel viewModel)
        {
            await this.useCase.UpdateRequestTeamsApproveState(viewModel.SelectedApproveState, viewModel.SelectedRequestTeamIds, viewModel.SelectedSeasonId, this.Options.DomainUrl);
            
            var pageIndex = viewModel.Page ?? 0;
            var result = await this.useCase.SearchRequestTeams(
                pageIndex, 
                TeamsController.DisplayCount,
                viewModel.SelectedSeasonId, 
                viewModel.TeamCodeForSearch, 
                viewModel.ReservationNumberForSearch,
                viewModel.SelectedApproveState);
            
            var seasons = await this.useCase.GetSeasons();
            return this.View(new ReceiptViewModel(result, seasons));
        }

        #region WEB API
        /// <summary>
        /// 団体一覧を取得します。
        /// </summary>
        /// <param name="page">ページ番号（1origin）。</param>
        /// <param name="teamTypes">団体種別一覧。</param>
        /// <param name="teamName">団体名。</param>
        /// <returns>IndexViewModel。</returns>
        [HttpGet]
        [ActionName("list")]
        public async Task<ActionResult> GetTeams(int? page, int[] teamTypes, string teamName)
        {
            var viewModel = await this.CreateIndexViewModel(page, teamTypes, teamName);
            if (!viewModel.Teams.Any())
            {
                return new EmptyResult();
            }

            return this.View("_TeamsTable", viewModel.Teams);
        }

        /// <summary>
        /// 受領一覧のダウンロードボタンから。CSVデータを生成します。
        /// </summary>
        /// <returns>CSVファイルの中身となる文字列。</returns>
        [HttpGet]
        [ActionName("download")]
        public async Task<string> DownloadReceiptData()
        {
            var requestTeams = await this.useCase.GetRequestTeams();
            var csv = string.Join("", requestTeams.Select(o => o.ToCsv()));

            return csv;
        }

        /// <summary>
        /// 受領一覧を取得します。
        /// </summary>
        /// <param name="page">ページ番号></param>
        /// <param name="SelectedSeasonId">選択された年度id</param>
        /// <param name="TeamCodeForSearch">検索団体番号</param>
        /// <param name="ReservationNumberForSearch">検索予約番号</param>
        /// <param name="SelectedApproveStates">選択された受領状態</param>
        /// <returns>受領一覧画面</returns>
        [HttpGet]
        [ActionName("ReceiptList")]
        public async Task<ActionResult> GetRequestTeams(int? page, int SelectedSeasonId, string TeamCodeForSearch, string ReservationNumberForSearch, int SelectedApproveState)
        {
            var viewModel = await this.CreateReciptViewModel(page, SelectedSeasonId, TeamCodeForSearch, ReservationNumberForSearch, SelectedApproveState);
            if (!viewModel.RequestTeams.Any())
            {
                return new EmptyResult();
            }

            return this.View("_RequestTeamsTable", viewModel.RequestTeams);
        }
        #endregion

        private async Task<IndexViewModel> CreateIndexViewModel(int? page, int[] teamTypes = null, string teamName = null)
        {
            var pageIndex = page ?? 0;
            var result = await this.useCase.SearchTeam(pageIndex, TeamsController.DisplayCount, teamTypes, teamName);
            return new IndexViewModel(result);
        }

        private async Task<ReceiptViewModel> CreateReciptViewModel(int? page, int? seasonId = null, string teamCode = null, string reservationNumber = null, int? approveState = null)
        {
            var seasons = await this.useCase.GetSeasons();
            var pageIndex = page ?? 0;
            var searchSeasonId = seasonId ?? 0;
            var searchApproveState = approveState ?? ApproveState.All.Id;
            var result = await this.useCase.SearchRequestTeams(pageIndex, TeamsController.DisplayCount, searchSeasonId, teamCode, reservationNumber, searchApproveState);
            return new ReceiptViewModel(result, seasons);
        }
    }
}
