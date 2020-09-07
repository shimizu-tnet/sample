using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// エントリー詳細一覧。
    /// </summary>
    public class EntryDetails : IList<EntryDetail>
    {
        /// <summary>
        /// エントリー詳細一覧を格納します。
        /// </summary>
        private readonly List<EntryDetail> Values;

        #region constructors
        /// <summary>
        /// エントリー詳細一覧の新しいインスタンスを生成します。
        /// </summary>
        public EntryDetails()
        {
            this.Values = new List<EntryDetail>();
        }

        /// <summary>
        /// エントリー詳細一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="entryDetails">エントリー詳細。</param>
        public EntryDetails(IEnumerable<EntryDetail> entryDetails)
        {
            this.Values = entryDetails.ToList();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// 指定された条件で抽出されたエントリー詳細の一覧を取得します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <param name="isSeed">シードフラグ。</param>
        /// <returns>指定された条件で抽出されたエントリー詳細の一覧。</returns>
        public Queue<EntryDetail> ExtractEntryDetailsQueue(ParticipationClassification participationClassification, bool isSeed)
        {
            var entryDetail = this.Values
                .Where(o => o.ParticipationClassification == participationClassification)
                .Where(o => o.SeedNumber.IsSeed == isSeed)
                .OrderBy(o => o.SeedNumber.Value)
                .ThenBy(o => o.TotalPoint);

            return new Queue<EntryDetail>(entryDetail);
        }

        /// <summary>
        /// 指定された条件で抽出されたエントリー詳細の一覧を取得します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>指定された条件で抽出されたエントリー詳細の一覧。</returns>
        public List<EntryDetail> ExtractEntryDetails(ParticipationClassification participationClassification)
        {
            var entryDetail = this.Values
                .Where(o => o.ParticipationClassification == participationClassification)
                .OrderBy(o => o.SeedNumber.Value)
                .ThenBy(o => o.TotalPoint);

            return entryDetail.ToList();
        }

        /// <summary>
        /// 本戦進出者をエントリー詳細一覧に追加します。
        /// </summary>
        /// <param name="entryDetails">エントリー詳細一覧。</param>
        public void AddFinalists(IEnumerable<EntryDetail> entryDetails)
        {
            if (entryDetails.Any(o => o.ParticipationClassification == ParticipationClassification.Qualifying))
            {
                throw new ArgumentException("本戦進出者のみ追加可能です。");
            }

            if (entryDetails.Any(o => !o.FromQualifying))
            {
                throw new ArgumentException("本戦進出者のみ追加可能です。");
            }

            this.Values.AddRange(entryDetails);
        }

        /// <summary>
        /// 予選からの本戦進出者を削除します。
        /// </summary>
        public void RemoveQualifyingWinners()
        {
            this.Values.RemoveAll(o => o.FromQualifying);
        }
        #endregion methods

        #region IList
        public EntryDetail this[int index]
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

        public void Add(EntryDetail item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(EntryDetail item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(EntryDetail[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<EntryDetail> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(EntryDetail item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, EntryDetail item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(EntryDetail item)
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
