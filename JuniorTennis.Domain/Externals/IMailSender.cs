using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Externals
{
    public interface IMailSender
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
