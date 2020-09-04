using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 申込選手。
    /// </summary>
    public class EntryPlayers : ValueObject
    {
        /// <summary>
        /// 申込選手を取得します。
        /// </summary>
        public Player[] Players { get; private set; }

        /// <summary>
        /// 1 番目の申込選手を取得します。
        /// </summary>
        public Player GetEntryPlayer1 => this.Players[0];

        /// <summary>
        /// 2 番目の申込選手を取得します。
        /// </summary>
        public Player GetEntryPlayer2 => this.Players[1];

        /// <summary>
        /// 申込選手の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tennisEvent">種目。</param>
        /// <param name="Players">申込選手。</param>
        public EntryPlayers(TennisEvent tennisEvent, Player[] players)
        {
            if (tennisEvent.IsSingles && players.Length != 1)
            {
                throw new ArgumentException($"種目[{Format.Singles.Name}]の参加選手は 1 件のみ指定可能です。", "選手");
            }

            if (!tennisEvent.IsSingles && players.Length != 2)
            {
                throw new ArgumentException($"種目[{Format.Doubles.Name}]の参加選手は 2 件のみ指定可能です。", "選手");
            }

            this.Players = players;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Players;
        }
    }
}
