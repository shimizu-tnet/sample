using Amazon;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuniorTennis.Infrastructure.Storage
{
    public class S3FileAccessorOptions
    {
        public string BucketName { get; set; }
        public string DomainName { get; set; }
        public RegionEndpoint RegionEndpoint { get; set; }
        public string PdfDirectoryName { get; set; }
    }
}
