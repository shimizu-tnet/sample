using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Home
{
    public class ViewModel
    {
        [Required(ErrorMessage ="未入力エラー")]
        public string Text { get; set; }
    }
}
