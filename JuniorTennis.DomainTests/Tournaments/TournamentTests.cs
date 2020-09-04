using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class TournamentTests
    {
        [Fact]
        public void 大会を正しく取得()
        {
            var act = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(new DateTime(2020, 6, 10), new DateTime(2020, 6, 20)),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                1
                );

            Assert.Equal("ジュニア選手権", act.TournamentName.Value);
            Assert.Equal(TournamentType.WithDraw, act.TournamentType);
            Assert.Equal(new DateTime(2020, 4, 1), act.RegistrationYear.Value);
            Assert.Equal(TypeOfYear.Odd, act.TypeOfYear);
            Assert.Equal(new DateTime(2020, 6, 1), act.AggregationMonth.Value);
            Assert.Equal(new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") }, act.TennisEvents);
            Assert.Equal(new DateTime(2020, 6, 10), act.HoldingPeriod.StartDate);
            Assert.Equal(new DateTime(2020, 6, 20), act.HoldingPeriod.EndDate);
            Assert.Equal(new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                act.HoldingDates);
            Assert.Equal("日本テニスコート", act.Venue.Value);
            Assert.Equal(100, act.EntryFee.Value);
            Assert.Equal(MethodOfPayment.PrePayment, act.MethodOfPayment);
            Assert.Equal(new DateTime(2020, 5, 1), act.ApplicationPeriod.StartDate);
            Assert.Equal(new DateTime(2020, 5, 31), act.ApplicationPeriod.EndDate);
            Assert.Equal("大会名：ジュニア選手　権場所：日本テニスコート", act.Outline.Value);
            Assert.Equal("メール件名", act.TournamentEntryReceptionMailSubject);
            Assert.Equal("メール本文", act.TournamentEntryReceptionMailBody);
        }

        [Fact]
        public void ポイントのみの大会を正しく取得()
        {
            var act = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.OnlyPoints,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                1
                );

            Assert.Equal("ジュニア選手権", act.TournamentName.Value);
            Assert.Equal(TournamentType.OnlyPoints, act.TournamentType);
            Assert.Equal(new DateTime(2020, 4, 1), act.RegistrationYear.Value);
            Assert.Equal(TypeOfYear.Odd, act.TypeOfYear);
            Assert.Equal(new DateTime(2020, 6, 1), act.AggregationMonth.Value);
            Assert.Equal(new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") }, act.TennisEvents);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        public void 支払い方法によって参加費の表示フラグが正常に切り替わること(int id, bool flag)
        {
            var act = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(new DateTime(2020, 6, 10), new DateTime(2020, 6, 20)),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                Enumeration.FromValue<MethodOfPayment>(id),
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール件名",
                "メール本文",
                1
                );

            Assert.Equal(flag, act.ShowEntryFee);
        }
    }
}
