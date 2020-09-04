using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.Externals;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.Utils;
using JuniorTennis.SeedWork.Exceptions;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Identity.Accounts
{
    public class AccountsUseCase : IAccountsUseCase
    {
        private readonly IMailSender mailSender;
        private readonly IAuthorizationLinkRepository authorizationLinkRepository;
        private readonly ITeamRepository teamRepository;

        public AccountsUseCase(
            IMailSender mailSender,
            IAuthorizationLinkRepository authorizationLinkRepository,
            ITeamRepository teamRepository)
        {
            this.mailSender = mailSender;
            this.authorizationLinkRepository = authorizationLinkRepository;
            this.teamRepository = teamRepository;
        }

        public async Task SendConfirmEmail(string mailAddress, AuthorizationCode authorizationCode, string domainUrl)
        {
            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/confirm_mail.json"));
                var json = sr.ReadToEnd();
                json = json.Replace("\r\n", "");
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                var subject = JsonConverter.ToString(jsonElement.GetProperty("subject"));
                var htmlMessage = JsonConverter.ToMailBodyString(jsonElement.GetProperty("body"));
                htmlMessage = htmlMessage.Replace("団体登録URL", $"{domainUrl}/Teams/RequestTeam?authorizationCode={authorizationCode.Value}");
                await this.mailSender.SendEmailAsync(mailAddress, subject, htmlMessage);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<AuthorizationLink> AddAuthorizationLink(string uniqueKey)
        {
            var confirmationMailAddress = new AuthorizationLink(
                uniqueKey,
                DateTime.Now);

            return await this.authorizationLinkRepository.Add(confirmationMailAddress);
        }

        public async Task<AuthorizationLink> GetAuthorizationLinkByCode(string authorizationCode)
        {
            return await this.authorizationLinkRepository.FindByAuthorizationCodeAsync(new AuthorizationCode(authorizationCode));
        }

        public async Task<string> UpdateTeamMailAddress(string teamCode, string mailAddress)
        {
            var domainTeamCode = new TeamCode(teamCode);
            var team = await this.teamRepository.FindByCodeAsync(domainTeamCode);
            if (team == null)
            {
                throw new NotFoundException(teamCode, typeof(Team));
            }
            var originalMailAddress = team.RepresentativeEmailAddress;
            team.ChangeRepresentativeEmailAddress(mailAddress);
            await this.teamRepository.Update(team);
            return originalMailAddress;
        }

        public async Task SendChangeMailAddressMail(string mailAddress)
        {
            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/changeMailAddress_mail.json"));
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

        public async Task SendSetupPasswordVerifyMail(string mailAddress, string linkUrl)
        {
            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/setupPasswordVerifyMail_mail.json"));
                var json = sr.ReadToEnd();
                json = json.Replace("\r\n", "");
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                var subject = JsonConverter.ToString(jsonElement.GetProperty("subject"));
                var htmlMessage = JsonConverter.ToMailBodyString(jsonElement.GetProperty("body"));
                htmlMessage = htmlMessage.Replace("{URL}", linkUrl);
                await this.mailSender.SendEmailAsync(mailAddress, subject, htmlMessage);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
