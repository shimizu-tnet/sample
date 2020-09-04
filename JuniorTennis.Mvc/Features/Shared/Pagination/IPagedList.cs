namespace JuniorTennis.Mvc.Features.Shared.Pagination
{
    /// <summary>
    /// １ページに分けた一覧のインターフェース。
    /// </summary>
    public interface IPagedList
    {
        /// <summary>
        /// 現在のページインデックスを取得します。
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// 全ページの総数を取得します。
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// ページ当たりの表示件数を取得します。
        /// </summary>
        int DisplayCount { get; }

        /// <summary>
        /// ページ総数を取得します。
        /// </summary>
        int TotalPageCount { get; }

        /// <summary>
        /// ページネイションの現在のページ番号を取得します。
        /// </summary>
        int SelectedPageNumber { get; }

        /// <summary>
        /// ページネイションの最初のページ番号を取得します。
        /// </summary>
        int FirstPageNumber { get; }

        /// <summary>
        /// ページネイションの最後のページ番号を取得します。
        /// </summary>
        int LastPageNumber { get; }

        /// <summary>
        /// ページネイションに表示するページ番号を取得します。
        /// </summary>
        int[] PageNumbers { get; }

        /// <summary>
        /// 前ページが存在するか判定します。
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 次ページが存在するか判定します。
        /// </summary>
        bool HasNextPage { get; }
    }
}
