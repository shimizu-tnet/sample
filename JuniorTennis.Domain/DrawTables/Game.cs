using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.SeedWork;
using System;
using System.Linq;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 試合。
    /// </summary>
    public class Game : EntityBase
    {
        #region properties
        /// <summary>
        /// 割り当て可能な対戦者の件数の上限。
        /// </summary>
        public const int maxOpponentsCount = 2;

        /// <summary>
        /// 試合番号を取得します。
        /// </summary>
        public GameNumber GameNumber { get; private set; }

        /// <summary>
        /// ラウンド番号を取得します。
        /// </summary>
        public RoundNumber RoundNumber { get; private set; }

        /// <summary>
        /// 対戦者の一覧を取得します。
        /// </summary>
        public Opponents Opponents { get; private set; }

        /// <summary>
        /// 試合結果を取得します。
        /// </summary>
        public GameResult GameResult { get; private set; }

        /// <summary>
        /// 試合が終了済みかどうか示します。
        /// </summary>
        public bool IsDone => this.GameResult.GameStatus != GameStatus.None && this.GameResult.GameStatus != GameStatus.NotReadied;

        /// <summary>
        /// 当該試合が準備完了状態かどうかを示します。
        /// </summary>
        public bool IsReadied => this.Opponents.Count(o => o.IsAssigned) == maxOpponentsCount;

        /// <summary>
        /// 当該試合が NotPlayed かどうか示す値を取得します。
        /// </summary>
        public bool IsNotPlayed => this.GameResult.GameStatus == GameStatus.NotPlayed;

        /// <summary>
        /// 当該試合が不戦勝となる試合かどうか示す値を取得します。
        /// </summary>
        public bool IsWalkover => this.GameResult.GameStatus == GameStatus.Walkover;

        /// <summary>
        /// 選手を割り当て可能かどうかを示します。
        /// </summary>
        public bool CanAssign => this.Opponents.Any(o => o.EntryNumber == null);

        /// <summary>
        /// 当該試合に BYE が割り当てられているかどうかを示します。
        /// </summary>
        public bool HasBye => this.Opponents.Any(o => o.PlayerClassification == PlayerClassification.Bye);

        /// <summary>
        /// ドロー設定 ID。
        /// </summary>
        public int DrawSettingsId { get; set; }

        /// <summary>
        /// ドロー設定。
        /// </summary>
        public DrawSettings DrawSettings { get; set; }
        #endregion properties

        #region constructors
        /// <summary>
        /// 試合の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="gameNumber">試合番号。</param>
        /// <param name="roundNumber">ラウンド番号。</param>
        /// <param name="drawSettings">ドロー設定。</param>
        public Game(GameNumber gameNumber, RoundNumber roundNumber, DrawSettings drawSettings)
        {
            this.GameNumber = gameNumber;
            this.RoundNumber = roundNumber;
            this.DrawSettings = drawSettings;
            this.Opponents = new Opponents();
            this.GameResult = new GameResult();
        }

        /// <summary>
        /// 試合の新しいインスタンスを生成します。
        /// </summary>
        private Game() { }
        #endregion constructors

        #region methods
        /// <summary>
        /// 試合結果を設定します。
        /// </summary>
        /// <param name="gameResult">試合結果。</param>
        public void SetGameResult(GameResult gameResult) => this.GameResult = gameResult;

        /// <summary>
        /// 対戦者を割り当てます。
        /// </summary>
        /// <param name="opponent">対戦者。</param>
        public void AssignOpponent(Opponent opponent)
        {
            if (this.Opponents.Count >= maxOpponentsCount)
            {
                throw new InvalidOperationException("この試合にこれ以上の選手を割り当てられません。");
            }

            this.Opponents.Add(opponent);
        }

        /// <summary>
        /// 対戦者を更新します。
        /// </summary>
        /// <param name="playerClassification">選手区分。</param>
        /// <param name="entryNumber">エントリー番号。</param>
        /// <param name="seedNumber">シード番号。</param>
        /// <param name="teamCodes">団体登録番号一覧。</param>
        /// <param name="teamAbbreviatedNames">団体名略称一覧。</param>
        /// <param name="playerCodes">登録番号一覧。</param>
        /// <param name="playerNames">氏名一覧。</param>
        /// <param name="fromGameNumber">一つ前の試合番号。</param>
        /// <param name="drawNumber">割当先のドロー番号。</param>
        public void UpdateOpponent(
            PlayerClassification playerClassification,
            EntryNumber entryNumber,
            SeedNumber seedNumber,
            TeamCodes teamCodes,
            TeamAbbreviatedNames teamAbbreviatedNames,
            PlayerCodes playerCodes,
            PlayerNames playerNames,
            GameNumber fromGameNumber = null,
            DrawNumber drawNumber = null)
        {
            if (drawNumber == null)
            {
                this.Opponents
                    .First(o => !o.IsAssigned)
                    .UpdateOpponent(
                        playerClassification,
                        entryNumber,
                        seedNumber,
                        teamCodes,
                        teamAbbreviatedNames,
                        playerCodes,
                        playerNames,
                        fromGameNumber
                    );
            }
            else
            {
                this.Opponents
                    .First(o => o.DrawNumber == drawNumber)
                    .UpdateOpponent(
                        playerClassification,
                        entryNumber,
                        seedNumber,
                        teamCodes,
                        teamAbbreviatedNames,
                        playerCodes,
                        playerNames,
                        fromGameNumber
                    );
            }

            if (this.Opponents.Count(o => o.IsAssigned) == maxOpponentsCount)
            {
                this.GameResult.Ready();
            }
        }

        /// <summary>
        /// 対戦者の割り当てを解除します。
        /// </summary>
        /// <param name="opponent">対戦者。</param>
        public void UnassignOpponent(Opponent opponent)
        {
            if (this.IsDone)
            {
                throw new InvalidOperationException("すでに試合結果が入力されているため、選手の割り当てを解除できません。");
            }

            var removeOpponent = this.Opponents.FirstOrDefault(o => o.DrawNumber == opponent.DrawNumber);
            removeOpponent.UpdateOpponent(
                playerClassification: null,
                entryNumber: null,
                seedNumber: null,
                teamCodes: null,
                teamAbbreviatedNames: null,
                playerCodes: null,
                playerNames: null,
                fromGameNumber: null);

            if (this.Opponents.Count(o => o.IsAssigned) < maxOpponentsCount)
            {
                this.GameResult.NotReadied();
            }
        }

        /// <summary>
        /// 対戦者の割り当てを解除します。
        /// </summary>
        /// <param name="gameNumber">試合番号。</param>
        public void UnassignOpponent(GameNumber gameNumber)
        {
            if (this.IsDone)
            {
                throw new InvalidOperationException("すでに試合結果が入力されているため、選手の割り当てを解除できません。");
            }

            var removeOpponent = this.Opponents.FirstOrDefault(o => o.FromGameNumber == gameNumber);
            removeOpponent?.UpdateOpponent(
                playerClassification: null,
                entryNumber: null,
                seedNumber: null,
                teamCodes: null,
                teamAbbreviatedNames: null,
                playerCodes: null,
                playerNames: null,
                fromGameNumber: null);

            if (this.Opponents.Count(o => o.IsAssigned) < maxOpponentsCount)
            {
                this.GameResult.NotReadied();
            }
        }

        /// <summary>
        /// 勝者を取得します。
        /// 試合が行われていなくても、対戦相手が BYE の場合は勝者とみなします。
        /// Not Played の場合は BYE を勝者とします。
        /// </summary>
        /// <returns>勝者。</returns>
        public Opponent GetWinner()
        {
            if (this.GameResult.GameStatus == GameStatus.NotPlayed)
            {
                return Opponent.CreateBye();
            }

            if (this.Opponents.Any(o => o.IsBye))
            {
                return this.Opponents.FirstOrDefault(o => !o.IsBye)?.Clone();
            }

            if (!this.IsDone)
            {
                return null;
            }

            var opponent = this.Opponents
                .Where(o => o.EntryNumber == this.GameResult.EntryNumberOfWinner)
                .FirstOrDefault();

            return opponent?.Clone();
        }
        #endregion methods

        /// <summary>
        /// 外部キー。
        /// </summary>
        public int BlockId { get; private set; }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public Block Block { get; private set; }
    }
}
