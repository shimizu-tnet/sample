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
    /// 選手所属変更申込画面ViewModel。
    /// </summary>
    public class TransferViewModel
    {
        /// <summary>
        /// 検索選手一覧。
        /// </summary>
        public List<Player> SearchedPlayers { get; set; }

        /// <summary>
        /// 検索選手id一覧。
        /// </summary>
        public List<int> SelectedPlayerIds { get; set; }

        /// <summary>
        /// 所属変更対象選手一覧。
        /// </summary>
        public List<Player> TransferPlayers { get; set; }

        /// <summary>
        /// 所属変更対象選手id一覧。
        /// </summary>
        public List<int> TransferPlayerIds { get; set; }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public TransferViewModel() { }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public TransferViewModel(List<Player> players) 
        {
            this.SearchedPlayers = players;
            this.TransferPlayers = new List<Player>();
        }
    }
}
