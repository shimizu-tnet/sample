using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 姓。
    /// </summary>
    public class PlayerFamilyName : ValueObject
    {
        /// <summary>
        /// 姓の最大文字数を取得します。
        /// </summary>
        public static int MaxLength => 20;

        /// <summary>
        /// 姓を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 姓の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">姓。</param>
        public PlayerFamilyName(string value) =>
            this.Value
                = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException("姓")
                : this.IsOverLength(value) ? throw new ArgumentException($"{MaxLength} 文字を超えています。", "姓")
                : value;

        /// <summary>
        /// 入力された姓が最大文字数を超えているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された姓。</param>
        /// <returns>最大文字数を超えている場合は true。それ以外の場合は false。</returns>
        private bool IsOverLength(string value) => value.Length > MaxLength;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
