namespace JuniorTennis.Mvc.Features.Tournaments
{
    /// <summary>
    /// 大会。
    /// </summary>
    public class DisplayTournament
    {
        /// <summary>
        /// 大会 ID を取得または設定します。
        /// </summary>
        public readonly string TournamentId;

        /// <summary>
        /// 大会名を取得または設定します。
        /// </summary>
        public readonly string TournamentName;

        /// <summary>
        /// 大会種別を取得または設定します。
        /// </summary>
        public readonly string TournamentType;

        /// <summary>
        /// 開催期間を取得または設定します。
        /// </summary>
        public readonly string HoldingPeriod;

        /// <summary>
        /// 申込期間を取得または設定します。
        /// </summary>
        public readonly string ApplicationPeriod;

        /// <summary>
        /// 会場を取得または設定します。
        /// </summary>
        public readonly string Venue;

        /// <summary>
        /// 参加費を取得または設定します。
        /// </summary>
        public readonly string EntryFee;

        /// <summary>
        /// 大会の新しいインスタンスを生成します。
        /// </summary>
        public DisplayTournament(
            string tournamentId,
            string tournamentName,
            string tournamentType,
            string holdingPeriod,
            string applicationPeriod,
            string venue,
            string entryFee)
        {
            this.TournamentId = tournamentId;
            this.TournamentName = tournamentName;
            this.TournamentType = tournamentType;
            this.HoldingPeriod = holdingPeriod;
            this.ApplicationPeriod = applicationPeriod;
            this.Venue = venue;
            this.EntryFee = entryFee;
        }
    }
}
