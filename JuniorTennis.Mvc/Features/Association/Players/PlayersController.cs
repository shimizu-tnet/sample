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

namespace JuniorTennis.Mvc.Features.Association.Players
{
    /// <summary>
    /// 協会選手管理コントローラ。
    /// </summary>
    [Area("Association")]
    public class PlayersController : Controller
    {
        /// <summary>
        /// 表示件数。
        /// </summary>
        private const int DisplayCount = 50;

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
        /// <param name="page">ページインデックス。</param>
        /// <returns>選手一覧画面。</returns>
        [HttpGet]
        public async Task<ActionResult> Index(int? page)
        {
            var seasons = await this.playerUseCase.GetSeasons();
            if (!page.HasValue)
            {
                return this.View(new IndexViewModel(DisplayCount, seasons));
            }
            var viewModel = await this.CreateIndexViewModel(page, 0);
            return this.View(viewModel);
        }

        private async Task<IndexViewModel> CreateIndexViewModel(int? page, int? season, int[] category = null, int[] gender = null, string playerName = null, string teamName = null)
        {
            var seasons = await this.playerUseCase.GetSeasons();
            var pageIndex = page ?? 0;
            var seasonId = season ?? 0;
            var result = await this.playerUseCase.SearchPlayerPagedList(pageIndex, DisplayCount, seasonId, category, gender, playerName, teamName);
            return new IndexViewModel(result, seasons);
        }

        #region WEB API
        /// <summary>
        /// 選手一覧を取得します。
        /// </summary>
        /// <param name="playerName">ページ番号。</param>
        /// <param name="season">年度。</param>
        /// <param name="teamTypes">カテゴリー。</param>
        /// <param name="teamName">性別。</param>
        /// <param name="playerName">氏名。</param>
        /// <param name="teamCode">団体名。</param>
        /// <returns>IndexViewModel。</returns>
        [HttpGet]
        [ActionName("list")]
        public async Task<ActionResult> GetPlayers(int? page, int season, int[] category, int[] gender, string playerName, string teamName)
        {
            var viewModel = await this.CreateIndexViewModel(page, season, category, gender, playerName, teamName);
            if (!viewModel.Players.Any())
            {
                return new EmptyResult();
            }

            return this.View("_PlayersTable", viewModel.Players);
        }

        /// <summary>
        /// 選手一覧のダウンロードボタンから。CSVデータを生成します。
        /// </summary>
        /// <param name="season">年度。</param>
        /// <param name="teamTypes">カテゴリー。</param>
        /// <param name="teamName">性別。</param>
        /// <param name="playerName">氏名。</param>
        /// <param name="teamCode">団体名。</param>
        /// <returns>CSVファイルの中身となる文字列。</returns>
        [HttpGet]
        [ActionName("downloadIndex")]
        public async Task<string> DownloadPlayersData(int season, int[] category, int[] gender, string playerName, string teamName)
        {
            var players = await this.playerUseCase.SearchPlayerList(season, category, gender, playerName, teamName);
            var csv = string.Join("", players.Select(o => o.ToCsv()));
            return csv;
        }
        #endregion
    }
}
