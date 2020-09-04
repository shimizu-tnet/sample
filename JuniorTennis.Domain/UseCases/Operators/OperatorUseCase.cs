using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.Externals;
using JuniorTennis.Domain.Operators;
using JuniorTennis.Domain.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Operators
{
    public class OperatorUseCase : IOperatorUseCase
    {
        private readonly IOperatorRepository repository;

        private readonly IMailSender mailSender;

        public OperatorUseCase(
            IOperatorRepository repository,
            IMailSender mailSender)
        {
            this.repository = repository;
            this.mailSender = mailSender;
        }

        public async Task<List<Operator>> GetOperators()
        {
            var seasons = await this.repository.FindAllAsync();
            return seasons.OrderByDescending(o => o.Id).ToList();
        }

        public async Task<Operator> RegisterOperator(string name, string emailAddress, string loginId)
        {
            var registerOperator = new Operator(
                name,
                new EmailAddress(emailAddress),
                new LoginId(loginId)
                );

            return await this.repository.AddAsync(registerOperator);
        }

        public async Task SendOperatorInvitaionMail(string emaliAddress, string invitaionUrl)
        {
            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/operatorInvitation_mail.json"));
                var json = sr.ReadToEnd();
                json = json.Replace("\r\n", "");
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                var subject = JsonConverter.ToString(jsonElement.GetProperty("subject"));
                var htmlMessage = JsonConverter.ToMailBodyString(jsonElement.GetProperty("body"));
                htmlMessage = htmlMessage.Replace("{URL}", invitaionUrl);
                await this.mailSender.SendEmailAsync(emaliAddress, subject, htmlMessage);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<Operator> GetOperator(int id) => await this.repository.FindByIdAsync(id);

        public async Task<Operator> UpdateOperator(int id, string name, string emailAddress)
        {
            var updateOperator = await this.repository.FindByIdAsync(id);
            updateOperator.Change(name, new EmailAddress(emailAddress));
            return await this.repository.UpdateAsync(updateOperator);
        }
    }
}
