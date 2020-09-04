using System;
using System.Collections.Generic;
using System.Text;
using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Seasons
{
    public class PlayerTradeFee : ValueObject
    {
        /// <summary>
        /// 選手移籍料を取得します。
        /// </summary>
        public int Value { get; private set; }

        public PlayerTradeFee(int value) =>
            this.Value
                = value < 0 ? throw new ArgumentException("選手移籍料がマイナスです。", "選手移籍料")
                : value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
