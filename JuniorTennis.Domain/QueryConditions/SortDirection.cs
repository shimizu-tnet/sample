using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.QueryConditions
{
    /// <summary>
    /// 並び替え方向を定義します。
    /// </summary>
    public class SortDirection : Enumeration
    {
        /// <summary>
        /// 昇順。
        /// </summary>
        public static readonly SortDirection Ascending = new SortDirection(1, "昇順");

        /// <summary>
        /// 降順。
        /// </summary>
        public static readonly SortDirection Descending = new SortDirection(2, "降順");

        /// <summary>
        /// 団体種別の新しいインスタンスを生成します。
        /// </summary>
        public SortDirection(int id, string name) : base(id, name) { }
    }
}
