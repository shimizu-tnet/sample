using JuniorTennis.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;

namespace JuniorTennis.MvcTests
{
    /// <summary>
    /// モックの生成クラス。
    /// </summary>
    public class MockMaker
    {
        /// <summary>
        /// UserManagerのモックを生成します。
        /// </summary>
        /// <returns>UserManagerのモック。</returns>
        public static Mock<UserManager<ApplicationUser>> MakeMoqUserManager()
        {
            return new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
        }

        /// <summary>
        /// RoleManagerのモックを生成します。
        /// </summary>
        /// <returns>RoleManagerのモック。</returns>
        public static Mock<RoleManager<ApplicationRole>> MakeMockRoleManager()
        {
            return new Mock<RoleManager<ApplicationRole>>(
                new Mock<IRoleStore<ApplicationRole>>().Object,
                new Mock<IEnumerable<IRoleValidator<ApplicationRole>>>().Object,
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<ILogger<RoleManager<ApplicationRole>>>().Object);
        }

        /// <summary>
        /// SignInManagerのモックを生成します。
        /// </summary>
        /// <returns>SignInManagerのモック。</returns>
        public static Mock<SignInManager<ApplicationUser>> MakeMockSignInManager() =>
            MockMaker.MakeMockSignInManager(MockMaker.MakeMoqUserManager().Object);

        /// <summary>
        /// SignInManagerのモックを生成します。
        /// カスタマイズしたUserManagerのモックを設定する場合に使用します。
        /// </summary>
        /// <param name="userManager">UserManager。</param>
        /// <returns>SignInManagerのモック。</returns>
        public static Mock<SignInManager<ApplicationUser>> MakeMockSignInManager(UserManager<ApplicationUser> userManager)
        {
            return new Mock<SignInManager<ApplicationUser>>(
                userManager,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<ApplicationUser>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object,
                new Mock<IUserConfirmation<ApplicationUser>>().Object);
        }
    }
}
