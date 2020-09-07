using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.UseCases.Identity.Accounts;
using JuniorTennis.Domain.UseCases.Operators;
using JuniorTennis.Infrastructure.Identity;
using JuniorTennis.Mvc.Features.Operators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace JuniorTennis.MvcTests.Features.Operators
{
    public class OperatorsControllerTests
    {
        [Fact]
        public void 管理ユーザー新規登録画面を表示()
        {
            // Arrange
            var mockOperatorUseCase = new Mock<IOperatorUseCase>();
            var mockaAuthorizationUseCase = new Mock<IAuthorizationUseCase>();
            var mockUserManage = this.GetMockUserManager();
            var controller = new OperatorsController(mockOperatorUseCase.Object, mockaAuthorizationUseCase.Object, mockUserManage.Object);

            // Act
            var result = controller.Register();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public async void 管理ユーザーを登録()
        {
            // Arrange
            var viewModel = new RegisterViewModel()
            {
                Name = "管理太郎",
                SelectedRoleName = "Administrator",
                EmailAddress = "test@example.com",
                LoginId = "testloginid"
            };

            var mockOperatorUseCase = new Mock<IOperatorUseCase>();
            var mockaAuthorizationUseCase = new Mock<IAuthorizationUseCase>();
            mockaAuthorizationUseCase
                .Setup(o => o.AddAuthorizationLink("testloginid"))
                .ReturnsAsync(new AuthorizationLink("C12345", DateTime.Now))
                .Verifiable();
            var mockUserManage = this.GetMockUserManager();
            var mockUrlHelper = MockMaker.MakeIUrlHelper();
            var controller = new OperatorsController(mockOperatorUseCase.Object, mockaAuthorizationUseCase.Object, mockUserManage.Object)
            {
                Url = mockUrlHelper.Object
            };

            // Act
            var result = await controller.Register(viewModel);

            // Assert
            mockOperatorUseCase.Verify(o => o.RegisterOperator("管理太郎", "test@example.com", "testloginid"), Times.Once);
            mockOperatorUseCase.Verify(o => o.SendOperatorInvitaionMail("test@example.com", It.Is<string>(p => p.Contains("authorizationCode"))), Times.Once);
            mockaAuthorizationUseCase.Verify();
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);
        }

        [Fact]
        public async void 管理ユーザー登録時値が不正な場合再表示()
        {
            var viewModel = new RegisterViewModel()
            {
                Name = "管理太郎",
                SelectedRoleName = "Administrator",
                EmailAddress = "test@example.com",
                LoginId = "testloginid"
            };

            var mockOperatorUseCase = new Mock<IOperatorUseCase>();
            var mockaAuthorizationUseCase = new Mock<IAuthorizationUseCase>();
            var mockUserManage = this.GetMockUserManager();
            var controller = new OperatorsController(mockOperatorUseCase.Object, mockaAuthorizationUseCase.Object, mockUserManage.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Register(viewModel);

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsType<RegisterViewModel>(viewResult.ViewData.Model);
            Assert.Equal(viewModel.Name, model.Name);
            Assert.Equal(viewModel.SelectedRoleName, model.SelectedRoleName);
            Assert.Equal(viewModel.EmailAddress, model.EmailAddress);
            Assert.Equal(viewModel.LoginId, model.LoginId);
        }

        [Fact]
        public async void 管理ユーザー更新時値が不正な場合再表示()
        {
            // Arrange
            var viewModel = new EditViewModel(
                1,
                "管理太郎",
                "test@example.com",
                "testloginid",
                "Administrator");

            var mockOperatorUseCase = new Mock<IOperatorUseCase>();
            var mockaAuthorizationUseCase = new Mock<IAuthorizationUseCase>();
            var mockUserManage = this.GetMockUserManager();
            var controller = new OperatorsController(mockOperatorUseCase.Object, mockaAuthorizationUseCase.Object, mockUserManage.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Edit(viewModel);

            // Arrange
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
            var model = Assert.IsType<EditViewModel>(viewResult.ViewData.Model);
            Assert.Equal(viewModel.Name, model.Name);
            Assert.Equal(viewModel.SelectedRoleName, model.SelectedRoleName);
            Assert.Equal(viewModel.EmailAddress, model.EmailAddress);
            Assert.Equal(viewModel.LoginId, model.LoginId);
        }

        private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
