using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 大会形式を定義します。
    /// </summary>
    public class TournamentFormat : Enumeration
    {
        /// <summary>
        /// 本戦のみ。
        /// </summary>
        public static readonly TournamentFormat OnlyMain = new TournamentFormat(1, "本戦のみ");

        /// <summary>
        /// 予選あり。
        /// </summary>
        public static readonly TournamentFormat WithQualifying = new TournamentFormat(2, "予選あり");

        /// <summary>
        /// 大会形式の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public TournamentFormat(int id, string name) : base(id, name) { }
    }
}
