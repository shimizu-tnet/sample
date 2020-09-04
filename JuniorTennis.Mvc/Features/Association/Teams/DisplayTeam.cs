using JuniorTennis.Domain.Teams;

namespace JuniorTennis.Mvc.Features.Association.Teams
{
    public class DisplayTeam
    {
        public string TeamCode { get; set; }

        public string TeamType { get; set; }

        public string TeamName { get; set; }

        public string RepresentativeName { get; set; }

        public string RepresentativeEmailAddress { get; set; }

        public string TelephoneNumber { get; set; }

        public string CoachName { get; set; }

        public string CoachEmailAddress { get; set; }

        public string TeamJpin { get; set; }

        public DisplayTeam(Team team)
        {
            this.TeamCode = team.TeamCode.Value;
            this.TeamType = team.TeamType.Name;
            this.TeamName = team.TeamName.Value;
            this.RepresentativeName = team.RepresentativeName;
            this.RepresentativeEmailAddress = team.RepresentativeEmailAddress;
            this.TelephoneNumber = team.TelephoneNumber;
            this.CoachName = team.CoachName;
            this.CoachEmailAddress = team.CoachEmailAddress;
            this.TeamJpin = team.TeamJpin;
        }
    }
}
