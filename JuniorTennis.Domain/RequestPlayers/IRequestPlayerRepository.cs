using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.RequestPlayers
{
    /// <summary>
    /// 登録選手管理リポジトリ。
    /// </summary>
    public interface IRequestPlayerRepository
    {
        /// <summary>
        /// 年度idと団体Idに紐づく登録選手一覧を取得します。
        /// </summary>
        /// <param name="teamId">団体id</param>
        /// <param name="seasonId">年度id</param>
        /// <returns>登録選手一覧。</returns>
        Task<List<RequestPlayer>> FindAllByTeamIdAndSeasonId(int teamId, int seasonId);

        /// <summary>
        /// 年度idと団体Idに紐づく、現在申請中登録選手一覧を取得します。
        /// </summary>
        /// <param name="requestPlayer">登録選手。</param>
        /// <returns>登録後の登録選手。</returns>
        Task<List<RequestPlayer>> FindRequestingAsync(int teamId, int seasonId);

        /// <summary>
        /// 選手が他団体に登録されているかどうかを取得します。
        /// </summary>
        /// <param name="teamId">団体id</param>
        /// <returns>選手が別団体に登録されている/いない。</returns>
        Task<bool> ExistsInOtherTeamAsync(int teamId, int playerId);

        /// <summary>
        /// 選手Idリストと年度idに紐づく、登録選手一覧を取得します。
        /// </summary>
        /// <param name="playerIds">選手idリスト</param>
        /// <param name="seasonId">年度id</param>
        /// <returns>登録選手一覧。</returns>
        Task<List<RequestPlayer>> FindAllByPlayerIdsAndSeasonId(List<int> playerIds, int seasonId);

        /// <summary>
        /// 登録選手を更新します。
        /// </summary>
        /// <param name="requestPlayer">登録選手。</param>
        /// <returns>更新後の登録選手。</returns>
        Task<RequestPlayer> UpdateAsync(RequestPlayer requestPlayer);

        /// <summary>
        /// 登録選手を新規登録します。
        /// </summary>
        /// <param name="requestPlayer">登録選手。</param>
        /// <returns>登録後の登録選手。</returns>
        Task<RequestPlayer> AddAsync(RequestPlayer requestPlayer);

        /// <summary>
        /// 検索条件に応じた登録選手の一覧を取得します。
        /// </summary>
        /// <param name="condition">検索条件</param>
        /// <returns>登録選手一覧。</returns>
        Task<List<RequestPlayer>> SearchAsync(RequestPlayerSearchCondition condition);
    }
}
