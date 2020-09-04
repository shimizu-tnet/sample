using System;
using System.Collections.Generic;
using System.Text;
using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.RequestTeams
{
    /// <summary>
    /// 申請登録料。
    /// </summary>
    public class RequestedFee : ValueObject
    {
        /// <summary>
        /// 申請登録料を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// 申請登録料の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">申請登録料。</param>
        public RequestedFee(int value) =>
            this.Value
                = value < 0 ? throw new ArgumentException("申請登録料がマイナスです。", "申請登録料")
                : value;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
