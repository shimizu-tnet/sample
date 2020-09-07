using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 出場可能日。
    /// </summary>
    public class CanParticipationDate : ValueObject
    {
        /// <summary>
        /// 出場可能日を取得します。
        /// </summary>
        public DateTime Value { get; private set; }

        /// <summary>
        /// 出場可能日の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">出場可能日。</param>
        public CanParticipationDate(DateTime value) => this.Value = value;

        /// <summary>
        /// 出場可能日の画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue => $"{this.Value:M/d}";

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
