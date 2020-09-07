using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.RequestPlayers;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.ReservationNumbers;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Mvc.Features.Shared;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Players
{
    /// <summary>
    /// 選手登録申込状況一覧画面 表示用ViewModel。
    /// </summary>
    public class RequestStateDisplayViewModel
    {
        /// <summary>
        /// 予約番号を取得または設定します。
        /// </summary>
        public string ReservationNumber { get; set; }

        /// <summary>
        /// 受領種別名を取得または設定します。
        /// </summary>
        public string RequestTypeName { get; set; }

        /// <summary>
        /// 申込日時を取得または設定します。
        /// </summary>
        public string RequestedDateTime { get; set; }

        /// <summary>
        /// 受領状態名を取得または設定します。
        /// </summary>
        public string ApproveStateName { get; set; }

        /// <summary>
        /// 氏名を取得または設定します。
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// フリガナを取得または設定します。
        /// </summary>
        public string PlayerNameKana { get; set; }

        /// <summary>
        /// カテゴリー名を取得または設定します。
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 性別を取得または設定します。
        /// </summary>
        public string GenderName { get; set; }

        /// <summary>
        /// 誕生日を取得または設定します。
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 登録選手からviewModelの新しいインスタンスを生成します。
        /// </summary
        public RequestStateDisplayViewModel(RequestPlayer requestplayer)
        {
            this.ReservationNumber = requestplayer.ReservationNumber.Value;
            this.RequestTypeName = requestplayer.RequestType.Name;
            this.RequestedDateTime = $"{requestplayer.RequestedDateTime:yyyy/M/d HH:mm}";
            this.ApproveStateName = requestplayer.ApproveState == ApproveState.Approved ? ApproveState.Approved.Name : "申請中";
            this.PlayerName = new PlayerName(requestplayer.Player.PlayerFamilyName, requestplayer.Player.PlayerFirstName).Value;
            this.PlayerNameKana = new PlayerNameKana(requestplayer.Player.PlayerFamilyNameKana, requestplayer.Player.PlayerFirstNameKana).Value;
            this.CategoryName = requestplayer.Category.Name;
            this.GenderName = requestplayer.Player.Gender.Name;
            this.BirthDate = requestplayer.Player.BirthDate.DisplayValue;
        }
    }
}
