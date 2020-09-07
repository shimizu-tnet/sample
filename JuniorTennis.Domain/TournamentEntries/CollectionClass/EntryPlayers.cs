using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 選手情報一覧。
    /// </summary>
    public class EntryPlayers : IList<EntryPlayer>
    {
        /// <summary>
        /// 選手情報一覧を格納します。
        /// </summary>
        private readonly List<EntryPlayer> Values;

        #region constructors
        /// <summary>
        /// 選手情報一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="entryPlayers">選手情報一覧。</param>
        public EntryPlayers(IEnumerable<EntryPlayer> entryPlayers)
        {
            this.Values = entryPlayers.ToList();
        }

        /// <summary>
        /// 選手情報一覧の新しいインスタンスを生成します。
        /// </summary>
        public EntryPlayers()
        {
            this.Values = new List<EntryPlayer>();
        }
        #endregion constructors

        #region IList
        public EntryPlayer this[int index]
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

        public void Add(EntryPlayer item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(EntryPlayer item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(EntryPlayer[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<EntryPlayer> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(EntryPlayer item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, EntryPlayer item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(EntryPlayer item)
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
