using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// エントリー番号。
    /// </summary>
    public class EntryNumber : ValueObject
    {
        /// <summary>
        /// エントリー番号を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// エントリー番号の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">エントリー番号。</param>
        public EntryNumber(int value) => this.Value = value;

        /// <summary>
        /// 入力された値からエントリー番号を生成します。
        /// </summary>
        /// <param name="value">入力値。</param>
        /// <returns>エントリー番号。</returns>
        public static EntryNumber FromValue(int? value)
        {
            if (value.HasValue)
            {
                return new EntryNumber(value.Value);
            }

            return null;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
