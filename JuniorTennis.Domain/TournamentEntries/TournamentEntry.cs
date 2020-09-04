using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Teams;
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
        /// 申込団体一覧を取得します。
        /// </summary>
        public EntryTeams EntryTeams { get; private set; }

        /// <summary>
        /// 大会名を取得します。
        /// </summary>
        public TournamentName TournamentName { get; private set; }

        /// <summary>
        /// 種目を取得します。
        /// </summary>
        public TennisEvent TennisEvent { get; private set; }

        /// <summary>
        /// 申込選手一覧を取得します。
        /// </summary>
        public EntryPlayers EntryPlayers { get; private set; }

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
        /// <param name="teams">申込団体。</param>
        /// <param name="tournamentName">大会名。</param>
        /// <param name="tennisEvent">種目。</param>
        /// <param name="players">申込選手。</param>
        /// <param name="entryFee">参加費。</param>
        /// <param name="receiptStatus">受領状況。</param>
        /// <param name="receivedDate">受領日。</param>
        /// <param name="applicant">申請者。</param>
        public TournamentEntry(
            string reservationNumber,
            DateTime reservationDate,
            Team[] teams,
            TournamentName tournamentName,
            TennisEvent tennisEvent,
            Player[] players,
            EntryFee entryFee,
            ReceiptStatus receiptStatus,
            DateTime? receivedDate,
            Applicant applicant)
        {
            this.ReservationNumber = new ReservationNumber(reservationNumber);
            this.ReservationDate = new ReservationDate(reservationDate);
            this.EntryTeams = new EntryTeams(tennisEvent, teams);
            this.TournamentName = tournamentName;
            this.TennisEvent = tennisEvent;
            this.EntryPlayers = new EntryPlayers(tennisEvent, players);
            this.EntryFee = entryFee;
            this.ReceiptStatus = receiptStatus;
            this.ReceivedDate = new ReceivedDate(receivedDate);
            this.Applicant = applicant;
        }
    }
}
