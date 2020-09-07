using JuniorTennis.Domain.QueryConditions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Tournaments
{
    public interface ITournamentRepository
    {
        Task<List<Tournament>> Find();
        Task<Tournament> Add(Tournament entity);
        Task<Tournament> FindById(int id);
        Task<Tournament> Update(Tournament entity);
        Task Delete(Tournament entity);

        Task<Tournament> FindByRegistrationYear(DateTime registrationYear);

        /// <summary>
        /// 検索条件に応じた大会の一覧を取得します。
        /// </summary>
        /// <param name="condition">検索条件。</param>
        /// <returns>大会一覧。</returns>
        Task<List<Tournament>> SearchAsync(SearchCondition<Tournament> condition);
    }
}
