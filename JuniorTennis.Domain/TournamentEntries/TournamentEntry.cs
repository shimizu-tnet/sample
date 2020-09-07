using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 大会申込。
    /// </summary>
    public class TournamentEntry : EntityBase
    {
        /// <summary>
        /// 予約番号を取得します。
        /// </summary>
        public ReservationNumber ReservationNumber { get; private set; }

        /// <summary>
        /// 予約受付日を取得します。
        /// </summary>
        public ReservationDate ReservationDate { get; private set; }

        /// <summary>
        /// エントリー詳細を取得します。
        /// </summary>
        public EntryDetail EntryDetail { get; private set; }

        /// <summary>
        /// 参加費を取得します。
        /// </summary>
        public EntryFee EntryFee { get; private set; }

        /// <summary>
        /// 受領状況を取得します。
        /// </summary>
        public ReceiptStatus ReceiptStatus { get; private set; }

        /// <summary>
        /// 受領日を取得します。
        /// </summary>
        public ReceivedDate ReceivedDate { get; private set; }

        /// <summary>
        /// 申請者を取得します。
        /// </summary>
        public Applicant Applicant { get; private set; }

        /// <summary>
        /// 大会申込の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="reservationNumber">予約番号。</param>
        /// <param name="reservationDate">予約受付日。</param>
        /// <param name="entryDetail">エントリー詳細。</param>
        /// <param name="entryFee">参加費。</param>
        /// <param name="receiptStatus">受領状況。</param>
        /// <param name="receivedDate">受領日。</param>
        /// <param name="applicant">申請者。</param>
        public TournamentEntry(
            string reservationNumber,
            DateTime reservationDate,
            EntryDetail entryDetail,
            EntryFee entryFee,
            ReceiptStatus receiptStatus,
            DateTime? receivedDate,
            Applicant applicant)
        {
            this.ReservationNumber = new ReservationNumber(reservationNumber);
            this.ReservationDate = new ReservationDate(reservationDate);
            this.EntryDetail = entryDetail;
            this.EntryFee = entryFee;
            this.ReceiptStatus = receiptStatus;
            this.ReceivedDate = new ReceivedDate(receivedDate);
            this.Applicant = applicant;
        }

        /// <summary>
        /// 大会申込の新しいインスタンスを生成します。
        /// </summary>
        private TournamentEntry() { }
    }
}
