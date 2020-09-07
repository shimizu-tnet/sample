using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JuniorTennis.Mvc.Features.DrawTables
{
    /// <summary>
    /// ドロー設定ビューモデル。
    /// </summary>
    public class SettingsViewModel
    {
        #region 共通
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
        /// 予選ドロー設定を表示するかどうか示す値を取得します。
        /// </summary>
        public readonly bool ShowQualifyingDrawSettings;

        /// <summary>
        /// ドロー表を取得します。
        /// </summary>
        public readonly DrawTable DrawTable;

        /// <summary>
        /// 大会形式を取得します。
        /// </summary>
        public readonly TournamentFormat TournamentFormat;
        #endregion 共通

        #region 本戦
        /// <summary>
        /// 本戦のドロー数の一覧を取得します。
        /// </summary>
        public readonly List<SelectListItem> MainNumberOfDraws;

        /// <summary>
        /// 大会グレードの一覧を取得します。
        /// </summary>
        public readonly List<SelectListItem> MainTournamentGrades;

        /// <summary>
        /// 本戦の出場者数の一覧を取得します。
        /// </summary>
        public readonly string MainNumberOfEntries;

        /// <summary>
        /// 選択された本戦のドロー数を取得または設定します。
        /// </summary>
        public string SelectedMainNumberOfDraws { get; set; }

        /// <summary>
        /// 選択された本戦の大会グレードを取得または設定します。
        /// </summary>
        public string SelectedMainTournamentGrade { get; set; }
        #endregion 本戦

        #region 予選
        /// <summary>
        /// 予選のドロー数の一覧を取得します。
        /// </summary>
        public readonly List<SelectListItem> QualifyingNumberOfDraws;

        /// <summary>
        /// 大会グレードの一覧を取得します。
        /// </summary>
        public readonly List<SelectListItem> QualifyingTournamentGrades;

        /// <summary>
        /// 本戦の出場者数の一覧を取得します。
        /// </summary>
        public readonly string QualifyingNumberOfEntries;

        /// <summary>
        /// 入力された予選のブロック数を取得または設定します。
        /// </summary>
        public string EnteredQualifyingNumberOfBlocks { get; set; }

        /// <summary>
        /// 選択された予選のドロー数を取得または設定します。
        /// </summary>
        public string SelectedQualifyingNumberOfDraws { get; set; }

        /// <summary>
        /// 選択された予選の大会グレードを取得または設定します。
        /// </summary>
        public string SelectedQualifyingTournamentGrade { get; set; }
        #endregion 予選

        /// <summary>
        /// ドロー設定ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tournamentName">大会名。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="tennisEvent">種目。</param>
        /// <param name="drawTable">ドロー表。</param>
        public SettingsViewModel(
            string tournamentId,
            string tournamentName,
            string tennisEventId,
            string tennisEvent,
            DrawTable drawTable)
        {
            this.TournamentId = tournamentId;
            this.TournamentName = tournamentName;
            this.TennisEventId = tennisEventId;
            this.TennisEvent = tennisEvent;
            this.DrawTable = drawTable;
            this.TournamentFormat = drawTable.TournamentFormat;
            this.ShowQualifyingDrawSettings = this.TournamentFormat == TournamentFormat.WithQualifying;
            this.MainNumberOfDraws = this.CreateNumberOfDraws(ParticipationClassification.Main);
            this.MainNumberOfEntries = $"{this.GetNumberOfEntries(ParticipationClassification.Main)}";
            this.MainTournamentGrades = this.CreateTournamentGrades(ParticipationClassification.Main);
            this.QualifyingNumberOfDraws = this.CreateNumberOfDraws(ParticipationClassification.Qualifying);
            this.QualifyingNumberOfEntries = $"{this.GetNumberOfEntries(ParticipationClassification.Qualifying)}";
            this.QualifyingTournamentGrades = this.CreateTournamentGrades(ParticipationClassification.Qualifying);
            this.EnteredQualifyingNumberOfBlocks = this.GetInitialQualifyingNumberOfBlocks();
        }

        /// <summary>
        /// ドロー設定ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        public SettingsViewModel() { }

        /// <summary>
        /// 出場者数を取得します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>出場者数。</returns>
        private int GetNumberOfEntries(ParticipationClassification participationClassification)
        {
            return DrawTable.EntryDetails
                .Count(o => o.ParticipationClassification == participationClassification);
        }

        /// <summary>
        /// ドロー数を取得します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>ドロー数。</returns>
        private NumberOfDraws GetNumberOfDraws(ParticipationClassification participationClassification)
        {
            return participationClassification == ParticipationClassification.Main
                ? DrawTable.MainDrawSettings.NumberOfDraws
                : DrawTable.QualifyingDrawSettings.NumberOfDraws;
        }

        /// <summary>
        /// ドロー数の一覧を作成します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>ドロー数の一覧。</returns>
        private List<SelectListItem> CreateNumberOfDraws(ParticipationClassification participationClassification)
        {
            var numberOfDraws = this.GetNumberOfDraws(participationClassification);
            var numberOfEntries = this.GetNumberOfEntries(participationClassification);

            return Enumerable
                .Range(2, 8)
                .Select(o => (previousValue: (int)Math.Pow(2, o - 1), currentValue: (int)Math.Pow(2, o)))
                .Select(o => CreateItem(o.previousValue, o.currentValue))
                .ToList();

            SelectListItem CreateItem(int previousValue, int currentValue)
            {
                return new SelectListItem(
                    $"{currentValue}",
                    $"{currentValue}",
                    numberOfDraws.IsConfigured
                        ? numberOfDraws.Value == currentValue
                        : previousValue < numberOfEntries && numberOfEntries <= currentValue);
            }
        }

        /// <summary>
        /// 大会グレードの一覧を作成します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>大会グレードの一覧。</returns>
        private List<SelectListItem> CreateTournamentGrades(ParticipationClassification participationClassification)
        {
            var tournamentGrade = this.GetTournamentGrade(participationClassification);

            return Enumeration
                      .GetAll<TournamentGrade>()
                      .Select(o => new SelectListItem(o.Name, $"{o.Id}", o.Id == tournamentGrade.Id))
                      .ToList();
        }

        /// <summary>
        /// 大会グレードを取得します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>大会グレード。</returns>
        private TournamentGrade GetTournamentGrade(ParticipationClassification participationClassification)
        {
            if (participationClassification == ParticipationClassification.Main)
            {
                return DrawTable.MainDrawSettings.TournamentGrade;
            }
            else
            {
                return DrawTable.QualifyingDrawSettings.TournamentGrade;
            }
        }

        /// <summary>
        /// 初期値として設定する予選ブロック数を取得します。
        /// </summary>
        /// <returns>予選ブロック数。</returns>
        private string GetInitialQualifyingNumberOfBlocks()
        {
            if (this.ShowQualifyingDrawSettings)
            {
                return $"{DrawTable.QualifyingDrawSettings.NumberOfBlocks.Value}";
            }

            return string.Empty;
        }
    }
}
