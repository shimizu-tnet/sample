using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.Operators;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Operators
{
    public class OperatorTests
    {
        [Fact]
        public void 名前がNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Operator(
                    null,
                    new EmailAddress("test@example.com"),
                    new LoginId("testloginid")
                    ));

            Assert.Equal("名前", exception.ParamName);
        }

        [Fact]
        public void メールアドレスがNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Operator(
                    "管理太郎",
                    null,
                    new LoginId("testloginid")
                    ));

            Assert.Equal("メールアドレス", exception.ParamName);
        }

        [Fact]
        public void ログインIdがNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new Operator(
                    "管理太郎",
                    new EmailAddress("test@example.com"),
                    null
                    ));

            Assert.Equal("ログインId", exception.ParamName);
        }
    }
}
