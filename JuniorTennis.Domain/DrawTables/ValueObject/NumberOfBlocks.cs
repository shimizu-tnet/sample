using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ブロック数。
    /// </summary>
    public class NumberOfBlocks : ValueObject
    {
        /// <summary>
        /// ブロック数を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// ブロック数の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">ブロック数。</param>
        public NumberOfBlocks(int value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
