using JuniorTennis.Domain.Accounts;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Accounts
{
    public class EmailAddressTests
    {
        [Fact]
        public void メールアドレスを正しく取得()
        {
            var act = new EmailAddress("test@example.com");

            Assert.Equal("test@example.com", act.Value);
        }

        [Fact]
        public void メールアドレスがNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new EmailAddress(null));
            Assert.Equal("メールアドレス", exception.ParamName);
        }

        [Theory()]
        [InlineData("test")]
        [InlineData("test@")]
        [InlineData("test@example")]
        public void メールアドレス形式ではない場合例外(string mailAddress)
        {
            var exception = Assert.Throws<ArgumentException>(
                () => new EmailAddress(mailAddress));
            Assert.Equal("メールアドレス形式ではありません。 (Parameter 'メールアドレス')", exception.Message);
        }
    }
}
