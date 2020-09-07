using JuniorTennis.Domain.QueryConditions;
using System;

namespace JuniorTennis.Domain.Tournaments
{
    public class ApplicationTournamentsSearchCondition : SearchCondition<Tournament>
    {
        /// <summary>
        /// 申込可能な大会の条件の新しいインスタンスを生成します。
        /// </summary>
        public ApplicationTournamentsSearchCondition()
        {
            //this.AddFilter(o => o.ApplicationPeriod.StartDate <= DateTime.Today);
            //this.AddFilter(o => DateTime.Today <= o.ApplicationPeriod.EndDate);
            //this.AddFilter(o => o.TournamentType == TournamentType.WithDraw);
            //this.AddSort(SortDirection.Descending, o => o.HoldingPeriod.StartDate);
        }
    }
}
