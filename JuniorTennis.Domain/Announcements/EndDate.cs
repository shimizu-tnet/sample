using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Announcements
{
    /// <summary>
    /// 終了日。
    /// </summary>
    public class EndDate : ValueObject
    {
        /// <summary>
        /// 終了日を取得します。
        /// </summary>
        public DateTime Value { get; private set; }

        /// <summary>
        /// 終了日を文字列で取得します。
        /// </summary>
        public string DisplayValue => $"{this.Value:yyyy/M/d}";

        /// <summary>
        /// 終了日の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="endDate">終了日。</param>
        public EndDate(DateTime endDate) => this.Value = endDate;

        private EndDate()
        {

        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
