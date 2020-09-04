using JuniorTennis.SeedWork;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 種目。
    /// </summary>
    public class TennisEvent : ValueObject
    {
        /// <summary>
        /// 大会 ID を取得または設定します。
        /// </summary>
        public int TournamentId { get; private set; }

        /// <summary>
        /// 種目 ID を取得します。
        /// </summary>
        /// <returns>種目 ID。</returns>
        public string TennisEventId => $"{this.Category.Id}_{this.Gender.Id}_{this.Format.Id}";

        /// <summary>
        /// カテゴリを取得または設定します。
        /// </summary>
        public Category Category { get; private set; }

        /// <summary>
        /// 性別を取得または設定します。
        /// </summary>
        public Gender Gender { get; private set; }

        /// <summary>
        /// 試合形式を取得または設定します。
        /// </summary>
        public Format Format { get; private set; }

        /// <summary>
        /// 種目がシングルスかどうか示す値を取得します。
        /// </summary>
        public bool IsSingles => this.Format == Format.Singles;

        /// <summary>
        /// 全ての種目の一覧を取得します。
        /// </summary>
        /// <returns>全ての種目の一覧。</returns>
        public static Dictionary<string, TennisEvent> GetAllEvents()
        {
            var allEvents = new Dictionary<string, TennisEvent>();
            var categories = Enumeration.GetAll<Category>().ToList();
            var genders = Enumeration.GetAll<Gender>().ToList();
            var formats = Enumeration.GetAll<Format>().ToList();
            foreach (var category in categories)
            {
                foreach (var gender in genders)
                {
                    foreach (var format in formats)
                    {
                        allEvents.Add(
                            $"{category.Id}_{gender.Id}_{format.Id}",
                            new TennisEvent(category, gender, format));
                    }
                }
            }
            return allEvents;
        }

        /// <summary>
        /// 種目 ID から種目のインスタンスを生成します。
        /// </summary>
        /// <param name="TennisEventId">種目 ID。</param>
        /// <returns>種目。</returns>
        public static TennisEvent FromId(string TennisEventId)
        {
            var ids = TennisEventId.Split('_');
            var category = Enumeration.FromValue<Category>(int.Parse(ids[0]));
            var gender = Enumeration.FromValue<Gender>(int.Parse(ids[1]));
            var format = Enumeration.FromValue<Format>(int.Parse(ids[2]));
            return new TennisEvent(category, gender, format);
        }

        /// <summary>
        /// 大会の種目に表示するための文字列を取得します。
        /// </summary>
        /// <returns></returns>
        public string DisplayTournamentEvent => $"{this.Category} {this.Gender} {this.Format}";

        /// <summary>
        /// ランキングのカテゴリに表示するための文字列を取得します。
        /// </summary>
        /// <returns></returns>
        public string DisplayRankingEvent => $"{this.Gender} {this.Format} {this.Category}";

        /// <summary>
        /// 種目の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="category">カテゴリ。</param>
        /// <param name="gender">性別。</param>
        /// <param name="format">試合形式。</param>
        public TennisEvent(Category category, Gender gender, Format format)
        {
            this.Category = category;
            this.Gender = gender;
            this.Format = format;
        }

        /// <summary>
        /// 種目の新しいインスタンスを生成します。
        /// </summary>
        private TennisEvent() { }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return (this.Category, this.Gender, this.Format);
        }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public Tournament Tournament { get; private set; }
    }
}
