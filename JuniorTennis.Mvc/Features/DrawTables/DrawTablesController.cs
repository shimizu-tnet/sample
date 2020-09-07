using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Ranking;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Domain.UseCases.DrawTables;
using JuniorTennis.Domain.UseCases.Tournaments;
using JuniorTennis.Domain.Utils;
using JuniorTennis.SeedWork;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.DrawTables
{
    /// <summary>
    /// ドロー表管理コントローラ。
    /// </summary>
    public class DrawTablesController : Controller
    {
        private readonly ITournamentUseCase tournamentUseCase;
        private readonly IDrawTableUseCase drawTableUseCase;
        public DrawTablesController(
            ITournamentUseCase tournamentUseCase,
            IDrawTableUseCase drawTableUseCase)
        {
            this.tournamentUseCase = tournamentUseCase;
            this.drawTableUseCase = drawTableUseCase;
        }

        #region MVC
        /// <summary>
        /// 大会・種目選択画面を表示します。
        /// </summary>
        /// <returns>大会・種目選択画面。</returns>
        [HttpGet]
        public ActionResult Index()
        {
            return this.View(new IndexViewModel()
            {
                OnlyMain = TournamentFormat.OnlyMain,
                WithQualifying = TournamentFormat.WithQualifying,
            });
        }

        /// <summary>
        /// 大会・種目選択画面を表示します。
        /// </summary>
        /// <returns>大会・種目選択画面。</returns>
        [HttpPost]
        public async Task<ActionResult> Index(IndexViewModel model)
        {
            var tournamentId = int.Parse(model.SelectedTournametId);
            var tennisEventId = model.SelectedTennisEventId;
            var tournamentFromatId = int.Parse(model.SelectedTournamentFormatId);
            await this.drawTableUseCase.StartEditingTheDrawTable(tournamentId, tennisEventId, tournamentFromatId);

            return this.RedirectToAction(nameof(Players), new
            {
                tournamentId = model.SelectedTournametId,
                tennisEventId = model.SelectedTennisEventId,
            });
        }

        /// <summary>
        /// 選手情報設定画面を表示します。
        /// </summary>
        /// <returns>選手情報設定画面。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/Players")]
        public async Task<ActionResult> Players(int tournamentId, string tennisEventId)
        {
            this.ViewData["Action"] = "Players";
            var tournament = await this.tournamentUseCase.GetTournament(tournamentId);
            var tennisEvent = TennisEvent.FromId(tennisEventId);
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeEntryDetails = true,
            };
            var drawTable = await this.drawTableUseCase.GetDrawTable(dto);
            var useQualifyingMenu = drawTable.TournamentFormat == TournamentFormat.WithQualifying;

            return this.View(new PlayerViewModel(
                $"{tournament.Id}",
                tournament.TournamentName.Value,
                tennisEvent.TennisEventId,
                tennisEvent.DisplayTournamentEvent,
                drawTable.EligiblePlayersType.Id,
                tournament.HoldingDates.Select(o => o.DisplayValue),
                useQualifyingMenu
            ));
        }

        /// <summary>
        /// 選手情報設定画面を表示します。
        /// </summary>
        /// <returns>選手情報設定画面。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/Players")]
        public async Task<ActionResult> Players(PlayerViewModel model, [FromQuery(Name = "next")] string next)
        {
            var dto = new DrawTableRepositoryDto(int.Parse(model.TournamentId), model.TennisEventId);
            var drawTable = await this.drawTableUseCase.GetDrawTable(dto);
            var eligiblePlayersType = Enumeration.FromValue<EligiblePlayersType>(model.EligiblePlayersTypeId);
            await this.drawTableUseCase.UpdateEligiblePlayersType(drawTable, eligiblePlayersType);

            return this.RedirectToAction(next, new
            {
                tournamentId = model.TournamentId,
                tennisEventId = model.TennisEventId,
            });
        }

        /// <summary>
        /// ドロー設定画面を表示します。
        /// </summary>
        /// <returns>ドロー設定画面。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/Settings")]
        public async Task<ActionResult> Settings(int tournamentId, string tennisEventId)
        {
            this.ViewData["Action"] = "Settings";
            var tournament = await this.tournamentUseCase.GetTournament(tournamentId);
            var tennisEvent = TennisEvent.FromId(tennisEventId);
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeEntryDetails = true,
                IncludeQualifyingDrawSettings = true,
                IncludeMainDrawSettings = true,
            };
            var drawTable = await this.drawTableUseCase.GetDrawTable(dto);

            return this.View(new SettingsViewModel(
                $"{tournament.Id}",
                tournament.TournamentName.Value,
                tennisEvent.TennisEventId,
                tennisEvent.DisplayTournamentEvent,
                drawTable
            ));
        }

        /// <summary>
        /// ドロー設定画面を表示します。
        /// </summary>
        /// <returns>ドロー設定画面。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/Settings")]
        public ActionResult Settings(SettingsViewModel model, [FromQuery(Name = "next")] string next)
        {
            return this.RedirectToAction(next, new
            {
                tournamentId = model.TournamentId,
                tennisEventId = model.TennisEventId,
            });
        }

        /// <summary>
        /// ドロー作成画面を表示します。
        /// </summary>
        /// <returns>ドロー作成画面。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/Create")]
        public async Task<ActionResult> Create(int tournamentId, string tennisEventId)
        {
            this.ViewData["Action"] = "Create";
            var tournament = await this.tournamentUseCase.GetTournament(tournamentId);
            var tennisEvent = TennisEvent.FromId(tennisEventId);
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId);
            var drawTable = await this.drawTableUseCase.GetDrawTable(dto);
            var useQualifyingMenu = drawTable.TournamentFormat == TournamentFormat.WithQualifying;

            return this.View(new CreateViewModel(
                $"{tournament.Id}",
                tournament.TournamentName.Value,
                tennisEvent.TennisEventId,
                tennisEvent.DisplayTournamentEvent,
                tournament.HoldingDates.Select(o => o.DisplayValue),
                useQualifyingMenu,
                tennisEvent.IsSingles
            ));
        }

        /// <summary>
        /// ドロー作成画面を表示します。
        /// </summary>
        /// <returns>ドロー作成画面。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/Create")]
        public ActionResult Create(CreateViewModel model, [FromQuery(Name = "next")] string next)
        {
            return this.RedirectToAction(next, new
            {
                tournamentId = model.TournamentId,
                tennisEventId = model.TennisEventId,
            });
        }

        /// <summary>
        /// 試合結果入力画面を表示します。
        /// </summary>
        /// <returns>試合結果入力画面。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/Results")]
        public async Task<ActionResult> Results(int tournamentId, string tennisEventId)
        {
            this.ViewData["Action"] = "Results";
            var tournament = await this.tournamentUseCase.GetTournament(tournamentId);
            var tennisEvent = TennisEvent.FromId(tennisEventId);

            return this.View(new ResultsViewModel(
                $"{tournament.Id}",
                tournament.TournamentName.Value,
                tennisEvent.TennisEventId,
                tennisEvent.DisplayTournamentEvent,
                tennisEvent.IsSingles
            ));
        }

        /// <summary>
        /// 試合結果入力画面を表示します。
        /// </summary>
        /// <returns>試合結果入力画面。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/Results")]
        public ActionResult Results(ResultsViewModel model, [FromQuery(Name = "next")] string next)
        {
            return this.RedirectToAction(next, new
            {
                tournamentId = model.TournamentId,
                tennisEventId = model.TennisEventId,
            });
        }
        #endregion MVC

        #region WEB API
        /// <summary>
        /// 大会の一覧を取得します。
        /// </summary>
        /// <returns>大会の覧。</returns>
        [HttpGet]
        [Route("DrawTables/tournaments")]
        public async Task<string> GetTournaments()
        {
            var tournaments = (await this.tournamentUseCase.GetTournaments())
                .OrderByDescending(o => o.HoldingPeriod.StartDate)
                .ThenBy(o => o.Id)
                .Select(o => new
                {
                    id = o.Id,
                    name = o.TournamentName.Value,
                    holdingPeriod = o.HoldingPeriod?.DisplayValue ?? "-",
                    venue = o.Venue?.Value ?? "-"
                });

            return JsonSerializer.Serialize(tournaments);
        }

        /// <summary>
        /// 大会に紐づく種目の一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <returns>種目一覧。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/tennis_events")]
        public async Task<string> GetTennisEvents(int tournamentId)
        {
            if (tournamentId == 0)
            {
                return "[]";
            }

            var tournament = await this.tournamentUseCase.GetTournament(tournamentId);
            var tennisEvents = tournament.TennisEvents.Select(o => new
            {
                id = o.TennisEventId,
                name = o.DisplayTournamentEvent
            });

            return JsonSerializer.Serialize(tennisEvents);
        }

        /// <summary>
        /// 大会、種目に紐づく大会形式を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>大会形式。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/tournament_format")]
        public async Task<string> GetTournamentFormat(int tournamentId, string tennisEventId)
        {
            if (await this.drawTableUseCase.ExistsDrawTable(tournamentId, tennisEventId))
            {
                var drawTable = await this.drawTableUseCase.GetDrawTableWithoutRelevantData(tournamentId, tennisEventId);
                return JsonSerializer.Serialize(new
                {
                    existsDrawTable = true,
                    tournamentFormatId = drawTable.TournamentFormat.Id
                });
            }

            return JsonSerializer.Serialize(new
            {
                existsDrawTable = false,
                tournamentFormatId = TournamentFormat.OnlyMain.Id
            });
        }

        /// <summary>
        /// 大会、種目に紐づく申込者数を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>申込者数。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/number_of_entries")]
        public async Task<string> GetNumberOfEntries(int tournamentId, string tennisEventId)
        {
            if (tournamentId == 0 || tennisEventId == "0")
            {
                return JsonSerializer.Serialize(new { count = 0 });
            }

            var numberOfEntries = await this.drawTableUseCase.GetNumberOfEntries(tournamentId, tennisEventId);
            return JsonSerializer.Serialize(new { count = numberOfEntries });
        }

        /// <summary>
        /// 参加選手の一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="reacquisition">再取得フラグ。</param>
        /// <param name="eligiblePlayersTypeId">出場対象選手の種別。</param>
        /// <param name="participationClassificationId">出場区分。</param>
        /// <returns>参加選手一覧。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/entry_players")]
        public async Task<string> GetEntryDetails(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "reacquisition")] bool? reacquisition,
            [FromQuery(Name = "eligiblePlayersTypeId")] int? eligiblePlayersTypeId,
            [FromQuery(Name = "participationClassification")] int? participationClassificationId)
        {
            if (tournamentId == 0)
            {
                return "[]";
            }

            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeEntryDetails = true,
                IncludeEntryPlayers = true,
                IncludeQualifyingDrawSettings = true,
                IncludeMainDrawSettings = true,
            };
            var drawTable = await this.drawTableUseCase.GetDrawTable(dto);
            var tournament = await this.tournamentUseCase.GetTournament(drawTable.TournamentId);

            if (reacquisition ?? false)
            {
                var entryDetails
                    = await this.drawTableUseCase.RetrievePlayers(drawTable.TournamentId, drawTable.TennisEventId);
                drawTable.UpdateEntryDetails(entryDetails);
            }

            var eligiblePlayersType =
                eligiblePlayersTypeId.HasValue
                ? Enumeration.FromValue<EligiblePlayersType>(eligiblePlayersTypeId.Value)
                : null;

            var participationClassification =
                participationClassificationId.HasValue
                ? Enumeration.FromValue<ParticipationClassification>(participationClassificationId.Value)
                : null;

            return this.drawTableUseCase.GetEntryDetailsJson(tournament, drawTable, eligiblePlayersType, participationClassification);
        }

        /// <summary>
        /// 参加選手の一覧を登録します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="json">参加選手 JSON。</param>
        /// <returns>処理が正常に終了した場合 OK。それ以外の場合 NG。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/entry_players")]
        public async Task<HttpResponseMessage> RegisterEntryDetails(int tournamentId, string tennisEventId, [FromBody] JsonElement json)
        {
            try
            {
                var entryDetails = json.EnumerateArray().Select(o =>
                {
                    var entryNumber = new EntryNumber(JsonConverter.ToInt32(o.GetProperty("entryNumber")));
                    var participationClassification = JsonConverter.ToEnumeration<ParticipationClassification>(o.GetProperty("participationClassificationId"));
                    var seedNumber = new SeedNumber(JsonConverter.ToInt32(o.GetProperty("seedNumber")));
                    var teamCodes = o.GetProperty("teamCodes").EnumerateArray().Select(p => new TeamCode(JsonConverter.ToString(p)));
                    var teamNames = o.GetProperty("teamNames").EnumerateArray().Select(p => new TeamName(JsonConverter.ToString(p)));
                    var teamAbbreviatedNames = o.GetProperty("teamAbbreviatedNames").EnumerateArray().Select(p => new TeamAbbreviatedName(JsonConverter.ToString(p)));
                    var playerCodes = o.GetProperty("playerCodes").EnumerateArray().Select(p => new PlayerCode(JsonConverter.ToString(p)));
                    var playerFamilyNames = o.GetProperty("playerFamilyNames").EnumerateArray().Select(p => new PlayerFamilyName(JsonConverter.ToString(p)));
                    var playerFirstNames = o.GetProperty("playerFirstNames").EnumerateArray().Select(p => new PlayerFirstName(JsonConverter.ToString(p)));
                    var points = o.GetProperty("points").EnumerateArray().Select(p => new Point(JsonConverter.ToInt32(p)));
                    var canParticipationDates = o.GetProperty("canParticipationDates").EnumerateArray()
                        .Where(p => JsonConverter.ToBoolean(p.GetProperty("isParticipate")))
                        .Select(p => new CanParticipationDate(JsonConverter.ToDateTime(p.GetProperty("value"))));
                    var receiptStatus = JsonConverter.ToEnumeration<ReceiptStatus>(o.GetProperty("receiptStatusId"));

                    var entryPlayers = teamCodes.Select((o, i) => new EntryPlayer(
                        o,
                        teamNames.ElementAt(i),
                        teamAbbreviatedNames.ElementAt(i),
                        playerCodes.ElementAt(i),
                        playerFamilyNames.ElementAt(i),
                        playerFirstNames.ElementAt(i),
                        points.ElementAt(i)
                    ));

                    return new EntryDetail(
                        entryNumber,
                        participationClassification,
                        seedNumber,
                        entryPlayers,
                        canParticipationDates,
                        receiptStatus,
                        UsageFeatures.DrawTable);
                });

                var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
                {
                    IncludeEntryDetails = true,
                    IncludeQualifyingDrawSettings = true,
                    IncludeMainDrawSettings = true,
                };
                var drawTable = await this.drawTableUseCase.GetDrawTable(dto);
                drawTable.UpdateEntryDetails(entryDetails);
                await this.drawTableUseCase.UpdateDrawTable(drawTable);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// ドロー設定を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="tournamentFromatId">出場区分 ID。</param>
        /// <returns>ドロー設定。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/draw_settings")]
        public async Task<string> GetDrawSettings(int tournamentId, string tennisEventId, [FromQuery(Name = "participationClassification")] int participationClassificationId)
        {
            return await this.drawTableUseCase.GetDrawSettingsJson(tournamentId, tennisEventId, participationClassificationId);
        }

        /// <summary>
        /// ドロー設定を登録します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="json">ドロー設定 JSON。</param>
        /// <returns>処理が正常に終了した場合ステータスコード 200。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/draw_settings")]
        public async Task<HttpResponseMessage> RegisterDrawSettings(int tournamentId, string tennisEventId, [FromBody] JsonElement json)
        {
            var participationClassification = JsonConverter.ToEnumeration<ParticipationClassification>(json.GetProperty("participationClassification"));
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeQualifyingDrawSettings = true,
                IncludeMainDrawSettings = true,
                IncludeGames = true,
            };
            var drawTable = await this.drawTableUseCase.GetDrawTable(dto);
            var drawSettings = drawTable.GetDrawSettings(participationClassification);
            drawSettings.UpdateNumberOfBlocks(JsonConverter.ToInt32(json.GetProperty("numberOfBlocks")));
            drawSettings.UpdateNumberOfDraws(JsonConverter.ToInt32(json.GetProperty("numberOfDraws")));
            drawSettings.UpdateNumberOfEntries(JsonConverter.ToInt32(json.GetProperty("numberOfEntries")));
            drawSettings.UpdateNumberOfWinners(JsonConverter.ToInt32(json.GetProperty("numberOfWinners")));
            drawSettings.UpdateTournamentGrade(JsonConverter.ToInt32(json.GetProperty("tournamentGrade")));
            drawTable.UpdateDrawSettings(participationClassification, drawSettings);
            drawTable.Blocks.ChangeNumberOfBlocks(participationClassification, drawTable.QualifyingDrawSettings);
            await this.drawTableUseCase.UpdateDrawTable(drawTable);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// ドロー表を初期化しましす。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>ドロー表 JSON。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/initialize_draw_table")]
        public async Task<string> InitializeDrawTable(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "participationClassification")] int participationClassificationId)
        {
            await this.drawTableUseCase.InitializeDrawTable(tournamentId, tennisEventId, participationClassificationId);
            return await this.drawTableUseCase.GetBlocksJson(tournamentId, tennisEventId, participationClassificationId);
        }

        /// <summary>
        /// シード位置の自動設定を行います。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>ドロー表 JSON。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/execute_seed_frame_setting")]
        public async Task<string> ExecuteSeedFrameSetting(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "participationClassification")] int participationClassificationId)
        {
            await this.drawTableUseCase.ExecuteSeedFrameSetting(tournamentId, tennisEventId, participationClassificationId);
            return await this.drawTableUseCase.GetBlocksJson(tournamentId, tennisEventId, participationClassificationId);
        }

        /// <summary>
        /// 割当済みのシード位置を全て削除します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>ドロー表 JSON。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/execute_seed_frame_remove")]
        public async Task<string> ExecuteSeedFrameRemove(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "participationClassification")] int participationClassificationId)
        {
            await this.drawTableUseCase.ExecuteSeedFrameRemove(tournamentId, tennisEventId, participationClassificationId);
            return await this.drawTableUseCase.GetBlocksJson(tournamentId, tennisEventId, participationClassificationId);
        }

        /// <summary>
        /// BYE 位置の自動設定を行います。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>ドロー表 JSON。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/execute_bye_frame_setting")]
        public async Task<string> ExecuteByeFrameSetting(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "participationClassification")] int participationClassificationId)
        {
            await this.drawTableUseCase.ExecuteByeFrameSetting(tournamentId, tennisEventId, participationClassificationId);
            return await this.drawTableUseCase.GetBlocksJson(tournamentId, tennisEventId, participationClassificationId);
        }

        /// <summary>
        /// 割当済みの BYE 位置を全て削除します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>ドロー表 JSON。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/execute_bye_frame_remove")]
        public async Task<string> ExecuteByeFrameRemove(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "participationClassification")] int participationClassificationId)
        {
            await this.drawTableUseCase.ExecuteByeFrameRemove(tournamentId, tennisEventId, participationClassificationId);
            return await this.drawTableUseCase.GetBlocksJson(tournamentId, tennisEventId, participationClassificationId);
        }

        /// <summary>
        /// 予選勝者を取り込みます。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>処理が正常に終了した場合ステータスコード 200。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/intake_qualifying_winners")]
        public async Task<HttpResponseMessage> IntakeQualifyingWinners(int tournamentId, string tennisEventId)
        {
            await this.drawTableUseCase.IntakeQualifyingWinners(tournamentId, tennisEventId);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// 試合日一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>試合日一覧。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/game_dates")]
        public async Task<string> GetGameDates(int tournamentId, string tennisEventId)
        {
            return await this.drawTableUseCase.GetGameDatesJson(tournamentId, tennisEventId);
        }

        /// <summary>
        /// 試合日一覧を登録します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="json">試合日一覧 JSON。</param>
        /// <returns>Task。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/game_dates")]
        public async Task RegisterGameDates(int tournamentId, string tennisEventId, [FromBody] JsonElement json)
        {
            var gameDates = new List<(int blockNumber, DateTime gameDate)>();
            foreach (var o in json.EnumerateArray())
            {
                var blockNumber = JsonConverter.ToInt32(o.GetProperty("blockNumber"));
                foreach (var p in o.GetProperty("gameDates").EnumerateArray())
                {
                    if (JsonConverter.ToBoolean(p.GetProperty("selected")))
                    {
                        var gameDate = JsonConverter.ToDateTime(p.GetProperty("value"));
                        gameDates.Add((blockNumber, gameDate));
                    }
                }
            }

            await this.drawTableUseCase.UpdateGameDates(tournamentId, tennisEventId, gameDates);
        }

        /// <summary>
        /// 空きドロー一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>空きドロー一覧 JSON。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/blank_draws")]
        public async Task<string> GetBlankDraws(int tournamentId, string tennisEventId)
        {
            var json = await this.drawTableUseCase.GetBlankDrawsJson(tournamentId, tennisEventId);
            return json;
        }

        /// <summary>
        /// 同団体戦一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>同団体戦一覧 JSON。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/game_of_same_teams")]
        public async Task<string> GetGameOfSameTeams(int tournamentId, string tennisEventId)
        {
            var json = await this.drawTableUseCase.GetGameOfSameTeamsJson(tournamentId, tennisEventId);
            return json;
        }

        /// <summary>
        /// ドロー表の抽選を実行します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>抽選済みドロー表 JSON。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/execute_drawing")]
        public async Task<string> ExecuteDrawing(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "participationClassification")] int participationClassificationId)
        {
            var participationClassification = Enumeration.FromValue<ParticipationClassification>(participationClassificationId);
            if (participationClassification == ParticipationClassification.Main)
            {
                await this.drawTableUseCase.ExecuteDrawingMain(tournamentId, tennisEventId);
            }
            else
            {
                await this.drawTableUseCase.ExecuteDrawingQualifying(tournamentId, tennisEventId);
            }

            var blocksJson = await this.drawTableUseCase.GetBlocksJson(tournamentId, tennisEventId, participationClassificationId);

            return blocksJson;
        }

        /// <summary>
        /// ドロー表の抽選取り消しを実行します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>処理が正常に終了した場合ステータスコード 200。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/cancel_drawing")]
        public async Task<HttpResponseMessage> CancelDrawing(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "participationClassification")] int participationClassificationId)
        {
            await this.drawTableUseCase.CancelDrawing(tournamentId, tennisEventId, participationClassificationId);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// ドロー表を登録します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>処理が正常に終了した場合ステータスコード 200。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/update_to_draft")]
        public async Task<HttpResponseMessage> UpdateToDraft(int tournamentId, string tennisEventId)
        {
            await this.drawTableUseCase.UpdateToDraft(tournamentId, tennisEventId);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// ドロー表を公開します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <returns>処理が正常に終了した場合ステータスコード 200。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/update_to_publish")]
        public async Task<HttpResponseMessage> UpdateToPublish(int tournamentId, string tennisEventId)
        {
            await this.drawTableUseCase.UpdateToPublish(tournamentId, tennisEventId);

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        /// <summary>
        /// ドロー枠の選手区分を変更します。
        /// </summary>
        /// <param name="json">ドロー枠の変更情報。</param>
        /// <returns>ドロー表 JSON。</returns>
        [HttpPost]
        [Route("DrawTables/draw_frame_player_classification_change")]
        public async Task<string> DrawFramePlayerClassificationChange([FromBody] JsonElement json)
        {
            var tournamentId = JsonConverter.ToInt32(json.GetProperty("tournamentId"));
            var tennisEventId = JsonConverter.ToString(json.GetProperty("tennisEventId"));
            var participationClassificationId = JsonConverter.ToInt32(json.GetProperty("participationClassificationId"));
            var blockNumber = JsonConverter.ToInt32(json.GetProperty("blockNumber"));
            var drawNumber = JsonConverter.ToInt32(json.GetProperty("drawNumber"));
            var playerClassificationId = JsonConverter.ToInt32(json.GetProperty("playerClassificationId"));

            await this.drawTableUseCase.DrawFramePlayerClassificationChange(
                tournamentId,
                tennisEventId,
                blockNumber,
                drawNumber,
                playerClassificationId);
            return await this.drawTableUseCase.GetBlocksJson(tournamentId, tennisEventId, participationClassificationId);
        }

        /// <summary>
        /// ドロー枠の選手割り当てを解除します。
        /// </summary>
        /// <param name="json">ドロー枠の変更情報。</param>
        /// <returns>ドロー表 JSON。</returns>
        [HttpPost]
        [Route("DrawTables/unassign_player")]
        public async Task<string> UnassignPlayer([FromBody] JsonElement json)
        {
            var tournamentId = JsonConverter.ToInt32(json.GetProperty("tournamentId"));
            var tennisEventId = JsonConverter.ToString(json.GetProperty("tennisEventId"));
            var participationClassificationId = JsonConverter.ToInt32(json.GetProperty("participationClassificationId"));
            var blockNumber = JsonConverter.ToInt32(json.GetProperty("blockNumber"));
            var drawNumber = JsonConverter.ToInt32(json.GetProperty("drawNumber"));

            await this.drawTableUseCase.UnassignPlayer(
                tournamentId,
                tennisEventId,
                blockNumber,
                drawNumber);
            return await this.drawTableUseCase.GetBlocksJson(tournamentId, tennisEventId, participationClassificationId);
        }

        /// <summary>
        /// 選手をドローにを割り当てます。
        /// </summary>
        /// <param name="json">ドロー枠の選手割り当て情報。</param>
        /// <returns>ドロー表 JSON。</returns>
        [HttpPost]
        [Route("DrawTables/assign_players_to_draw")]
        public async Task<string> AssignPlayersToDraw([FromBody] JsonElement json)
        {
            var tournamentId = JsonConverter.ToInt32(json.GetProperty("tournamentId"));
            var tennisEventId = JsonConverter.ToString(json.GetProperty("tennisEventId"));
            var participationClassificationId = JsonConverter.ToInt32(json.GetProperty("participationClassificationId"));
            var fromEntryNumber = JsonConverter.ToInt32(json.GetProperty("fromEntryNumber"));
            var toBlockNumber = JsonConverter.ToInt32(json.GetProperty("toBlockNumber"));
            var toDrawNumber = JsonConverter.ToInt32(json.GetProperty("toDrawNumber"));

            await this.drawTableUseCase.AssignPlayersToDraw(tournamentId, tennisEventId, fromEntryNumber, toBlockNumber, toDrawNumber);
            return await this.drawTableUseCase.GetBlocksJson(tournamentId, tennisEventId, participationClassificationId);
        }

        /// <summary>
        /// ブロック名一覧を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <returns>抽選済みドロー表 JSON。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/block_names")]
        public async Task<string> GetBlockNames(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "participationClassificationId")] int? participationClassificationId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeBlocks = true,
                IncludeGames = true,
            };
            var drawTable = await this.drawTableUseCase.GetDrawTable(dto);
            var participationClassification = participationClassificationId.HasValue
                ? Enumeration.FromValue<ParticipationClassification>(participationClassificationId.Value)
                : null;
            var blocks = drawTable.Blocks.GetIniitalizedBlocks(participationClassification);
            if (!blocks.Any())
            {
                return "[]";
            }

            var json = blocks.Select(o => new
            {
                blockNumber = o.BlockNumber?.Value,
                name = o.DisplayValue,
                participationClassificationId = o.ParticipationClassification.Id,
            });

            return JsonSerializer.Serialize(json);
        }

        /// <summary>
        /// 抽選済みドロー表を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <param name="blockNumber">ブロック番号。</param>
        /// <returns>抽選済みドロー表 JSON。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/assigned_players")]
        public async Task<string> GetAssignedPlayers(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "participationClassificationId")] int participationClassificationId,
            [FromQuery(Name = "blockNumber")] int? blockNumber)
        {
            var blocksJson = await this.drawTableUseCase.GetBlocksJson(tournamentId, tennisEventId, participationClassificationId, blockNumber);

            return blocksJson;
        }

        /// <summary>
        /// 試合結果を取得します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="participationClassificationId">出場区分 ID。</param>
        /// <param name="blockNumber">ブロック番号。</param>
        /// <returns>試合結果 JSON。</returns>
        [HttpGet]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/game_results")]
        public async Task<string> GetGameResults(
            int tournamentId,
            string tennisEventId,
            [FromQuery(Name = "participationClassificationId")] int participationClassificationId,
            [FromQuery(Name = "blockNumber")] int blockNumber)
        {
            var gemeResultsJson = await this.drawTableUseCase.GetGemeResultsJson(tournamentId, tennisEventId, participationClassificationId, blockNumber);

            return gemeResultsJson;
        }

        /// <summary>
        /// 試合結果を更新します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="json">試合結果 JSON。</param>
        /// <returns>処理が正常に終了した場合ステータスコード 200。</returns>
        [HttpPost]
        [Route("DrawTables/{tournamentId}/{tennisEventId}/game_results")]
        public async Task<HttpResponseMessage> UpdateGameStatus(int tournamentId, string tennisEventId, [FromBody] JsonElement json)
        {
            var gameResult = new GameResult();
            var gameStatus = JsonConverter.ToEnumeration<GameStatus>(json.GetProperty("gameStatus"));
            var playerClassification = JsonConverter.ToEnumeration<PlayerClassification>(json.GetProperty("playerClassification"));
            var entryNumber = EntryNumber.FromValue(JsonConverter.ToNullableInt32(json.GetProperty("entryNumber")));
            var gameScore = new GameScore(JsonConverter.ToString(json.GetProperty("gameScore")));
            gameResult.UpdateGameResult(
                gameStatus,
                playerClassification,
                entryNumber,
                gameScore
            );

            var blockNumber = new BlockNumber(JsonConverter.ToInt32(json.GetProperty("blockNumber")));
            var gameNumber = new GameNumber(JsonConverter.ToInt32(json.GetProperty("gameNumber")));
            await this.drawTableUseCase.UpdateGameStatus(
                tournamentId,
                tennisEventId,
                blockNumber,
                gameNumber,
                gameResult
            );

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
        #endregion WEB API
    }
}
