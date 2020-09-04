using JuniorTennis.Domain.UseCases.Shared;
using System;
using System.Collections.Generic;
using Xunit;

namespace JuniorTennis.DomainTests.UseCases.Shared
{
    public class PagableTests
    {
        [Fact]
        public void ページ番号がマイナスの場合例外()
        {
            var pageIndex = -1;
            var totalCount = 2;
            var displayCount = 30;
            var exception = Assert.Throws<ArgumentException>(
                () => new Pagable<string>(
                    new List<string>(){ "test1", "test2" },
                    pageIndex,
                    totalCount,
                    displayCount
                    ));

            Assert.Equal(
                "ページ番号が不正です。 (Parameter 'PageIndex')",
                exception.Message);
        }

        [Fact]
        public void 表示オブジェクトの総数がマイナスの場合例外()
        {
            var pageIndex = 1;
            var totalCount = -2;
            var displayCount = 30;
            var exception = Assert.Throws<ArgumentException>(
                () => new Pagable<string>(
                    new List<string>() { "test1", "test2" },
                    pageIndex,
                    totalCount,
                    displayCount
                    ));

            Assert.Equal(
                "表示オブジェクトの総数が不正です。 (Parameter 'TotalCount')",
                exception.Message);
        }

        [Fact]
        public void ページ当たりの表示件数がマイナスの場合例外()
        {
            var pageIndex = 1;
            var totalCount = 2;
            var displayCount = -30;
            var exception = Assert.Throws<ArgumentException>(
                () => new Pagable<string>(
                    new List<string>() { "test1", "test2" },
                    pageIndex,
                    totalCount,
                    displayCount
                    ));

            Assert.Equal(
                "ページ当たりの表示件数が不正です。 (Parameter 'DisplayCount')",
                exception.Message);
        }

    }
}
