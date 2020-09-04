using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Shared.Pagination
{
    /// <summary>
    /// １ページに分けた一覧。
    /// </summary>
    /// <typeparam name="T">表示する要素の型。</typeparam>
    public class PagedList<T> : IReadOnlyList<T>, IPagedList
    {
        /// <summary>
        /// ページネイションに表示するページ件数。
        /// </summary>
        const int DisplayPaginationCount = 5;

        /// <summary>
        /// １ページ分の一覧。
        /// </summary>
        private readonly IReadOnlyList<T> list;

        /// <summary>
        /// 現在のページインデックスを取得します。
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// 全ページの総数を取得します。
        /// </summary>
        public int TotalCount { get; }
        /// <summary>
        /// ページ当たりの表示件数を取得します。
        /// </summary>
        public int DisplayCount { get; }

        /// <summary>
        /// ページ総数を取得します。
        /// </summary>
        public int TotalPageCount { get; }

        /// <summary>
        /// ページネイションの現在のページ番号を取得します。
        /// </summary>
        public int SelectedPageNumber => this.PageIndex + 1;

        /// <summary>
        /// ページネイションの最初のページ番号を取得します。
        /// </summary>
        public int FirstPageNumber { get; }

        /// <summary>
        /// ページネイションの最後のページ番号を取得します。
        /// </summary>
        public int LastPageNumber { get; }

        /// <summary>
        /// ページネイションに表示するページ番号を取得します。
        /// </summary>
        public int[] PageNumbers => Enumerable.Range(this.FirstPageNumber, this.LastPageNumber - this.FirstPageNumber + 1).ToArray();

        /// <summary>
        /// 前ページが存在するか判定します。
        /// </summary>
        public bool HasPreviousPage => this.SelectedPageNumber > 1;

        /// <summary>
        /// 次ページが存在するか判定します。
        /// </summary>
        public bool HasNextPage => this.SelectedPageNumber < this.TotalPageCount;

        /// <summary>
        /// 現在のページの要素の件数を取得します。
        /// </summary>
        public int Count => this.list.Count;

        /// <summary>
        /// インデクサー。
        /// </summary>
        /// <param name="index">インデックス。</param>
        /// <returns>T。</returns>
        public T this[int index] => this.list[index];

        /// <summary>
        /// ページング処理の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="list">表示オブジェクトリスト。</param>
        /// <param name="pageIndex">現在のページ数</param>
        /// <param name="totalCount">表示オブジェクトの総数。</param>
        /// <param name="displayCount">ページ当たりの表示件数。</param>
        public PagedList(List<T> list, int pageIndex, int totalCount, int displayCount)
        {
            this.list = list;
            this.TotalCount = totalCount;
            this.DisplayCount = displayCount;
            this.TotalPageCount = (int)Math.Ceiling(totalCount / (decimal)displayCount);
            this.PageIndex = pageIndex >= this.TotalPageCount ? 0 : pageIndex;
            this.FirstPageNumber = this.GetFirstPageNumber();
            this.LastPageNumber = this.GetLastPageNumber();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An System.Collections.IEnumerator object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An System.Collections.IEnumerator object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        private int GetFirstPageNumber()
        {
            if (this.TotalPageCount <= DisplayPaginationCount)
            {
                return 1;
            }

            if (this.SelectedPageNumber <= 2)
            {
                return 1;
            }

            if ((this.TotalPageCount - 1) <= this.SelectedPageNumber)
            {
                return this.TotalPageCount - (DisplayPaginationCount - 1);
            }

            return this.SelectedPageNumber - (int)Math.Floor(DisplayPaginationCount / (decimal)2);
        }

        private int GetLastPageNumber()
        {
            if (this.TotalPageCount <= DisplayPaginationCount)
            {
                return this.TotalPageCount;
            }

            if (this.SelectedPageNumber <= 2)
            {
                return DisplayPaginationCount;
            }

            if ((this.TotalPageCount - 1) <= this.SelectedPageNumber)
            {
                return this.TotalPageCount;
            }

            return this.SelectedPageNumber + (int)Math.Floor(DisplayPaginationCount / (decimal)2);
        }
    }
}
