using System;
using System.Collections.Generic;
using System.Text;

namespace JuniorTennis.Domain.UseCases.Teams
{
    public class GetRequestTeamStateDto
    {
        public int TeamId { get; set; }
        public int SeasonId { get; set; }
        public string SeasonName { get; set; }
        public int RequestedFee { get; set; }
        public string TeamCode { get; set; }
        public int TeamType { get; set; }
        public string TeamName { get; set; }
        public string RepresentativeName { get; set; }
        public bool IsApproved{ get; set; }
        public bool IsRequestDone { get; set; }
    }
}
