using System;
using System.Collections.Generic;
using System.Text;
using JuniorTennis.SeedWork;
using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.ReservationNumbers;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.Tournaments;

namespace JuniorTennis.Domain.RequestPlayers
{
    /// <summary>
    /// 登録選手。
    /// </summary>
    public class RequestPlayer : EntityBase
    {
        /// <summary>
        /// 選手テーブルのidを取得します。
        /// </summary>
        public int PlayerId { get; private set; }

        /// <summary>
        /// 選手を取得します。
        /// </summary>
        public Player Player { get; private set; }

        /// <summary>
        /// 団体テーブルのidを取得します。
        /// </summary>
        public int TeamId { get; private set; }

        /// <summary>
        /// 団体を取得します。
        /// </summary>
        public Team Team { get; private set; }

        /// <summary>
        /// 年度テーブルのidを取得します。
        /// </summary>
        public int SeasonId { get; private set; }

        /// <summary>
        /// 年度を取得します。
        /// </summary>
        public Season Season { get; private set; }

        /// <summary>
        /// 予約番号を取得します。
        /// </summary>
        public ReservationNumber ReservationNumber { get; private set; }

        /// <summary>
        /// 予約番号枝番を取得します。
        /// </summary>
        public int ReservationBranchNumber { get; private set; }

        /// <summary>
        /// カテゴリーを取得します。
        /// </summary>
        public Category Category { get; private set; }

        /// <summary>
        /// 申請種別を取得します。
        /// </summary>
        public RequestType RequestType { get; private set; }

        /// <summary>
        /// 受領状態を取得します。
        /// </summary>
        public ApproveState ApproveState { get; private set; }

        /// <summary>
        /// 申請受付日を取得します。
        /// </summary>
        public DateTime RequestedDateTime { get; private set; }

        /// <summary>
        /// 選手登録料を取得します。
        /// </summary>
        public PlayerRegistrationFee PlayerRegistrationFee { get; private set; }

        /// <summary>
        /// 受領日を取得します。
        /// </summary>
        public DateTime? ApproveDateTime { get; private set; }

        /// <summary>
        /// 登録選手の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="playerId">選手id。</param>
        /// <param name="teamId">団体id。</param>
        /// <param name="seasonId">年度id。</param>
        /// <param name="reservationNumber">予約番号。</param>
        /// <param name="reservationBranchNumber">予約番号枝番。</param>
        /// <param name="category">カテゴリー。</param>
        /// <param name="requestType">申請種別。</param>
        /// <param name="approveState">受領状態。</param>
        /// <param name="requestedDateTime">申請受付日。</param>
        /// <param name="playerRegistrationFee">選手登録料。</param>
        /// <param name="approveDateTime">受領日。</param>
        public RequestPlayer(
            int playerId,
            int teamId,
            int seasonId,
            ReservationNumber reservationNumber,
            int reservationBranchNumber,
            Category category,
            RequestType requestType,
            ApproveState approveState,
            DateTime requestedDateTime,
            PlayerRegistrationFee playerRegistrationFee,
            DateTime? approveDateTime
            )
        {
            this.PlayerId = playerId;
            this.TeamId = teamId;
            this.SeasonId = seasonId;
            this.ReservationNumber = reservationNumber;
            this.ReservationBranchNumber = reservationBranchNumber;
            this.Category = category;
            this.RequestType = requestType;
            this.ApproveState = approveState;
            this.RequestedDateTime = requestedDateTime;
            this.PlayerRegistrationFee = playerRegistrationFee;
            this.ApproveDateTime = approveDateTime;
        }

        /// <summary>
        /// 受領状態にします。
        /// </summary>
        public void Approve()
        {
            this.ApproveState = ApproveState.Approved;
            this.ApproveDateTime = DateTime.Now;
        }

        /// <summary>
        /// 未納状態にします。
        /// </summary>
        public void Unapprove()
        {
            this.ApproveState = ApproveState.Unapproved;
            this.ApproveDateTime = null;
        }
    }
}
