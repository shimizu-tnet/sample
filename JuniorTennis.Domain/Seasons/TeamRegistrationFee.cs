using System;
using System.Collections.Generic;
using System.Text;
using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Seasons
{
    public class TeamRegistrationFee : ValueObject
    {
        /// <summary>
        /// 団体登録料を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// 団体登録料を単位付きで取得します。
        /// </summary>
        public string DisplayValue => $"{string.Format("{0:#,0}", this.Value)}円";

        public TeamRegistrationFee(int value) =>
            this.Value
                = value < 0 ? throw new ArgumentException("団体登録料がマイナスです。", "団体登録料")
                : value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
