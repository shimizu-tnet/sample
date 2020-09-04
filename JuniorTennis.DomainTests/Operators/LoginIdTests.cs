using JuniorTennis.Domain.Operators;
using System;
using Xunit;

namespace JuniorTennis.DomainTests.Operators
{
    public class LoginIdTests
    {
        [Fact]
        public void ログインIdが255文字の場合正しく取得()
        {
            var loginId = "TestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextextTestTextTestTextTestTextTestTextTest";
            var act = new LoginId(loginId);

            Assert.Equal(loginId, act.Value);
        }

        [Fact]
        public void ログインIdが6文字の場合正しく取得()
        {
            var loginId = "TestTe";
            var act = new LoginId(loginId);

            Assert.Equal(loginId, act.Value);
        }

        [Fact]
        public void ログインIdがNullの場合例外()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new LoginId(null));
            Assert.Equal("ログインId", exception.ParamName);
        }

        [Fact]
        public void ログインIdが256文字以上の場合例外()
        {
            var loginId = "TestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextTestTextextTestTextTestTextTestTextTestTextTestt";
            var exception = Assert.Throws<ArgumentException>(
                () => new LoginId(loginId));
            Assert.Equal("255 文字を超えています。 (Parameter 'ログインId')", exception.Message);
        }

        [Fact]
        public void ログインIdが5文字以下の場合例外()
        {
            var loginId = "TestT";
            var exception = Assert.Throws<ArgumentException>(
                () => new LoginId(loginId));
            Assert.Equal("6 文字を下回っています。 (Parameter 'ログインId')", exception.Message);
        }

        [Fact]
        public void ログインIdの文字種が半角英数字以外の場合例外()
        {
            var loginId = "テストテストテスト";
            var exception = Assert.Throws<ArgumentException>(
                () => new LoginId(loginId));
            Assert.Equal("半角英数字以外の文字が使われています。 (Parameter 'ログインId')", exception.Message);
        }
    }
}
