using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 大会。
    /// </summary>
    public class Tournament : EntityBase
    {
        /// <summary>
        /// 大会名を取得します。
        /// </summary>
        public TournamentName TournamentName { get; private set; }

        /// <summary>
        /// 大会種別を取得します。
        /// </summary>
        public TournamentType TournamentType { get; private set; }

        /// <summary>
        /// 登録年度を取得します。
        /// </summary>
        public RegistrationYear RegistrationYear { get; private set; }

        /// <summary>
        /// 年度種別を取得します。
        /// </summary>
        public TypeOfYear TypeOfYear { get; private set; }

        /// <summary>
        /// 集計月度を取得します。
        /// </summary>
        public AggregationMonth AggregationMonth { get; private set; }

        /// <summary>
        /// 種目を格納します。
        /// </summary>
        private List<TennisEvent> tennisEvents;

        /// <summary>
        /// 種目を取得します。
        /// </summary>
        public IReadOnlyList<TennisEvent> TennisEvents => tennisEvents.AsReadOnly();

        /// <summary>
        /// 開催期間を取得します。
        /// </summary>
        public HoldingPeriod HoldingPeriod { get; private set; }

        /// <summary>
        /// 開催日を格納します。
        /// </summary>
        private List<HoldingDate> holdingDates;

        /// <summary>
        /// 開催日を取得します。
        /// </summary>
        public IReadOnlyList<HoldingDate> HoldingDates => holdingDates.AsReadOnly();

        /// <summary>
        /// 会場を取得します。
        /// </summary>
        public Venue Venue { get; private set; }

        /// <summary>
        /// 支払い方法を取得します。
        /// </summary>
        public MethodOfPayment MethodOfPayment { get; private set; }

        /// <summary>
        /// 参加費を表示するか示す値を取得または設定します。
        /// </summary>
        public bool ShowEntryFee =>
            this.MethodOfPayment.Equals(MethodOfPayment.PostPayment) || this.MethodOfPayment.Equals(MethodOfPayment.PrePayment);

        /// <summary>
        /// 参加費を取得します。
        /// </summary>
        public EntryFee EntryFee { get; private set; }

        /// <summary>
        /// 申込期間を取得します。
        /// </summary>
        public ApplicationPeriod ApplicationPeriod { get; private set; }

        /// <summary>
        /// 大会要領を取得します。
        /// </summary>
        public Outline Outline { get; private set; }

        /// <summary>
        /// 大会申込受付メールの件名を取得します。
        /// </summary>
        public string TournamentEntryReceptionMailSubject { get; private set; }

        /// <summary>
        /// 大会申込受付メールの本文を取得します。
        /// </summary>
        public string TournamentEntryReceptionMailBody { get; private set; }

        /// <summary>
        /// 大会の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournamentName">大会名。</param>
        /// <param name="tournamentType">大会種別。</param>
        /// <param name="registrationYear">登録年度。</param>
        /// <param name="typeOfYear">年度種別。</param>
        /// <param name="aggregationMonth">集計月度。</param>
        /// <param name="tennisEvents">種目一覧。</param>
        /// <param name="holdingPeriod">開催期間。</param>
        /// <param name="holdingDates">開催日一覧。</param>
        /// <param name="venue">会場。</param>
        /// <param name="entryFee">参加費。</param>
        /// <param name="methodOfPayment">支払い方法。</param>
        /// <param name="applicationPeriod">申込期間。</param>
        /// <param name="outline">大会要領。</param>
        /// <param name="tournamentEntryReceptionMailSubject">大会申込受付メールの件名。</param>
        /// <param name="tournamentEntryReceptionMailBody">大会申込受付メールの本文。</param>
        /// <param name="id">大会 ID。</param>
        public Tournament(
            TournamentName tournamentName,
            TournamentType tournamentType,
            RegistrationYear registrationYear,
            TypeOfYear typeOfYear,
            AggregationMonth aggregationMonth,
            List<TennisEvent> tennisEvents,
            HoldingPeriod holdingPeriod,
            List<HoldingDate> holdingDates,
            Venue venue, EntryFee entryFee,
            MethodOfPayment methodOfPayment,
            ApplicationPeriod applicationPeriod,
            Outline outline,
            string tournamentEntryReceptionMailSubject,
            string tournamentEntryReceptionMailBody,
            int id = 0)
        {
            this.Initialize(
                tournamentName,
                tournamentType,
                registrationYear,
                typeOfYear,
                aggregationMonth,
                tennisEvents,
                holdingPeriod,
                holdingDates,
                venue,
                entryFee,
                methodOfPayment,
                applicationPeriod,
                outline,
                tournamentEntryReceptionMailSubject,
                tournamentEntryReceptionMailBody,
                id);
        }

        /// <summary>
        /// ポイントのみの大会の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournamentName">大会名。</param>
        /// <param name="tournamentType">大会種別。</param>
        /// <param name="registrationYear">登録年度。</param>
        /// <param name="typeOfYear">年度種別。</param>
        /// <param name="aggregationMonth">集計月度。</param>
        /// <param name="tennisEvents">種目一覧。</param>
        /// <param name="id">大会 ID。</param>
        public Tournament(
            TournamentName tournamentName,
            TournamentType tournamentType,
            RegistrationYear registrationYear,
            TypeOfYear typeOfYear,
            AggregationMonth aggregationMonth,
            List<TennisEvent> tennisEvents,
            int id = 0)
        {
            this.Initialize(
                tournamentName,
                tournamentType,
                registrationYear,
                typeOfYear,
                aggregationMonth,
                tennisEvents,
                null,
                new List<HoldingDate>(),
                null,
                null,
                MethodOfPayment.NotRecieve,
                null,
                null,
                null,
                null,
                id);
        }

        /// <summary>
        /// コンストラクタのプロパティ値設定処理を共通化。
        /// </summary>
        /// <param name="tournamentName">大会名。</param>
        /// <param name="tournamentType">大会種別。</param>
        /// <param name="registrationYear">登録年度。</param>
        /// <param name="typeOfYear">年度種別。</param>
        /// <param name="aggregationMonth">集計月度。</param>
        /// <param name="tennisEvents">種目一覧。</param>
        /// <param name="holdingPeriod">開催期間。</param>
        /// <param name="holdingDates">開催日一覧。</param>
        /// <param name="venue">会場。</param>
        /// <param name="entryFee">参加費。</param>
        /// <param name="methodOfPayment">支払い方法。</param>
        /// <param name="applicationPeriod">申込期間。</param>
        /// <param name="outline">大会要領。</param>
        /// <param name="tournamentEntryReceptionMailSubject">大会申込受付メールの件名。</param>
        /// <param name="tournamentEntryReceptionMailBody">大会申込受付メールの本文。</param>
        private void Initialize(
            TournamentName tournamentName,
            TournamentType tournamentType,
            RegistrationYear registrationYear,
            TypeOfYear typeOfYear,
            AggregationMonth aggregationMonth,
            List<TennisEvent> tennisEvents,
            HoldingPeriod holdingPeriod,
            List<HoldingDate> holdingDates,
            Venue venue,
            EntryFee entryFee,
            MethodOfPayment methodOfPayment,
            ApplicationPeriod applicationPeriod,
            Outline outline,
            string tournamentEntryReceptionMailSubject,
            string tournamentEntryReceptionMailBody,
            int id)
        {
            registrationYear.EnsureValidAggregationMonth(aggregationMonth.Value);
            registrationYear.EnsureValidHoldingStartDate(holdingPeriod?.StartDate);
            holdingPeriod?.EnsureValidHoldingDates(holdingDates);
            holdingPeriod?.EnsureValidApplicationEndDate(applicationPeriod?.EndDate);

            this.TournamentName = tournamentName;
            this.TournamentType = tournamentType;
            this.RegistrationYear = registrationYear;
            this.TypeOfYear = typeOfYear;
            this.AggregationMonth = aggregationMonth;
            this.tennisEvents = tennisEvents;
            this.HoldingPeriod = holdingPeriod;
            this.holdingDates = holdingDates;
            this.Venue = venue;
            this.EntryFee = entryFee;
            this.MethodOfPayment = methodOfPayment;
            this.ApplicationPeriod = applicationPeriod;
            this.Outline = outline;
            this.TournamentEntryReceptionMailSubject = tournamentEntryReceptionMailSubject;
            this.TournamentEntryReceptionMailBody = tournamentEntryReceptionMailBody;
            this.Id = id;
        }

        /// <summary>
        /// 大会の新しいインスタンスを生成します。
        /// </summary>
        private Tournament() { }
    }
}
