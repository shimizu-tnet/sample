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
    /// 新規登録申請一覧画面ViewModel。
    /// </summary>
    public class RequestPlayersViewModel
    {     
        /// <summary>
        /// 選手一覧を取得または設定します。
        /// </summary>
        public List<Player> Players { get; set; }

        /// <summary>
        /// 選手id一覧を取得または設定します。
        /// </summary>
        public List<int> PlayerIds { get; set; }

        /// <summary>
        /// 選手のリストをもとにViewModelの新しいインスタンスを生成します。
        /// </summary>
        public RequestPlayersViewModel(List<Player> players)
        {
            this.Players = players;
        }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public RequestPlayersViewModel() { }
    }
}
