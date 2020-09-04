using JuniorTennis.Domain.UseCases.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Teams
{
    public interface ITeamRepository
    {
        /// <summary>
        /// 団体の一覧を取得します。
        /// </summary>
        /// <returns>団体一覧。</returns>
        Task<List<Team>> FindAsync();

        /// <summary>
        /// 団体コードに紐づく団体を取得します。
        /// </summary>
        /// <param name="teamCode">団体登録番号。</param>
        /// <returns>団体。</returns>
        Task<Team> FindByCodeAsync(TeamCode teamCode);

        /// <summary>
        /// 団体Idに紐づく団体を取得します。
        /// </summary>
        /// <param name="teamId">団体Id。</param>
        /// <returns>団体。</returns>
        Task<Team> FindByIdAsync(int teamId);

        /// <summary>
        /// 団体を更新します。
        /// </summary>
        /// <param name="team">団体。</param>
        /// <returns>更新後の団体。</returns>
        Task<Team> Update(Team team);

        /// <summary>
        /// 団体を新規登録申請します。
        /// </summary>
        /// <param name="team">団体。</param>
        /// <returns>登録後の団体。</returns>
        Task<Team> Add(Team entity);

        /// <summary>
        /// 検索条件に応じた団体の一覧を取得します。
        /// </summary>
        /// <returns>団体一覧。</returns>
        Task<Pagable<Team>> SearchAsync(TeamSearchCondition condition);
    }
}
