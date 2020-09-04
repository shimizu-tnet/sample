using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 名。
    /// </summary>
    public class PlayerFirstName : ValueObject
    {
        /// <summary>
        /// 名の最大文字数を取得します。
        /// </summary>
        public static int MaxLength => 30;

        /// <summary>
        /// 名を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 名の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">名。</param>
        public PlayerFirstName(string value) =>
            this.Value
                = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException("名")
                : this.IsOverLength(value) ? throw new ArgumentException($"{MaxLength} 文字を超えています。", "名")
                : value;

        /// <summary>
        /// 入力された名が最大文字数を超えているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された名。</param>
        /// <returns>最大文字数を超えている場合は true。それ以外の場合は false。</returns>
        private bool IsOverLength(string value) => value.Length > MaxLength;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
