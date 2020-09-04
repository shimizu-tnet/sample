using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// カテゴリを定義します。
    /// </summary>
    public class Category : Enumeration
    {
        /// <summary>
        /// 17/18歳以下
        /// </summary>
        public static readonly Category Under17Or18 = new Category(1, "17/18歳以下");

        /// <summary>
        /// 15/16歳以下
        /// </summary>
        public static readonly Category Under15Or16 = new Category(2, "15/16歳以下");

        /// <summary>
        /// 13/14歳以下
        /// </summary>
        public static readonly Category Under13Or14 = new Category(3, "13/14歳以下");

        /// <summary>
        /// 11/12歳以下
        /// </summary>
        public static readonly Category Under11Or12 = new Category(4, "11/12歳以下");

        /// <summary>
        /// カテゴリの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Category(int id, string name) : base(id, name) { }
    }
}
