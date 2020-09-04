using System.IO;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Externals
{
    /// <summary>
    /// 添付ファイルの管理。
    /// </summary>
    public interface IFileAccessor
    {
        /// <summary>
        /// 添付ファイルのアップロードを行います。
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="fileStream">ファイルストリーム。</param>
        /// <returns>添付ファイルパス。</returns>
        Task<string> UploadAsync(string fileName, Stream fileStream);

        /// <summary>
        /// 添付ファイルの削除を行います。
        /// </summary>
        /// <param name="attachedFilePath">添付ファイルパス。</param>
        Task DeleteAsync(string filePath);
    }
}
