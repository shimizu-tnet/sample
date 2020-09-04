using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 登録番号。
    /// </summary>
    public class PlayerCode : ValueObject
    {
        /// <summary>
        /// 登録番号の最大文字数を取得します。
        /// </summary>
        public static int MaxLength => 7;

        /// <summary>
        /// 登録番号を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 登録番号の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">登録番号。</param>
        public PlayerCode(string value) =>
            this.Value
                = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException("登録番号")
                : this.IsOverLength(value) ? throw new ArgumentException($"{MaxLength} 文字を超えています。", "登録番号")
                : value;

        /// <summary>
        /// 入力された登録番号が最大文字数を超えているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された登録番号。</param>
        /// <returns>最大文字数を超えている場合は true。それ以外の場合は false。</returns>
        private bool IsOverLength(string value) => value.Length > MaxLength;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
