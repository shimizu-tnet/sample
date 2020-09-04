using System;
using System.Collections.Generic;
using System.Text;

namespace JuniorTennis.Domain.UseCases.Players
{
    /// <summary>
    /// 選手登録用 DTO。
    /// </summary>
    public class AddPlayerDto
    {
        public int TeamId { get; set; }
        public string PlayerFamilyName { get; set; }
        public string PlayerFirstName { get; set; }
        public string PlayerFamilyNameKana { get; set; }
        public string PlayerFirstNameKana { get; set; }
        public int Gender { get; set; }
        public int Category { get; set; }
        public DateTime BirthDate { get; set; }
        public string TelephoneNumber { get; set; }
    }
}
