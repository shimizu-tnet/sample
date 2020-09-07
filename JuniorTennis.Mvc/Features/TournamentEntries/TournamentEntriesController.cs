using JuniorTennis.Domain.UseCases.TournamentEntries;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.TournamentEntries
{
    public class TournamentEntriesController : Controller
    {
        private readonly ITournamentEntryUseCase tournamentEntryUseCase;

        public TournamentEntriesController(ITournamentEntryUseCase tournamentEntryUseCase)
        {
            this.tournamentEntryUseCase = tournamentEntryUseCase;
        }

        /// <summary>
        /// 大会申込種目選択画面を表示します。
        /// </summary>
        /// <returns>大会申込種目選択画面。</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var applicationTournaments = await this.tournamentEntryUseCase.GetApplicationTournaments();
            return this.View(new IndexViewModel(applicationTournaments));
        }

        /// <summary>
        /// シングルスの大会申込選手選択画面を表示します。
        /// </summary>
        /// <returns>シングルスの大会申込選手選択画面。</returns>
        [HttpGet]
        public IActionResult EntrySingles()
        {
            var players = Enumerable.Range(0, 10)
                .Select(o => new DisplayEntryPlayer(
                    o,
                    "0000001",
                    "大成太郎",
                    "タイセイタロウ",
                    "男",
                    "17 / 18歳以下",
                    "2020年4月1日",
                    "18歳",
                    "大成高校",
                    "高校3年"
                    ))
                .ToList();
            var viewModel = new EntrySinglesViewModel()
            {
                EntryPlayers = players,
                TournamentName = "大成杯",
                TennisEvent = "17/18歳以下 男子 シングルス"
            };
            return this.View(viewModel);
        }

        /// <summary>
        /// 選択された大会を取得します。
        /// </summary>
        /// <param name="id">大会 ID。</param>
        /// <returns>大会の申込期間と開催期間と種目一覧。</returns>
        [HttpGet]
        [ActionName("selectedTournament")]
        public async Task<SelectedTournamentDto> GetSelectedTournament(int id)
        {
            return await this.tournamentEntryUseCase.GetSelectedTournament(id);
        }
    }
}
