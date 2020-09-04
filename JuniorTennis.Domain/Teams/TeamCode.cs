using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Teams
{
    /// <summary>
    /// 団体登録番号。
    /// </summary>
    public class TeamCode : ValueObject, IComparable
    {
        /// <summary>
        /// 団体登録番号を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 登録番号の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">登録番号。</param>
        public TeamCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException("団体登録番号");
            }

            this.Value = value;
        }

        public int CompareTo(object other) => this.Value.CompareTo(((TeamCode)other).Value);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
