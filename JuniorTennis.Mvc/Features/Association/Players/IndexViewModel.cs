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
using JuniorTennis.Domain.Seasons;

namespace JuniorTennis.Mvc.Features.Association.Players
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
        /// 性別ボタン。
        /// </summary>
        public List<SelectListItem> GenderButton { get; set; }

        /// <summary>
        /// 年度一覧。
        /// </summary>
        public List<Season> Seasons { get; set; }

        /// <summary>
        /// 選手一覧。
        /// </summary>
        public PagedList<Player> Players { get; set; }

        /// <summary>
        /// 選手のリストをもとにViewModelの新しいインスタンスを生成します。
        /// </summary>
        public IndexViewModel(Pagable<Player> players, List<Season> seasons)
        {
            this.Players = new PagedList<Player>(
                players.List.ToList(),
                players.PageIndex,
                players.TotalCount,
                players.DisplayCount);
            this.CategoryButton = MvcViewHelper.CreateSelectListItem<Category>();
            this.GenderButton = MvcViewHelper.CreateSelectListItem<Gender>();
            this.Seasons = seasons;
        }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public IndexViewModel(int displayCount, List<Season> seasons)
        {
            this.Players = new PagedList<Player>(new List<Player>(), 0, 0, displayCount);
            this.CategoryButton = MvcViewHelper.CreateSelectListItem<Category>();
            this.GenderButton = MvcViewHelper.CreateSelectListItem<Gender>();
            this.Seasons = seasons.OrderByDescending(o => o.Id).ToList();
        }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public IndexViewModel() { }
    }
}
