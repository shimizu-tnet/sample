using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.ReservationNumbers
{
    public interface IReservationNumberRepository
    {
        /// <summary>
        /// 登録日付が本日日付の最大の予約番号を取得します。
        /// １件も存在しない場合nullを返します。
        /// </summary>
        /// <returns>予約番号</returns>
        Task<ReservationNumber> Max();

        /// <summary>
        /// 予約番号を登録します。
        /// </summary>
        /// <param name="entity">entity。</param>
        /// <returns>Task。</returns>
        Task<ReservationNumber> Add(ReservationNumber entity);
    }
}
