using JuniorTennis.Domain.Tournaments;
using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Features.DrawTables
{
    /// <summary>
    /// ドロー表一覧ビューモデル。
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// 選択された大会 ID を取得または設定します。
        /// </summary>
        [Display(Name = "大会")]
        public string SelectedTournametId { get; set; }

        /// <summary>
        /// 選択された種目 ID を取得または設定します。
        /// </summary>
        [Display(Name = "種目")]
        public string SelectedTennisEventId { get; set; }

        /// <summary>
        /// 選択された大会形式 ID を取得または設定します。
        /// </summary>
        public string SelectedTournamentFormatId { get; set; }

        /// <summary>
        /// 大会形式（本戦のみ）を取得または設定します。
        /// </summary>
        public TournamentFormat OnlyMain { get; set; }

        /// <summary>
        /// 大会形式（予選あり）を取得または設定します。
        /// </summary>
        public TournamentFormat WithQualifying { get; set; }

        /// <summary>
        /// ドロー表一覧ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        public IndexViewModel() { }
    }
}
