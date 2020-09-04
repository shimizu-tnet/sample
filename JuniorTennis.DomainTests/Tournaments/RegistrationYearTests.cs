using JuniorTennis.Domain.Tournaments;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class RegistrationYearTests
    {
        [Fact]
        public void 登録年度を表示する()
        {
            var act = new RegistrationYear(new DateTime(2020, 9, 3));

            Assert.Equal("2020年度", act.DisplayValue);
        }

        [Theory]
        [InlineData("2020/04/01", 2020, 4, 1)]
        public void HTML要素のValueに設定する用の文字列が0埋めされた形式となっていること(string value, int year, int month, int day)
        {
            var act = new RegistrationYear(new DateTime(year, month, day));

            Assert.Equal(value, act.ElementValue);
        }

        [Theory]
        [InlineData(2019, 4, 3)]
        [InlineData(2020, 9, 10)]
        [InlineData(2021, 10, 15)]
        public void 登録年度には常に4月1日が設定設定されること(int year, int month, int day)
        {
            var act = new RegistrationYear(new DateTime(year, month, day));

            Assert.Equal(new DateTime(year, 4, 1), act.Value);
        }

        [Theory]
        [InlineData(2020, 4, 1)]
        [InlineData(2021, 7, 1)]
        public void 集計月度が登録年度内で有効な日付であること(int year, int month, int day)
        {
            var registrationYear = new RegistrationYear(new DateTime(2020, 4, 1));

            registrationYear.EnsureValidAggregationMonth(new DateTime(year, month, day));
        }

        [Theory]
        [InlineData(2020, 3, 1)]
        [InlineData(2021, 8, 1)]
        public void 集計月度が登録年度の範囲外の場合例外(int year, int month, int day)
        {
            var registrationYear = new RegistrationYear(new DateTime(2020, 4, 1));

            var exception = Assert.Throws<ArgumentException>(
                () => registrationYear.EnsureValidAggregationMonth(new DateTime(year, month, day)));
            Assert.Equal(
                "登録年度の対象範囲外の集計月度が指定されています。",
                exception.Message);
        }

        [Theory]
        [InlineData(2020, 4, 1)]
        [InlineData(2021, 7, 31)]
        public void 開催期間の開始日が登録年度内で有効な日付であること(int year, int month, int day)
        {
            var registrationYear = new RegistrationYear(new DateTime(2020, 4, 1));

            registrationYear.EnsureValidHoldingStartDate(new DateTime(year, month, day));
        }

        [Theory]
        [InlineData(2020, 3, 31)]
        [InlineData(2021, 8, 1)]
        public void 開催期間の開始日が登録年度の範囲外の場合例外(int year, int month, int day)
        {
            var registrationYear = new RegistrationYear(new DateTime(2020, 4, 1));

            var exception = Assert.Throws<ArgumentException>(
                () => registrationYear.EnsureValidHoldingStartDate(new DateTime(year, month, day)));
            Assert.Equal(
                "登録年度の対象範囲外の開催期間が指定されています。",
                exception.Message);
        }

        [Fact]
        public void 開催期間の開始日がNullの場合は例外にしない()
        {
            var registrationYear = new RegistrationYear(new DateTime(2020, 4, 1));

            registrationYear.EnsureValidHoldingStartDate(null);
        }
    }
}
