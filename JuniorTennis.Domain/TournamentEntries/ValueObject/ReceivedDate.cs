using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 受領日。
    /// </summary>
    public class ReceivedDate : ValueObject
    {
        /// <summary>
        /// 大会申込の受領日を取得します。
        /// </summary>
        public DateTime? Value { get; private set; }

        /// <summary>
        /// 受領日の画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue =>
            this.Value.HasValue
            ? $"{this.Value.Value:yyyy/M/d}"
            : "-";

        /// <summary>
        /// 受領日の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="receivedDate">受領日。</param>
        public ReceivedDate(DateTime? receivedDate) => this.Value = receivedDate;

        /// <summary>
        /// 受領日の新しいインスタンスを生成します。
        /// </summary>
        private ReceivedDate() { }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
