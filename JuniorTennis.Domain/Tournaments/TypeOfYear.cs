using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 年度種別を定義します。
    /// </summary>
    public class TypeOfYear : Enumeration
    {
        /// <summary>
        /// 奇数。
        /// </summary>
        public static readonly TypeOfYear Odd = new TypeOfYear(1, "奇数");

        /// <summary>
        /// 偶数。
        /// </summary>
        public static readonly TypeOfYear Evem = new TypeOfYear(2, "偶数");

        /// <summary>
        /// 大会種別の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public TypeOfYear(int id, string name) : base(id, name) { }
    }
}
