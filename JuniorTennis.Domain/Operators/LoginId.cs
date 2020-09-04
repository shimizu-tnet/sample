using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JuniorTennis.Domain.Operators
{
    /// <summary>
    /// ログインId。
    /// </summary>
    public class LoginId : ValueObject
    {
        /// <summary>
        /// ログインIdの最大文字数を取得します。
        /// </summary>
        public static int MaxLength => 255;

        /// <summary>
        /// ログインIdの最小文字数を取得します。
        /// </summary>
        public static int MinLength => 6;

        /// <summary>
        /// ログインIdを取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 入力されたログインIdが最大文字数を超えているかどうか判定します。
        /// </summary>
        /// <param name="value">ログインId。</param>
        /// <returns>最大文字数を超えている場合は true。それ以外の場合は false。</returns>
        private bool IsOverLength(string value) => value.Length > MaxLength;

        /// <summary>
        /// 入力されたログインIdが最小文字数を下回るかどうか判定します。
        /// </summary>
        /// <param name="value">ログインId。</param>
        /// <returns>最小文字数を下回る場合は true。それ以外の場合は false。</returns>
        private bool IsBelowLength(string value) => value.Length < MinLength;

        /// <summary>
        /// 入力されたログインIdが半角英数かどうか判定します。
        /// </summary>
        /// <param name="value">ログインId。</param>
        /// <returns>半角英数のみの場合は false。それ以外の場合は true。</returns>
        private bool IsCharacterTypeHalfwidthAlphanumeric(string value)
        {
            return !Regex.IsMatch(
                value,
                @"^[0-9a-zA-Z]*$",
                RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// ログインIdの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">ログインId。</param>
        public LoginId(string value)
        {
            this.Value
                = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException("ログインId")
                : this.IsOverLength(value) ? throw new ArgumentException($"{MaxLength} 文字を超えています。", "ログインId")
                : this.IsBelowLength(value) ? throw new ArgumentException($"{MinLength} 文字を下回っています。", "ログインId")
                : this.IsCharacterTypeHalfwidthAlphanumeric(value) ? throw new ArgumentException($"半角英数字以外の文字が使われています。", "ログインId")
                : value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
        private LoginId()
        {

        }
    }
}
