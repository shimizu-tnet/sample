using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 利用機能を定義します。
    /// </summary>
    public class UsageFeatures : Enumeration
    {
        /// <summary>
        /// 大会申込み。
        /// </summary>
        public static readonly UsageFeatures TournamentEntry = new UsageFeatures(1, "大会申込み");

        /// <summary>
        /// ドロー表。
        /// </summary>
        public static readonly UsageFeatures DrawTable = new UsageFeatures(2, "ドロー表");

        /// <summary>
        /// 利用機能の新しいインスタンスを生成します。
        /// </summary>
        public UsageFeatures(int id, string name) : base(id, name) { }
    }
}
