using System;
using System.Collections.Generic;
using System.Text;

namespace JuniorTennis.Domain.UseCases.Teams
{
    public class AddRequestTeamDto
    {
        public int TeamId { get; set; }
        public int TeamType { get; set; }
        public int RequestedFee { get; set; }
        public int SeasonId { get; set; }
    }
}
