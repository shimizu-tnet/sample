using System;
using System.Collections.Generic;
using System.Text;
using JuniorTennis.SeedWork;
using JuniorTennis.Domain.ReservationNumbers;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.Teams;

namespace JuniorTennis.Domain.RequestTeams
{
    /// <summary>
    /// 登録団体。
    /// </summary>
    public class RequestTeam : EntityBase
    {
        /// <summary>
        /// 団体テーブルのidを取得します。
        /// </summary>
        public int TeamId { get; private set; }

        /// <summary>
        /// 団体を取得します。
        /// </summary>
        public Team Team { get; set; }

        /// <summary>
        /// 年度テーブルのidを取得します。
        /// </summary>
        public int SeasonId { get; private set; }

        /// <summary>
        /// 年度を取得します。
        /// </summary>
        public Season Season { get; set; }

        /// <summary>
        /// 予約番号を取得します。
        /// </summary>
        public ReservationNumber ReservationNumber { get; private set; }

        /// <summary>
        /// 受領状態を取得します。
        /// </summary>
        public ApproveState ApproveState { get; private set; }

        /// <summary>
        /// 申請受付日を取得します。
        /// </summary>
        public DateTime RequestedDateTime { get; private set; }

        /// <summary>
        /// 申請登録料を取得します。
        /// </summary>
        public RequestedFee RequestedFee { get; private set; }

        /// <summary>
        /// 受領日を取得します。
        /// </summary>
        public DateTime? ApproveDateTime { get; private set; }

        /// <summary>
        /// メール送信状態を取得します。
        /// </summary>
        public MailState MailState { get; private set; }

        /// <summary>
        /// 登録団体の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="teamId">団体id。</param>
        /// <param name="seasonId">年度id。</param>
        /// <param name="reservationNumber">予約番号。</param>
        /// <param name="approveState">受領状態。</param>
        /// <param name="requestedDateTime">申請受付日。</param>
        /// <param name="requestedFee">申請登録料。</param>
        /// <param name="approveDateTime">受領日。</param>
        /// <param name="mailState">メール送信状態。</param>
        public RequestTeam(
                int teamId,
                int seasonId,
                ReservationNumber reservationNumber,
                ApproveState approveState,
                DateTime requestedDateTime,
                RequestedFee requestedFee,
                DateTime? approveDateTime,
                MailState mailState
                )
        {
            this.TeamId = teamId;
            this.SeasonId = seasonId;
            this.ReservationNumber = reservationNumber;
            this.ApproveState = approveState;
            this.RequestedDateTime = requestedDateTime;
            this.RequestedFee = requestedFee;
            this.ApproveDateTime = approveDateTime;
            this.MailState = mailState;
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

        /// <summary>
        /// メール送信状態を送信済みにします。
        /// </summary>
        public void UpdateToMailSent() => this.MailState = MailState.Sent;

        /// <summary>
        /// CSV1レコード文の文字列を生成します。
        /// </summary>
        public string ToCsv()
        {
            return
            this.ApproveState.Name + "," +
            this.ReservationNumber.Value + "," +
            this.RequestedDateTime.ToString() + "," +
            (this.Team.TeamCode?.Value ?? "") + "," +
            this.Team.TeamType.Name + "," +
            this.Team.TeamName.Value + "," +
            this.MailState.Name + "," +
            this.Team.RepresentativeEmailAddress + 
            "\r\n";
        }
    }
}
