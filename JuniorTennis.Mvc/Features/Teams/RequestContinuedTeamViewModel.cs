using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.UseCases.Teams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Teams
{
    /// <summary>
    /// 団体継続申請画面ビューモデル。
    /// </summary>
    public class RequestContinuedTeamViewModel
    {
        [Display(Name = "登録年度")]
        public string SeasonName { get; set; }

        [Display(Name = "ID")]
        public string TeamCode { get; set; }

        [Display(Name = "団体名")]
        public string TeamName { get; set; }        

        [Display(Name = "代表者")]
        public string RepresentativeName { get; set; }

        public int TeamId { get; set; }

        public int TeamType { get; set; }

        public int SeasonId { get; set; }

        public int RequestedFee { get; set; }

        public string DisplayMessage { get; set; }

        /// <summary>
        /// 可能な申請が全て済である
        /// </summary>
        public bool IsRequestDone { get; set; }

        /// <summary>
        /// 登録ボタンを表示する/しない
        /// </summary>
        public bool IsDisplaySubmitButton { get; set; }

        /// <summary>
        /// 団体申請情報を取得したDtoからViewModelを生成します。
        /// </summary>
        public static RequestContinuedTeamViewModel FromDto(GetRequestTeamStateDto requestTeamState)
        {
            var viewModel =  new RequestContinuedTeamViewModel
            {
                SeasonName = requestTeamState.SeasonName,
                TeamCode = requestTeamState.TeamCode,
                TeamName = requestTeamState.TeamName,
                RepresentativeName = requestTeamState.RepresentativeName,
                TeamId = requestTeamState.TeamId,
                TeamType = requestTeamState.TeamType,
                SeasonId = requestTeamState.SeasonId,
                RequestedFee = requestTeamState.RequestedFee,
                IsRequestDone = requestTeamState.IsRequestDone,
            };
            if (viewModel.IsRequestDone)
            {
                viewModel.DisplayMessage = requestTeamState.IsApproved ? "受領しました。" : "申請済みです。";
            }
            else
            {
                viewModel.DisplayMessage = $"{requestTeamState.SeasonName}の継続登録申込を行います。";
                viewModel.IsDisplaySubmitButton = true;
            }
            return viewModel;
        }

        /// <summary>
        /// ViewModelの情報を元に団体継続申請用にdtoを生成します。
        /// </summary>
        public AddRequestTeamDto ToDto()
        {
            return new AddRequestTeamDto()
            {
                TeamId = this.TeamId,
                TeamType = this.TeamType,
                RequestedFee = this.RequestedFee,
                SeasonId = this.SeasonId
            };
        }
    }
}
