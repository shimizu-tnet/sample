using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Mvc.Features.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Players
{
    /// <summary>
    /// 継続登録申請画面ViewModel。
    /// </summary>
    public class RequestContinuedPlayersViewModel
    {
        /// <summary>
        /// 継続登録申請選手一覧を取得または設定します。
        /// </summary>
        public List<RequestContinuedPlayersInputViewModel> RequestContinuedPlayers { get; set; }

        /// <summary>
        /// dtoをもとにViewModelの新しいインスタンスを生成します。
        /// </summary>
        public RequestContinuedPlayersViewModel(List<RequestContinuedPlayersDto> requestContinuedPlayersDtos)
        {
            this.RequestContinuedPlayers = requestContinuedPlayersDtos
                .Select(o => new RequestContinuedPlayersInputViewModel(o))
                .ToList();
        }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public RequestContinuedPlayersViewModel() { }
    }
}
