using JuniorTennis.Domain.QueryConditions;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.SeedWork;
using System.Linq;

namespace JuniorTennis.Domain.Teams
{
    /// <summary>
    /// 登録団体検索の条件。
    /// </summary>
    public class RequestTeamSearchCondition : SearchCondition<RequestTeam>
    {
        /// <summary>
        /// 登録団体検索の条件の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="pageIndex">ページ番号。</param>
        /// <param name="displayCount">表示件数。</param>
        /// <param name="seasonId">年度id。</param>
        /// <param name="teamCode">団体番号。</param>
        /// <param name="reservationNumber">予約番号。</param>
        /// <param name="approveState">受領状態。</param>
        public RequestTeamSearchCondition(int pageIndex, int displayCount, int seasonId, string teamCode, string reservationNumber, int approveState)
        {
            this.AddFilter(o => seasonId == o.SeasonId);

            if (!string.IsNullOrEmpty(teamCode))
            {
                this.AddFilter(o => ((string)(object)o.Team.TeamCode).Contains(teamCode));
            }

            if (!string.IsNullOrEmpty(reservationNumber))
            {
                this.AddFilter(o => ((string)(object)o.ReservationNumber).Contains(reservationNumber));
            }

            if (approveState != ApproveState.All.Id)
            {
                this.AddFilter(o => o.ApproveState == Enumeration.FromValue<ApproveState>(approveState));
            }

            this.AddSort(SortDirection.Descending, requestTeam => requestTeam.Id);
            this.Paginate(pageIndex, displayCount);
        }
    }
}
