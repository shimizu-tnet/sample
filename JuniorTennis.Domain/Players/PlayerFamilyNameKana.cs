using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 姓(カナ)。
    /// </summary>
    public class PlayerFamilyNameKana : ValueObject
    {
        /// <summary>
        /// 姓(カナ)の最大文字数を取得します。
        /// </summary>
        public static int MaxLength => 20;

        /// <summary>
        /// 姓(カナ)を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 姓(カナ)の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">姓(カナ)。</param>
        public PlayerFamilyNameKana(string value) =>
            this.Value
                = string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException("姓(カナ)")
                : this.IsOverLength(value) ? throw new ArgumentException($"{MaxLength} 文字を超えています。", "姓(カナ)")
                : this.ContainsNotKanaChar(value) ? throw new ArgumentException($"カナ以外が入力されています。", "姓(カナ)")
                : value;

        /// <summary>
        /// 入力された姓(カナ)が最大文字数を超えているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された姓(カナ)。</param>
        /// <returns>最大文字数を超えている場合は true。それ以外の場合は false。</returns>
        private bool IsOverLength(string value) => value.Length > MaxLength;

        /// <summary>
        /// 入力された姓(カナ)にカナ以外が入力されているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された姓(カナ)。</param>
        /// <returns>カナ以外が入力されている場合は true。それ以外の場合は false。</returns>
        private bool ContainsNotKanaChar(string value)
        {
            return !Regex.IsMatch(value, @"^[ァ-ヶー]*$");
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }       
    }
}
