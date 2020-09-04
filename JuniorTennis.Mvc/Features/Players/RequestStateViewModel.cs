using System.Collections.Generic;
using System.Linq;
using JuniorTennis.Domain.RequestPlayers;

namespace JuniorTennis.Mvc.Features.Players
{
    /// <summary>
    /// 選手登録申込状況一覧画面ViewModel。
    /// </summary>
    public class RequestStateViewModel
    {
        /// <summary>
        /// 登録選手一覧を取得または設定します。
        /// </summary>
        public List<RequestPlayer> RequestPlayers { get; set; }

        /// <summary>
        /// 登録選手のリストをもとにViewModelの新しいインスタンスを生成します。
        /// </summary>
        public RequestStateViewModel(List<RequestPlayer> requestplayers)
        {
            this.RequestPlayers = requestplayers;
        }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public RequestStateViewModel() { }
    }
}
