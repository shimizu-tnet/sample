using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 割当順。
    /// </summary>
    public class AssignOrder : ValueObject
    {
        /// <summary>
        /// 割当順を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// 割当順の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">割り当て順。</param>
        public AssignOrder(int value)
        {
            this.Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
