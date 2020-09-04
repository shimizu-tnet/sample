using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 大会名。
    /// </summary>
    public class TournamentName : ValueObject
    {
        /// <summary>
        /// 大会名の最大文字数を取得します。
        /// </summary>
        public static int MaxLength => 50;

        /// <summary>
        /// 大会名を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 入力された大会名が最大文字数を超えているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された大会名。</param>
        /// <returns>最大文字数を超えている場合は true。それ以外の場合は false。</returns>
        private bool IsOverLength(string value) => value.Length > MaxLength;

        /// <summary>
        /// 大会名の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">大会名。</param>
        public TournamentName(string value)
        {
            this.Value
                = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException("大会名")
                : this.IsOverLength(value) ? throw new ArgumentException($"{MaxLength} 文字を超えています。", "大会名")
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
