using JuniorTennis.Domain.Tournaments;
using System.Linq;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class TennisEventTests
    {
        [Fact]
        public void 種目Idから種目を設定する()
        {
            var tennisEvent = TennisEvent.FromId("1_1_1");

            Assert.Equal(Category.Under17Or18, tennisEvent.Category);
            Assert.Equal(Gender.Boys, tennisEvent.Gender);
            Assert.Equal(Format.Singles, tennisEvent.Format);
        }

        [Fact]
        public void 種目IDを表示()
        {
            var act = new TennisEvent(Category.Under17Or18, Gender.Boys, Format.Singles);

            Assert.Equal("1_1_1", act.TennisEventId);
        }

        [Fact]
        public void 大会種目に表示する種目を表示()
        {
            var act = new TennisEvent(Category.Under17Or18, Gender.Boys, Format.Singles);

            Assert.Equal("17/18歳以下 男子 シングルス", act.DisplayTournamentEvent);
        }

        [Fact]
        public void ランキングのカテゴリーで表示する種目を表示()
        {
            var act = new TennisEvent(Category.Under17Or18, Gender.Boys, Format.Singles);
            
            Assert.Equal("男子 シングルス 17/18歳以下", act.DisplayRankingEvent);
        }

        [Fact]
        public void 全ての種目一覧を取得()
        {
            var act = TennisEvent.GetAllEvents();

            Assert.Equal(new TennisEvent(Category.Under17Or18, Gender.Boys, Format.Singles),
                act.Values.First());
            Assert.Equal(new TennisEvent(Category.Under17Or18, Gender.Boys, Format.Doubles),
                act.Values.Skip(1).First());
            Assert.Equal(new TennisEvent(Category.Under17Or18, Gender.Girls, Format.Singles),
                act.Values.Skip(2).First());
            Assert.Equal(new TennisEvent(Category.Under17Or18, Gender.Girls, Format.Doubles),
                act.Values.Skip(3).First());
            Assert.Equal(new TennisEvent(Category.Under15Or16, Gender.Boys, Format.Singles),
                act.Values.Skip(4).First());
            Assert.Equal(new TennisEvent(Category.Under15Or16, Gender.Boys, Format.Doubles),
                act.Values.Skip(5).First());
            Assert.Equal(new TennisEvent(Category.Under15Or16, Gender.Girls, Format.Singles),
                act.Values.Skip(6).First());
            Assert.Equal(new TennisEvent(Category.Under15Or16, Gender.Girls, Format.Doubles),
                act.Values.Skip(7).First());
            Assert.Equal(new TennisEvent(Category.Under13Or14, Gender.Boys, Format.Singles),
                act.Values.Skip(8).First());
            Assert.Equal(new TennisEvent(Category.Under13Or14, Gender.Boys, Format.Doubles),
                act.Values.Skip(9).First());
            Assert.Equal(new TennisEvent(Category.Under13Or14, Gender.Girls, Format.Singles),
                act.Values.Skip(10).First());
            Assert.Equal(new TennisEvent(Category.Under13Or14, Gender.Girls, Format.Doubles),
                act.Values.Skip(11).First());
            Assert.Equal(new TennisEvent(Category.Under11Or12, Gender.Boys, Format.Singles),
                act.Values.Skip(12).First());
            Assert.Equal(new TennisEvent(Category.Under11Or12, Gender.Boys, Format.Doubles),
                act.Values.Skip(13).First());
            Assert.Equal(new TennisEvent(Category.Under11Or12, Gender.Girls, Format.Singles),
                act.Values.Skip(14).First());
            Assert.Equal(new TennisEvent(Category.Under11Or12, Gender.Girls, Format.Doubles),
                act.Values.Skip(15).First());
        }
    }
}
