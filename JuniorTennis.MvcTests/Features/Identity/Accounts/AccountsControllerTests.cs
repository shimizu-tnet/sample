using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.UseCases.Identity.Accounts;
using JuniorTennis.Mvc.Configurations;
using JuniorTennis.Mvc.Features.Identity.Accounts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace JuniorTennis.MvcTests.Features.Identity.Accounts
{
    public class AccountsControllerTests
    {
        [Fact]
        public async Task パスワード設定画面で認証コードなしの場合４０４ページを表示()
        {
            // Arrange
            var accountsUseCase = new Mock<IAccountsUseCase>();
            var authorizationUseCase = new Mock<IAuthorizationUseCase>();
            var userManager = MockMaker.MakeMoqUserManager();
            var signInManager = MockMaker.MakeMockSignInManager();
            var controller = new AccountsController(
                accountsUseCase: accountsUseCase.Object,
                authorizationUseCase: authorizationUseCase.Object,
                userManager: userManager.Object,
                signInManager: signInManager.Object,
                loggerFactory: new Mock<ILoggerFactory>().Object,
                optionsAccessor: new Mock<IOptions<UrlSettings>>().Object);

            // Act
            var result = await controller.SetupPassword(string.Empty, null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task パスワード設定画面で認証コードが一致しない場合４０４ページを表示()
        {
            // Arrange
            var accountsUseCase = new Mock<IAccountsUseCase>();
            var authorizationUseCase = new Mock<IAuthorizationUseCase>();
            accountsUseCase
                .Setup(o => o.GetAuthorizationLinkByCode("abcdef123456"))
                .ReturnsAsync((AuthorizationLink)null)
                .Verifiable();
            var userManager = MockMaker.MakeMoqUserManager();
            var signInManager = MockMaker.MakeMockSignInManager();
            var controller = new AccountsController(
                accountsUseCase: accountsUseCase.Object,
                authorizationUseCase: authorizationUseCase.Object,
                userManager: userManager.Object,
                signInManager: signInManager.Object,
                loggerFactory: new Mock<ILoggerFactory>().Object,
                optionsAccessor: new Mock<IOptions<UrlSettings>>().Object);

            // Act
            var result = await controller.SetupPassword("abcdef123456", null);

            // Assert
            accountsUseCase.Verify();
            accountsUseCase.VerifyNoOtherCalls();
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task 有効な認証コードを用いてパスワード設定画面を表示()
        {
            // Arrange
            var accountsUseCase = new Mock<IAccountsUseCase>();
            var authorizationUseCase = new Mock<IAuthorizationUseCase>();
            accountsUseCase
                .Setup(o => o.GetAuthorizationLinkByCode("abcdef123456"))
                .ReturnsAsync(new AuthorizationLink("C12345", DateTime.Now))
                .Verifiable();
            var userManager = MockMaker.MakeMoqUserManager();
            var signInManager = MockMaker.MakeMockSignInManager();
            var controller = new AccountsController(
                accountsUseCase: accountsUseCase.Object,
                authorizationUseCase: authorizationUseCase.Object,
                userManager: userManager.Object,
                signInManager: signInManager.Object,
                loggerFactory: new Mock<ILoggerFactory>().Object,
                optionsAccessor: new Mock<IOptions<UrlSettings>>().Object);

            // Act
            var result = await controller.SetupPassword("abcdef123456", null);

            // Assert
            accountsUseCase.Verify();
            accountsUseCase.VerifyNoOtherCalls();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task 認証コードとメール確認トークンをアクションパラメーターに持つ()
        {
            // Arrange
            var accountsUseCase = new Mock<IAccountsUseCase>();
            var authorizationUseCase = new Mock<IAuthorizationUseCase>();
            accountsUseCase
                .Setup(o => o.GetAuthorizationLinkByCode("abcdef123456"))
                .ReturnsAsync(new AuthorizationLink("C12345", DateTime.Now))
                .Verifiable();
            var userManager = MockMaker.MakeMoqUserManager();
            var signInManager = MockMaker.MakeMockSignInManager();
            var controller = new AccountsController(
                accountsUseCase: accountsUseCase.Object,
                authorizationUseCase: authorizationUseCase.Object,
                userManager: userManager.Object,
                signInManager: signInManager.Object,
                loggerFactory: new Mock<ILoggerFactory>().Object,
                optionsAccessor: new Mock<IOptions<UrlSettings>>().Object);

            // Act
            var result = await controller.SetupPassword("abcdef123456", "abcdefghijklmn");

            // Assert
            accountsUseCase.Verify();
            accountsUseCase.VerifyNoOtherCalls();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SetupPasswordViewModel>(viewResult.ViewData.Model);
            Assert.Equal("abcdef123456", model.ActionParameters["authorizationCode"]);
            Assert.Equal("abcdefghijklmn", model.ActionParameters["token"]);
        }
    }
}
