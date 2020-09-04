using JuniorTennis.Domain.Announcements;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Announcements
{
    public class RegisteredDateDateTests
    {
        [Theory]
        [InlineData(2019, 4, 3, "2019/4/3")]
        [InlineData(2020, 9, 10, "2020/9/10")]
        [InlineData(2021, 10, 15, "2021/10/15")]
        public void 登録日をスラッシュで表示する(int year, int month, int day, string value)
        {
            var act = new RegisteredDate(new DateTime(year, month, day));

            Assert.Equal(value, act.DisplayValue);
        }
    }
}
