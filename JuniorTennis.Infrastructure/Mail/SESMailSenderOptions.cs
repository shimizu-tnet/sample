using Amazon;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuniorTennis.Infrastructure.Mail
{
    public class SESMailSenderOptions
    {
        public string SenderAddress { get; set; }
        public RegionEndpoint RegionEndpoint { get; set; }
    }
}
