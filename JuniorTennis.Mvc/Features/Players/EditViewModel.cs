using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.UseCases.Players;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Players
{
    public class EditViewModel
    {
        [Display(Name = "登録番号")]
        public string PlayerCode { get; set; }

        [Display(Name = "氏名")]
        public string PlayerName { get; set; }

        [Display(Name = "フリガナ")]
        public string PlayerNameKana { get; set; }

        [Display(Name = "性別")]
        public string Gender { get; set; }

        [Display(Name = "JPIN")]
        public string PlayerJpin { get; set; }

        [Display(Name = "カテゴリー")]
        public string Category { get; set; }

        [Display(Name = "生年月日")]
        public string BirthDate { get; set; }

        [Phone(ErrorMessage = "電話番号の形式で入力してください。")]
        [Display(Name = "電話番号")]
        public string TelephoneNumber { get; set; }

        public static EditViewModel FromEntity(Player player)
        {
            return new EditViewModel
            {
                PlayerCode = player.PlayerCode.Value,
                PlayerName = new PlayerName(player.PlayerFamilyName, player.PlayerFirstName).Value,
                PlayerNameKana = new PlayerNameKana(player.PlayerFamilyNameKana, player.PlayerFirstNameKana).Value,
                Gender = player.Gender.Name,
                PlayerJpin = player.PlayerJpin,
                Category = player.Category.Name,
                BirthDate = player.BirthDate.DisplayValue,
                TelephoneNumber = player.TelephoneNumber
            };
        }
    }
}
