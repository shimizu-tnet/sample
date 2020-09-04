using JuniorTennis.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Identity.Accounts
{
    public class AuthorizationUseCase : IAuthorizationUseCase
    {
        private readonly IAuthorizationLinkRepository authorizationLinkRepository;

        public AuthorizationUseCase(IAuthorizationLinkRepository authorizationLinkRepository)
        {
            this.authorizationLinkRepository = authorizationLinkRepository;
        }

        public async Task<AuthorizationLink> AddAuthorizationLink(string uniqueKey)
        {
            var confirmationMailAddress = new AuthorizationLink(
                uniqueKey,
                DateTime.Now);
            return await this.authorizationLinkRepository.Add(confirmationMailAddress);
        }
    }
}
