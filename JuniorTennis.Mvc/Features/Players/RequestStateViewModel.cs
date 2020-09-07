using System.Collections.Generic;
using System.Linq;
using JuniorTennis.Domain.RequestPlayers;
using JuniorTennis.Domain.ReservationNumbers;

namespace JuniorTennis.Mvc.Features.Players
{
    /// <summary>
    /// 選手登録申込状況一覧画面ViewModel。
    /// </summary>
    public class RequestStateViewModel
    {
        /// <summary>
        /// 予約番号ごとの登録選手一覧を取得または設定します。
        /// </summary>
        public Dictionary<string, List<RequestStateDisplayViewModel>> RequestPlayersMap { get; set; }

        /// <summary>
        /// 予約番号リスト。
        /// </summary>
        public List<string> ReservationNumbers { get; set; }

        /// <summary>
        /// 登録選手一覧をもとにViewModelの新しいインスタンスを生成します。
        /// </summary>
        public RequestStateViewModel(List<RequestPlayer> requestplayers)
        {
            this.RequestPlayersMap = requestplayers
                .Select(o => new RequestStateDisplayViewModel(o))
                .GroupBy(o => o.ReservationNumber)
                .ToDictionary(o => o.Key, o => o.ToList());
            this.ReservationNumbers = this.RequestPlayersMap.Keys.OrderByDescending(o => o).ToList();
        }

        /// <summary>
        /// ViewModelの新しいインスタンスを生成します。
        /// </summary>
        public RequestStateViewModel() { }
    }
}
