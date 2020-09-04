using JuniorTennis.Domain.DrawTables;
using JuniorTennis.SeedWork;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JuniorTennis.Mvc.Features.DrawTables
{
    /// <summary>
    /// 選手情報設定ビューモデル。
    /// </summary>
    public class PlayerViewModel
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
        public string TennisEvent { get; set; }

        /// <summary>
        /// 開催日一覧を取得します。
        /// </summary>
        [Display(Name = "開催日")]
        public List<string> HoldingDates { get; set; }

        /// <summary>
        /// 出場対象選手の種別 ID を取得します。
        /// </summary>
        public int EligiblePlayersTypeId { get; set; }

        /// <summary>
        /// 出場対象選手の種別の一覧を取得します。
        /// </summary>
        public List<SelectListItem> EligiblePlayersTypes { get; set; }

        /// <summary>
        /// 予選用メニューを使用するかどうかを示します。
        /// </summary>
        public bool UseQualifyingMenu { get; set; }

        /// <summary>
        /// 選手情報設定ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tournamentName">大会名。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="tennisEvent">種目。</param>
        /// <param name="eligiblePlayersTypeId">出場対象選手の種別 ID。</param>
        /// <param name="holdingDates">開催日一覧。</param>
        /// <param name="useQualifyingMenu">予選メニュー使用フラグ。</param>
        public PlayerViewModel(
            string tournamentId,
            string tournamentName,
            string tennisEventId,
            string tennisEvent,
            int eligiblePlayersTypeId,
            IEnumerable<string> holdingDates,
            bool useQualifyingMenu)
        {
            this.TournamentId = tournamentId;
            this.TournamentName = tournamentName;
            this.TennisEventId = tennisEventId;
            this.TennisEvent = tennisEvent;
            this.EligiblePlayersTypeId = eligiblePlayersTypeId;
            this.HoldingDates = holdingDates.ToList();
            this.EligiblePlayersTypes = this.CreateReceiptStatusList();
            this.UseQualifyingMenu = useQualifyingMenu;
        }

        /// <summary>
        /// 選手情報設定ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        public PlayerViewModel() { }

        private List<SelectListItem> CreateReceiptStatusList()
        {
            return this.EligiblePlayersTypes = Enumeration.GetAll<EligiblePlayersType>()
                 .Select(o => new SelectListItem(o.Name, $"{o.Id}", o.Id == this.EligiblePlayersTypeId))
                 .ToList();
        }
    }
}
