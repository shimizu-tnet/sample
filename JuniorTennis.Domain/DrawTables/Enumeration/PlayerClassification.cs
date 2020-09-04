using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 選手区分を定義します。
    /// </summary>
    public class PlayerClassification : Enumeration
    {
        /// <summary>
        /// シード。
        /// </summary>
        public static readonly PlayerClassification Seed = new PlayerClassification(1, "シード");

        /// <summary>
        /// 一般。
        /// </summary>
        public static readonly PlayerClassification General = new PlayerClassification(2, "一般");

        /// <summary>
        /// BYE。
        /// </summary>
        public static readonly PlayerClassification Bye = new PlayerClassification(3, "BYE");

        /// <summary>
        /// 選手区分の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public PlayerClassification(int id, string name) : base(id, name) { }
    }
}
