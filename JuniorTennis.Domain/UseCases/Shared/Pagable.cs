using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.UseCases.Shared
{
    /// <summary>
    /// ページング処理
    /// </summary>
    /// <typeparam name="T">表示する要素の型。</typeparam>
    public class Pagable<T>
    {
        /// <summary>
        /// 表示オブジェクトのリストを取得します。
        /// </summary>
        public IReadOnlyList<T> List { get; }

        /// <summary>
        /// 現在のページ番号を取得します。
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// 表示オブジェクトの総数を取得します。
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// ページ当たりの表示件数を取得します。
        /// </summary>
        public int DisplayCount { get; }

        /// <summary>
        /// ページング処理の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="list">表示オブジェクトリスト。</param>
        /// <param name="pageIndex">現在のページ番号</param>
        /// <param name="totalCount">表示オブジェクトの総数。</param>
        /// <param name="displayCount">ページ当たりの表示件数。</param>
        public Pagable(List<T> list, int pageIndex, int totalCount, int displayCount)
        {
            this.List = list;
            this.PageIndex = pageIndex < 0 ? throw new ArgumentException("ページ番号が不正です。", "PageIndex") : pageIndex;
            this.TotalCount = totalCount < 0 ? throw new ArgumentException("表示オブジェクトの総数が不正です。", "TotalCount") : totalCount;
            this.DisplayCount = displayCount < 0 ? throw new ArgumentException("ページ当たりの表示件数が不正です。", "DisplayCount") : displayCount;
        }
    }
}
