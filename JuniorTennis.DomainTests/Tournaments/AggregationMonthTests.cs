using JuniorTennis.Domain.Tournaments;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class AggregationMonthTests
    {
        [Fact]
        public void 集計月度を年月で表示する()
        {
            var act = new AggregationMonth(new DateTime(2020, 9, 25));

            Assert.Equal("2020年9月", act.DisplayValue);
        }

        [Fact]
        public void 集計月度の月が2桁の場合でも正しく表示()
        {
            var act = new AggregationMonth(new DateTime(2020, 10, 25));

            Assert.Equal("2020年10月", act.DisplayValue);
        }

        [Theory]
        [InlineData("2020/09/01", 2020, 9, 1)]
        [InlineData("2020/10/01", 2020, 10, 1)]
        public void HTML要素のValueに設定する用の文字列が0埋めされた形式となっていること(string value, int year, int month, int day)
        {
            var act = new AggregationMonth(new DateTime(year, month, day));

            Assert.Equal(value, act.ElementValue);
        }

        [Theory]
        [InlineData(2020, 9, 24)]
        [InlineData(2020, 10, 3)]
        public void Valueの値は常に1日を設定する(int year, int month, int day)
        {
            var act = new AggregationMonth(new DateTime(year, month, day));

            Assert.Equal(new DateTime(year, month, 1), act.Value);
        }
    }
}
