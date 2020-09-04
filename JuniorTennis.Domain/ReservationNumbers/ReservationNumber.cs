using JuniorTennis.SeedWork;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

namespace JuniorTennis.Domain.ReservationNumbers
{
    public class ReservationNumber : ValueObject
    {
        /// <summary>
        /// 予約番号を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 登録日を取得します。
        /// </summary>
        public DateTime RegistratedDate { get; private set; }

        /// <summary>
        /// 連番を取得します。
        /// </summary>
        public int SerialNumber { get; private set; }

        /// <summary>
        /// 予約番号を生成します。
        /// </summary>
        public ReservationNumber()
        {
            this.RegistratedDate = DateTime.Today;
            this.SerialNumber = 1;
            this.Value = $"{this.RegistratedDate:yyyyMMdd}{this.SerialNumber:0000}";
        }

        /// <summary>
        /// 登録日と連番から予約番号を生成します。
        /// </summary>
        private ReservationNumber(
            DateTime registratedDate,
            int serialNumber
            )
        {
            this.RegistratedDate = registratedDate.Date;
            this.SerialNumber = serialNumber;
            this.Value = $"{this.RegistratedDate:yyyyMMdd}{this.SerialNumber:0000}";
        }

        /// <summary>
        /// 取得した値から予約番号を生成します。
        /// </summary>
        /// <param name="value">取得した値。</param>
        /// <returns>予約番号。</returns>
        public static ReservationNumber FromValue(string value)
        {
            var registratedDate = DateTime.Parse($"{value.Substring(0, 4)}/{value.Substring(4, 2)}/{value.Substring(6, 2)}");
            var serialNumber = int.Parse(value.Substring(8));
            return new ReservationNumber(registratedDate, serialNumber);
        }

        /// <summary>
        /// 連番を+1した予約番号を生成します。
        /// </summary>
        public ReservationNumber Next() => new ReservationNumber(this.RegistratedDate, this.SerialNumber + 1);

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
