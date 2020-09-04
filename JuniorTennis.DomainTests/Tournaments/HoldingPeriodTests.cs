using JuniorTennis.Domain.Tournaments;
using System;
using System.Collections.Generic;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class HoldingPeriodTests
    {
        [Fact]
        public void 開始日に終了日を超える日付は設定不可()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new HoldingPeriod(
                    new DateTime(2020, 8, 2),
                    new DateTime(2020, 8, 1)));
            Assert.Equal(
                "開催期間の開始日に、終了日を超える日付が指定されています。",
                exception.Message);
        }

        [Fact]
        public void 大会期間を年月日で表示する()
        {
            var act = new HoldingPeriod(
                new DateTime(2020, 9, 25),
                new DateTime(2020, 10, 2));

            Assert.Equal("2020年9月25日 ～ 2020年10月2日", act.DisplayValue);
        }

        [Fact]
        public void 大会期間をスラッシュで表示する()
        {
            var act = new HoldingPeriod(
                new DateTime(2020, 9, 25),
                new DateTime(2020, 10, 2));

            Assert.Equal("2020/9/25 ～ 2020/10/2", act.ShortDisplayValue);
        }

        [Fact]
        public void 開催日が開催期間の範囲に収まっていること()
        {
            var holdingPeriod = new HoldingPeriod(
                new DateTime(2020, 9, 25),
                new DateTime(2020, 10, 2));

            var holdingDates = new List<HoldingDate>() {
                new HoldingDate(new DateTime(2020, 9, 25)),
                new HoldingDate(new DateTime(2020, 9, 26)),
                new HoldingDate(new DateTime(2020, 10, 1)),
                new HoldingDate(new DateTime(2020, 10, 2))
            };

            holdingPeriod.EnsureValidHoldingDates(holdingDates);
        }

        [Theory]
        [InlineData(2020, 9, 24)]
        [InlineData(2020, 10, 3)]
        public void 開催日が開催期間の範囲に収まっていない場合例外(int year, int month, int day)
        {
            var holdingPeriod = new HoldingPeriod(
                new DateTime(2020, 9, 25),
                new DateTime(2020, 10, 2));

            var holdingDates = new List<HoldingDate>() {
                new HoldingDate(new DateTime(year, month, day))
            };

            var exception = Assert.Throws<ArgumentException>(
                () => holdingPeriod.EnsureValidHoldingDates(holdingDates));
            Assert.Equal(
                "開催期間の範囲外の開催日が指定されています。",
                exception.Message);
        }

        [Fact]
        public void 申込終了日が開催期間と重なっている場合例外()
        {
            var holdingPeriod = new HoldingPeriod(
                new DateTime(2020, 9, 25),
                new DateTime(2020, 10, 2));
            holdingPeriod.EnsureValidApplicationEndDate(null);
            holdingPeriod.EnsureValidApplicationEndDate(new DateTime(2020, 9, 24));
            var exception = Assert.Throws<ArgumentException>(
                () => holdingPeriod.EnsureValidApplicationEndDate(new DateTime(2020, 9, 25)));
            Assert.Equal(
                "申込期間が開催期間と重複しています。",
                exception.Message);
        }

        [Theory]
        [InlineData(2020, 4, 10, 2020, 10, 6)]
        public void JSONのシリアライズとデシリアライズが正常に実行されること(
            int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay)
        {
            var startDate = new DateTime(startYear, startMonth, startDay);
            var endDate = new DateTime(endYear, endMonth, endDay);
            var oldHoldingPeriod = new HoldingPeriod(startDate, endDate);
            var json = oldHoldingPeriod.ToJson();
            var newHoldingPeriod = HoldingPeriod.FromJson(json);

            Assert.Equal(startDate, newHoldingPeriod.StartDate);
            Assert.Equal(endDate, newHoldingPeriod.EndDate);
        }
    }
}
