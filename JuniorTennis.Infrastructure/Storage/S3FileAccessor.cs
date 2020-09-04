using System;
using System.IO;
using System.Threading.Tasks;
using JuniorTennis.Domain.Externals;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace JuniorTennis.Infrastructure.Storage
{
    public class S3FileAccessor : IFileAccessor
    {
        public S3FileAccessorOptions Options { get; }

        public S3FileAccessor(IOptions<S3FileAccessorOptions> optionsAccessor)
        {
            this.Options = optionsAccessor.Value;
        }
  
        private IAmazonS3 s3Client;
        public async Task DeleteAsync(string filePath)
        {
            this.s3Client = new AmazonS3Client(this.Options.RegionEndpoint);
            var keyName = filePath; // keyNameが"フォルダ名/ファイル名"の形式で取得できる必要あり
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = this.Options.BucketName,
                    Key = keyName
                };

                await s3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
        }

        public async Task<string> UploadAsync(string fileName, Stream fileStream)
        {
            this.s3Client = new AmazonS3Client(this.Options.RegionEndpoint);
            Guid guidValue = Guid.NewGuid();
            string keyName = $"/{this.Options.PdfDirectoryName}/{guidValue}/{fileName}";
            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);
                await fileTransferUtility.UploadAsync(fileStream,
                                               this.Options.BucketName, keyName);
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            string objectURL = $"{this.Options.DomainName}{keyName}";
            return objectURL;
        }
    }
}
