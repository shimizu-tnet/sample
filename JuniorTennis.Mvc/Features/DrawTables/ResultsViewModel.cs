using System.ComponentModel.DataAnnotations;

namespace JuniorTennis.Mvc.Features.DrawTables
{
    /// <summary>
    /// 試合結果入力ビューモデル。
    /// </summary>
    public class ResultsViewModel
    {
        /// <summary>
        /// 大会 ID を取得します。
        /// </summary>
        public string TournamentId { get; set; }

        /// <summary>
        /// 大会名を取得します。
        /// </summary>
        [Display(Name = "大会名")]
        public string TournamentName { get; set; }

        /// <summary>
        /// 種目 ID を取得します。
        /// </summary>
        public string TennisEventId { get; set; }

        /// <summary>
        /// 種目を取得します。
        /// </summary>
        [Display(Name = "種目")]
        public string TennisEventName { get; set; }

        /// <summary>
        /// シングルスかどうかを示します。
        /// </summary>
        public bool IsSingles { get; set; }

        /// <summary>
        /// 試合結果入力ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tournamentName">大会名。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="tennisEventName">種目。</param>
        /// <param name="isSingles">シングルスフラグ。</param>
        public ResultsViewModel(
            string tournamentId,
            string tournamentName,
            string tennisEventId,
            string tennisEventName,
            bool isSingles)
        {
            this.TournamentId = tournamentId;
            this.TournamentName = tournamentName;
            this.TennisEventId = tennisEventId;
            this.TennisEventName = tennisEventName;
            this.IsSingles = isSingles;
        }

        /// <summary>
        /// 試合結果入力ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        public ResultsViewModel() { }
    }
}
