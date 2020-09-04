using JuniorTennis.Domain.Tournaments;
using Xunit;

namespace JuniorTennis.DomainTests.Tournaments
{
    public class OutlineTests
    {
        [Fact]
        public void 大会要領を正しく取得()
        {
            var act = new Outline(
                "ジュニアカップ No.01\r\n2020年 06月 01日 ～ 2020年 06月 30日\r\n日本テニスコート");

            Assert.Equal("ジュニアカップ No.01\r\n2020年 06月 01日 ～ 2020年 06月 30日\r\n日本テニスコート", act.Value);
        }
    }
}
