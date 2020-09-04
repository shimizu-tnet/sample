using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// スコア。
    /// </summary>
    public class GameScore : ValueObject
    {
        /// <summary>
        /// スコアを取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// スコアの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">エントリー番号。</param>
        public GameScore(string value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
