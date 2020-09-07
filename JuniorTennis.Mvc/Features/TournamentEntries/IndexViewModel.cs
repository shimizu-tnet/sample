using JuniorTennis.Domain.UseCases.TournamentEntries;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JuniorTennis.Mvc.Features.TournamentEntries
{
    /// <summary>
    /// 大会申込種目一覧のビューモデル。
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// 申込可能な大会の名前の一覧を取得または設定します。
        /// </summary>
        public List<SelectListItem> TournamentNames { get; set; }

        /// <summary>
        /// 選択された大会IDを取得または設定します。
        /// </summary>
        [Display(Name = "大会")]
        public string SelectedTournamentId { get; set; }

        /// <summary>
        /// 選択された種目 ID を取得または設定します。
        /// </summary>
        [Display(Name = "種目")]
        public string SelectedTennisEventId { get; set; }

        /// <summary>
        /// 大会申込種目一覧のビューモデルの新しいインスタンスを生成します。
        /// </summary>
        public IndexViewModel(List<ApplicationTournamentsDto> applicationTournaments)
        {
            this.TournamentNames = applicationTournaments.Select(o => new SelectListItem
            {
                Value = o.Id,
                Text = o.Name
            })
            .ToList();
        }
    }
}
