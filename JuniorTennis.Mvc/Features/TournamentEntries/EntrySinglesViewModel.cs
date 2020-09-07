using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Mvc.Features.TournamentEntries
{
    /// <summary>
    /// シングルスの申込画面用ビューモデル。
    /// </summary>
    public class EntrySinglesViewModel
    {
        /// <summary>
        /// 申込可能な選手の一覧を取得または設定します。
        /// </summary>
        public List<DisplayEntryPlayer> EntryPlayers { get; set; }

        /// <summary>
        /// 大会名を取得または設定します。
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// 種目を取得または設定します。
        /// </summary>
        public string TennisEvent { get; set; }
    }
}
