using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 大会種別を定義します。
    /// </summary>
    public class TournamentType : Enumeration
    {
        /// <summary>
        /// ドローを作成する。
        /// </summary>
        public static readonly TournamentType WithDraw = new TournamentType(1, "ドローを作成する");

        /// <summary>
        /// ポイントのみ。
        /// </summary>
        public static readonly TournamentType OnlyPoints = new TournamentType(2, "ポイントのみ");

        /// <summary>
        /// 大会種別の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public TournamentType(int id, string name) : base(id, name) { }
    }
}
