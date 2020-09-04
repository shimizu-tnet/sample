using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 性別を定義します。
    /// </summary>
    public class Gender : Enumeration
    {
        /// <summary>
        /// 男子。
        /// </summary>
        public static readonly Gender Boys = new Gender(1, "男子");

        /// <summary>
        /// 女子。
        /// </summary>
        public static readonly Gender Girls = new Gender(2, "女子");

        /// <summary>
        /// 性別の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Gender(int id, string name) : base(id, name) { }
    }
}
