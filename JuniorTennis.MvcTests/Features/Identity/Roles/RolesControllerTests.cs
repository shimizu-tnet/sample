using JuniorTennis.Infrastructure.Identity;
using JuniorTennis.Mvc.Features.Identity.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace JuniorTennis.MvcTests.Features.Identity.Roles
{
    public class RolesControllerTests
    {
        [Fact]
        public void 未登録Roleがある場合メッセージを表示()
        {
            // Arrange
            var mockUserManager = this.GetMockUserManager();
            var mockRoleManager = this.GetMockRoleManager();
            mockRoleManager.Setup(r => r.Roles)
                .Returns(new List<ApplicationRole>()
                {
                    new ApplicationRole() { Name = "Administrator" },
                }
                .AsQueryable());
            var controller = new RolesController(mockUserManager.Object, mockRoleManager.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            Assert.True(model.HasUnregistered);
        }

        [Fact]
        public void 未削除Roleがある場合メッセージを表示()
        {
            // Arrange
            var mockUserManager = this.GetMockUserManager();
            var mockRoleManager = this.GetMockRoleManager();
            mockRoleManager.Setup(r => r.Roles)
                .Returns(new List<ApplicationRole>()
                {
                    new ApplicationRole() { Name = "Administrator" },
                    new ApplicationRole() { Name = "TournamentCreator" },
                    new ApplicationRole() { Name = "Customer" }
                }
                .AsQueryable());
            IList<ApplicationUser> roles = new List<ApplicationUser>() { new ApplicationUser() };
            var appRole = new ApplicationRole() { Name = "Customer", Id = "" };
            mockRoleManager.Setup(r => r.FindByNameAsync("Customer"))
                .Returns(Task.FromResult(appRole));
            mockUserManager.Setup(u => u.GetUsersInRoleAsync("Customer"))
                .Returns(Task.FromResult(roles));
            var controller = new RolesController(mockUserManager.Object, mockRoleManager.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IndexViewModel>(viewResult.ViewData.Model);
            Assert.True(model.HasUndeleted);
        }

        [Fact]
        public async Task 新規登録Roleがある時Roleを登録しIndexを表示()
        {
            // Arrange
            var addRoles = new List<string> { "Developer", "Administrator" };
            var mockUserManager = this.GetMockUserManager();
            var mockRoleManager = this.GetMockRoleManager();
            var controller = new RolesController(mockUserManager.Object, mockRoleManager.Object);

            // Act
            var result = await controller.CreateRoles(addRoles);

            // Assert
            mockRoleManager.Verify(r => r.CreateAsync(It.IsAny<ApplicationRole>()), Times.Exactly(2));
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task 未登録Roleがある時Roleを削除しIndexを表示()
        {
            // Arrange
            var deleteRoleNames = new List<string>() { "customer", "browser" };
            var mockUserManager = this.GetMockUserManager();
            var mockRoleManager = this.GetMockRoleManager();
            mockRoleManager.Setup(r => r.FindByNameAsync("customer"))
                .Returns(Task.FromResult(new ApplicationRole() { Name = "customer" }));
            mockRoleManager.Setup(r => r.FindByNameAsync("browser"))
                .Returns(Task.FromResult(new ApplicationRole() { Name = "browser" }));
            var controller = new RolesController(mockUserManager.Object, mockRoleManager.Object);

            // Act
            var result = await controller.DeleteRoles(deleteRoleNames);

            // Assert
            mockRoleManager.Verify(r => r.DeleteAsync(It.IsAny<ApplicationRole>()), Times.Exactly(2));
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.Index), redirectToActionResult.ActionName);

        }

        [Fact]
        public async Task RoleCliamsが存在する時RoleClaimsを一覧表示()
        {
            var roleName = "Developer";
            var role = new ApplicationRole() { Name = "Developer" };
            var receiptClaim = new Claim("Receipt","");
            var createTournamentClaim = new Claim("CreateTournament", "");
            IList<Claim> claims = new List<Claim>() { receiptClaim, createTournamentClaim };
            var mockUserManager = this.GetMockUserManager();
            var mockRoleManager = this.GetMockRoleManager();
            mockRoleManager.Setup(r => r.FindByNameAsync(roleName))
                .Returns(Task.FromResult(role));
            mockRoleManager.Setup(r => r.GetClaimsAsync(role))
                .Returns(Task.FromResult(claims));
            var controller = new RolesController(mockUserManager.Object, mockRoleManager.Object);

            // Act
            var result = await controller.ShowRoleClaims(roleName);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ShowRoleClaimsViewModel>(viewResult.ViewData.Model);
            Assert.Equal(claims, model.RoleClaims);
            Assert.Equal(roleName, model.RoleName);
        }

        [Fact]
        public async Task RoleClaimsを新規登録しShowRoleClaimsを表示()
        {
            // Arrange
            var roleName = "Developer";
            var claimType = "Receipt";
            var mockUserManager = this.GetMockUserManager();
            var mockRoleManager = this.GetMockRoleManager();
            mockRoleManager.Setup(r => r.FindByNameAsync(roleName))
                .Returns(Task.FromResult(new ApplicationRole() { Name = roleName }));
            var controller = new RolesController(mockUserManager.Object, mockRoleManager.Object);

            // Act
            var result = await controller.AddRoleClaims(roleName, claimType);

            // Assert
            mockRoleManager.Verify(r => r.AddClaimAsync(It.IsAny<ApplicationRole>(), It.IsAny<Claim>()), Times.Once);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.ShowRoleClaims), redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task RoleClaimsを削除しShowRoleClaimsを表示()
        {
            // Arrange
            var roleName = "Developer";
            var claimType = "Receipt";
            var mockUserManager = this.GetMockUserManager();
            var mockRoleManager = this.GetMockRoleManager();
            mockRoleManager.Setup(r => r.FindByNameAsync(roleName))
                .Returns(Task.FromResult(new ApplicationRole() { Name = roleName }));
            var controller = new RolesController(mockUserManager.Object, mockRoleManager.Object);

            // Act
            var result = await controller.DeleteRoleClaims(roleName, claimType);

            // Assert
            mockRoleManager.Verify(r => r.RemoveClaimAsync(It.IsAny<ApplicationRole>(), It.IsAny<Claim>()), Times.Once);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal(nameof(controller.ShowRoleClaims), redirectToActionResult.ActionName);
        }

        private Mock<UserManager<ApplicationUser>> GetMockUserManager()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
        }
        private Mock<RoleManager<ApplicationRole>> GetMockRoleManager()
        {
            var roleStore = new Mock<IRoleStore<ApplicationRole>>();
            return new Mock<RoleManager<ApplicationRole>>(
                         roleStore.Object, null, null, null, null);
        }
    }
}
