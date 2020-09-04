using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Infrastructure.DataBase
{
    /// <summary>
    /// ValueComparerの生成クラス。
    /// </summary>
    public static class ValueComparerFactory
    {
        /// <summary>
        /// IList<>のValueComparerを生成します。
        /// </summary>
        /// <typeparam name="T">IList<>を継承する型。</typeparam>
        /// <typeparam name="U">IList<>のGenelic型。</typeparam>
        /// <returns>ValueComparer。</returns>
        public static ValueComparer<T> CreateListComparer<T, U>() where T : IList<U>
        {
            return new ValueComparer<T>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c);
        }
    }
}
