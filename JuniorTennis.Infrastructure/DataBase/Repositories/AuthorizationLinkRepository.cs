using System.Linq;
using JuniorTennis.Domain.Teams;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using JuniorTennis.Domain.Accounts;

namespace JuniorTennis.Infrastructure.DataBase.Repositories
{
    public class AuthorizationLinkRepository : IAuthorizationLinkRepository
    {
        private readonly JuniorTennisDbContext context;
        public AuthorizationLinkRepository(JuniorTennisDbContext context) => this.context = context;

        public async Task<AuthorizationLink> FindByAuthorizationCodeAsync(AuthorizationCode authorizationCode) =>
            await this.context.AuthorizationLinks
                .Where(o => o.AuthorizationCode == authorizationCode)
                .FirstOrDefaultAsync();

        /// <summary>
        /// 認証codeとユニークキーを登録します。
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>Task</returns>
        public async Task<AuthorizationLink> Add(AuthorizationLink entity)
        {
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }
    }
}
