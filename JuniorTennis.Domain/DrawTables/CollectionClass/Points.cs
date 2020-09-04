using JuniorTennis.Domain.Ranking;
using JuniorTennis.Domain.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ポイント一覧。
    /// </summary>
    public class Points : IList<Point>
    {
        /// <summary>
        /// ポイント一覧を格納します。
        /// </summary>
        private readonly List<Point> Values;

        #region constructors
        /// <summary>
        /// ポイント一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="points">ポイント一覧。</param>
        public Points(IEnumerable<Point> points)
        {
            this.Values = points.ToList();
        }

        /// <summary>
        /// ポイント一覧の新しいインスタンスを生成します。
        /// </summary>
        private Points()
        {
            this.Values = new List<Point>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// ポイント一覧を表す JSON 文字列に変換します。
        /// </summary>
        /// <returns>ポイント一覧を表す JSON 文字列。</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(this.Values.Select(o => new
            {
                point = o.Value
            }));
        }

        /// <summary>
        /// JSON 文字列からポイント一覧を生成します。
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>ポイント一覧。</returns>
        public static Points FromJson(string json)
        {
            var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
            var points = jsonElement
                .EnumerateArray()
                .Select(o => JsonConverter.ToInt32(o.GetProperty("point")))
                .Select(o => new Point(o));
            return new Points(points);
        }
        #endregion methods

        #region IList
        public Point this[int index]
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

        public void Add(Point item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(Point item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(Point[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(Point item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, Point item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(Point item)
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
