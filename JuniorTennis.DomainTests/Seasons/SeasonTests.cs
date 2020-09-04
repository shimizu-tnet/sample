using JuniorTennis.Domain.Seasons;
using JuniorTennis.SeedWork;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Seasons
{
    public class SeasonTests
    {
        [Fact]
        public void 登録開始日に登録終了日より後の日付は設定不可()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new Season(
                    "2020年度",
                    new DateTime(2020, 4, 1),
                    new DateTime(2020, 3, 31),
                    new DateTime(2020, 3, 31),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    ));

            Assert.Equal("年度開始日は年度終了日より前の日付を設定してください。 (Parameter '年度開始日')", exception.Message);
        }

        [Fact]
        public void 登録開始日に登録終了日より後の日付は設定不可_時刻設定あり()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new Season(
                    "2020年度",
                    new DateTime(2020, 4, 1, 1, 2, 3),
                    new DateTime(2020, 3, 31, 1, 2, 3),
                    new DateTime(2020, 3, 31, 1, 2, 3),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    ));

            Assert.Equal("年度開始日は年度終了日より前の日付を設定してください。 (Parameter '年度開始日')", exception.Message);
        }

        [Fact]
        public void 年度登録受付開始日に登録開始日より後の日付は設定不可()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new Season(
                    "2020年度",
                    new DateTime(2020, 4, 1),
                    new DateTime(2021, 3, 31),
                    new DateTime(2020, 4, 2),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    ));

            Assert.Equal("年度登録受付開始日は年度開始日より前の日付を設定してください。 (Parameter '年度登録受付開始日')", exception.Message);
        }

        [Fact]
        public void 年度登録受付開始日に登録開始日より後の日付は設定不可_時刻設定あり()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new Season(
                    "2020年度",
                    new DateTime(2020, 4, 1, 1, 2, 3),
                    new DateTime(2021, 3, 31, 1, 2, 3),
                    new DateTime(2020, 4, 2, 1, 2, 3),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    ));

            Assert.Equal("年度登録受付開始日は年度開始日より前の日付を設定してください。 (Parameter '年度登録受付開始日')", exception.Message);
        }

        [Fact]
        public void 正しい年度を設定()
        {
            var act = new Season(
                    "2020年度",
                    new DateTime(2020, 4, 1),
                    new DateTime(2021, 3, 31),
                    new DateTime(2020, 2, 1),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    );

            Assert.Equal("2020年度", act.Name);
            Assert.Equal(new DateTime(2020, 4, 1), act.FromDate);
            Assert.Equal(new DateTime(2021, 3, 31), act.ToDate);
            Assert.Equal(new DateTime(2020, 2, 1), act.RegistrationFromDate);
            Assert.Equal(5000, act.TeamRegistrationFee.Value);
            Assert.Equal(500, act.PlayerRegistrationFee.Value);
            Assert.Equal(200, act.PlayerTradeFee.Value);
            Assert.Equal("2020/4/1 ～ 2021/3/31", act.DisplaySeasonPeriod);
        }

        [Fact]
        public void 時刻まで渡した場合でも時刻は無視して設定()
        {
            var act = new Season(
                    "2020年度",
                    new DateTime(2020, 4, 1, 1, 2, 3),
                    new DateTime(2021, 3, 31, 1, 2, 3),
                    new DateTime(2020, 2, 1, 1, 2, 3),
                    new TeamRegistrationFee(5000),
                    new PlayerRegistrationFee(500),
                    new PlayerTradeFee(200)
                    );

            Assert.Equal("2020年度", act.Name);
            Assert.Equal(new DateTime(2020, 4, 1), act.FromDate);
            Assert.Equal(new DateTime(2021, 3, 31), act.ToDate);
            Assert.Equal(new DateTime(2020, 2, 1), act.RegistrationFromDate);
            Assert.Equal(5000, act.TeamRegistrationFee.Value);
            Assert.Equal(500, act.PlayerRegistrationFee.Value);
            Assert.Equal(200, act.PlayerTradeFee.Value);
        }

        [Fact]
        public void お知らせを更新する()
        {
            var act = new Season(
                "2020年度",
                new DateTime(2020, 4, 1),
                new DateTime(2021, 3, 31),
                new DateTime(2020, 2, 1),
                new TeamRegistrationFee(5000),
                new PlayerRegistrationFee(500),
                new PlayerTradeFee(200)
                );

            act.Change(
                new DateTime(2020, 9, 1),
                new DateTime(2021, 8, 31),
                new DateTime(2020, 7, 1),
                new TeamRegistrationFee(6000),
                new PlayerRegistrationFee(600),
                new PlayerTradeFee(300)
                );

            Assert.Equal(new DateTime(2020, 9, 1), act.FromDate);
            Assert.Equal(new DateTime(2021, 8, 31), act.ToDate);
            Assert.Equal(new DateTime(2020, 7, 1), act.RegistrationFromDate);
            Assert.Equal(new TeamRegistrationFee(6000), act.TeamRegistrationFee);
            Assert.Equal(new PlayerRegistrationFee(600), act.PlayerRegistrationFee);
            Assert.Equal(new PlayerTradeFee(300), act.PlayerTradeFee);
        }
    }
}
