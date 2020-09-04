using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.RequestTeams
{
    /// <summary>
    /// 登録団体リポジトリー。
    /// </summary>
    public interface IRequestTeamRepository
    {
        /// <summary>
        /// 登録団体の一覧を取得します。
        /// </summary>
        /// <returns>団体一覧。</returns>
        Task<List<RequestTeam>> FindAllAsync();

        /// <summary>
        /// 登録団体idに紐づく登録団体を取得します。
        /// </summary>
        /// <param name="requestTeamId">登録団体id。</param>
        /// <returns>登録団体。</returns>
        Task<RequestTeam> FindByRequestTeamIdAsync(int requestTeamId);

        /// <summary>
        /// 年度idに紐づく登録団体を取得します。
        /// </summary>
        /// <param name="seasonId">年度id。</param>
        /// <returns>登録団体。</returns>
        Task<List<RequestTeam>> FindBySeasonIdAsync(int seasonId);

        /// <summary>
        /// 団体idと年度idに紐づく登録団体を取得します。
        /// </summary>
        /// <param name="teamId">団体id。</param>
        /// <param name="seasonId">年度id。</param>
        /// <returns>登録団体。</returns>
        Task<RequestTeam> FindByTeamIdAndSeasonId(int teamId, int seasonId);

        /// <summary>
        /// 登録団体を更新します。
        /// </summary>
        /// <param name="team">団体。</param>
        /// <returns>更新後の登録団体。</returns>
        Task<RequestTeam> Update(RequestTeam entity);

        /// <summary>
        /// 団体を追加します。
        /// </summary>
        /// <param name="team">団体。</param>
        /// <returns>追加後の登録団体。</returns>
        Task<RequestTeam> Add(RequestTeam entity);

        /// <summary>
        /// 検索条件に応じた登録団体の一覧を取得します。
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>登録団体一覧。</returns>
        Task<Pagable<RequestTeam>> SearchAsync(RequestTeamSearchCondition condition);
    }
}
