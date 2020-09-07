using System;
using JuniorTennis.Domain.Teams;
using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Seasons
{
    /// <summary>
    /// 年度。
    /// </summary>
    public class Season : EntityBase
    {
        /// <summary>
        /// 年度を取得します。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 年度開始日を取得します。
        /// </summary>
        public DateTime FromDate { get; private set; }

        /// <summary>
        /// 年度終了日を取得します。
        /// </summary>
        public DateTime ToDate { get; private set; }

        /// <summary>
        /// 年度登録受付開始日を取得します。
        /// </summary>
        public DateTime RegistrationFromDate { get; private set; }

        /// <summary>
        /// 団体登録料を取得します。
        /// </summary>
        public TeamRegistrationFee TeamRegistrationFee { get; private set; }

        /// <summary>
        /// 選手登録料を取得します。
        /// </summary>
        public PlayerRegistrationFee PlayerRegistrationFee { get; private set; }

        /// <summary>
        /// 選手移籍料を取得します。
        /// </summary>
        public PlayerTradeFee PlayerTradeFee { get; private set; }

        /// <summary>
        /// 年度の期間を文字列で取得します。
        /// </summary>
        public string DisplaySeasonPeriod => $"{this.FromDate:yyyy/M/d} ～ {this.ToDate:yyyy/M/d}";

        /// <summary>
        /// 年度を更新します。
        /// </summary>
        /// <param name="fromDate">年度開始日</param>
        /// <param name="toDate">年度終了日</param>
        /// <param name="registrationFromDate">年度登録受付開始日</param>
        /// <param name="teamRegistrationFee">団体登録料</param>
        /// <param name="playerRegistrationFee">選手登録料</param>
        /// <param name="playerTradeFee">選手移籍料</param>
        public void Change(
            DateTime fromDate,
            DateTime toDate,
            DateTime registrationFromDate,
            TeamRegistrationFee teamRegistrationFee,
            PlayerRegistrationFee playerRegistrationFee,
            PlayerTradeFee playerTradeFee)
        {
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.RegistrationFromDate = registrationFromDate;
            this.TeamRegistrationFee = teamRegistrationFee;
            this.PlayerRegistrationFee = playerRegistrationFee;
            this.PlayerTradeFee = playerTradeFee;
        }

        private Season() { }

        public Season(
            string name,
            DateTime fromDate,
            DateTime toDate,
            DateTime registrationFromDate,
            TeamRegistrationFee teamRegistrationFee,
            PlayerRegistrationFee playerRegistrationFee,
            PlayerTradeFee playerTradeFee
            )
        {
            if (fromDate.Date > toDate.Date)
            {
                throw new ArgumentException("年度開始日は年度終了日より前の日付を設定してください。", "年度開始日");
            }
            if (registrationFromDate.Date > fromDate.Date)
            {
                throw new ArgumentException("年度登録受付開始日は年度開始日より前の日付を設定してください。", "年度登録受付開始日");
            }
            this.Name = name;
            this.FromDate = fromDate.Date;
            this.ToDate = toDate.Date;
            this.RegistrationFromDate = registrationFromDate.Date;
            this.TeamRegistrationFee = teamRegistrationFee;
            this.PlayerRegistrationFee = playerRegistrationFee;
            this.PlayerTradeFee = playerTradeFee;
        }

        /// <summary>
        /// 選手登録料を取得します。
        /// </summary>
        /// <param name="teamType">団体種別</param>
        public PlayerRegistrationFee GetPlayerRegistrationFee()
        {
            return this.PlayerRegistrationFee;
        }

        /// <summary>
        /// 今年度の登録有無に応じた選手移籍料を取得します。
        /// </summary>
        /// <param name="isRegisteredThisSeason">今年度既に登録されている/いない</param>
        /// <returns>選手移籍料。</returns>
        public PlayerRegistrationFee GetPlayerTradeFee(bool isRegisteredThisSeason)
        {
            return isRegisteredThisSeason ? new PlayerRegistrationFee(this.PlayerTradeFee.Value) : this.PlayerRegistrationFee;
        }
    }
}
