using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 出場対象選手の種別を定義します。
    /// </summary>
    public class EligiblePlayersType : Enumeration
    {
        /// <summary>
        /// すべての選手を表示する。
        /// </summary>
        public static readonly EligiblePlayersType AllPlayers = new EligiblePlayersType(1, "すべての選手を表示する");

        /// <summary>
        /// 受領済みの選手のみを表示する。
        /// </summary>
        public static readonly EligiblePlayersType RecievedPlayers = new EligiblePlayersType(2, "受領済みの選手のみを表示する");

        /// <summary>
        /// 出場対象選手の種別の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public EligiblePlayersType(int id, string name) : base(id, name) { }
    }
}
