using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JuniorTennis.Domain.Accounts
{
    /// <summary>
    /// メールアドレス。
    /// </summary>
    public class EmailAddress : ValueObject
    {
        /// <summary>
        /// メールアドレスを取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 入力されたメールアドレスがメールaddress形式かどうか判定します。
        /// </summary>
        /// <returns>メールアドレス形式の場合は false。それ以外の場合は true。</returns>
        /// <param name="value">メールアドレス。</param>
        private bool IsValidEmailAddress(string value)
        {
            return !Regex.IsMatch(
                value,
                @"\A[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\z",
                RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// メールアドレスの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">メールアドレス。</param>
        public EmailAddress(string value)
        {
            this.Value = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentNullException("メールアドレス")
                : IsValidEmailAddress(value) ? throw new ArgumentException("メールアドレス形式ではありません。", "メールアドレス")
                : value;
        }

        private EmailAddress()
        {

        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
