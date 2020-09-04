using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ドロー数。
    /// </summary>
    public class NumberOfDraws : ValueObject
    {
        /// <summary>
        /// ドロー数を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// ドロー数が設定済みかどうか示す値を取得します。
        /// </summary>
        public bool IsConfigured => this.Value != 0;

        /// <summary>
        /// ドロー数の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">ドロー数。</param>
        public NumberOfDraws(int value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
