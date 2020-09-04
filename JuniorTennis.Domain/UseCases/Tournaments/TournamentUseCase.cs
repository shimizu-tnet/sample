using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Tournaments
{
    /// <summary>
    /// 大会管理。
    /// </summary>
    public class TournamentUseCase : ITournamentUseCase
    {
        private readonly ITournamentRepository repository;
        public TournamentUseCase(ITournamentRepository repository) => this.repository = repository;

        /// <summary>
        /// 大会一覧を取得します。
        /// </summary>
        /// <returns>大会一覧。</returns>
        public async Task<IEnumerable<Tournament>> GetTournaments() => await repository.Find();

        public async Task<Tournament> GetTournament(int id) => await repository.FindById(id);

        /// <summary>
        /// 大会を登録します。
        /// </summary>
        /// <param name="dto">大会登録用 DTO。</param>
        /// <returns>大会。</returns>
        public async Task<Tournament> RegisterTournament(RegisterTournamentDto dto)
        {
            var tournament = Enumeration.FromValue<TournamentType>(dto.TournamentType) == TournamentType.WithDraw
                ? new Tournament(
                    new TournamentName(dto.TournamentName),
                    Enumeration.FromValue<TournamentType>(dto.TournamentType),
                    new RegistrationYear(dto.RegistrationYear),
                    Enumeration.FromValue<TypeOfYear>(dto.TypeOfYear),
                    new AggregationMonth(dto.AggregationMonth),
                    dto.TennisEvents.Select(o => new TennisEvent(
                        Enumeration.FromValue<Category>(o.Item1),
                        Enumeration.FromValue<Gender>(o.Item2),
                        Enumeration.FromValue<Format>(o.Item3))
                    ).ToList(),
                    new HoldingPeriod(dto.HoldingStartDate, dto.HoldingEndDate),
                    dto.HoldingDates.Select(o => new HoldingDate(o)).ToList(),
                    new Venue(dto.Venue),
                    new EntryFee(dto.EntryFee),
                    Enumeration.FromValue<MethodOfPayment>(dto.MethodOfPayment),
                    new ApplicationPeriod(dto.ApplicationStartDate, dto.ApplicationEndDate),
                    new Outline(dto.Outline),
                    dto.TournamentEntryReceptionMailSubject,
                    dto.TournamentEntryReceptionMailBody)

                : new Tournament(
                    new TournamentName(dto.TournamentName),
                    Enumeration.FromValue<TournamentType>(dto.TournamentType),
                    new RegistrationYear(dto.RegistrationYear),
                    Enumeration.FromValue<TypeOfYear>(dto.TypeOfYear),
                    new AggregationMonth(dto.AggregationMonth),
                    dto.TennisEvents.Select(o => new TennisEvent(
                        Enumeration.FromValue<Category>(o.Item1),
                        Enumeration.FromValue<Gender>(o.Item2),
                        Enumeration.FromValue<Format>(o.Item3))
                    ).ToList()
                );

            return await this.repository.Add(tournament);
        }

        /// <summary>
        /// 大会を更新します。
        /// </summary>
        /// <param name="dto">大会更新用 DTO。</param>
        /// <returns>大会。</returns>
        public async Task<Tournament> UpdateTournament(UpdateTournamentDto dto)
        {
            var tournament = Enumeration.FromValue<TournamentType>(dto.TournamentType) == TournamentType.WithDraw
                ? new Tournament(
                    new TournamentName(dto.TournamentName),
                    Enumeration.FromValue<TournamentType>(dto.TournamentType),
                    new RegistrationYear(dto.RegistrationYear),
                    Enumeration.FromValue<TypeOfYear>(dto.TypeOfYear),
                    new AggregationMonth(dto.AggregationMonth),
                    dto.TennisEvents.Select(o => new TennisEvent(
                        Enumeration.FromValue<Category>(o.Item1),
                        Enumeration.FromValue<Gender>(o.Item2),
                        Enumeration.FromValue<Format>(o.Item3))
                    ).ToList(),
                    new HoldingPeriod(dto.HoldingStartDate, dto.HoldingEndDate),
                    dto.HoldingDates.Select(o => new HoldingDate(o)).ToList(),
                    new Venue(dto.Venue),
                    new EntryFee(dto.EntryFee),
                    Enumeration.FromValue<MethodOfPayment>(dto.MethodOfPayment),
                    new ApplicationPeriod(dto.ApplicationStartDate, dto.ApplicationEndDate),
                    new Outline(dto.Outline),
                    dto.TournamentEntryReceptionMailSubject,
                    dto.TournamentEntryReceptionMailBody,
                    dto.TournamentId)

                : new Tournament(
                    new TournamentName(dto.TournamentName),
                    Enumeration.FromValue<TournamentType>(dto.TournamentType),
                    new RegistrationYear(dto.RegistrationYear),
                    Enumeration.FromValue<TypeOfYear>(dto.TypeOfYear),
                    new AggregationMonth(dto.AggregationMonth),
                    dto.TennisEvents.Select(o => new TennisEvent(
                        Enumeration.FromValue<Category>(o.Item1),
                        Enumeration.FromValue<Gender>(o.Item2),
                        Enumeration.FromValue<Format>(o.Item3))
                    ).ToList(),
                    dto.TournamentId
                );

            return await this.repository.Update(tournament);
        }

        /// <summary>
        /// 大会を削除します。
        /// </summary>
        /// <param name="id">大会 ID。</param>
        public async Task DeleteTournament(int id)
        {
            var tournament = await repository.FindById(id);
            await this.repository.Delete(tournament);
        }

        /// <summary>
        /// 開催日の一覧を作成します。
        /// </summary>
        /// <param name="holdingStartDate">開催期間の開始日。</param>
        /// <param name="holdingEndDate">開催期間の終了日。</param>
        /// <returns>開催日の一覧。</returns>
        public List<JsonHoldingDate> CreateHoldingDates(DateTime holdingStartDate, DateTime holdingEndDate)
        {
            if (holdingStartDate >= holdingEndDate)
            {
                return new List<JsonHoldingDate>();
            }

            var holdingBeforeDate = holdingStartDate;
            var holdingBeforeDates = new List<JsonHoldingDate>();
            while (holdingBeforeDate.DayOfWeek != DayOfWeek.Monday)
            {
                holdingBeforeDate = holdingBeforeDate.AddDays(-1);
                holdingBeforeDates.Add(new JsonHoldingDate(holdingBeforeDate, true));
            }
            holdingBeforeDates.Reverse();

            var days = (int)(holdingEndDate - holdingStartDate).TotalDays + 1;
            var holdingDates = Enumerable
                .Range(0, days)
                .Select(o => holdingStartDate.AddDays(o))
                .Select(o => new JsonHoldingDate(o, false))
                .ToList();

            var holdingAgterDate = holdingEndDate;
            var holdingAfterDates = new List<JsonHoldingDate>();
            while (holdingAgterDate.DayOfWeek != DayOfWeek.Sunday)
            {
                holdingAgterDate = holdingAgterDate.AddDays(1);
                holdingAfterDates.Add(new JsonHoldingDate(holdingAgterDate, true));
            }

            return Enumerable.Concat(Enumerable.Concat(holdingBeforeDates, holdingDates), holdingAfterDates).ToList();
        }

        public Dictionary<string, string> GetTournamentEntryReceptionMailBodies()
        {
            var mailBodys = new Dictionary<string, string>();
            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/tournamentEntryReception_mail.json"));
                var json = sr.ReadToEnd();
                json = json.Replace("\r\n", "");
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                mailBodys.Add(
                    "PrePayment",
                    jsonElement.GetProperty("PrePayment").GetProperty("body").ToString().Replace("<br>", "\r\n"));
                mailBodys.Add(
                     "PostPayment",
                     jsonElement.GetProperty("PostPayment").GetProperty("body").ToString().Replace("<br>", "\r\n"));
                mailBodys.Add(
                    "NotRecieve",
                    jsonElement.GetProperty("NotRecieve").GetProperty("body").ToString().Replace("<br>", "\r\n"));
                mailBodys.Add(
                    "Other",
                    jsonElement.GetProperty("Other").GetProperty("body").ToString().Replace("<br>", "\r\n"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return mailBodys;
        }
    }

    /// <summary>
    /// 開催日を表す API 返却用のクラス。
    /// </summary>
    public class JsonHoldingDate
    {
        /// <summary>
        /// 開催日を取得します。
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// [yyyy/MM/dd] 形式の値を取得します。
        /// </summary>
        public string Value => $"{this.Date:yyyy/MM/dd}";

        /// <summary>
        /// [M/d] 形式の値を取得します。
        /// </summary>
        public string Text => $"{this.Date:M/d}";

        /// <summary>
        /// エレメントが有効かどうか示す値を取得します。
        /// </summary>
        public bool Disabled { get; private set; }

        /// <summary>
        /// 開催日を表す API 返却用のクラスの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="holdingDate">開催日。</param>
        public JsonHoldingDate(DateTime holdingDate)
        {
            this.Date = holdingDate;
            this.Disabled = false;
        }

        /// <summary>
        /// 開催日を表す API 返却用のクラスの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="holdingDate">開催日。</param>
        /// <param name="disabled">無効フラグ。</param>
        public JsonHoldingDate(DateTime holdingDate, bool disabled)
        {
            this.Date = holdingDate;
            this.Disabled = disabled;
        }

        /// <summary>
        /// 開催日インスタンスを返却します。
        /// </summary>
        /// <returns>開催日インスタンス。</returns>
        public HoldingDate ToHoldingDate()
        {
            return new HoldingDate(this.Date);
        }
    }
}
