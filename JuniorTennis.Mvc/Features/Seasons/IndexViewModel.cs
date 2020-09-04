using JuniorTennis.Domain.Seasons;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Seasons
{
    /// <summary>
    /// 年度一覧のビューモデル。
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// 年度一覧を取得します。
        /// </summary>
        public IReadOnlyList<DisplaySeason> Seasons;

        /// <summary>
        /// 年度一覧のビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="seasons">年度一覧。</param>
        public IndexViewModel(List<Season> seasons)
        {
            this.Seasons = seasons.Select(o => new DisplaySeason(
                    o.Id,
                    o.Name,
                    o.DisplaySeasonPeriod,
                    o.TeamRegistrationFee.DisplayValue,
                    o.PlayerRegistrationFee.DisplayValue
                )).ToList();
        }
    }
}
