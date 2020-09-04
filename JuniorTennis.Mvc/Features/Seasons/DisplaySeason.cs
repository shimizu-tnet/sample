namespace JuniorTennis.Mvc.Features.Seasons
{
    /// <summary>
    /// 年度の表示用モデル
    /// </summary>
    public class DisplaySeason
    {
        /// <summary>
        /// 年度Idを取得します。
        /// </summary>
        public readonly int SeasonId;

        /// <summary>
        /// 年度を取得します。
        /// </summary>
        public readonly string SeasonName;

        /// <summary>
        /// 年度の期間を取得します。
        /// </summary>
        public readonly string SeasonPeriod;

        /// <summary>
        /// 団体登録料を取得します。
        /// </summary>
        public readonly string TeamRegistrationFee;

        /// <summary>
        /// 選手登録料を取得します。
        /// </summary>
        public readonly string PlayerRegistrationFee;

        /// <summary>
        /// 年度の表示用モデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="seasonId">年度Id。</param>
        /// <param name="seasonName">年度。</param>
        /// <param name="seasonPeriod">年度の期間。</param>
        /// <param name="teamRegistrationFee">団体登録料。</param>
        /// <param name="playerRegistrationFee">選手登録証。</param>
        public DisplaySeason(
            int seasonId,
            string seasonName,
            string seasonPeriod,
            string teamRegistrationFee,
            string playerRegistrationFee
            )
        {
            this.SeasonId = seasonId;
            this.SeasonName = seasonName;
            this.SeasonPeriod = seasonPeriod;
            this.TeamRegistrationFee = teamRegistrationFee;
            this.PlayerRegistrationFee = playerRegistrationFee;
        }
    }
}
