using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Domain.UseCases.Tournaments;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JuniorTennis.Mvc.Features.Tournaments
{
    /// <summary>
    /// 詳細画面。
    /// </summary>
    public class DetailsViewModel
    {
        /// <summary>
        /// 大会 ID を取得または設定します。
        /// </summary>
        [Display(Name = "大会 ID")]
        public int TournamentId { get; set; }

        /// <summary>
        /// 大会名を取得または設定します。
        /// </summary>
        [Display(Name = "大会名")]
        public string TournamentName { get; set; }

        /// <summary>
        /// 大会種別を取得または設定します。
        /// </summary>
        [Display(Name = "大会種別")]
        public string TournamentType { get; set; }

        /// <summary>
        /// 登録年度を取得または設定します。
        /// </summary>
        [Display(Name = "登録年度")]
        public string RegistrationYear { get; set; }

        /// <summary>
        /// 年度種別を取得または設定します。
        /// </summary>
        [Display(Name = "年度種別")]
        public string TypeOfYear { get; set; }

        /// <summary>
        /// 集計月度を取得または設定します。
        /// </summary>
        [Display(Name = "集計月度")]
        public string AggregationMonth { get; set; }

        /// <summary>
        /// 登録済みの大会種目を取得または設定します。
        /// </summary>
        public List<TennisEvent> RegisterdTennisEvents { get; set; }

        /// <summary>
        /// 大会種目を取得または設定します。
        /// </summary>
        [Display(Name = "大会種目")]
        public List<SelectListItem> TennisEvents => this.CreateTennisEvents();

        /// <summary>
        /// 開催期間を取得または設定します。
        /// </summary>
        [Display(Name = "開催期間")]
        public string HoldingPeriod { get; set; }

        /// <summary>
        /// 開催期間の開始日を取得します。
        /// </summary>
        public DateTime HoldingStartDate { get; set; }

        /// <summary>
        /// 開催期間の終了日を取得します。
        /// </summary>
        public DateTime HoldingEndDate { get; set; }

        /// <summary>
        /// 登録済みの開催日を取得または設定します。
        /// </summary>
        public List<HoldingDate> RegisteredHoldingDates { get; set; }

        /// <summary>
        /// 全ての開催日を取得または設定します。
        /// </summary>
        public List<JsonHoldingDate> AllHoldingDates { get; set; }

        /// <summary>
        /// 大会の開催日を取得または設定します。
        /// </summary>
        [Display(Name = "開催日")]
        public List<SelectListItem[]> HoldingDates => this.CreateHoldingDates().ToList();

        /// <summary>
        /// 会場を取得または設定します。
        /// </summary>
        [Display(Name = "会場")]
        public string Venue { get; set; }

        /// <summary>
        /// 支払い方法名を取得または設定します。
        /// </summary>
        [Display(Name = "支払い方法")]
        public string MethodOfPayment { get; set; }

        /// <summary>
        /// 参加費を表示するか示す値を取得または設定します。
        /// </summary>
        public bool ShowEntryFee { get; set; }

        /// <summary>
        /// 参加費を取得または設定します。
        /// </summary>
        [Display(Name = "参加費")]
        public string EntryFee { get; set; }

        /// <summary>
        /// 申込期間を取得または設定します。
        /// </summary>
        [Display(Name = "申込期間")]
        public string ApplicationPeriod { get; set; }

        /// <summary>
        /// 大会要領を取得または設定します。
        /// </summary>
        [Display(Name = "大会要領")]
        public string Outline { get; set; }

        /// <summary>
        /// 大会申込受付メールの件名を取得または設定します。 
        /// </summary>
        [Display(Name = "メール件名")]
        public string TournamentEntryReceptionMailSubject { get; set; }

        /// <summary>
        /// 大会申込受付メールの本文を取得または設定します。 
        /// </summary>
        [Display(Name = "メール本文")]
        public string TournamentEntryReceptionMailBody { get; set; }

        /// <summary>
        /// 種目の一覧を作成します。
        /// </summary>
        /// <returns>種目の一覧。</returns>
        public List<SelectListItem> CreateTennisEvents() =>
            TennisEvent
                .GetAllEvents()
                .Select(o => new SelectListItem(o.Value.DisplayTournamentEvent, o.Key, this.IsSelected(this.RegisterdTennisEvents, o.Key)))
                .ToList();

        private bool IsSelected(IEnumerable<TennisEvent> tennisEvents, string key) =>
             tennisEvents.Any(o => this.CreateTennisEventId(o) == key);

        private string CreateTennisEventId(TennisEvent tennisEvents) =>
             $"{tennisEvents.Category.Id}_{tennisEvents.Gender.Id}_{tennisEvents.Format.Id}";

        /// <summary>
        /// 開催日の一覧を作成します。
        /// </summary>
        /// <returns>種目の一覧。</returns>
        public IEnumerable<SelectListItem[]> CreateHoldingDates()
        {
            var jsonHoldingDates = this.AllHoldingDates
                .Select(o => new SelectListItem(o.Text, o.Value, this.IsSelected(o.Value), !this.IsEnabled(o.Date)))
                .ToList();

            var createHoldingDates = new List<SelectListItem[]>();
            for (var i = 0; i < jsonHoldingDates.Count; i += 7)
            {
                yield return jsonHoldingDates.Skip(i).Take(7).ToArray();
            }
        }

        private bool IsSelected(string value) =>
             this.RegisteredHoldingDates.Any(o => $"{o.Value:yyyy/MM/dd}" == value);

        private bool IsEnabled(DateTime holdingDates) =>
            this.HoldingStartDate <= holdingDates && holdingDates <= this.HoldingEndDate;
    }
}
