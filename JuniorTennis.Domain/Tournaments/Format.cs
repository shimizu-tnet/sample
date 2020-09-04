using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 試合形式を定義します。
    /// </summary>
    public class Format : Enumeration
    {
        /// <summary>
        /// シングルス。
        /// </summary>
        public static readonly Format Singles = new Format(1, "シングルス");

        /// <summary>
        /// ダブルス。
        /// </summary>
        public static readonly Format Doubles = new Format(2, "ダブルス");

        /// <summary>
        /// 試合形式の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Format(int id, string name) : base(id, name) { }
    }
}
