using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// エントリー数。
    /// </summary>
    public class NumberOfEntries : ValueObject
    {
        /// <summary>
        /// エントリー数を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// エントリー数の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">エントリー数。</param>
        public NumberOfEntries(int value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
