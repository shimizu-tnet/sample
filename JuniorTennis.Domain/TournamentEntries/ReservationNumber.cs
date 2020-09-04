using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 予約番号。
    /// </summary>
    public class ReservationNumber : ValueObject
    {
        /// <summary>
        /// 予約番号を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// チームごとの予約番号を取得します。
        /// </summary>
        public string TeamValue => this.Value.Substring(0, 12);

        /// <summary>
        /// 個人ごとの予約番号を取得します。
        /// </summary>
        public string PersonalValue => this.Value.Substring(12);

        /// <summary>
        /// 予約番号の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">予約番号。</param>
        public ReservationNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("予約番号");
            }

            if (!this.IsDesignatedLength(value))
            {
                throw new ArgumentException("指定された文字数と異なります。", "予約番号");
            }

            if (!this.IsNumericValue(value))
            {
                throw new ArgumentException("不正な文字が含まれています。", "予約番号");
            }

            this.Value = value;
        }

        /// <summary>
        /// 予約番号の指定文字数を取得します。
        /// </summary>
        private static int DesignatedLength => 14;

        /// <summary>
        /// 指定された文字数かどうかを判定します。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <returns>指定された文字数の場合 true。それ以外は false。</returns>
        private bool IsDesignatedLength(string value) => value.Length == DesignatedLength;

        /// <summary>
        /// 指定された文字列が全て数字で構成されているかどうか判定します。
        /// </summary>
        /// <param name="value">文字列。</param>
        /// <returns>全て数字で構成されている場合 true。それ以外は false。</returns>
        private bool IsNumericValue(string value) => new Regex(@"^\d+$").IsMatch(value);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
