using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.Mvc.Features.Shared;
using JuniorTennis.Mvc.Features.Shared.Pagination;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using JuniorTennis.Domain.Tournaments;

namespace JuniorTennis.Mvc.Features.Players
{
    /// <summary>
    /// 選手一覧画面ViewModel。
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// カテゴリーボタン。
        /// </summary>
        public List<SelectListItem> CategoryButton { get; set; }

        /// <summary>
        /// カテゴリーボタンで選択されている値。
        /// </summary>
        public int SelectedCategoryState { get; set; }

        /// <summary>
        /// 性別ボタン。
        /// </summary>
        public List<SelectListItem> GenderButton { get; set; }

        /// <summary>
        /// 性別ボタンで選択されている値。
        /// </summary>
        public int SelectedGenderState { get; set; }

        /// <summary>
        /// 選手一覧。
        /// </summary>
        public List<Player> Players { get; set; }

        /// <summary>
        /// 選手のリストをもとにViewModelの新しいインスタンスを生成します。
        /// </summary>
        public IndexViewModel(List<Player> players)
        {
            this.Players = players;
            this.CategoryButton = MvcViewHelper.CreateSelectListItem<Category>();
            this.GenderButton = MvcViewHelper.CreateSelectListItem<Gender>();
        }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public IndexViewModel() { }
    }
}
