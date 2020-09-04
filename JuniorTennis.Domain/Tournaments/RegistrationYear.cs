using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 登録年度。
    /// </summary>
    public class RegistrationYear : ValueObject
    {
        /// <summary>
        /// 登録年度を取得します。
        /// </summary>
        public DateTime Value { get; private set; }

        /// <summary>
        /// 登録年度の画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue => $"{this.Value:yyyy年度}";

        /// <summary>
        /// 登録年度の HTML 要素の値に設定する文字列を取得します。
        /// </summary>
        public string ElementValue => $"{this.Value:yyyy/MM/dd}";

        /// <summary>
        /// 集計月度が登録年度内で有効な日付（開始月から16ヶ月）かどうかを判定します。
        /// </summary>
        /// <param name="aggreateMonth">集計月度。</param>
        public void EnsureValidAggregationMonth(DateTime aggreateMonth)
        {
            if (this.Value <= aggreateMonth && aggreateMonth < this.Value.AddMonths(16))
            {
                return;
            }
            throw new ArgumentException("登録年度の対象範囲外の集計月度が指定されています。");
        }

        /// <summary>
        /// 開催期間の開始日が登録年度内で有効な日付（開始月から16ヶ月）かどうかを判定します。
        /// </summary>
        /// <param name="holdingStartDate">開催期間の開始日。</param>
        public void EnsureValidHoldingStartDate(DateTime? holdingStartDate)
        {
            if (holdingStartDate is null || this.Value <= holdingStartDate && holdingStartDate <= this.Value.AddMonths(16).AddDays(-1))
            {
                return;
            }
            throw new ArgumentException("登録年度の対象範囲外の開催期間が指定されています。");
        }

        /// <summary>
        /// 登録年度の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">登録年度。</param>
        public RegistrationYear(DateTime value) => this.Value = new DateTime(value.Year, 4, 1);

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
