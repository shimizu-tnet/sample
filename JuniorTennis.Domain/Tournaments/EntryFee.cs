using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 参加費。
    /// </summary>
    public class EntryFee : ValueObject
    {
        /// <summary>
        /// 参加費を取得します。
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// 参加費の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">参加費。</param>
        public EntryFee(int value) => this.Value = value;

        /// <summary>
        /// 参加費の画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue => $"{this.Value:#,###} 円";

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
