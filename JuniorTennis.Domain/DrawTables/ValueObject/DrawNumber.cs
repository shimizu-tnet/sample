using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ドロー番号。
    /// </summary>
    public class DrawNumber : ValueObject
    {
        /// <summary>
        /// ドロー番号を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// ドロー番号の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">ドロー番号。</param>
        public DrawNumber(int value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
