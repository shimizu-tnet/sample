using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 団体名一覧。
    /// </summary>
    public class TeamNames : IList<TeamName>
    {
        /// <summary>
        /// 団体名一覧を格納します。
        /// </summary>
        private readonly List<TeamName> Values;

        #region constructors
        /// <summary>
        /// 団体名一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="teamNames">団体名。</param>
        public TeamNames(IEnumerable<TeamName> teamNames)
        {
            this.Values = teamNames.ToList();
        }

        /// <summary>
        /// 団体名一覧の新しいインスタンスを生成します。
        /// </summary>
        private TeamNames()
        {
            this.Values = new List<TeamName>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// 団体名一覧を表す JSON 文字列に変換します。
        /// </summary>
        /// <returns>団体名一覧を表す JSON 文字列。</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this.Values.Select(o => new
            {
                teamName = o.Value
            }));
        }

        /// <summary>
        /// JSON 文字列から団体名一覧を生成します。
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>団体名一覧。</returns>
        public static TeamNames FromJson(string json)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var teamNames = jsonElement
                .EnumerateArray()
                .Select(o => JsonConverter.ToString(o.GetProperty("teamName")))
                .Select(o => new TeamName(o));
            return new TeamNames(teamNames);
        }
        #endregion methods

        #region IList
        public TeamName this[int index]
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

        public void Add(TeamName item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(TeamName item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(TeamName[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TeamName> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(TeamName item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, TeamName item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(TeamName item)
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
