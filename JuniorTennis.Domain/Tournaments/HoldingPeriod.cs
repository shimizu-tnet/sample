using JuniorTennis.Domain.Utils;
using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 開催期間。
    /// </summary>
    public class HoldingPeriod : ValueObject
    {
        /// <summary>
        /// 開催期間の開始日を取得します。
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// 開催期間の終了日を取得します。
        /// </summary>
        public DateTime EndDate { get; private set; }

        /// <summary>
        /// 大会期間の画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue => $"{this.StartDate:yyyy年M月d日} ～ {this.EndDate:yyyy年M月d日}";

        /// <summary>
        /// 大会期間の画面表示用の短い文字列を取得します。
        /// </summary>
        public string ShortDisplayValue => $"{this.StartDate:yyyy/M/d} ～ {this.EndDate:yyyy/M/d}";

        /// <summary>
        /// 大会期間を表す JSON 文字列に変換します。
        /// </summary>
        /// <returns>大会期間を表す JSON 文字列。</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(new
            {
                startDate = this.StartDate,
                endDate = this.EndDate,
            });
        }

        /// <summary>
        /// JSON 文字列から大会期間のインスタンスを生成します。
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>大会期間。</returns>
        public static HoldingPeriod FromJson(string json)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var startDate = JsonConverter.ToDateTime(jsonElement.GetProperty("startDate"));
            var endDate = JsonConverter.ToDateTime(jsonElement.GetProperty("endDate"));
            return new HoldingPeriod(startDate, endDate);
        }

        /// <summary>
        /// 開催日が開催期間の範囲に収まっているかどうかを判定します。
        /// </summary>
        /// <param name="holdingDates">開催日の一覧。</param>
        public void EnsureValidHoldingDates(List<HoldingDate> holdingDates)
        {
            if (holdingDates is null || !holdingDates.Where(o => o.Value < this.StartDate || o.Value > this.EndDate).Any())
            {
                return;
            }
            throw new ArgumentException("開催期間の範囲外の開催日が指定されています。");
        }

        /// <summary>
        /// 申込期間が開催期間と重複しているかどうかを判定します。
        /// </summary>
        /// <param name="applicationEndDate">申込期間の終了日。</param>
        public void EnsureValidApplicationEndDate(DateTime? applicationEndDate)
        {
            if (applicationEndDate is null || applicationEndDate.Value < this.StartDate)
            {
                return;
            }
            throw new ArgumentException("申込期間が開催期間と重複しています。");
        }

        /// <summary>
        /// 開催期間の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="startDate">開始日。</param>
        /// <param name="endDate">終了日。</param>
        public HoldingPeriod(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
            {
                throw new ArgumentException("開催期間の開始日に、終了日を超える日付が指定されています。");
            }

            this.StartDate = startDate.Date;
            this.EndDate = endDate.Date;
        }

        /// <summary>
        /// 開催期間の新しいインスタンスを生成します。
        /// </summary>
        private HoldingPeriod() { }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return (this.StartDate, this.EndDate);
        }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public Tournament Tournament { get; private set; }
    }
}
