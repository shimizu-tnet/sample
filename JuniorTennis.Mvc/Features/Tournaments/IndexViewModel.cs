using JuniorTennis.Domain.Tournaments;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Tournaments
{
    /// <summary>
    /// 大会一覧ビューモデル。
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// 大会一覧を取得します。
        /// </summary>
        [Display(Name = "大会一覧")]
        public readonly List<DisplayTournament> Tournaments;

        /// <summary>
        /// 大会一覧ビューモデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournaments">大会一覧。</param>
        public IndexViewModel(IEnumerable<Tournament> tournaments) =>
            this.Tournaments = tournaments.Select(o => new DisplayTournament(
                    $"{o.Id}",
                    o.TournamentName?.Value ?? "-",
                    o.TournamentType?.Name ?? "-",
                    o.HoldingPeriod?.DisplayValue ?? "-",
                    o.ApplicationPeriod?.DisplayValue ?? "-",
                    o.Venue?.Value ?? "-",
                    o.EntryFee?.DisplayValue ?? "-"
                )).ToList();
    }
}
