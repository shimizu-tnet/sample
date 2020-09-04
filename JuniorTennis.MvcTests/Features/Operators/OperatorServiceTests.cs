using JetBrains.Annotations;
using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.Operators;
using JuniorTennis.Domain.UseCases.Operators;
using JuniorTennis.Infrastructure.Identity;
using JuniorTennis.Mvc.Features.Operators;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace JuniorTennis.MvcTests.Features.Operators
{
    public class OperatorServiceTests
    {
        [Fact]
        public async void 管理ユーザー認証ユーザーの新規登録招待メールの送信を行う()
        {
            // Arrange
            var roleName = "Administrator";
            var name = "管理太郎";
            var emailAddress = "test@example.com";
            var loginId = "loginID";
            var mockUseCase = new Mock<IOperatorUseCase>();
            var mockUserManager = this.GetMockUserManager();
            var service = new OperatorService(mockUseCase.Object, mockUserManager.Object);

            // Act
            await service.RegisterOperator(roleName, name, emailAddress, loginId);

            // Assert
            mockUseCase.Verify(m => m.RegisterOperator(name, emailAddress, loginId), Times.Once());
            mockUseCase.Verify(m => m.SendOperatorInvitaionMail(emailAddress), Times.Once());
            mockUserManager.Verify(m => m.CreateAsync(It.IsAny<ApplicationUser>()), Times.Once());
            mockUserManager.Verify(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), roleName), Times.Once());
        }

        [Fact]
        public async void 管理ユーザー編集ビューモデルを生成する()
        {
            // Arrange
            var id = 100000;
            var name = "管理太郎";
            var emailAddress = "test@example.com";
            var loginId = "AdministratorId";
            var updateOperator = new Operator(
                name,
                new EmailAddress(emailAddress),
                new LoginId(loginId)
                )
                { Id = id };
            var appUser = new ApplicationUser() { UserName = loginId };
            var appRoles = new List<string>() { "Administrator"};
            var mockUseCase = new Mock<IOperatorUseCase>();
            mockUseCase.Setup(m => m.GetOperator(id))
                .ReturnsAsync(updateOperator)
                .Verifiable();
            var mockUserManager = this.GetMockUserManager();
            mockUserManager.Setup(o => o.FindByNameAsync(loginId))
                .ReturnsAsync(appUser)
                .Verifiable();
            mockUserManager.Setup(o => o.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(appRoles)
                .Verifiable();
            var service = new OperatorService(mockUseCase.Object, mockUserManager.Object);

            // Act
            var result =  await service.CreateEditViewModel(id);

            // Assert
            mockUseCase.Verify();
            mockUserManager.Verify();
            Assert.Equal(id, result.OperatorId);
            Assert.Equal(name, result.Name);
            Assert.Equal(emailAddress, result.EmailAddress);
            Assert.Equal(loginId, result.LoginId);
        }

        [Fact]
        public async void 管理ユーザーと認証ユーザーを更新する()
        {
            // Arrange
            var id = 100000;
            var name = "管理太郎";
            var emailAddress = "test@example.com";
            var loginId = "AdministratorId";
            var roleName = "Administrator";
            var updateOperator = new Operator(
                name,
                new EmailAddress("testtest@example.com"),
                new LoginId(loginId)
                )
                { Id = id };

            var appUser = new ApplicationUser() { UserName = loginId, Email = "testtest@example.com" };
            var appRoles = new List<string>() { "TournamentCreator" };
            var mockUseCase = new Mock<IOperatorUseCase>();
            mockUseCase.Setup(m => m.UpdateOperator(id, name, emailAddress))
                .ReturnsAsync(updateOperator)
                .Verifiable();
            var mockUserManager = this.GetMockUserManager();
            mockUserManager.Setup(o => o.FindByNameAsync(loginId))
                .ReturnsAsync(appUser)
                .Verifiable();
            mockUserManager.Setup(o => o.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(appRoles)
                .Verifiable();
            var service = new OperatorService(mockUseCase.Object, mockUserManager.Object);

            // Act
            await service.UpdateOperator(id, roleName, name, emailAddress);

            // Assert
            mockUserManager.Verify(m => m.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once());
            mockUserManager.Verify(o => o.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(o => o.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void メールアドレスが変更なしの場合認証ユーザーのメールアドレスを更新しない()
        {
            // Arrange
            var id = 100000;
            var name = "管理太郎";
            var emailAddress = "test@example.com";
            var loginId = "AdministratorId";
            var roleName = "Administrator";
            var updateOperator = new Operator(
                name,
                new EmailAddress("test@example.com"),
                new LoginId(loginId)
                )
            { Id = id };

            var appUser = new ApplicationUser() { UserName = loginId, Email = "test@example.com" };
            var appRoles = new List<string>() { "TournamentCreator" };
            var mockUseCase = new Mock<IOperatorUseCase>();
            mockUseCase.Setup(m => m.UpdateOperator(id, name, emailAddress))
                .ReturnsAsync(updateOperator)
                .Verifiable();
            var mockUserManager = this.GetMockUserManager();
            mockUserManager.Setup(o => o.FindByNameAsync(loginId))
                .ReturnsAsync(appUser)
                .Verifiable();
            mockUserManager.Setup(o => o.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(appRoles)
                .Verifiable();
            var service = new OperatorService(mockUseCase.Object, mockUserManager.Object);

            // Act
            await service.UpdateOperator(id, roleName, name, emailAddress);

            // Assert
            mockUserManager.Verify(m => m.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            mockUserManager.Verify(o => o.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(o => o.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void 権限名更新無しの場合認証ユーザーの権限名を更新しない()
        {
            // Arrange
            var id = 100000;
            var name = "管理太郎";
            var emailAddress = "test@example.com";
            var loginId = "AdministratorId";
            var roleName = "Administrator";
            var updateOperator = new Operator(
                name,
                new EmailAddress("testtest@example.com"),
                new LoginId(loginId)
                )
            { Id = id };

            var appUser = new ApplicationUser() { UserName = loginId, Email = "testtest@example.com" };
            var appRoles = new List<string>() { "Administrator" };
            var mockUseCase = new Mock<IOperatorUseCase>();
            mockUseCase.Setup(m => m.UpdateOperator(id, name, emailAddress))
                .ReturnsAsync(updateOperator)
                .Verifiable();
            var mockUserManager = this.GetMockUserManager();
            mockUserManager.Setup(o => o.FindByNameAsync(loginId))
                .ReturnsAsync(appUser)
                .Verifiable();
            mockUserManager.Setup(o => o.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(appRoles)
                .Verifiable();
            var service = new OperatorService(mockUseCase.Object, mockUserManager.Object);

            // Act
            await service.UpdateOperator(id, roleName, name, emailAddress);

            // Assert
            mockUserManager.Verify(m => m.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
            mockUserManager.Verify(o => o.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
            mockUserManager.Verify(o => o.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async void 管理ユーザーの一覧ビューモデルを生成する()
        {
            // Arrange
            var operators = new List<Operator>
            {
                new Operator(
                    "管理太郎",
                    new EmailAddress("test@test.com"),
                    new LoginId("AdministratorId")
                    )
                    { Id = 1 },
                new Operator(
                    "大会太郎",
                    new EmailAddress("test@test.com"),
                    new LoginId("TournamentCreatorId")
                    )
                    { Id = 2 },
                new Operator(
                    "記録太郎",
                    new EmailAddress("test@test.com"),
                    new LoginId("RecorderId")
                    )
                    { Id = 3 },
            };

            var administratorRoleUser = new List<ApplicationUser>() {
                new ApplicationUser() { UserName = "AdministratorId", Id = "" }};
            var tournamentCreatorRoleUser = new List<ApplicationUser>() {
                new ApplicationUser() { UserName = "TournamentCreatorId", Id = "" }};
            var recorderRoleUser = new List<ApplicationUser>() {
                new ApplicationUser() { UserName = "RecorderId", Id = "" }};

            var mockUseCase = new Mock<IOperatorUseCase>();
            var mockUserManage = this.GetMockUserManager();
            mockUseCase.Setup(m => m.GetOperators())
                .ReturnsAsync(operators)
                .Verifiable();
            mockUserManage.Setup(m => m.GetUsersInRoleAsync("Administrator"))
                .ReturnsAsync(administratorRoleUser)
                .Verifiable();
            mockUserManage.Setup(m => m.GetUsersInRoleAsync("TournamentCreator"))
                .ReturnsAsync(tournamentCreatorRoleUser)
                .Verifiable();
            mockUserManage.Setup(m => m.GetUsersInRoleAsync("Recorder"))
                .ReturnsAsync(recorderRoleUser)
                .Verifiable();
            var service = new OperatorService(mockUseCase.Object, mockUserManage.Object);

            // Act
            var result = await service.CreateIndexViewModel();

            // Assert
            mockUseCase.Verify();
            mockUserManage.Verify();
            Assert.IsType<IndexViewModel>(result);
            Assert.Equal(3, result.Operators.Count);
        }

        private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
    }
}
