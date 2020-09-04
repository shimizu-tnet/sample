using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 団体登録番号一覧。
    /// </summary>
    public class TeamCodes : IList<TeamCode>
    {
        /// <summary>
        /// 団体登録番号一覧を格納します。
        /// </summary>
        private readonly List<TeamCode> Values;

        #region constructors
        /// <summary>
        /// 団体登録番号一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="teamCodes">団体登録番号一覧。</param>
        public TeamCodes(IEnumerable<TeamCode> teamCodes)
        {
            this.Values = teamCodes.ToList();
        }

        /// <summary>
        /// 団体登録番号一覧の新しいインスタンスを生成します。
        /// </summary>
        private TeamCodes()
        {
            this.Values = new List<TeamCode>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// 団体登録番号一覧を表す JSON 文字列に変換します。
        /// </summary>
        /// <returns>団体登録番号一覧を表す JSON 文字列。</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this.Values.Select(o => new
            {
                teamCode = o.Value
            }));
        }

        /// <summary>
        /// JSON 文字列から団体登録番号一覧を生成します。
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>団体登録番号一覧。</returns>
        public static TeamCodes FromJson(string json)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var teamCodes = jsonElement
                .EnumerateArray()
                .Select(o => JsonConverter.ToString(o.GetProperty("teamCode")))
                .Select(o => new TeamCode(o));
            return new TeamCodes(teamCodes);
        }
        #endregion methods

        #region IList
        public TeamCode this[int index]
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

        public void Add(TeamCode item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(TeamCode item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(TeamCode[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TeamCode> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(TeamCode item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, TeamCode item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(TeamCode item)
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
