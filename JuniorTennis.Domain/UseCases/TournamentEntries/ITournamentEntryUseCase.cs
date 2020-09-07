using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.TournamentEntries
{
    /// <summary>
    /// 大会申込。
    /// </summary>
    public interface ITournamentEntryUseCase
    {
        /// <summary>
        /// 申込期間中の大会Id大会名を取得します。
        /// </summary>
        /// <returns>大会IDと大会名。</returns>
        Task<List<ApplicationTournamentsDto>> GetApplicationTournaments();

        /// <summary>
        /// 選択された大会を取得します。
        /// </summary>
        /// <param name="tournamentId">大会ID。</param>
        /// <returns>申込期間と開催期間と種目一覧。</returns>
        Task<SelectedTournamentDto> GetSelectedTournament(int tournamentId);
    }
}
