using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Ranking
{
    /// <summary>
    /// ランキング管理。
    /// </summary>
    public interface IRankingRepository
    {
        /// <summary>
        /// ポイント一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>ドロー表。</returns>
        Task<List<EarnedPoints>> FindByTournamentEvent(int tournamentId, string tennisEventId);
    }
}
