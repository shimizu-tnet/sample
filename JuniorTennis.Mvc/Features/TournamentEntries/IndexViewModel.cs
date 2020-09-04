using JuniorTennis.Domain.Tournaments;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Features.TournamentEntries
{
    /// <summary>
    /// 大会申込種目一覧のビューモデル。
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// 選択された大会IDを取得または設定します。
        /// </summary>
        [Display(Name = "大会")]
        public string SelectedTournamentId { get; set; }

        /// <summary>
        /// 選択された種目 ID を取得または設定します。
        /// </summary>
        [Display(Name = "種目")]
        public string SelectedTennisEventId { get; set; }

        /// <summary>
        /// 大会申込種目一覧のビューモデルの新しいインスタンスを生成します。
        /// </summary>
        public IndexViewModel()
        {
        }
    }
}
