using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ブロック番号。
    /// </summary>
    public class BlockNumber : ValueObject
    {
        /// <summary>
        /// ブロック番号を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// ブロック番号の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">ブロック番号。</param>
        public BlockNumber(int value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
