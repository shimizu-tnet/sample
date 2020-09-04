using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JuniorTennis.Domain.QueryConditions
{
    /// <summary>
    /// 検索の抽出条件一覧。
    /// </summary>
    /// <typeparam name="T">抽出条件の対象となるエンティティの型引数。</typeparam>
    public class FilterList<T> : IList<Expression<Func<T, bool>>>, IList
    {
        private List<Expression<Func<T, bool>>> filters;

        /// <summary>
        /// FilterListの新しいインスタンスを生成します。
        /// </summary>
        public FilterList()
        {
            this.filters = new List<Expression<Func<T, bool>>>();
        }

        /// <summary>
        /// 抽出条件を追加します。
        /// </summary>
        /// <param name="filter">抽出条件。</param>
        public void Add(Expression<Func<T, bool>> filter) => this.filters.Add(filter);

        /// <summary>
        /// 列挙子を取得します。
        /// </summary>
        /// <returns>列挙子。</returns>
        public IEnumerator<Expression<Func<T, bool>>> GetEnumerator() => this.filters.GetEnumerator();
        
        Expression<Func<T, bool>> IList<Expression<Func<T, bool>>>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        int ICollection<Expression<Func<T, bool>>>.Count => throw new NotImplementedException();

        int ICollection.Count => throw new NotImplementedException();

        bool ICollection<Expression<Func<T, bool>>>.IsReadOnly => throw new NotImplementedException();

        bool IList.IsReadOnly => throw new NotImplementedException();

        bool IList.IsFixedSize => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();

        void ICollection<Expression<Func<T, bool>>>.Add(Expression<Func<T, bool>> item) => throw new NotImplementedException();
        int IList.Add(object value) => throw new NotImplementedException();
        void ICollection<Expression<Func<T, bool>>>.Clear() => throw new NotImplementedException();
        void IList.Clear() => throw new NotImplementedException();
        bool ICollection<Expression<Func<T, bool>>>.Contains(Expression<Func<T, bool>> item) => throw new NotImplementedException();
        bool IList.Contains(object value) => throw new NotImplementedException();
        void ICollection<Expression<Func<T, bool>>>.CopyTo(Expression<Func<T, bool>>[] array, int arrayIndex) => throw new NotImplementedException();
        void ICollection.CopyTo(Array array, int index) => throw new NotImplementedException();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
        int IList<Expression<Func<T, bool>>>.IndexOf(Expression<Func<T, bool>> item) => throw new NotImplementedException();
        int IList.IndexOf(object value) => throw new NotImplementedException();
        void IList<Expression<Func<T, bool>>>.Insert(int index, Expression<Func<T, bool>> item) => throw new NotImplementedException();
        void IList.Insert(int index, object value) => throw new NotImplementedException();
        bool ICollection<Expression<Func<T, bool>>>.Remove(Expression<Func<T, bool>> item) => throw new NotImplementedException();
        void IList.Remove(object value) => throw new NotImplementedException();
        void IList<Expression<Func<T, bool>>>.RemoveAt(int index) => throw new NotImplementedException();
        void IList.RemoveAt(int index) => throw new NotImplementedException();
    }
}
