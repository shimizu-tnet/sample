using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.Mvc.Features.Shared;
using JuniorTennis.Mvc.Features.Shared.Pagination;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace JuniorTennis.Mvc.Features.Association.Teams
{
    /// <summary>
    /// 受領一覧画面ViewModel。
    /// </summary>
    public class ReceiptViewModel
    {
        /// <summary>
        /// 選択された年度id。
        /// </summary>
        public int SelectedSeasonId { get; set; }

        /// <summary>
        /// 団体番号検索用文字列。
        /// </summary>
        public string TeamCodeForSearch { get; set; }

        /// <summary>
        /// 予約番号検索用文字列。
        /// </summary>
        public string ReservationNumberForSearch { get; set; }

        /// <summary>
        /// 受領状態ボタン。
        /// </summary>
        public List<SelectListItem> ApproveStateButtons { get; set; }

        /// <summary>
        /// 年度一覧。
        /// </summary>
        public List<Season> Seasons { get; set; }

        /// <summary>
        /// 受領状態ボタンで選択されている値。
        /// </summary>
        public int SelectedApproveState { get; set; }

        /// <summary>
        /// 登録団体一覧。
        /// </summary>
        public PagedList<RequestTeam> RequestTeams { get; set; }

        /// <summary>
        /// 選択カラムのチェックボックスで選択された登録団体のidリスト。
        /// </summary>
        public List<int> SelectedRequestTeamIds { get; set; }

        /// <summary>
        /// ページ番号。
        /// </summary>
        public int? Page { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="teams">１ページ分の団体一覧。</param>
        public ReceiptViewModel(Pagable<RequestTeam> requestTeams, List<Season> seasons)
        {
            this.RequestTeams = new PagedList<RequestTeam>(
                requestTeams.List.ToList(),
                requestTeams.PageIndex,
                requestTeams.TotalCount,
                requestTeams.DisplayCount);
            this.Seasons = seasons.OrderByDescending(o => o.Id).ToList();
            this.ApproveStateButtons = MvcViewHelper.CreateSelectListItem<ApproveState>();
            this.Page = requestTeams.PageIndex;
        }

        public ReceiptViewModel(int displayCount, List<Season> seasons)
        {
            this.RequestTeams = new PagedList<RequestTeam>(new List<RequestTeam>(), 0, 0, displayCount);
            this.Seasons = seasons.OrderByDescending(o => o.Id).ToList();
            this.ApproveStateButtons = MvcViewHelper.CreateSelectListItem<ApproveState>(ApproveState.All.Id);
        }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public ReceiptViewModel() { }
    }
}
