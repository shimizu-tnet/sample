using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 集計月度。
    /// </summary>
    public class AggregationMonth : ValueObject
    {
        /// <summary>
        /// 集計月度を取得します。
        /// </summary>
        public DateTime Value { get; private set; }

        /// <summary>
        /// 集計月度の画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue => $"{this.Value:yyyy年M月}";

        /// <summary>
        /// 集計月度の HTML 要素の値に設定する文字列を取得します。
        /// </summary>
        public string ElementValue => $"{this.Value:yyyy/MM/dd}";

        /// <summary>
        /// 集計月度の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">集計月度。</param>
        public AggregationMonth(DateTime value) => this.Value = new DateTime(value.Year, value.Month, 1);

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
