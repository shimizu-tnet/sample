using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Ranking;
using JuniorTennis.Domain.Teams;
using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 選手情報。
    /// </summary>
    public class EntryPlayer : EntityBase
    {
        #region properties
        /// <summary>
        /// 団体登録番号を取得します。
        /// </summary>
        public TeamCode TeamCode { get; private set; }

        /// <summary>
        /// 団体名を取得します。
        /// </summary>
        public TeamName TeamName { get; private set; }

        /// <summary>
        /// 団体名略称を取得します。
        /// </summary>
        public TeamAbbreviatedName TeamAbbreviatedName { get; private set; }

        /// <summary>
        /// 登録番号を取得します。
        /// </summary>
        public PlayerCode PlayerCode { get; private set; }

        /// <summary>
        /// 氏名を取得します。
        /// </summary>
        public PlayerName PlayerName => new PlayerName(this.PlayerFamilyName, this.PlayerFirstName);

        /// <summary>
        /// 姓を取得します。
        /// </summary>
        public PlayerFamilyName PlayerFamilyName { get; private set; }

        /// <summary>
        /// 名を取得します。
        /// </summary>
        public PlayerFirstName PlayerFirstName { get; private set; }

        /// <summary>
        /// ポイントを取得します。
        /// </summary>
        public Point Point { get; private set; }
        #endregion properties

        #region constructors
        /// <summary>
        /// エントリー詳細の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="teamCode">団体登録番号。</param>
        /// <param name="teamName">団体名。</param>
        /// <param name="teamAbbreviatedName">団体名略称。</param>
        /// <param name="playerCode">登録番号。</param>
        /// <param name="playerFamilyName">姓。</param>
        /// <param name="playerFirstName">名。</param>
        /// <param name="point">ポイント。</param>
        public EntryPlayer(
            TeamCode teamCode,
            TeamName teamName,
            TeamAbbreviatedName teamAbbreviatedName,
            PlayerCode playerCode,
            PlayerFamilyName playerFamilyName,
            PlayerFirstName playerFirstName,
            Point point)
        {
            this.TeamCode = teamCode;
            this.TeamName = teamName;
            this.TeamAbbreviatedName = teamAbbreviatedName;
            this.PlayerCode = playerCode;
            this.PlayerFamilyName = playerFamilyName;
            this.PlayerFirstName = playerFirstName;
            this.Point = point;
        }

        /// <summary>
        /// 選手情報の新しいインスタンスを生成します。
        /// </summary>
        private EntryPlayer() { }
        #endregion constructors

        #region foreign key
        /// <summary>
        /// 外部キー。
        /// </summary>
        public int EntryDetailId { get; set; }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public EntryDetail EntryDetail { get; set; }
        #endregion foreign key
    }
}
