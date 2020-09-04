using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 誕生日。
    /// </summary>
    public class BirthDate : ValueObject
    {
        /// <summary>
        /// 誕生日を取得します。
        /// </summary>
        public DateTime Value { get; private set; }

        /// <summary>
        /// 誕生日の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">出場可能日。</param>
        public BirthDate(DateTime value) => this.Value = value;

        /// <summary>
        /// 誕生日の画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue => $"{this.Value:yyyy/M/d}";

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
