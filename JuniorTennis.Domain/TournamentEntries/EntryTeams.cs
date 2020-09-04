using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 申込団体。
    /// </summary>
    public class EntryTeams : ValueObject
    {
        /// <summary>
        /// 申込団体を取得します。
        /// </summary>
        public Team[] Teams { get; private set; }

        /// <summary>
        /// 1 番目の申込団体を取得します。
        /// </summary>
        public Team GetEntryTeam1 => this.Teams[0];

        /// <summary>
        /// 2 番目の申込団体を取得します。
        /// </summary>
        public Team GetEntryTeam2 => this.Teams[1];

        /// <summary>
        /// 申込団体の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tennisEvent">種目。</param>
        /// <param name="teams">申込団体。</param>
        public EntryTeams(TennisEvent tennisEvent, Team[] teams)
        {
            if (tennisEvent.Format == Format.Singles && teams.Length != 1)
            {
                throw new ArgumentException($"種目[{Format.Singles.Name}]の参加団体は 1 件のみ指定可能です。", "団体");
            }

            if (tennisEvent.Format == Format.Doubles && teams.Length != 2)
            {
                throw new ArgumentException($"種目[{Format.Doubles.Name}]の参加団体は 2 件のみ指定可能です。", "団体");
            }

            this.Teams = teams;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Teams;
        }
    }
}
