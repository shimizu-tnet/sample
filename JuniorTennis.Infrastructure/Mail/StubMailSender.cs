using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using JuniorTennis.Domain.Externals;

namespace JuniorTennis.Infrastructure.Mail
{
    public class StubMailSender : IMailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var builder = new StringBuilder();
            builder.AppendLine("【email】");
            builder.AppendLine(email);
            builder.AppendLine();
            builder.AppendLine("【subject】");
            builder.AppendLine(subject);
            builder.AppendLine();
            builder.AppendLine("【message】");
            builder.AppendLine(message);
            builder.AppendLine();
            string logsPath = @"logs\testEmail\";
            if (!Directory.Exists(logsPath))
            {
                Directory.CreateDirectory(logsPath);
            }
            return File.WriteAllTextAsync(Path.Combine(logsPath, $"{Guid.NewGuid()}.txt"), builder.ToString());
        }
    }
}
