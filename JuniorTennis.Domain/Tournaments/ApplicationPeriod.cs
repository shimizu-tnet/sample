using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Utils;
using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 申込期間。
    /// </summary>
    public class ApplicationPeriod : ValueObject
    {
        /// <summary>
        /// 申込期間の開始日を取得または設定します。
        /// </summary>
        public DateTime StartDate { get; private set; }

        /// <summary>
        /// 申込期間の終了日を取得または設定します。
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
        /// 予約受付日が申込期間の範囲に収まっているかどうかを判定します。
        /// </summary>
        /// <param name="reservationDate">予約受付日。</param>
        public void EnsureValidReservationDate(ReservationDate reservationDate)
        {
            if (this.StartDate <= reservationDate.Value || reservationDate.Value <= this.EndDate)
            {
                return;
            }

            throw new ArgumentException("申込期間の範囲外の予約受付日が指定されています。");
        }

        /// <summary>
        /// 申込期間を表す JSON 文字列に変換します。
        /// </summary>
        /// <returns>申込期間を表す JSON 文字列。</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(new
            {
                startDate = this.StartDate,
                endDate = this.EndDate,
            });
        }

        /// <summary>
        /// JSON 文字列から申込期間のインスタンスを生成します。
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>申込期間。</returns>
        public static ApplicationPeriod FromJson(string json)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var startDate = JsonConverter.ToDateTime(jsonElement.GetProperty("startDate"));
            var endDate = JsonConverter.ToDateTime(jsonElement.GetProperty("endDate"));
            return new ApplicationPeriod(startDate, endDate);
        }

        /// <summary>
        /// 申込期間の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="startDate">開始日。</param>
        /// <param name="endDate">終了日。</param>
        public ApplicationPeriod(DateTime startDate, DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
            {
                throw new ArgumentException("申込期間の開始日に、終了日を超える日付が指定されています。");
            }

            this.StartDate = startDate.Date;
            this.EndDate = endDate.Date;
        }

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
