using JuniorTennis.Domain.Externals;
using System.IO;
using System.Threading.Tasks;

namespace JuniorTennis.Infrastructure
{
    public class StubFileAccessor : IFileAccessor
    {
        public Task<string> UploadAsync(string fileName, Stream fileStream)
        {
            return Task.FromResult("https://www.taiseinet.com/wp/wp-content/uploads/2020/06/suyappi_leaflet.pdf");
        }

        public Task DeleteAsync(string filePath) => Task.CompletedTask;
    }
}
