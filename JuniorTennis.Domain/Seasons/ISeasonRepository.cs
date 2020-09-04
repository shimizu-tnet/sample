using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Seasons
{
    public interface ISeasonRepository
    {
        /// <summary>
        /// 指定した日付を含む期間を持つ年度を取得します。
        /// </summary>
        /// <param name="date">日付</param>
        /// <returns>年度。</returns>
        Task<List<Season>> FindByRegistrationPeriod(DateTime date);

        /// <summary>
        /// 指定した日付に合致する年度を取得します。
        /// </summary>
        /// <returns>現在の年度。</returns>
        Task<Season> FindByDate(DateTime date);

        /// <summary>
        /// 年度を追加します。
        /// </summary>
        /// <param name="entity">entity。</param>
        /// <returns>登録後の年度。</returns>
        Task<Season> Add(Season entity);

        /// <summary>
        /// 年度一覧を取得します。
        /// </summary>
        /// <returns>年度一覧。</returns>
        Task<List<Season>> FindAll();

        /// <summary>
        /// 指定したIdで年度を取得します。
        /// </summary>
        /// <param name="id">年度Id。</param>
        /// <returns>年度。</returns>
        Task<Season> FindById(int id);

        /// <summary>
        /// 年度を更新します。
        /// </summary>
        /// <param name="entity">年度。</param>
        /// <returns>年度。</returns>
        Task<Season> Update(Season entity);
    }
}
