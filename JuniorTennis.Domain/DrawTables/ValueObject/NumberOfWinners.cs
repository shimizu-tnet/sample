using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 勝ち抜き数。
    /// </summary>
    public class NumberOfWinners : ValueObject
    {
        /// <summary>
        /// 勝ち抜き数を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// 勝ち抜き数の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">勝ち抜き数。</param>
        public NumberOfWinners(int value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
