using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ドロー表管理。
    /// </summary>
    public interface IDrawTableRepository
    {
        /// <summary>
        /// ドロー表一覧を取得します。
        /// </summary>
        /// <returns>ドロー表一覧。</returns>
        Task<List<DrawTable>> FindAllAsync();

        /// <summary>
        /// ドロー表を取得します。
        /// </summary>
        /// <param name="dto">ドロー表リポジトリ DTO。</param>
        /// <param name="asNoTracking">追跡なしフラグ。</param>
        /// <returns>ドロー表。</returns>
        Task<DrawTable> FindByDtoAsync(DrawTableRepositoryDto dto, bool asNoTracking = false);

        /// <summary>
        /// 新規にドロー表を登録します。
        /// </summary>
        /// <param name="entity">ドロー表。</param>
        /// <returns>ドロー表。</returns>
        Task<DrawTable> AddAsync(DrawTable entity);

        /// <summary>
        /// ドロー表を更新します。
        /// </summary>
        /// <param name="entity">ドロー表。</param>
        /// <returns>ドロー表。</returns>
        Task<DrawTable> UpdateAsync(DrawTable entity);

        /// <summary>
        /// ドロー表を削除します。
        /// </summary>
        /// <param name="entity">ドロー表。</param>
        Task DeleteAsync(DrawTable entity);

        /// <summary>
        /// ドロー表が存在するかどうか示す値を取得します。
        /// </summary>
        /// <param name="dto">ドロー表リポジトリ DTO。</param>
        /// <returns>存在する場合は true。それ以外は false。</returns>
        Task<bool> ExistsByDtoAsync(DrawTableRepositoryDto dto);
    }
}
