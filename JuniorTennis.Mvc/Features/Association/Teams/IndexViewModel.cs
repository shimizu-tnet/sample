using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.Mvc.Features.Shared.Pagination;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Association.Teams
{
    public class IndexViewModel
    {
        public PagedList<DisplayTeam> Teams { get; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="teams">１ページ分の団体一覧。</param>
        public IndexViewModel(Pagable<Team> teams)
        {
            var list = teams.List
                .Select(o => new DisplayTeam(o))
                .ToList();
            this.Teams = new PagedList<DisplayTeam>(list, teams.PageIndex, teams.TotalCount, teams.DisplayCount);
        }

        public IndexViewModel(int displayCount)
        {
            this.Teams = new PagedList<DisplayTeam>(new List<DisplayTeam>(), 0, 0, displayCount);
        }
    }
}
