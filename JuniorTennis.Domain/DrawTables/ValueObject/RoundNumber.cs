using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ラウンド番号。
    /// </summary>
    public class RoundNumber : ValueObject
    {
        /// <summary>
        /// ラウンド番号を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// ラウンド番号の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">ラウンド番号。</param>
        public RoundNumber(int value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
