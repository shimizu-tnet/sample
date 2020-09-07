using JuniorTennis.Domain.DrawTables;
using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// エントリー詳細。
    /// </summary>
    public class EntryDetail : EntityBase
    {
        #region properties
        /// <summary>
        /// エントリー番号を取得します。
        /// </summary>
        public EntryNumber EntryNumber { get; private set; }

        /// <summary>
        /// 出場区分を取得します。
        /// </summary>
        public ParticipationClassification ParticipationClassification { get; private set; }

        /// <summary>
        /// シード番号を取得します。
        /// </summary>
        public SeedNumber SeedNumber { get; private set; }

        /// <summary>
        /// 団体登録番号一覧を取得します。
        /// </summary>
        public EntryPlayers EntryPlayers { get; private set; }

        /// <summary>
        /// 合計ポイントを取得します。
        /// </summary>
        public int TotalPoint => this.EntryPlayers.Sum(o => o.Point.Value);

        /// <summary>
        /// 出場可能日一覧を取得します。
        /// </summary>
        public CanParticipationDates CanParticipationDates { get; private set; }

        /// <summary>
        /// 受領状況を取得します。
        /// </summary>
        public ReceiptStatus ReceiptStatus { get; private set; }

        /// <summary>
        /// 利用機能を取得します。
        /// </summary>
        public UsageFeatures UsageFeatures { get; private set; }

        /// <summary>
        /// 予選からの進出者かどうか示す値を取得します。
        /// </summary>
        public bool FromQualifying { get; private set; }

        /// <summary>
        /// ブロック番号を取得します。
        /// </summary>
        public BlockNumber BlockNumber { get; private set; }
        #endregion properties

        #region constructors
        /// <summary>
        /// エントリー詳細の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="entryNumber">エントリー番号。</param>
        /// <param name="participationClassification">出場区分。</param>
        /// <param name="seedNumber">シード番号。</param>
        /// <param name="entryPlayers">選手情報一覧。</param>
        /// <param name="canParticipationDates">出場可能日一覧。</param>
        /// <param name="receiptStatus">受領状況。</param>
        /// <param name="usageFeatures">利用機能。</param>
        /// <param name="fromQualifying">予選からの進出者かどうか示す値。</param>
        /// <param name="blockNumber">ブロック番号。</param>
        public EntryDetail(
            EntryNumber entryNumber,
            ParticipationClassification participationClassification,
            SeedNumber seedNumber,
            IEnumerable<EntryPlayer> entryPlayers,
            IEnumerable<CanParticipationDate> canParticipationDates,
            ReceiptStatus receiptStatus,
            UsageFeatures usageFeatures,
            bool fromQualifying = false,
            BlockNumber blockNumber = null)
        {
            this.EntryNumber = entryNumber;
            this.ParticipationClassification = participationClassification;
            this.SeedNumber = seedNumber;
            this.EntryPlayers = new EntryPlayers(entryPlayers);
            this.CanParticipationDates = new CanParticipationDates(canParticipationDates);
            this.ReceiptStatus = receiptStatus;
            this.UsageFeatures = usageFeatures;
            this.FromQualifying = fromQualifying;

            if (!this.FromQualifying)
            {
                return;
            }

            this.BlockNumber = blockNumber ?? throw new ArgumentNullException("ブロック番号");
        }

        /// <summary>
        /// エントリー詳細の新しいインスタンスを生成します。
        /// </summary>
        private EntryDetail() { }
        #endregion constructors

        #region methods
        #endregion methods

        #region foreign key
        /// <summary>
        /// 外部キー。
        /// </summary>
        public int? TournamentEntryId { get; set; }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public TournamentEntry TournamentEntry { get; set; }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public int? DrawTableId { get; set; }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public DrawTable DrawTable { get; set; }
        #endregion foreign key
    }
}
