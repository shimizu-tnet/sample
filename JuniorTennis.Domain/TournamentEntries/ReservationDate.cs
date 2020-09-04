using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 受付日。
    /// </summary>
    public class ReservationDate : ValueObject
    {
        /// <summary>
        /// 大会申込の受付日を取得します。
        /// </summary>
        public DateTime Value { get; private set; }

        /// <summary>
        /// 受付日の画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue => $"{this.Value:yyyy/M/d}";

        /// <summary>
        /// 受付日の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="reservationDate">受付日。</param>
        public ReservationDate(DateTime reservationDate) => this.Value = reservationDate;

        /// <summary>
        /// 受付日の新しいインスタンスを生成します。
        /// </summary>
        private ReservationDate() { }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
