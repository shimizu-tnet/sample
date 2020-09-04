using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace JuniorTennis.Mvc.Features.Identity.Roles
{
    public class ShowRoleClaimsViewModel
    {
        public IReadOnlyList<Claim> RoleClaims { get; }

        public List<SelectListItem> AppClaimTypes { get; set; }

        public string RoleName { get; set; }

        public string ClaimType { get; set; }

        public ShowRoleClaimsViewModel(string roleName, IList<Claim> roleClaims, List<string> appClaimTypes)
        {
            this.RoleName = roleName;
            this.RoleClaims = roleClaims.ToList();
            var appClaimTypesItems = new List<SelectListItem>();
            foreach(var item in appClaimTypes)
            {
                appClaimTypesItems.Add(new SelectListItem { Value = item, Text = item });
            }

            this.AppClaimTypes = appClaimTypesItems;
        }
    }
}
