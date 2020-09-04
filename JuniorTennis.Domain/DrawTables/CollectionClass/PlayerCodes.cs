using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 登録番号一覧。
    /// </summary>
    public class PlayerCodes : IList<PlayerCode>
    {
        /// <summary>
        /// 登録番号一覧を格納します。
        /// </summary>
        private readonly List<PlayerCode> Values;

        #region constructors
        /// <summary>
        /// 登録番号一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="playerCodes">登録番号一覧。</param>
        public PlayerCodes(IEnumerable<PlayerCode> playerCodes)
        {
            this.Values = playerCodes.ToList();
        }

        /// <summary>
        /// 登録番号一覧の新しいインスタンスを生成します。
        /// </summary>
        private PlayerCodes()
        {
            this.Values = new List<PlayerCode>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// 登録番号一覧を表す JSON 文字列に変換します。
        /// </summary>
        /// <returns>登録番号一覧を表す JSON 文字列。</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this.Values.Select(o => new
            {
                playerCode = o.Value
            }));
        }

        /// <summary>
        /// JSON 文字列から登録番号一覧を生成します。
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>登録番号一覧。</returns>
        public static PlayerCodes FromJson(string json)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var playerCodes = jsonElement
                .EnumerateArray()
                .Select(o => JsonConverter.ToString(o.GetProperty("playerCode")))
                .Select(o => new PlayerCode(o));
            return new PlayerCodes(playerCodes);
        }
        #endregion methods

        #region IList
        public PlayerCode this[int index]
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

        public void Add(PlayerCode item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(PlayerCode item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(PlayerCode[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<PlayerCode> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(PlayerCode item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, PlayerCode item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(PlayerCode item)
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
