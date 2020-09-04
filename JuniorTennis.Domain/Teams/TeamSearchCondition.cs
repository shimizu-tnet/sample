using JuniorTennis.Domain.QueryConditions;
using JuniorTennis.SeedWork;
using System.Linq;

namespace JuniorTennis.Domain.Teams
{
    /// <summary>
    /// 団体検索の条件。
    /// </summary>
    public class TeamSearchCondition : SearchCondition<Team>
    {
        /// <summary>
        /// 団体検索の条件の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="pageIndex">ページ番号。</param>
        /// <param name="displayCount">表示件数。</param>
        public TeamSearchCondition(int pageIndex, int displayCount, int[] teamTypes, string teamName)
        {
            if (teamTypes?.Any() ?? false)
            {
                var types = teamTypes
                    .Select(o => Enumeration.FromValue<TeamType>(o))
                    .ToArray();
                this.AddFilter(o => types.Contains(o.TeamType));
            }

            if (!string.IsNullOrEmpty(teamName))
            {
                this.AddFilter(o => ((string)(object)o.TeamName).Contains(teamName));
            }

            this.AddSort(SortDirection.Descending, team => team.Id);
            this.Paginate(pageIndex, displayCount);
        }
    }
}
