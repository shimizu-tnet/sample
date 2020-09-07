using System.Collections.Generic;

namespace JuniorTennis.Domain.UseCases.TournamentEntries
{
    /// <summary>
    /// 大会の開催期間と申込期間の取得用 Dto。
    /// </summary>
    public class SelectedTournamentDto
    {
        public List<TennisEventDto> TennisEvents { get; set; }
        public string HoldingPeriod { get; set; }
        public string ApplicationPeriod { get; set; }
    }
}
