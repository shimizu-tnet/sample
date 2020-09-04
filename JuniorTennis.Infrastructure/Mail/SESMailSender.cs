using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using JuniorTennis.Domain.Externals;

namespace JuniorTennis.Infrastructure.Mail
{
    /// <summary>
    /// Amazon SESを用いたEmail送信機能。
    /// </summary>
    public class SESMailSender : IMailSender
    {
        public SESMailSenderOptions Options { get; }

        public SESMailSender(IOptions<SESMailSenderOptions> optionsAccessor)
        {
            this.Options = optionsAccessor.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            using var client = new AmazonSimpleEmailServiceClient(this.Options.RegionEndpoint);
            var sendRequest = new SendEmailRequest
            {
                Source = this.Options.SenderAddress,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { email }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content { Charset = "UTF-8", Data = message },
                        Text = new Content { Charset = "UTF-8", Data = message }
                    }
                },
                // todo ConfigurationSetNameについて調査
                // ConfigurationSetName = configSet
            };

            return client.SendEmailAsync(sendRequest);
        }
    }
}
