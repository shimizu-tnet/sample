using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.TournamentEntries
{
    public interface ITournamentEntryRepository
    {
        /// <summary>
        /// 大会申込の一覧を取得します。
        /// </summary>
        /// <returns>大会申込の一覧。</returns>
        Task<List<TournamentEntry>> FindAllAsync();

        /// <summary>
        /// 大会、種目を条件に抽出した大会申込の一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>大会、種目を条件に抽出した大会申込の一覧。</returns>
        Task<List<TournamentEntry>> FindByIdAsync(int tournamentId, string tennisEventId);
    }
}
