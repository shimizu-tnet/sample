using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JuniorTennis.Mvc.Features.DrawTables
{
    /// <summary>
    /// ドロー作成ビューモデル。
    /// </summary>
    public class CreateViewModel
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
        /// 開催日一覧を取得します。
        /// </summary>
        public List<string> HoldingDates { get; set; }

        /// <summary>
        /// 予選用メニューを使用するかどうかを示します。
        /// </summary>
        public bool UseQualifyingMenu { get; set; }

        /// <summary>
        /// シングルスかどうかを示します。
        /// </summary>
        public bool IsSingles { get; set; }

        /// <summary>
        /// 出場区分の一覧を取得します。
        /// </summary>
        public List<SelectListItem> ParticipationClassifications { get; set; }

        /// <summary>
        /// ドロー作成ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tournamentName">大会名。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="tennisEventName">種目。</param>
        /// <param name="holdingDates">開催日一覧。</param>
        /// <param name="useQualifyingMenu">予選メニュー使用フラグ。</param>
        /// <param name="isSingles">シングルスフラグ。</param>
        public CreateViewModel(
            string tournamentId,
            string tournamentName,
            string tennisEventId,
            string tennisEventName,
            IEnumerable<string> holdingDates,
            bool useQualifyingMenu,
            bool isSingles)
        {
            this.TournamentId = tournamentId;
            this.TournamentName = tournamentName;
            this.TennisEventId = tennisEventId;
            this.TennisEventName = tennisEventName;
            this.UseQualifyingMenu = useQualifyingMenu;
            this.IsSingles = isSingles;
            this.HoldingDates = holdingDates.ToList();
            this.ParticipationClassifications = this.CreateParticipationClassifications();
        }

        /// <summary>
        /// ドロー作成ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        public CreateViewModel() { }

        private List<SelectListItem> CreateParticipationClassifications()
        {
            var participationClassifications = Enumeration.GetAll<ParticipationClassification>();
            participationClassifications = participationClassifications
                .Where(o => o != ParticipationClassification.NotParticipate);

            if (!this.UseQualifyingMenu)
            {
                participationClassifications = participationClassifications
                    .Where(o => o != ParticipationClassification.Qualifying);
            }

            return participationClassifications
                .Select(o => new SelectListItem(o.Name, $"{o.Id}"))
                .ToList();
        }
    }
}
