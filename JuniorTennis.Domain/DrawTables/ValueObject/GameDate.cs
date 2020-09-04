using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 試合日。
    /// </summary>
    public class GameDate : ValueObject
    {
        /// <summary>
        /// 試合日を取得します。
        /// </summary>
        public DateTime Value { get; private set; }

        /// <summary>
        /// 試合日の画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue => $"{this.Value:M/d}";

        /// <summary>
        /// 試合日の HTML 要素の値に設定する文字列を取得します。
        /// </summary>
        public string ElementValue => $"{this.Value:yyyy/MM/dd}";

        /// <summary>
        /// 試合日の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">試合日。</param>
        public GameDate(DateTime value) => this.Value = value.Date;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
