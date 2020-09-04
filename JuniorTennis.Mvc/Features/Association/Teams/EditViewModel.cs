using JuniorTennis.Domain.Teams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Association.Teams
{
    public class EditViewModel
    {
        [Display(Name = "団体番号")]
        public string TeamCode { get; set; }

        [Display(Name = "団体種別")]
        public string TeamType { get; set; }

        [Display(Name = "団体名称")]
        public string TeamName { get; set; }

        [Display(Name = "団体名略称")]
        public string TeamAbbreviatedName { get; set; }

        [Display(Name = "代表者")]
        public string RepresentativeName { get; set; }

        [Display(Name = "代表者 メールアドレス")]
        public string RepresentativeEmailAddress { get; set; }

        [Display(Name = "電話番号")]
        public string TelephoneNumber { get; set; }

        [Display(Name = "住所")]
        public string Address { get; set; }

        [Display(Name = "顧問/コーチ")]
        public string CoachName { get; set; }

        [Display(Name = "顧問/コーチ メールアドレス")]
        public string CoachEmailAddress { get; set; }

        [Display(Name = "JPIN")]
        public string TeamJpin { get; set; }

        public static EditViewModel FromEntity(Team team)
        {
            return  new EditViewModel
            {
                TeamCode = team.TeamCode.Value,
                TeamType = team.TeamType.Name,
                TeamName = team.TeamName.Value,
                TeamAbbreviatedName = team.TeamAbbreviatedName?.Value,
                RepresentativeName = team.RepresentativeName,
                RepresentativeEmailAddress = team.RepresentativeEmailAddress,
                TelephoneNumber = team.TelephoneNumber,
                Address = team.Address,
                CoachName = team.CoachName,
                CoachEmailAddress = team.CoachEmailAddress,
                TeamJpin = team.TeamJpin
            };
        }
    }
}
