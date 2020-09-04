using System;
using System.Linq.Expressions;

namespace JuniorTennis.Domain.QueryConditions
{
    /// <summary>
    /// 並び替え条件。
    /// </summary>
    public class SortCondition<T>
    {
        /// <summary>
        /// 並び替え方向を取得します。
        /// </summary>
        public SortDirection Direction { get; }

        /// <summary>
        /// 並び替え条件を取得します。
        /// </summary>
        public Expression<Func<T, object>> Condition { get; }

        /// <summary>
        /// 並び替え条件の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="sortDirection">並び替え方向。</param>
        /// <param name="condition">並び替え条件。</param>
        public SortCondition(SortDirection sortDirection, Expression<Func<T, object>> condition)
        {
            this.Direction = sortDirection;
            this.Condition = condition;
        }

        /// <summary>
        /// 昇順の場合trueを返します。
        /// </summary>
        public bool IsAscending => this.Direction == SortDirection.Ascending;
    }
}
