using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 試合状況を定義します。
    /// </summary>
    public class GameStatus : Enumeration
    {
        /// <summary>
        /// 未。
        /// </summary>
        public static readonly GameStatus None = new GameStatus(1, "未");

        /// <summary>
        /// 済。
        /// </summary>
        public static readonly GameStatus Done = new GameStatus(2, "済");

        /// <summary>
        /// NotPlayed。
        /// </summary>
        public static readonly GameStatus NotPlayed = new GameStatus(3, "NotPlayed");

        /// <summary>
        /// Walkover。
        /// </summary>
        public static readonly GameStatus Walkover = new GameStatus(4, "Walkover");

        /// <summary>
        /// 準備未完了。
        /// </summary>
        public static readonly GameStatus NotReadied = new GameStatus(5, "準備未完了");

        /// <summary>
        /// 試合状況の新しいインスタンスを生成します。
        /// </summary>
        public GameStatus(int id, string name) : base(id, name) { }
    }
}
