using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Teams
{
    /// <summary>
    /// 団体名称。
    /// </summary>
    public class TeamName : ValueObject
    {
        /// <summary>
        /// 団体名称の最大文字数を取得します。
        /// </summary>
        public static int MaxLength => 50;

        /// <summary>
        /// 団体名称を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 団体名称の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">団体名称。</param>
        public TeamName(string value) =>
            this.Value
                = value == null ? throw new ArgumentNullException("団体名称")
                : string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("未入力です。", "団体名称")
                : this.IsOverLength(value) ? throw new ArgumentException($"{MaxLength} 文字を超えています。", "団体名称")
                : value;

        /// <summary>
        /// 入力された団体名称が最大文字数を超えているかどうかを判定します。
        /// </summary>
        /// <param name="value">入力された団体名称。</param>
        /// <returns>最大文字数を超えている場合は true。それ以外の場合は false。</returns>
        private bool IsOverLength(string value) => value.Length > MaxLength;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
