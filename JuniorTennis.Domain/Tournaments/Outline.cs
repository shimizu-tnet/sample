using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 大会要領。
    /// </summary>
    public class Outline : ValueObject
    {
        /// <summary>
        /// 大会要領を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 大会要領の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">大会要領。</param>
        public Outline(string value)
        {
            this.Value = value;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public Tournament Tournament { get; private set; }
    }
}
