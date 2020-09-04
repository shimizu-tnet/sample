using System;
using System.Linq;
using System.Linq.Expressions;

namespace JuniorTennis.Domain.QueryConditions
{
    /// <summary>
    /// 検索条件の抽象クラス。
    /// </summary>
    /// <typeparam name="T">検索条件の対象となるエンティティの型引数。</typeparam>
    public abstract class SearchCondition<T>
    {
        /// <summary>
        /// 抽出条件一覧。
        /// </summary>
        private FilterList<T> filters;

        /// <summary>
        /// 並び替え条件。
        /// </summary>
        private SortCondition<T> sortCondition;

        /// <summary>
        /// trueの場合ページ分割します。
        /// </summary>
        private bool isPaging = false;

        /// <summary>
        /// trueの場合並び替え条件があります。
        /// </summary>
        private bool isSorting => this.sortCondition != null;

        /// <summary>
        /// ページ番号を取得します。
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// 表示件数を取得します。
        /// </summary
        public int DisplayCount { get; private set; }

        /// <summary>
        /// スキップ数を取得します。
        /// </summary>
        public int SkipCount => this.PageIndex * this.DisplayCount;

        /// <summary>
        /// 検索条件の新しいインスタンスを生成します。
        /// </summary>
        public SearchCondition() => this.filters = new FilterList<T>();

        /// <summary>
        /// すべての検索条件を適用します。
        /// </summary>
        /// <param name="query">クエリ。</param>
        /// <returns>検索条件適用後のクエリ。</returns>
        public IQueryable<T> Apply(IQueryable<T> query)
        {
            var appliedQuery = this.ApplyConditions(query);
            if (this.isPaging)
            {
                appliedQuery = appliedQuery
                    .Skip(this.SkipCount)
                    .Take(this.DisplayCount);
            }

            return appliedQuery;
        }

        /// <summary>
        /// ページング以外の検索条件を適用します。
        /// </summary>
        /// <param name="query">クエリ。</param>
        /// <remarks>ページングは適用しません。</remarks>
        /// <returns>検索条件適用後のクエリ。</returns>
        public IQueryable<T> ApplyWithoutPagination(IQueryable<T> query) => this.ApplyConditions(query);

        /// <summary>
        /// 検索条件を適用します。
        /// </summary>
        /// <param name="query">クエリ。</param>
        /// <remarks>ページングは適用しません。</remarks>
        /// <returns>検索条件適用後のクエリ。</returns>
        private IQueryable<T> ApplyConditions(IQueryable<T> query)
        {
            var appliedQuery = query;
            if (this.filters.Any())
            {
                foreach (var filter in this.filters)
                {
                    appliedQuery = appliedQuery.Where(filter);
                }
            }

            if (this.isSorting)
            {
                appliedQuery = this.sortCondition.IsAscending
                    ? appliedQuery.OrderBy(this.sortCondition.Condition)
                    : appliedQuery.OrderByDescending(this.sortCondition.Condition);
            }

            return appliedQuery;
        }

        /// <summary>
        /// 抽出条件を追加します。
        /// </summary>
        /// <param name="filter">抽出条件。</param>
        protected void AddFilter(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            this.filters.Add(filter);
        }

        /// <summary>
        /// 並び替え条件を追加します。
        /// </summary>
        /// <param name="direction">並び替え方向。</param>
        /// <param name="condition">並び替え条件。</param>
        protected void AddSort(SortDirection direction, Expression<Func<T, object>> condition)
        {
            if (this.sortCondition != null)
            {
                throw new InvalidOperationException("並び替え条件が既に設定されています。");
            }

            if (direction == null) 
            {
                throw new ArgumentNullException(nameof(direction));
            }

            if(condition==null)
            { 
                throw new ArgumentNullException(nameof(condition));
            }

            this.sortCondition = new SortCondition<T>(direction, condition);
        }

        /// <summary>
        /// ページ分割します。
        /// </summary>
        /// <param name="pageIndex">ページ番号。</param>
        /// <param name="displayCount">表示件数。</param>
        protected void Paginate(int pageIndex, int displayCount)
        {
            this.PageIndex = pageIndex;
            this.DisplayCount = displayCount;
            this.isPaging = true;
        }
    }
}
