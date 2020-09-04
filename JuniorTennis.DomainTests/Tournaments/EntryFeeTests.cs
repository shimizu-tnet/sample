using JuniorTennis.Domain.Tournaments;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class EntryFeeTests
    {
        [Fact]
        public void 参加費が3桁の場合カンマ無しで表示()
        {
            var act = new EntryFee(100);

            Assert.Equal("100 円", act.DisplayValue);
        }

        [Fact]
        public void 参加費が4桁の場合カンマ有りで表示()
        {
            var act = new EntryFee(1000);

            Assert.Equal("1,000 円", act.DisplayValue);
        }
    }
}
