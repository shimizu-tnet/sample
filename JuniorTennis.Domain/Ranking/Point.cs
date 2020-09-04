using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Ranking
{
    /// <summary>
    /// ポイント。
    /// </summary>
    public class Point : ValueObject
    {
        /// <summary>
        /// ポイントを取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// ポイントの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">ポイント。</param>
        public Point(int value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
