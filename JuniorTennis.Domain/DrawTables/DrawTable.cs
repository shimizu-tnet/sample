using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ドロー表。
    /// </summary>
    public class DrawTable : EntityBase
    {
        /// <summary>
        /// 大会 ID を取得します。
        /// </summary>
        public int TournamentId { get; private set; }

        /// <summary>
        /// 種目 ID を取得します。
        /// </summary>
        public string TennisEventId { get; private set; }

        /// <summary>
        /// 大会形式を取得します。
        /// </summary>
        public TournamentFormat TournamentFormat { get; private set; }

        /// <summary>
        /// 出場対象選手の種別を取得します。
        /// </summary>
        public EligiblePlayersType EligiblePlayersType { get; private set; }

        /// <summary>
        /// エントリー詳細の一覧を取得します。
        /// </summary>
        public EntryDetails EntryDetails { get; private set; }

        /// <summary>
        /// 本戦のドロー設定を取得します。
        /// </summary>
        public DrawSettings MainDrawSettings { get; private set; }

        /// <summary>
        /// 予選のドロー設定を取得します。
        /// </summary>
        public DrawSettings QualifyingDrawSettings { get; private set; }

        /// <summary>
        /// ブロックの一覧を取得します。
        /// </summary>
        public Blocks Blocks { get; private set; }

        /// <summary>
        /// 編集状態を取得します。
        /// </summary>
        public EditStatus EditStatus { get; private set; }

        /// <summary>
        /// ドロー表の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournament">大会。</param>
        /// <param name="tennisEvent">種目。</param>
        /// <param name="tournamentFormat">大会形式。</param>
        /// <param name="eligiblePlayersType">出場対象選手の種別。</param>
        /// <param name="entryDetails">エントリー詳細の一覧。</param>
        /// <param name="mainDrawSettings">本戦のドロー設定。</param>
        /// <param name="qualifyingDrawSettings">予選のドロー設定。</param>
        /// <param name="blocks">ブロックの一覧。</param>
        /// <param name="editStatus">編集状態。</param>
        public DrawTable(
            Tournament tournament,
            TennisEvent tennisEvent,
            TournamentFormat tournamentFormat,
            EligiblePlayersType eligiblePlayersType,
            List<EntryDetail> entryDetails,
            DrawSettings mainDrawSettings,
            DrawSettings qualifyingDrawSettings,
            IEnumerable<Block> blocks,
            EditStatus editStatus)
        {
            this.TournamentId = tournament.Id;
            this.TennisEventId = tennisEvent.TennisEventId;
            this.TournamentFormat = tournamentFormat;
            this.EligiblePlayersType = eligiblePlayersType;
            this.EntryDetails = new EntryDetails(entryDetails);
            this.MainDrawSettings = mainDrawSettings;
            this.QualifyingDrawSettings = qualifyingDrawSettings;
            this.Blocks = new Blocks(blocks);
            this.EditStatus = editStatus;
        }

        /// <summary>
        /// ドロー表の新しいインスタンスを生成します。
        /// </summary>
        private DrawTable() { }

        /// <summary>
        /// 出場対象選手の種別を更新します。
        /// </summary>
        /// <param name="eligiblePlayersType">出場対象選手の種別。</param>
        public void UpdateEligiblePlayersType(EligiblePlayersType eligiblePlayersType)
        {
            this.EligiblePlayersType = eligiblePlayersType;
        }

        /// <summary>
        /// エントリー詳細の一覧を更新します。
        /// </summary>
        /// <param name="entryDetails">エントリー詳細の一覧。</param>
        public void UpdateEntryDetails(IEnumerable<EntryDetail> entryDetails)
        {
            var fromQualifyingPlayers = this.EntryDetails.Where(o => o.FromQualifying).ToList();
            this.EntryDetails = new EntryDetails(entryDetails);
            foreach (var fromQualifyingPlayer in fromQualifyingPlayers)
            {
                this.EntryDetails.Add(fromQualifyingPlayer);
            }
            this.QualifyingDrawSettings.UpdateNumberOfEntries(this.NumberOfEntries(ParticipationClassification.Qualifying));
            this.MainDrawSettings.UpdateNumberOfEntries(this.NumberOfEntries(ParticipationClassification.Main));
        }

        /// <summary>
        /// ドロー設定を取得します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        public DrawSettings GetDrawSettings(ParticipationClassification participationClassification)
        {
            return participationClassification == ParticipationClassification.Main
                ? this.MainDrawSettings
                : this.QualifyingDrawSettings;
        }

        /// <summary>
        /// ドロー設定を更新します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <param name="drawSettings">ドロー設定。</param>
        public void UpdateDrawSettings(ParticipationClassification participationClassification, DrawSettings drawSettings)
        {
            if (participationClassification == ParticipationClassification.Main)
            {
                this.MainDrawSettings.UpdateFromObject(drawSettings);
            }
            else
            {
                this.QualifyingDrawSettings.UpdateFromObject(drawSettings);
            }
        }

        /// <summary>
        /// 編集状態を編集中に変更します。
        /// </summary>
        public void AsEditing()
        {
            this.ResetId();
            this.EditStatus = EditStatus.Editing;
        }

        /// <summary>
        /// 編集状態をドラフトに変更します。
        /// </summary>
        public void AsDraft()
        {
            this.ResetId();
            this.EditStatus = EditStatus.Draft;
        }

        /// <summary>
        /// 編集状態を公開済みに変更します。
        /// </summary>
        public void AsPublished()
        {
            this.ResetId();
            this.EditStatus = EditStatus.Published;
        }

        /// <summary>
        /// エンティティの ID をリセットします。
        /// </summary>
        private void ResetId()
        {
            this.Id = 0;
            this.MainDrawSettings.Id = 0;
            this.QualifyingDrawSettings.Id = 0;
            foreach (var entryDetail in this.EntryDetails)
            {
                entryDetail.Id = 0;
                foreach (var entryPlayer in entryDetail.EntryPlayers)
                {
                    entryPlayer.Id = 0;
                }
            }
            foreach (var block in this.Blocks)
            {
                block.Id = 0;
                block.DrawSettingsId = block.IsMain ? this.MainDrawSettings.Id : this.QualifyingDrawSettings.Id;
                block.DrawSettings = block.IsMain ? this.MainDrawSettings : this.QualifyingDrawSettings;
                foreach (var game in block.Games)
                {
                    game.Id = 0;
                    game.GameResult.Id = 0;
                    game.DrawSettingsId = block.IsMain ? this.MainDrawSettings.Id : this.QualifyingDrawSettings.Id;
                    game.DrawSettings = block.IsMain ? this.MainDrawSettings : this.QualifyingDrawSettings;
                    foreach (var opponents in game.Opponents)
                    {
                        opponents.Id = 0;
                    }
                }
            }
        }

        /// <summary>
        /// 出場者数を取得します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>出場者数。</returns>
        private int NumberOfEntries(ParticipationClassification participationClassification)
        {
            return this.EntryDetails
                .Count(o => o.ParticipationClassification == participationClassification);
        }

        #region foreign key
        /// <summary>
        /// 外部キー。
        /// </summary>
        public int MainDrawSettingsId { get; private set; }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public int QualifyingDrawSettingsId { get; private set; }
        #endregion foreign key
    }
}
