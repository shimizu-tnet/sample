using JuniorTennis.Domain.Tournaments;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class HoldingDateTests
    {
        [Theory]
        [InlineData("9/1", 2020, 9, 1)]
        [InlineData("10/1", 2020, 10, 1)]
        [InlineData("9/10", 2020, 9, 10)]
        [InlineData("10/10", 2020, 10, 10)]
        public void 開催日の画面表示用文字列を表示(string expected, int year, int month, int day)
        {
            var act = new HoldingDate(new DateTime(year, month, day));
            Assert.Equal(expected, act.DisplayValue);
        }

        [Theory]
        [InlineData("2020/09/09", 2020, 9, 9)]
        [InlineData("2020/09/10", 2020, 9, 10)]
        [InlineData("2020/10/09", 2020, 10, 9)]
        [InlineData("2020/10/10", 2020, 10, 10)]
        public void HTML要素のValueに設定する用の文字列が0埋めされた形式となっていること(string value, int year, int month, int day)
        {
            var act = new HoldingDate(new DateTime(year, month, day));

            Assert.Equal(value, act.ElementValue);
        }
    }
}
