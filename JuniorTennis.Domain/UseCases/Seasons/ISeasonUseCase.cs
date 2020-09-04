using JuniorTennis.Domain.Seasons;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Seasons
{
    /// <summary>
    /// 年度管理
    /// </summary>
    public interface ISeasonUseCase
    {
        /// <summary>
        /// 年度一覧を登録の降順で取得します。
        /// </summary>
        /// <returns>年度一覧。</returns>
        Task<List<Season>> GetSeasons();

        /// <summary>
        /// 年度を登録します。
        /// </summary>
        /// <param name="name">年度。</param>
        /// <param name="fromDate">年度開始日。</param>
        /// <param name="toDate">年度終了日。</param>
        /// <param name="registrationFromDate">年度登録受付開始日。</param>
        /// <param name="teamRegistrationFee">団体登録料。</param>
        /// <param name="playerRegistrationFee">選手登録料。</param>
        /// <param name="playerTradeFee">選手移籍料。</param>
        /// <returns>年度。</returns>
        Task<Season> RegisterSeason(
            string name,
            DateTime fromDate,
            DateTime toDate,
            DateTime registrationFromDate,
            int teamRegistrationFee,
            int playerRegistrationFee,
            int playerTradeFee);

        /// <summary>
        /// 年度を更新します。
        /// </summary>
        /// <param name="id">年度Id。</param>
        /// <param name="fromDate">年度開始日。</param>
        /// <param name="toDate">年度終了日。</param>
        /// <param name="registrationFromDate">年度登録受付開始日。</param>
        /// <param name="teamRegistrationFee">団体登録料。</param>
        /// <param name="playerRegistrationFee">選手登録料。</param>
        /// <param name="playerTradeFee">選手移籍料。</param>
        /// <returns>年度。</returns>
        Task<Season> UpdateSeason(
            int id,
            DateTime fromDate,
            DateTime toDate,
            DateTime registrationFromDate,
            int teamRegistrationFee,
            int playerRegistrationFee,
            int playerTradeFee);

        /// <summary>
        /// 年度を取得します。
        /// </summary>
        /// <param name="id">年度Id。</param>
        /// <returns>年度。</returns>
        Task<Season> GetSeason(int id);
    }
}
