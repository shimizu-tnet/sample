using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 出場可能日一覧。
    /// </summary>
    public class CanParticipationDates : IList<CanParticipationDate>
    {
        /// <summary>
        /// 出場可能日一覧を格納します。
        /// </summary>
        private readonly List<CanParticipationDate> Values;

        #region constructors
        /// <summary>
        /// 出場可能日一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="canParticipationDates">出場可能日一覧。</param>
        public CanParticipationDates(IEnumerable<CanParticipationDate> canParticipationDates)
        {
            this.Values = canParticipationDates.ToList();
        }

        /// <summary>
        /// 出場可能日一覧の新しいインスタンスを生成します。
        /// </summary>
        private CanParticipationDates()
        {
            this.Values = new List<CanParticipationDate>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// 出場可能日一覧を表す JSON 文字列に変換します。
        /// </summary>
        /// <returns>出場可能日一覧を表す JSON 文字列。</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this.Values.Select(o => new
            {
                canParticipationDate = o.Value
            }));
        }

        /// <summary>
        /// JSON 文字列から出場可能日一覧を生成します。
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>出場可能日一覧。</returns>
        public static CanParticipationDates FromJson(string json)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var canParticipationDates = jsonElement
                .EnumerateArray()
                .Select(o => JsonConverter.ToDateTime(o.GetProperty("canParticipationDate")))
                .Select(o => new CanParticipationDate(o));
            return new CanParticipationDates(canParticipationDates);
        }
        #endregion methods

        #region IList
        public CanParticipationDate this[int index]
        {
            get
            {
                return Values[index];
            }
            set
            {
                Values[index] = value;
            }
        }

        public int Count => this.Values.Count;

        public bool IsReadOnly => throw new System.NotImplementedException();

        public void Add(CanParticipationDate item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(CanParticipationDate item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(CanParticipationDate[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<CanParticipationDate> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(CanParticipationDate item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, CanParticipationDate item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(CanParticipationDate item)
        {
            return this.Values.Remove(item);
        }

        public void RemoveAt(int index)
        {
            this.Values.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }
        #endregion IList
    }
}
