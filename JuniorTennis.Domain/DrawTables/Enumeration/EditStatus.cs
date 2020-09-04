using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 編集状態を定義します。
    /// </summary>
    public class EditStatus : Enumeration
    {
        /// <summary>
        /// 編集中。
        /// </summary>
        public static readonly EditStatus Editing = new EditStatus(1, "編集中");

        /// <summary>
        /// ドラフト。
        /// </summary>
        public static readonly EditStatus Draft = new EditStatus(2, "ドラフト");

        /// <summary>
        /// 公開済み。
        /// </summary>
        public static readonly EditStatus Published = new EditStatus(3, "公開済み");

        /// <summary>
        /// 編集状態の新しいインスタンスを生成します。
        /// </summary>
        public EditStatus(int id, string name) : base(id, name) { }
    }
}
