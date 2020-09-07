using JuniorTennis.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing;
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

        /// <summary>
        /// IUrlHelperのモックを生成します。
        /// </summary>
        /// <param name="context">HTTPリクエストの一部として選択されたアクションを実行するためのコンテキストオブジェクト。</param>
        /// <see cref="https://github.com/dotnet/aspnetcore/blob/4c3996f8241df09ad811008111440e8c6cf20c9d/src/Mvc/Mvc.Core/test/Routing/UrlHelperExtensionsTest.cs#L856"/>
        /// <returns>IUrlHelperのモック</returns>
        public static Mock<IUrlHelper> MakeIUrlHelper(ActionContext context = null)
        {
            if (context == null)
            {
                context = new ActionContext
                {
                    ActionDescriptor = new ActionDescriptor
                    {
                        RouteValues = new Dictionary<string, string>
                        {
                            { "page", "/Page" },
                        },
                    },
                    RouteData = new RouteData
                    {
                        Values =
                        {
                            [ "page" ] = "/Page"
                        },
                    },
                    HttpContext = new DefaultHttpContext()
                };
            }

            var urlHelper = new Mock<IUrlHelper>();
            urlHelper
                .SetupGet(h => h.ActionContext)
                .Returns(context);
            return urlHelper;
        }
    }
}
