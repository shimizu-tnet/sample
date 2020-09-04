using JuniorTennis.Domain.Tournaments;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class ApplicationPeriodTests
    {
        [Fact]
        public void 大会期間を年月日で表示する()
        {
            var act = new ApplicationPeriod(
                new DateTime(2020, 9, 25),
                new DateTime(2020, 10, 2));

            Assert.Equal("2020年9月25日 ～ 2020年10月2日", act.DisplayValue);
        }

        [Fact]
        public void 大会期間をスラッシュで表示する()
        {
            var act = new ApplicationPeriod(
                new DateTime(2020, 9, 25),
                new DateTime(2020, 10, 2));

            Assert.Equal("2020/9/25 ～ 2020/10/2", act.ShortDisplayValue);
        }

        [Fact]
        public void 開始日に終了日を超える日付は設定不可()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new ApplicationPeriod(
                    new DateTime(2020, 8, 2),
                    new DateTime(2020, 8, 1)));

            Assert.Equal(
                "申込期間の開始日に、終了日を超える日付が指定されています。",
                exception.Message);
        }

        [Theory]
        [InlineData(2020, 4, 10, 2020, 10, 6)]
        public void JSONのシリアライズとデシリアライズが正常に実行されること(
            int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay)
        {
            var startDate = new DateTime(startYear, startMonth, startDay);
            var endDate = new DateTime(endYear, endMonth, endDay);
            var oldApplicationPeriod = new ApplicationPeriod(startDate, endDate);
            var json = oldApplicationPeriod.ToJson();
            var newApplicationPeriod = ApplicationPeriod.FromJson(json);

            Assert.Equal(startDate, newApplicationPeriod.StartDate);
            Assert.Equal(endDate, newApplicationPeriod.EndDate);
        }
    }
}
