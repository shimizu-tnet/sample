using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 氏名一覧。
    /// </summary>
    public class PlayerNames : IList<PlayerName>
    {
        /// <summary>
        /// 氏名一覧を格納します。
        /// </summary>
        private readonly List<PlayerName> Values;

        #region constructors
        /// <summary>
        /// 氏名一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="playerNames">氏名一覧。</param>
        public PlayerNames(IEnumerable<PlayerName> playerNames)
        {
            this.Values = playerNames.ToList();
        }

        /// <summary>
        /// 氏名一覧の新しいインスタンスを生成します。
        /// </summary>
        private PlayerNames()
        {
            this.Values = new List<PlayerName>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// 氏名一覧を表す JSON 文字列に変換します。
        /// </summary>
        /// <returns>氏名一覧を表す JSON 文字列。</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this.Values.Select(o => new
            {
                playerName = o.Value
            }));
        }

        /// <summary>
        /// JSON 文字列から氏名一覧を生成します。
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>氏名一覧。</returns>
        public static PlayerNames FromJson(string json)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var playerNames = jsonElement
                .EnumerateArray()
                .Select(o => JsonConverter.ToString(o.GetProperty("playerName")))
                .Select(o => CreatePlayerName(o));

            return new PlayerNames(playerNames);
        }

        public static PlayerName CreatePlayerName(string playerName)
        {
            var fullName = playerName.Split(" ");
            var familyName = new PlayerFamilyName(fullName[0]);
            var firstName = new PlayerFirstName(fullName[1]);

            return new PlayerName(familyName, firstName);
        }
        #endregion methods

        #region IList
        public PlayerName this[int index]
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

        public void Add(PlayerName item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(PlayerName item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(PlayerName[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<PlayerName> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(PlayerName item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, PlayerName item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(PlayerName item)
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
