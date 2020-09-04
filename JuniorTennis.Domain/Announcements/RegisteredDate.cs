using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Announcements
{
    /// <summary>
    /// 登録日。
    /// </summary>
    public class RegisteredDate : ValueObject
    {
        /// <summary>
        /// 登録日を取得します。
        /// </summary>
        public DateTime Value { get; private set; }

        /// <summary>
        /// 登録日を文字列で取得します。
        /// </summary>
        public string DisplayValue => $"{this.Value:yyyy/M/d}";

        /// <summary>
        /// 登録日の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="registeredDate">登録日</param>
        public RegisteredDate(DateTime registeredDate) => this.Value = registeredDate;

        private RegisteredDate()
        {

        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
