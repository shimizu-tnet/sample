using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// シード番号。
    /// </summary>
    public class SeedNumber : ValueObject
    {
        /// <summary>
        /// シード番号を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// シード選手かどうか示す値を取得します。
        /// </summary>
        public bool IsSeed => this.Value != 0;

        /// <summary>
        /// シード番号の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">シード番号。</param>
        public SeedNumber(int value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
