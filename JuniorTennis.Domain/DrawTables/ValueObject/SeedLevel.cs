using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// シードレベル。
    /// </summary>
    public class SeedLevel : ValueObject
    {
        /// <summary>
        /// シードレベルを取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// シード選手かどうか示す値を取得します。
        /// </summary>
        public bool IsSeed => this.Value != 0;

        /// <summary>
        /// シードレベルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">シードレベル。</param>
        public SeedLevel(int value)
        {
            if (value > 6)
            {
                throw new ArgumentException("シードレベルは 0 ～ 6 の範囲で指定してください。");
            }

            this.Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
