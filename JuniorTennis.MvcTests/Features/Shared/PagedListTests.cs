using JuniorTennis.Mvc.Features.Shared.Pagination;
using System.Linq;
using Xunit;

namespace JuniorTennis.MvcTests.Features.Shared
{
    public class PagedListTests
    {
        [Fact]
        public void 一覧の6ページ中1ページ目を表示()
        {
            var pageIndex = 0;
            var totalCount = 60;
            var displayCount = 10;
            var list = Enumerable.Range(0, displayCount).ToList();
            var act = new PagedList<int>(list, pageIndex, totalCount, displayCount);
            Assert.Equal(1, act.SelectedPageNumber);
            Assert.Equal(1, act.FirstPageNumber);
            Assert.Equal(5, act.LastPageNumber);
            Assert.False(act.HasPreviousPage);
            Assert.True(act.HasNextPage);
            Assert.Equal(totalCount, act.TotalCount);
            Assert.Equal(displayCount, act.DisplayCount);
            Assert.Equal(list, act);
            Assert.Equal(6, act.TotalPageCount);
        }

        [Fact]
        public void 一覧の6ページ中2ページ目を表示()
        {
            var pageIndex = 1;
            var totalCount = 60;
            var displayCount = 10;
            var list = Enumerable.Range(0, displayCount).ToList();
            var act = new PagedList<int>(list, pageIndex, totalCount, displayCount);
            Assert.Equal(2, act.SelectedPageNumber);
            Assert.Equal(1, act.FirstPageNumber);
            Assert.Equal(5, act.LastPageNumber);
            Assert.True(act.HasPreviousPage);
            Assert.True(act.HasNextPage);
        }

        [Fact]
        public void 一覧の6ページ中3ページ目を表示()
        {
            var pageIndex = 2;
            var totalCount = 60;
            var displayCount = 10;
            var list = Enumerable.Range(0, displayCount).ToList();
            var act = new PagedList<int>(list, pageIndex, totalCount, displayCount);
            Assert.Equal(3, act.SelectedPageNumber);
            Assert.Equal(1, act.FirstPageNumber);
            Assert.Equal(5, act.LastPageNumber);
            Assert.True(act.HasPreviousPage);
            Assert.True(act.HasNextPage);
        }

        [Fact]
        public void 一覧の6ページ中5ページ目を表示()
        {
            var pageIndex = 4;
            var totalCount = 60;
            var displayCount = 10;
            var list = Enumerable.Range(0, displayCount).ToList();
            var act = new PagedList<int>(list, pageIndex, totalCount, displayCount);
            Assert.Equal(5, act.SelectedPageNumber);
            Assert.Equal(6, act.LastPageNumber);
            Assert.True(act.HasPreviousPage);
            Assert.True(act.HasNextPage);
        }

        [Fact]
        public void 一覧の6ページ中6ページ目を表示()
        {
            var pageIndex = 5;
            var totalCount = 60;
            var displayCount = 10;
            var list = Enumerable.Range(0, displayCount).ToList();
            var act = new PagedList<int>(list, pageIndex, totalCount, displayCount);
            Assert.Equal(6, act.SelectedPageNumber);
            Assert.Equal(2, act.FirstPageNumber);
            Assert.Equal(6, act.LastPageNumber);
            Assert.True(act.HasPreviousPage);
            Assert.False(act.HasNextPage);
        }

        [Fact]
        public void 存在しないページ数の場合()
        {
            var pageIndex = 6;
            var totalCount = 60;
            var displayCount = 10;
            var list = Enumerable.Range(0, displayCount).ToList();
            var act = new PagedList<int>(list, pageIndex, totalCount, displayCount);
            Assert.Equal(1, act.SelectedPageNumber);
            Assert.Equal(1, act.FirstPageNumber);
            Assert.Equal(5, act.LastPageNumber);
            Assert.False(act.HasPreviousPage);
            Assert.True(act.HasNextPage);
        }

        [Fact]
        public void 現在のページ数が5以下の場合()
        {
            var pageIndex = 2;
            var totalCount = 30;
            var displayCount = 10;
            var list = Enumerable.Range(0, displayCount).ToList();
            var act = new PagedList<int>(list, pageIndex, totalCount, displayCount);
            Assert.Equal(3, act.SelectedPageNumber);
            Assert.Equal(1, act.FirstPageNumber);
            Assert.Equal(3, act.LastPageNumber);
            Assert.True(act.HasPreviousPage);
            Assert.False(act.HasNextPage);
        }
    }
}
