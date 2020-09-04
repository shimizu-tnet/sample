using System;
using System.Collections.Generic;
using System.Text;

namespace JuniorTennis.Domain.UseCases.Teams
{
    public class UpdateTeamDto
    {
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string TeamAbbreviatedName { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeEmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string Address { get; set; }
        public string CoachName { get; set; }
        public string CoachEmailAddress { get; set; }
    }
}
