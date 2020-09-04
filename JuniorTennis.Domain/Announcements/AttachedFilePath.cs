using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Announcements
{
    /// <summary>
    /// 添付ファイルパス
    /// </summary>
    public class AttachedFilePath : ValueObject
    {
        /// <summary>
        /// 添付ファイルパスを取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 添付ファイルパスの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">添付ファイルパス。</param>
        public AttachedFilePath(string value) =>
            this.Value
                = value == null ? throw new ArgumentNullException("添付ファイルパス")
                : string.IsNullOrWhiteSpace(value) ? throw new ArgumentException("未入力です。", "添付ファイルパス")
                : value;

        private AttachedFilePath()
        {

        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
