using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.ReservationNumbers
{
    public class ReservationNumberFactory
    {
        /// <summary>
        /// 新規の予約番号を発行します。
        /// </summary>
        public async static Task<ReservationNumber> Create(IReservationNumberRepository reservationNumberRepository)
        {
            var findMaxSerialNumberResult = await reservationNumberRepository.Max();
            var newReservationNumber = findMaxSerialNumberResult?.Next() ?? new ReservationNumber();
            await reservationNumberRepository.Add(newReservationNumber);

            return newReservationNumber;
        }
    }
}
