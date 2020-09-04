using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Mvc.Features.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Players
{
    /// <summary>
    /// 継続登録申請画面入力ViewModel。
    /// </summary>
    public class RequestContinuedPlayersInputViewModel
    {
        /// <summary>
        /// 選手idを取得または設定します。
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// 氏名を取得または設定します。
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// 氏名(カナ)を取得または設定します。
        /// </summary>
        public string PlayerNameKana { get; set; }

        /// <summary>
        /// カテゴリーidを取得または設定します。
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// カテゴリー名を取得または設定します。
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 性別を取得または設定します。
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 誕生日を取得または設定します。
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 既に申請済みかどうかを取得または設定します。
        /// </summary>
        public bool IsRequested { get; set; }

        /// <summary>
        /// 選択されているかどうかを取得または設定します。
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// カテゴリーセレクトボックスを取得または設定します。
        /// </summary>
        public List<SelectListItem> CategorySelect { get; set; }

        /// <summary>
        /// dtoからviewModelの新しいインスタンスを生成します。
        /// </summary>
        public RequestContinuedPlayersInputViewModel(RequestContinuedPlayersDto dto)
        {
            this.PlayerId = dto.PlayerId;
            this.PlayerName = dto.PlayerName.Value;
            this.PlayerNameKana = dto.PlayerNameKana.Value;
            this.CategoryId = dto.Category.Id;
            this.CategoryName = dto.Category.Name;
            this.Gender = dto.Gender.Name;
            this.BirthDate = dto.BirthDate.DisplayValue;
            this.IsRequested = dto.IsRequested;
            this.IsSelected = false;
            var select = MvcViewHelper.CreateSelectListItem<Category>();
            var extracted = select.Where(o => int.Parse(o.Value) <= this.CategoryId).ToArray();
            this.CategorySelect = extracted.ToList();
        }

        /// <summary>
        /// viewModelの新しいインスタンスを生成します。
        /// </summary>
        public RequestContinuedPlayersInputViewModel() { }
    }
}
