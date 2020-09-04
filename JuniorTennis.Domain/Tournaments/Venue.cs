using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 会場。
    /// </summary>
    public class Venue : ValueObject
    {
        /// <summary>
        /// 会場の最大文字数を取得します。
        /// </summary>
        public static int MaxLength => 50;

        /// <summary>
        /// 会場を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 入力された会場が最大文字数を超えているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された会場。</param>
        /// <returns>最大文字数を超えている場合は true。それ以外の場合は false。</returns>
        private bool IsOverLength(string value) => value.Length > MaxLength;

        /// <summary>
        /// 会場の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">会場。</param>
        public Venue(string value)
        {
            this.Value
                = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException("会場")
                : this.IsOverLength(value) ? throw new ArgumentException($"{MaxLength} 文字を超えています。", "会場")
                : value;
        }

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
