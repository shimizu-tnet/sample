using JuniorTennis.SeedWork;
using JuniorTennis.Domain.UseCases.Teams;

namespace JuniorTennis.Domain.Teams
{
    /// <summary>
    /// 団体。
    /// </summary>
    public class Team : EntityBase
    {
        /// <summary>
        /// 団体登録番号を取得します。
        /// </summary>
        public TeamCode TeamCode { get; private set; }

        /// <summary>
        /// 団体種別を取得します。
        /// </summary>
        public TeamType TeamType { get; private set; }

        /// <summary>
        /// 団体名称を取得します。
        /// </summary>
        public TeamName TeamName { get; private set; }

        /// <summary>
        /// 団体名略称を取得します。
        /// </summary>
        public TeamAbbreviatedName TeamAbbreviatedName { get; private set; }

        /// <summary>
        /// 代表者氏名を取得します。
        /// </summary>
        public string RepresentativeName { get; private set; }

        /// <summary>
        /// 代表者Eメールアドレスを取得します。
        /// </summary>
        public string RepresentativeEmailAddress { get; private set; }

        /// <summary>
        /// 電話番号を取得します。
        /// </summary>
        public string TelephoneNumber { get; private set; }

        /// <summary>
        /// 住所を取得します。
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// コーチ氏名を取得します。
        /// </summary>
        public string CoachName { get; private set; }

        /// <summary>
        /// コーチEメールアドレスを取得します。
        /// </summary>
        public string CoachEmailAddress { get; private set; }

        /// <summary>
        /// JPINの団体番号を取得します。
        /// </summary>
        public string TeamJpin { get; private set; }

        private Team() { }

        public Team(
            TeamCode teamCode,
            TeamType teamType,
            TeamName teamName,
            TeamAbbreviatedName teamAbbreviatedName,
            string representativeName,
            string representativeEmailAddress,
            string telephoneNumber,
            string address,
            string coachName,
            string coachEmailAddress,
            string teamJpin
            )
        {
            this.TeamCode = teamCode;
            this.TeamType = teamType;
            this.TeamName = teamName;
            this.TeamAbbreviatedName = teamAbbreviatedName;
            this.RepresentativeName = representativeName;
            this.RepresentativeEmailAddress = representativeEmailAddress;
            this.TelephoneNumber = telephoneNumber;
            this.Address= address;
            this.CoachName = coachName;
            this.CoachEmailAddress = coachEmailAddress;
            this.TeamJpin = teamJpin;
        }

        /// <summary>
        /// JPINの団体番号を変更します。
        /// </summary>
        /// <param name="teamJpin">JPINの団体番号。</param>
        public void ChangeTeamJpin(string teamJpin) => this.TeamJpin = teamJpin;

        /// <summary>
        /// 代表者メールアドレスを変更します。
        /// </summary>
        /// <param name="representativeEmailAddress">代表者メールアドレス。</param>
        public void ChangeRepresentativeEmailAddress(string representativeEmailAddress) => this.RepresentativeEmailAddress = representativeEmailAddress;

        /// <summary>
        /// 団体番号を変更します。
        /// </summary>
        /// <param name="teamCode">団体番号。</param>
        public void ChangeTeamCode(string teamCode) => this.TeamCode = new TeamCode(teamCode);

        /// <summary>
        /// 団体情報を変更します。
        /// </summary>
        /// <param name="teamJpin">JPINの団体番号。</param>
        public void ChangeTeamData(UpdateTeamDto dto)
        {
            this.TeamName = new TeamName(dto.TeamName);
            this.TeamAbbreviatedName = TeamAbbreviatedName.Parse(dto.TeamAbbreviatedName);
            this.RepresentativeName = dto.RepresentativeName;
            this.RepresentativeEmailAddress = dto.RepresentativeEmailAddress;
            this.TelephoneNumber = dto.TelephoneNumber;
            this.Address = dto.Address;
            this.CoachName = dto.CoachName;
            this.CoachEmailAddress = dto.CoachEmailAddress;
        }
    }
}
