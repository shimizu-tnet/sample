using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Domain.UseCases.Tournaments;
using JuniorTennis.SeedWork;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Tournaments
{
    /// <summary>
    /// 大会管理コントローラ。
    /// </summary>
    public class TournamentsController : Controller
    {
        private readonly ITournamentUseCase useCase;
        public TournamentsController(
            ITournamentUseCase useCase)
        {
            this.useCase = useCase;
        }

        /// <summary>
        /// 大会一覧を表示します。
        /// </summary>
        /// <returns>大会管理のトップ画面。</returns>
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var tournaments = await this.useCase.GetTournaments();
            var model = new IndexViewModel(tournaments);
            return this.View(model);
        }

        /// <summary>
        /// 大会の登録画面を表示します。
        /// </summary>
        /// <returns>大会の登録画面。</returns>
        [HttpGet]
        public ActionResult Register()
        {
            var tournamentEntryReceptionMailBodies = this.useCase.GetTournamentEntryReceptionMailBodies();
            return this.View(new RegisterViewModel(tournamentEntryReceptionMailBodies));
        }

        /// <summary>
        /// 大会を登録します。
        /// </summary>
        /// <param name="model">RegisterViewModel。</param>
        /// <returns>大会管理のトップ画面。</returns>
        [HttpPost]
        public async Task<ActionResult> Register([Bind(
            "TournamentName",
            "SelectedTournamentType",
            "SelectedRegistrationYear",
            "SelectedTypeOfYear",
            "SelectedAggregationMonth",
            "SelectedTennisEvents",
            "HoldingStartDate",
            "HoldingEndDate",
            "SelectedHoldingDates",
            "SelectedTennisEvents",
            "Venue",
            "EntryFee",
            "SelectedMethodOfPayments",
            "ApplicationStartDate",
            "ApplicationEndDate",
            "Outline",
            "TournamentEntryReceptionMailBody",
            "TournamentEntryReceptionMailSubject")]
            RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                var allHoldingDates = this.useCase
                    .CreateHoldingDates(model.HoldingStartDate.Value, model.HoldingEndDate.Value)
                    .Select(o => o.Value)
                    .ToList();
                model.AllHoldingDates = allHoldingDates;
                return this.View(model);
            }

            var dto = model.ToDto();
            await this.useCase.RegisterTournament(dto);
            return this.RedirectToAction(nameof(this.Index));
        }

        /// <summary>
        /// 大会の詳細画面を表示します。
        /// </summary>
        /// <param name="id">大会 ID。</param>
        /// <returns>大会の詳細画面。</returns>
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            var tournament = await this.useCase.GetTournament(id);
            var registeredTennisEvents = tournament.TennisEvents.ToList();
            var holdingPeriod = tournament.HoldingPeriod;
            var registeredHoldingDates = tournament.HoldingDates.ToList();
            var allHoldingDates = this.useCase
                .CreateHoldingDates(holdingPeriod.StartDate, holdingPeriod.EndDate)
                .ToList();
            var model = new DetailsViewModel()
            {
                TournamentId = id,
                TournamentName = tournament.TournamentName.Value,
                TournamentType = tournament.TournamentType.Name,
                RegistrationYear = tournament.RegistrationYear.DisplayValue,
                TypeOfYear = tournament.TypeOfYear.Name,
                AggregationMonth = tournament.AggregationMonth.DisplayValue,
                RegisterdTennisEvents = registeredTennisEvents,
                HoldingPeriod = tournament.HoldingPeriod.DisplayValue,
                HoldingStartDate = tournament.HoldingPeriod.StartDate,
                HoldingEndDate = tournament.HoldingPeriod.EndDate,
                RegisteredHoldingDates = registeredHoldingDates,
                AllHoldingDates = allHoldingDates,
                Venue = tournament.Venue.Value,
                MethodOfPayment = tournament.MethodOfPayment.Name,
                ShowEntryFee = tournament.ShowEntryFee,
                EntryFee = tournament.EntryFee.DisplayValue,
                ApplicationPeriod = tournament.ApplicationPeriod.DisplayValue,
                Outline = tournament.Outline.Value,
                TournamentEntryReceptionMailBody = tournament.TournamentEntryReceptionMailBody,
                TournamentEntryReceptionMailSubject = tournament.TournamentEntryReceptionMailSubject,
            };
            return this.View(model);
        }

        /// <summary>
        /// 大会の編集画面を表示します。
        /// </summary>
        /// <param name="id">大会 ID。</param>
        /// <returns>大会の編集画面。</returns>
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var tournament = await this.useCase.GetTournament(id);
            var holdingPeriod = tournament.HoldingPeriod;
            var allHoldingDates = this.useCase
                .CreateHoldingDates(holdingPeriod.StartDate, holdingPeriod.EndDate)
                .Select(o => o.Value)
                .ToList();
            var tournamentEntryReceptionMailBodies = this.useCase.GetTournamentEntryReceptionMailBodies();
            var model = new EditViewModel()
            {
                TournamentId = tournament.Id,
                TournamentName = tournament.TournamentName.Value,
                HoldingStartDate = tournament.HoldingPeriod.StartDate,
                HoldingEndDate = tournament.HoldingPeriod.EndDate,
                AllHoldingDates = allHoldingDates,
                Venue = tournament.Venue.Value,
                EntryFee = tournament.EntryFee.Value,
                ApplicationStartDate = tournament.ApplicationPeriod.StartDate,
                ApplicationEndDate = tournament.ApplicationPeriod.EndDate,
                Outline = tournament.Outline.Value,
                TournamentEntryReceptionMailBody = tournament.TournamentEntryReceptionMailBody,
                TournamentEntryReceptionMailSubject = tournament.TournamentEntryReceptionMailSubject,
                SelectedTournamentType = $"{tournament.TournamentType.Id}",
                SelectedRegistrationYear = $"{tournament.RegistrationYear.ElementValue}",
                SelectedTypeOfYear = $"{tournament.TypeOfYear.Id}",
                SelectedAggregationMonth = $"{tournament.AggregationMonth.ElementValue}",
                SelectedTennisEvents = tournament.TennisEvents.Select(o => o.TennisEventId).ToList(),
                SelectedHoldingDates = tournament.HoldingDates.Select(o => o.ElementValue).ToList(),
                SelectedMethodOfPayments = $"{tournament.MethodOfPayment.Id}",
                TournamentEntryReceptionMailBodies = tournamentEntryReceptionMailBodies
            };
            return this.View(model);
        }

        /// <summary>
        /// 大会を更新します。
        /// </summary>
        /// <param name="model">EditViewModel。</param>
        /// <returns>大会の詳細画面。</returns>
        [HttpPost]
        public async Task<ActionResult> Edit([Bind(
            "TournamentId",
            "TournamentName",
            "SelectedTournamentType",
            "SelectedRegistrationYear",
            "SelectedTypeOfYear",
            "SelectedAggregationMonth",
            "SelectedTennisEvents",
            "HoldingStartDate",
            "HoldingEndDate",
            "AllHoldingDates",
            "SelectedHoldingDates",
            "SelectedTennisEvents",
            "Venue",
            "EntryFee",
            "SelectedMethodOfPayments",
            "ApplicationStartDate",
            "ApplicationEndDate",
            "Outline",
            "TournamentEntryReceptionMailBody",
            "TournamentEntryReceptionMailSubject")]
            EditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var dto = model.ToDto();
            await this.useCase.UpdateTournament(dto);
            return this.RedirectToAction("Details", new { id = model.TournamentId });
        }

        /// <summary>
        /// 大会を削除します。
        /// </summary>
        /// <param name="id">大会 ID。</param>
        /// <returns>大会管理のトップ画面。</returns>
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await this.useCase.DeleteTournament(id);
            return this.RedirectToAction(nameof(this.Index));
        }

        /// <summary>
        /// 指定された登録年度から集計月度の一覧を取得します。
        /// </summary>
        /// <param name="registrationYear">登録年度。</param>
        /// <returns>集計月度の一覧。</returns>
        [HttpGet]
        [Route("Tournaments/{registrationYear}/aggregation_months")]
        public string GetAggregationMonths(string registrationYear)
        {
            if (string.IsNullOrEmpty(registrationYear))
            {
                return "";
            }

            var registrationYears = Enumerable
                .Range(0, 16)
                .Select(o => DateTime.Parse($"{registrationYear}/04/01").AddMonths(o))
                .Select(o => new
                {
                    value = $"{o:yyyy/MM/dd}",
                    text = $"{o:yyyy年M月}"
                });

            return JsonSerializer.Serialize(registrationYears);
        }

        /// <summary>
        /// 指定された開催期間から開催日の一覧を取得します。
        /// </summary>
        /// <param name="holdingStartDate">開催期間の開始日。</param>
        /// <param name="holdingEndDate">開催期間の終了日。</param>
        /// <returns>開催日の一覧。</returns>
        [HttpGet]
        [Route("Tournaments/{holdingStartDate}/{holdingEndDate}/holding_dates")]
        public string GetHoldingDates(string holdingStartDate, string holdingEndDate)
        {
            if (string.IsNullOrEmpty(holdingStartDate) || string.IsNullOrEmpty(holdingEndDate))
            {
                return "";
            }

            var holdingDates = this.useCase
                .CreateHoldingDates(DateTime.Parse(holdingStartDate), DateTime.Parse(holdingEndDate))
                .Select(o => new
                {
                    value = o.Value,
                    text = o.Text,
                    disabled = o.Disabled,
                });

            return JsonSerializer.Serialize(holdingDates);
        }
    }
}
