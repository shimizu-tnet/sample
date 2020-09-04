using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Announcements
{
    /// <summary>
    /// お知らせタイトル。
    /// </summary>
    public class AnnouncementTitle : ValueObject
    {
        /// <summary>
        /// お知らせタイトルを取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// お知らせタイトルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="value">お知らせタイトル。</param>
        public AnnouncementTitle(string value)
        {
            this.Value = string.IsNullOrWhiteSpace(value)
                ? throw new ArgumentNullException("お知らせタイトル")
                : value;
        }

        private AnnouncementTitle()
        {

        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
