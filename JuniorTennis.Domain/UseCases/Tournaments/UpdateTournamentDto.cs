using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.UseCases.Tournaments
{
    /// <summary>
    /// 大会更新用 DTO。
    /// </summary>
    public class UpdateTournamentDto
    {
        public int TournamentId { get; set; }
        public string TournamentName { get; set; }
        public int TournamentType { get; set; }
        public DateTime RegistrationYear { get; set; }
        public int TypeOfYear { get; set; }
        public DateTime AggregationMonth { get; set; }
        public List<(int, int, int)> TennisEvents { get; set; }
        public DateTime HoldingStartDate { get; set; }
        public DateTime HoldingEndDate { get; set; }
        public List<DateTime> HoldingDates { get; set; }
        public string Venue { get; set; }
        public int EntryFee { get; set; }
        public int MethodOfPayment { get; set; }
        public DateTime ApplicationStartDate { get; set; }
        public DateTime ApplicationEndDate { get; set; }
        public string Outline { get; set; }
        public string TournamentEntryReceptionMailSubject { get; set; }
        public string TournamentEntryReceptionMailBody { get; set; }
    }
}
