using JuniorTennis.Domain.Players;
using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Ranking
{
    /// <summary>
    /// 獲得ポイント。
    /// </summary>
    public class EarnedPoints : EntityBase
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
        /// 登録番号を取得します。
        /// </summary>
        public PlayerCode PlayerCode { get; private set; }

        /// <summary>
        /// ポイントを取得します。
        /// </summary>
        public Point Point { get; private set; }

        /// <summary>
        /// 獲得ポイントの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="playerCode">登録番号。</param>
        /// <param name="point">ポイント。</param>
        public EarnedPoints(int tournamentId, string tennisEventId, PlayerCode playerCode, Point point)
        {
            this.TournamentId = tournamentId;
            this.TennisEventId = tennisEventId;
            this.PlayerCode = playerCode;
            this.Point = point;
        }
    }
}
