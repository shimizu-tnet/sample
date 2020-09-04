using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using JuniorTennis.Infrastructure.Identity;

namespace JuniorTennis.Mvc.Features.Identity.Roles
{
    [Area("Identity")]
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> roleManager;

        public RolesController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roleNames = new List<string>(this.roleManager.Roles.Select(o => o.Name));
            var appRoles = new List<string>()
            {
                AppRoleName.Administrator.Name,
                AppRoleName.TournamentCreator.Name,
                AppRoleName.Recorder.Name,
            };
            var addRoleNames = appRoles.Except(roleNames).ToList();
            var deleteRoleNames = roleNames.Except(appRoles).ToList();
            var deleteRoles = new List<DeleteRoleViewModel>();
            if (deleteRoleNames != null)
            {
                foreach (var item in deleteRoleNames)
                {
                    var deleteRole = this.roleManager.FindByNameAsync(item);
                    var addRoleModel = new DeleteRoleViewModel()
                    {
                        RoleName = deleteRole.Result.Name,
                        RoleId = deleteRole.Result.Id,
                        MemberCount = this.userManager.GetUsersInRoleAsync(item).Result.Count()
                    };
                    deleteRoles.Add(addRoleModel);
                }
            }
            var viewModel = new IndexViewModel(roleNames, addRoleNames, deleteRoles);
            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRoles(List<string> AddRoleNames)
        {
            foreach (var item in AddRoleNames)
            {
                await this.roleManager.CreateAsync(new ApplicationRole { Name = item });
            }
            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoles(List<string> deleteRoleNames)
        {
            foreach (var item in deleteRoleNames)
            {
                var targetRole = await this.roleManager.FindByNameAsync(item);
                await this.roleManager.DeleteAsync(targetRole);
            }
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> ShowRoleClaims(string roleName)
        {
            var targetRole = await this.roleManager.FindByNameAsync(roleName);
            var roleClaims = await this.roleManager.GetClaimsAsync(targetRole);
            var viewModel = new ShowRoleClaimsViewModel(roleName, roleClaims, AppClaimTypes.ClaimTypes);
            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRoleClaims(string roleName, string claimType)
        {
            var targetRole = await this.roleManager.FindByNameAsync(roleName);
            await this.roleManager.AddClaimAsync(targetRole, new Claim(claimType, ""));
            return this.RedirectToAction("ShowRoleClaims", new { roleName });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRoleClaims(string roleName, string claimType)
        {
            var targetRole = await this.roleManager.FindByNameAsync(roleName);
            await this.roleManager.RemoveClaimAsync(targetRole, new Claim(claimType, ""));
            return this.RedirectToAction("ShowRoleClaims", new { roleName });
        }
    }
}
