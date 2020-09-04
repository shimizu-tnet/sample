using JuniorTennis.Domain.Tournaments;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Tournaments
{
    public interface ITournamentUseCase
    {
        Task<IEnumerable<Tournament>> GetTournaments();
        Task<Tournament> GetTournament(int id);
        Task<Tournament> RegisterTournament(RegisterTournamentDto dto);
        Task<Tournament> UpdateTournament(UpdateTournamentDto dto);
        Task DeleteTournament(int id);
        List<JsonHoldingDate> CreateHoldingDates(DateTime holdingStartDate, DateTime holdingEndDate);

        /// <summary>
        /// 大会申込受付メールの本文一覧を取得します。
        /// </summary>
        /// <returns>大会申込受付メールの本文一覧</returns>
        Dictionary<string, string> GetTournamentEntryReceptionMailBodies();
    }
}
