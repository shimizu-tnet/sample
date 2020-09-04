using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.Externals;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.ReservationNumbers;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.Domain.Utils;
using JuniorTennis.SeedWork;
using JuniorTennis.SeedWork.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Teams
{
    public class TeamUseCase : ITeamUseCase
    {
        private readonly ITeamRepository teamRepository;
        private readonly IAuthorizationLinkRepository authorizationLinkRepository;
        private readonly IRequestTeamRepository requestTeamRepository;
        private readonly IReservationNumberRepository reservationNumberRepository;
        private readonly ISeasonRepository seasonRepository;
        private readonly IMailSender mailSender;

        public TeamUseCase(
            ITeamRepository teamRepository,
            IAuthorizationLinkRepository authorizationLinkRepository,
            IRequestTeamRepository requestTeamRepository,
            IReservationNumberRepository reservationNumberRepository,
            ISeasonRepository seasonRepository,
            IMailSender mailSender)
        {
            this.teamRepository = teamRepository;
            this.authorizationLinkRepository = authorizationLinkRepository;
            this.requestTeamRepository = requestTeamRepository;
            this.reservationNumberRepository = reservationNumberRepository;
            this.seasonRepository = seasonRepository;
            this.mailSender = mailSender;
        }

        public async Task<Pagable<Team>> SearchTeam(int pageIndex, int displayCount, int[] teamTypes, string teamName)
        {
            var condition = new TeamSearchCondition(pageIndex, displayCount, teamTypes, teamName);
            return await this.teamRepository.SearchAsync(condition);
        }

        public async Task<Team> GetTeam(string teamCode)
        {
            var code = new TeamCode(teamCode);
            var team = await this.teamRepository.FindByCodeAsync(code);
            if (team == null)
            {
                throw new NotFoundException(teamCode, typeof(Team));
            }

            return team;
        }

        public async Task<Team> UpdateTeamJpin(string teamCode, string teamJpin)
        {
            var domainTeamCode = new TeamCode(teamCode);
            var team = await this.teamRepository.FindByCodeAsync(domainTeamCode);
            if (team == null)
            {
                throw new NotFoundException(teamCode, typeof(Team));
            }

            team.ChangeTeamJpin(teamJpin);
            var changed = await this.teamRepository.Update(team);
            return changed;
        }

        /// <summary>
        /// 団体新規登録申請
        /// </summary>
        /// <param name="dto">団体申請用 DTO。</param>
        /// <returns>団体</returns>
        public async Task<Team> RequestTeamNewRegistration(RequestTeamNewRegistrationDto dto)
        {
            var team = new Team(
                null,
                Enumeration.FromValue<TeamType>(dto.TeamType),
                new TeamName(dto.TeamName),
                TeamAbbreviatedName.Parse(dto.TeamName),
                dto.RepresentativeName,
                dto.RepresentativeEmailAddress,
                dto.TelephoneNumber,
                dto.Address,
                dto.CoachName,
                dto.CoachEmailAddress,
                null);

            team = await this.teamRepository.Add(team);

            var season = await this.seasonRepository.FindByDate(DateTime.Today);

            var teamRegistrationFee = team.TeamType == TeamType.School ? 0 : season.TeamRegistrationFee.Value;
            var reservationNumber = await ReservationNumberFactory.Create(this.reservationNumberRepository);
            var requestTeam = new RequestTeam(
                team.Id,
                season.Id,
                reservationNumber,
                ApproveState.Unapproved,
                DateTime.Now,
                new RequestedFee(teamRegistrationFee),
                null,
                MailState.Unsent
                );
            await this.requestTeamRepository.Add(requestTeam);

            return team;
        }

        /// <summary>
        /// 団体申請後の通知メールを送信します。
        /// </summary>
        /// <param name="mailAddress">メールアドレス</param>
        /// <returns>Task</returns>
        public async Task SendRequestTeamNewRegistrationMail(string mailAddress)
        {
            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/register_mail.json"));
                var json = sr.ReadToEnd();
                json = json.Replace("\r\n", "");
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                var subject = JsonConverter.ToString(jsonElement.GetProperty("subject"));
                var htmlMessage = JsonConverter.ToMailBodyString(jsonElement.GetProperty("body"));
                await this.mailSender.SendEmailAsync(mailAddress, subject, htmlMessage);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<AuthorizationLink> GetAuthorizationLinkByCode(string authorizationCode)
        {
            return await this.authorizationLinkRepository.FindByAuthorizationCodeAsync(new AuthorizationCode(authorizationCode));
        }

        public async Task<Team> UpdateTeam(UpdateTeamDto dto)
        {
            var domainTeamCode = new TeamCode(dto.TeamCode);
            var team = await this.teamRepository.FindByCodeAsync(domainTeamCode);
            if (team == null)
            {
                throw new NotFoundException(dto.TeamCode, typeof(Team));
            }
            team.ChangeTeamData(dto);

            return await this.teamRepository.Update(team);
        }

        public async Task<GetRequestTeamStateDto> GetRequestTeamState(int teamId)
        {
            var requestTeamStateDto = new GetRequestTeamStateDto();
            var seasons = await this.seasonRepository.FindByRegistrationPeriod(DateTime.Today);
            foreach (var season in seasons.OrderBy(o => o.Id))
            {
                var requestTeam = await this.requestTeamRepository.FindByTeamIdAndSeasonId(teamId, season.Id);
                if (requestTeam == null)
                {
                    requestTeamStateDto.SeasonId = season.Id;
                    requestTeamStateDto.SeasonName = season.Name;
                    requestTeamStateDto.RequestedFee = season.TeamRegistrationFee.Value;
                    requestTeamStateDto.IsApproved = false;
                    requestTeamStateDto.IsRequestDone = false;
                    break;
                }
                else
                {
                    requestTeamStateDto.SeasonId = season.Id;
                    requestTeamStateDto.SeasonName = season.Name;
                    requestTeamStateDto.IsApproved = requestTeam.ApproveState == ApproveState.Approved;
                    requestTeamStateDto.IsRequestDone = true;
                }
            }
            var team = await this.teamRepository.FindByIdAsync(teamId);
            requestTeamStateDto.TeamId = teamId;
            requestTeamStateDto.TeamCode = team.TeamCode.Value;
            requestTeamStateDto.TeamType = team.TeamType.Id;
            requestTeamStateDto.TeamName = team.TeamName.Value;
            requestTeamStateDto.RepresentativeName = team.RepresentativeName;

            return requestTeamStateDto;
        }

        public async Task<RequestTeam> AddRequestTeam(AddRequestTeamDto dto)
        {
            var requestedFee = dto.TeamType == TeamType.School.Id ? 0 : dto.RequestedFee;
            var reservationNumber = await ReservationNumberFactory.Create(this.reservationNumberRepository);
            var requestTeam = new RequestTeam(
                dto.TeamId,
                dto.SeasonId,
                reservationNumber,
                ApproveState.Unapproved,
                DateTime.Now,
                new RequestedFee(requestedFee),
                null,
                MailState.Unsent
                );
            return await this.requestTeamRepository.Add(requestTeam);
        }

        public async Task<AuthorizationLink> AddAuthorizationLink(string uniqueKey)
        {
            var confirmationMailAddress = new AuthorizationLink(
                uniqueKey,
                DateTime.Now);

            return await this.authorizationLinkRepository.Add(confirmationMailAddress);
        }

        public async Task SendChangeMailAddressVerifyMail(AuthorizationCode authorizationCode, string mailAddress, string domainUrl)
        {
            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/requestChangeMailAddress_mail.json"));
                var json = sr.ReadToEnd();
                json = json.Replace("\r\n", "");
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                var subject = JsonConverter.ToString(jsonElement.GetProperty("subject"));
                var htmlMessage = JsonConverter.ToMailBodyString(jsonElement.GetProperty("body"));
                htmlMessage = htmlMessage.Replace("メールアドレス再設定URL", $"{domainUrl}/Identity/Accounts/ChangeMailAddress?authorizationCode={authorizationCode.Value}");
                await this.mailSender.SendEmailAsync(mailAddress, subject, htmlMessage);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<List<RequestTeam>> GetRequestTeams()
        {
            return await this.requestTeamRepository.FindAllAsync();
        }

        public async Task UpdateRequestTeamsApproveState(int approveStateId, List<int> selectedRequestTeamIds, int selectedSeasonId, string domainUrl)
        {
            var isApproveRegister = approveStateId == ApproveState.Unapproved.Id;
            var seasonRequestTeams = await this.requestTeamRepository.FindBySeasonIdAsync(selectedSeasonId);
            foreach (var requestTeamId in selectedRequestTeamIds)
            {
                var requestTeam = seasonRequestTeams.Find(o => o.Id == requestTeamId);
                var isNewRegister = requestTeam.Team.TeamCode == null;
                if (!isApproveRegister)
                {
                    requestTeam.Unapprove();
                }
                else
                {
                    requestTeam.Approve();
                    // TeamCodeがnullの場合新規団体の受領となり、TeamCodeの発行とパスワード設定メールを通知する
                    if (isNewRegister)
                    {
                        var teamType = requestTeam.Team.TeamType;
                        var newTeamCode = await this.CreateTeamCode(teamType);
                        requestTeam.Team.ChangeTeamCode(newTeamCode);
                        await this.teamRepository.Update(requestTeam.Team);
                    }
                }
                await this.requestTeamRepository.Update(requestTeam);

                if (isNewRegister)
                {
                    await this.SendRequestRegisterPasswordMail(requestTeam.Team, domainUrl);
                    requestTeam.UpdateToMailSent();
                    await this.requestTeamRepository.Update(requestTeam);
                }
            }
        }

        public async Task<string> CreateTeamCode(TeamType teamType)
        {
            var teams = await this.teamRepository.FindAsync();
            if (teams == null)
            {
                return $"{TeamType.GetTeamCodePrefix(teamType)}00001";
            }
            else
            {
                var teamCodes = new List<string>();
                foreach (var team in teams)
                {
                    if (team.TeamCode != null)
                    {
                        teamCodes.Add(team.TeamCode.Value.Substring(1));
                    }
                }

                if (teamCodes.Count == 0)
                {
                    return $"{TeamType.GetTeamCodePrefix(teamType)}00001";
                }
                teamCodes.Sort();
                teamCodes.Reverse();
                var serialNumber = int.Parse(teamCodes.FirstOrDefault()) + 1;
                return $"{TeamType.GetTeamCodePrefix(teamType)}{serialNumber:00000}";
            }
        }

        public async Task SendRequestRegisterPasswordMail(Team team, string domainUrl)
        {
            var authorizationLink = await this.AddAuthorizationLink(team.TeamCode.Value);

            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/requestRegisterPassword_mail.json"));
                var json = sr.ReadToEnd();
                json = json.Replace("\r\n", "");
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                var subject = JsonConverter.ToString(jsonElement.GetProperty("subject"));
                var htmlMessage = JsonConverter.ToMailBodyString(jsonElement.GetProperty("body"));
                htmlMessage = htmlMessage.Replace("パスワード登録URL", $"{domainUrl}/Identity/Accounts/RegisterPassword?authorizationCode={authorizationLink.AuthorizationCode.Value}");
                htmlMessage = htmlMessage.Replace("TeamCode", team.TeamCode.Value);
                await this.mailSender.SendEmailAsync(team.RepresentativeEmailAddress, subject, htmlMessage);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<List<Season>> GetSeasons() => await seasonRepository.FindAll();

        public async Task<Pagable<RequestTeam>> SearchRequestTeams(int pageIndex, int displayCount, int seasonId, string teamCode, string reservationNumber, int approveState)
        {
            var condition = new RequestTeamSearchCondition(pageIndex, displayCount, seasonId, teamCode, reservationNumber, approveState);
            var requestTeams =  await this.requestTeamRepository.SearchAsync(condition);
            return requestTeams;
        }
    }
}
