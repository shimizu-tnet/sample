using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 試合番号。
    /// </summary>
    public class GameNumber : ValueObject
    {
        /// <summary>
        /// 試合番号を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// 次の試合の割り当て位置として指定するドロー番号を取得します。
        /// </summary>
        public DrawNumber NextDrawNumber => this.Value % 2 != 0 ? new DrawNumber(1) : new DrawNumber(2);

        /// <summary>
        /// 試合番号の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">試合番号。</param>
        public GameNumber(int value) => this.Value = value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
