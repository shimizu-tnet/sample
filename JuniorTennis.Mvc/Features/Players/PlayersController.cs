using System.Linq;
using System.Threading.Tasks;
using JuniorTennis.SeedWork.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using JuniorTennis.Domain.UseCases.Players;
using JuniorTennis.Mvc.Configurations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using JuniorTennis.Domain.Players;
using System.Collections;

namespace JuniorTennis.Mvc.Features.Players
{
    /// <summary>
    /// 団体選手管理コントローラ。
    /// </summary>
    public class PlayersController : Controller
    {
        private readonly IPlayerUseCase playerUseCase;
        public UrlSettings Options { get; }

        public PlayersController(
            IPlayerUseCase playerUseCase,
            IOptions<UrlSettings> optionsAccessor)
        {
            this.playerUseCase = playerUseCase;
            this.Options = optionsAccessor.Value;
        }

        /// <summary>
        /// 選手一覧画面を表示します。
        /// </summary>
        /// <returns>選手一覧画面。</returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var teamId = 1; //TODO ログイン機能出来次第現在のidを取る
            var players = await this.playerUseCase.GetTeamPlayersThisSeason(teamId);
            return this.View(new IndexViewModel(players));
        }

        /// <summary>
        /// 選手編集画面を表示します。
        /// </summary>
        /// <param name="playerCode">登録番号。</param>
        /// <returns>選手編集画面。</returns>
        [HttpGet]
        public async Task<ActionResult> Edit(string playerCode)
        {
            try
            {
                var player = await this.playerUseCase.GetPlayer(playerCode);
                return this.View(EditViewModel.FromEntity(player));
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// 選手電話番号を更新します。
        /// </summary>
        /// <param name="viewModel">選手編集画面ViewModel。</param>
        /// <returns>選手編集画面。</returns>
        [HttpPost]
        public async Task<ActionResult> Edit([Bind("PlayerCode,TelephoneNumber")] EditViewModel viewModel)
        {
            try
            {
                var player = await this.playerUseCase.UpdatePlayerTelephoneNumber(viewModel.PlayerCode, viewModel.TelephoneNumber);
                return this.RedirectToAction(nameof(Edit), new { playerCode = player.PlayerCode.Value });
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// 新規登録申請一覧画面を表示します。
        /// </summary>
        /// <returns>新規登録申請画面。</returns>
        [HttpGet]
        public async Task<ActionResult> RequestPlayers()
        {
            var teamId = 1; //TODO ログイン機能出来次第現在のidを取る
            var players = await this.playerUseCase.GetUnrequestedPlayers(teamId);
            return this.View(new RequestPlayersViewModel(players));
        }

        /// <summary>
        /// 選手を新規登録申請します。
        /// </summary>
        /// <param name="viewModel">新規登録申請のViewModel。</param>
        /// <returns>新規登録申請画面。</returns>
        [HttpPost]
        public async Task<ActionResult> RequestPlayers([Bind("PlayerIds")] RequestPlayersViewModel viewModel)
        {
            var teamId = 1; //TODO ログイン機能出来次第現在のidを取る
            await this.playerUseCase.RequestPlayersNewRegistration(viewModel.PlayerIds, teamId, Options.DomainUrl);
            return this.RedirectToAction(nameof(RequestState));
        }

        /// <summary>
        /// 仮登録選手を取消します。
        /// </summary>
        /// <param name="playerId">選手id。</param>
        /// <returns>新規登録申請画面。</returns>
        [HttpPost]
        public async Task<ActionResult> Cancel(int playerId)
        {
            await this.playerUseCase.DeleteUnrequestedPlayer(playerId);
            return this.RedirectToAction(nameof(RequestPlayers));
        }

        /// <summary>
        /// 選手登録画面を表示します。
        /// </summary>
        /// <returns>選手登録画面。</returns>
        [HttpGet]
        public ActionResult Register()
        {
            return this.View(new RegisterViewModel());
        }

        /// <summary>
        /// 選手を登録します。
        /// </summary>
        /// <param name="viewModel">選手登録のViewModel。</param>
        /// <returns>新規登録申請画面。</returns>
        [HttpPost]
        public async Task<ActionResult> Register([Bind(
            "PlayerFamilyName",
            "PlayerFirstName",
            "PlayerFamilyNameKana",
            "PlayerFirstNameKana",
            "SelectedGender",
            "SelectedCategory",
            "BirthYear",
            "BirthMonth",
            "BirthDate",
            "TelephoneNumber")] RegisterViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }
            else if (!viewModel.ValidateBirthDate())
            {
                viewModel.IsIllegalBirthDate = true;
                return this.View(viewModel);
            }
            var teamId = 1; //TODO ログイン機能出来次第現在のidを取る
            var dto = viewModel.ToDto();
            dto.TeamId = teamId;
            var IsDuplicated = await this.playerUseCase.ExistsDuplicatedPlayer(
                dto.PlayerFamilyName,
                dto.PlayerFirstName,
                dto.BirthDate);
            if (IsDuplicated)
            {
                viewModel.IsDuplicated = true;
                return this.View(viewModel);
            }
            await this.playerUseCase.AddPlayer(dto);
            return this.RedirectToAction(nameof(RequestPlayers));
        }

        /// <summary>
        /// 選手登録申込状況一覧を表示します。
        /// </summary>
        /// <returns>選手登録申込状況一覧画面。</returns>
        [HttpGet]
        public async Task<ActionResult> RequestState()
        {
            try
            {
                var teamId = 1; //TODO ログイン機能出来次第現在のidを取る
                var players = await this.playerUseCase.GetRequestState(teamId);
                return this.View(new RequestStateViewModel(players));
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        private async Task<IndexViewModel> CreateIndexViewModel(string playerName = null, int[] category = null, int[] gender = null)
        {
            var result = await this.playerUseCase.SearchPlayers(playerName, category, gender);
            return new IndexViewModel(result);
        }

        /// <summary>
        /// 選手継続登録申請画面を表示します。
        /// </summary>
        /// <returns>選手継続登録申請画面。</returns>
        [HttpGet]
        public async Task<ActionResult> RequestContinuedPlayers()
        {
            try
            {
                var teamId = 1; //TODO ログイン機能出来次第現在のidを取る
                var players = await this.playerUseCase.GetRequestContinuedPlayers(teamId);
                return this.View(new RequestContinuedPlayersViewModel(players));
            }
            catch (NotFoundException)
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// 選手の継続登録申請をします。
        /// </summary>
        /// <param name="viewModel">継続登録申請画面のViewModel。</param>
        /// <returns>選手登録申込状況一覧画面。</returns>
        [HttpPost]
        public async Task<ActionResult> RequestContinuedPlayers(RequestContinuedPlayersViewModel viewModel)
        {
            var teamId = 1; //TODO ログイン機能出来次第現在のidを取る
            var submitPlayers = viewModel.RequestContinuedPlayers.Where(o => o.IsRequested == false).Where(o => o.IsSelected == true).ToList();
            var dtos = new List<AddRequestPlayersDto>();
            foreach(var submitPlayer in submitPlayers)
            {
                var dto = new AddRequestPlayersDto();
                dto.PlayerId = submitPlayer.PlayerId;
                dto.CategoryId = submitPlayer.CategoryId;
                dtos.Add(dto);
            }

            await this.playerUseCase.AddRequestPlayers(dtos, teamId);
            return this.RedirectToAction(nameof(RequestState));
        }

        /// <summary>
        /// 選手所属変更申込画面を表示します。
        /// </summary>
        /// <returns>選手所属変更申込画面。</returns>
        [HttpGet]
        public ActionResult Transfer()
        {
            var viewModel = new TransferViewModel(new List<Player>());
            return this.View(viewModel);
        }

        private async Task<TransferViewModel> CreateTransferViewModel(string playerName = null)
        {
            var teamId = 1; //TODO ログイン機能出来次第現在のidを取る
            var result = playerName == null ? new List<Player>() : await this.playerUseCase.SearchOtherTeamPlayers(playerName, teamId);
            return new TransferViewModel(result);
        }

        /// <summary>
        /// 選択された選手を所属変更画面の一覧に表示します。
        /// </summary>
        /// <param name="viewModel">選手所属変更申込画面ViewModel。</param>
        /// <returns>選手登録申込状況一覧画面。</returns>
        [HttpPost]
        public async Task<ActionResult> Transfer([Bind("SelectedPlayerIds")] TransferViewModel viewModel)
        {
            var result = await this.playerUseCase.GetTransferPlayers(viewModel.SelectedPlayerIds);
            viewModel.SearchedPlayers = new List<Player>();
            viewModel.TransferPlayers = result;
            return this.View(viewModel);
        }

        /// <summary>
        /// 選手所属変更を申込します。
        /// </summary>
        /// <param name="viewModel">選手所属変更申込画面ViewModel。</param>
        /// <returns>選手登録申込状況一覧画面。</returns>
        [HttpPost]
        public async Task<ActionResult> RequestTransfer([Bind("TransferPlayerIds")] TransferViewModel viewModel)
        {
            var teamId = 1; //TODO ログイン機能出来次第現在のidを取る
            await this.playerUseCase.AddRequestTransferPlayers(viewModel.TransferPlayerIds, teamId);
            return this.RedirectToAction(nameof(RequestState));
        }

        #region WEB API
        /// <summary>
        /// 選手一覧を取得します。
        /// </summary>
        /// <param name="playerName">氏名。</param>
        /// <param name="teamTypes">カテゴリー。</param>
        /// <param name="teamName">性別。</param>
        /// <returns>IndexViewModel。</returns>
        [HttpGet]
        [ActionName("list")]
        public async Task<ActionResult> GetTeams(string playerName, int[] category, int[] gender)
        {
            var viewModel = await this.CreateIndexViewModel(playerName, category, gender);
            if (!viewModel.Players.Any())
            {
                return new EmptyResult();
            }

            return this.View("_PlayersTable", viewModel.Players);
        }

        /// <summary>
        /// 変更候補選手一覧を取得します。
        /// </summary>
        /// <param name="playerName">氏名。</param>
        /// <returns>選手所属変更申込画面。</returns>
        [HttpGet]
        [ActionName("transferList")]
        public async Task<ActionResult> GetOtherTeamPlayers(string playerName)
        {
            var viewModel = await this.CreateTransferViewModel(playerName);
            if (!viewModel.SearchedPlayers.Any())
            {
                return new EmptyResult();
            }

            return this.View("_SearchedTable", viewModel.SearchedPlayers);
        }
        #endregion
    }
}
