using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Teams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Teams
{
    public class EditViewModel
    {
        [Display(Name = "団体番号")]
        public string TeamCode { get; set; }

        [Display(Name = "団体種別")]
        public string TeamType { get; set; }

        [Required(ErrorMessage = "団体名を入力してください。")]
        [Display(Name = "団体名")]
        public string TeamName { get; set; }

        [Required(ErrorMessage = "団体名略称を入力してください。")]
        [Display(Name = "団体名略称")]
        public string TeamAbbreviatedName { get; set; }

        [Required(ErrorMessage = "代表者を入力してください。")]
        [Display(Name = "代表者")]
        public string RepresentativeName { get; set; }

        [Required(ErrorMessage = "代表者 メールアドレスを入力してください。")]
        [Display(Name = "代表者 メールアドレス")]
        public string RepresentativeEmailAddress { get; set; }

        [Required(ErrorMessage = "電話番号を入力してください。")]
        [Phone(ErrorMessage = "電話番号の形式で入力してください。")]
        [Display(Name = "電話番号")]
        public string TelephoneNumber { get; set; }

        [Required(ErrorMessage = "住所を入力してください。")]
        [Display(Name = "住所")]
        public string Address { get; set; }

        [Display(Name = "顧問/コーチ")]
        public string CoachName { get; set; }

        [EmailAddress(ErrorMessage = "メールアドレスの形式で入力してください。")]
        [Display(Name = "顧問/コーチ メールアドレス")]
        public string CoachEmailAddress { get; set; }

        public string OriginalMailAddress { get; set; }

        public bool IsDuplicated { get; set; }

        /// <summary>
        /// メールアドレスが変更されたかどうか示す値を取得します。
        /// </summary>
        public bool IsMailAddressChanged => this.OriginalMailAddress != this.RepresentativeEmailAddress;

        public static EditViewModel FromEntity(Team team)
        {
            return new EditViewModel
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
                OriginalMailAddress = team.RepresentativeEmailAddress
            };
        }

        public UpdateTeamDto ToDto()
        {
            return new UpdateTeamDto()
            {
                TeamCode = this.TeamCode,
                TeamName = this.TeamName,
                TeamAbbreviatedName = this.TeamAbbreviatedName,
                RepresentativeName = this.RepresentativeName,
                RepresentativeEmailAddress = this.RepresentativeEmailAddress,
                TelephoneNumber = this.TelephoneNumber,
                Address = this.Address,
                CoachName = this.CoachName,
                CoachEmailAddress = this.CoachEmailAddress
            };
        }
    }
}
