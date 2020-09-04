using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 対戦者一覧。
    /// </summary>
    public class Opponents : IList<Opponent>
    {
        /// <summary>
        /// 対戦者一覧を格納します。
        /// </summary>
        private readonly List<Opponent> Values;

        #region constructors
        /// <summary>
        /// 対戦者一覧の新しいインスタンスを生成します。
        /// </summary>
        public Opponents()
        {
            this.Values = new List<Opponent>();
        }
        #endregion constructors

        #region methods
        public bool HasOpponents()
        {
            return this.Values.Count != 0;
        }
        #endregion methods

        #region IList
        public Opponent this[int index]
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

        public void Add(Opponent item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(Opponent item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(Opponent[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Opponent> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(Opponent item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, Opponent item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(Opponent item)
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
