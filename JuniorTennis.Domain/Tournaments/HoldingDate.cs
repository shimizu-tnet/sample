using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 開催日。
    /// </summary>
    public class HoldingDate : ValueObject
    {
        /// <summary>
        /// 大会 ID を取得または設定します。
        /// </summary>
        public int TournamentId { get; private set; }

        /// <summary>
        /// 大会の開催日を取得します。
        /// </summary>
        public DateTime Value { get; private set; }

        /// <summary>
        /// 開催日の画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue => $"{this.Value:M/d}";

        /// <summary>
        /// 開催日の HTML 要素の値に設定する文字列を取得します。
        /// </summary>
        public string ElementValue => $"{this.Value:yyyy/MM/dd}";

        /// <summary>
        /// 開催日の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="holdingDate">開催日。</param>
        public HoldingDate(DateTime holdingDate) => this.Value = holdingDate;

        /// <summary>
        /// 開催日の新しいインスタンスを生成します。
        /// </summary>
        private HoldingDate() { }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public Tournament Tournament { get; private set; }
    }
}
