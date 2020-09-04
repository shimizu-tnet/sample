using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 団体名称一覧。
    /// </summary>
    public class TeamAbbreviatedNames : IList<TeamAbbreviatedName>
    {
        /// <summary>
        /// 団体名称一覧を格納します。
        /// </summary>
        private readonly List<TeamAbbreviatedName> Values;

        #region constructors
        /// <summary>
        /// 団体名称一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="teamAbbreviatedNames">団体名称一覧。</param>
        public TeamAbbreviatedNames(IEnumerable<TeamAbbreviatedName> teamAbbreviatedNames)
        {
            this.Values = teamAbbreviatedNames.ToList();
        }

        /// <summary>
        /// 団体名称一覧の新しいインスタンスを生成します。
        /// </summary>
        private TeamAbbreviatedNames()
        {
            this.Values = new List<TeamAbbreviatedName>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// 団体名称一覧を表す JSON 文字列に変換します。
        /// </summary>
        /// <returns>団体名称一覧を表す JSON 文字列。</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this.Values.Select(o => new
            {
                teamAbbreviatedName = o.Value
            }));
        }

        /// <summary>
        /// JSON 文字列から団体名称一覧を生成します。
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>団体名称一覧。</returns>
        public static TeamAbbreviatedNames FromJson(string json)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var teamAbbreviatedNames = jsonElement
                .EnumerateArray()
                .Select(o => JsonConverter.ToString(o.GetProperty("teamAbbreviatedName")))
                .Select(o => new TeamAbbreviatedName(o));
            return new TeamAbbreviatedNames(teamAbbreviatedNames);
        }
        #endregion methods

        #region IList
        public TeamAbbreviatedName this[int index]
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

        public void Add(TeamAbbreviatedName item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(TeamAbbreviatedName item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(TeamAbbreviatedName[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TeamAbbreviatedName> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(TeamAbbreviatedName item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, TeamAbbreviatedName item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(TeamAbbreviatedName item)
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
