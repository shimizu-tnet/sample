using Moq;
using Xunit;
using JuniorTennis.Mvc.Features.Identity.Accounts;
using System.Threading.Tasks;
using JuniorTennis.Domain.Accounts;
using System;
using JuniorTennis.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using JuniorTennis.Domain.UseCases.Identity.Accounts;

namespace JuniorTennis.MvcTests.Features.Identity.Accounts
{
    public class AccountServiceTests
    {
        [Fact]
        public async Task IDとパスワードを使ってログイン認証を行う()
        {
            // Arrange
            var signInManager = MockMaker.MakeMockSignInManager();
            var service = new AccountService(
                signInManager.Object,
                new Mock<IAccountsUseCase>().Object,
                new Mock<IAuthorizationUseCase>().Object);

            // Act
            var result = await service.Login("loginid", "password");

            // Assert
            signInManager.Verify(o => o.PasswordSignInAsync("loginid", "password", false, false), Times.Once);
        }

        [Fact]
        public async Task パスワードを新しく登録する()
        {
            // Arrange
            var authorizationLink = new AuthorizationLink("C12345", new DateTime(2020, 9, 1));
            var userManager = MockMaker.MakeMoqUserManager();
            userManager
                .Setup(o => o.FindByNameAsync("C12345"))
                .ReturnsAsync(new ApplicationUser())
                .Verifiable();
            userManager
                .Setup(o => o.ConfirmEmailAsync(It.IsAny<ApplicationUser>(), "hijklmn"))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();
            userManager
                .Setup(o => o.RemovePasswordAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();
            userManager
                .Setup(o => o.AddPasswordAsync(It.IsAny<ApplicationUser>(), "abcdefg"))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();
            var signInManager = MockMaker.MakeMockSignInManager(userManager.Object);
            var service = new AccountService(
                signInManager.Object,
                new Mock<IAccountsUseCase>().Object,
                new Mock<IAuthorizationUseCase>().Object);

            // Act
            await service.SetupPassword("C12345", "abcdefg", "hijklmn");

            // Assert
            userManager.Verify();
        }

        [Fact]
        public async Task メールアドレスに紐づくユーザーが存在する場合true()
        {
            var userManager = MockMaker.MakeMoqUserManager();
            userManager
                .Setup(o => o.FindByEmailAsync("test@example.com"))
                .ReturnsAsync(new ApplicationUser())
                .Verifiable();
            var signInManager = MockMaker.MakeMockSignInManager(userManager.Object);
            var service = new AccountService(
                signInManager.Object,
                new Mock<IAccountsUseCase>().Object,
                new Mock<IAuthorizationUseCase>().Object);

            // Act
            var act = await service.ExistApplicationUser("test@example.com");

            // Assert
            userManager.Verify();
            Assert.True(act);
        }

        [Fact]
        public async Task メールアドレスに紐づくユーザーが存在しない場合false()
        {
            var userManager = MockMaker.MakeMoqUserManager();
            userManager
                .Setup(o => o.FindByEmailAsync("test@example.com"))
                .ReturnsAsync((ApplicationUser)null)
                .Verifiable();
            var signInManager = MockMaker.MakeMockSignInManager(userManager.Object);
            var service = new AccountService(
                signInManager.Object,
                new Mock<IAccountsUseCase>().Object,
                new Mock<IAuthorizationUseCase>().Object);

            // Act
            var act = await service.ExistApplicationUser("test@example.com");

            // Assert
            userManager.Verify();
            Assert.False(act);
        }
    }
}
