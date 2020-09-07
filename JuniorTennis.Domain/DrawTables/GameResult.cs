using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 試合結果。
    /// </summary>
    public class GameResult : EntityBase
    {
        /// <summary>
        /// 試合状況を取得します。
        /// </summary>
        public GameStatus GameStatus { get; private set; }

        /// <summary>
        /// 勝者の選手区分を取得します。
        /// </summary>
        public PlayerClassification PlayerClassificationOfWinner { get; private set; }

        /// <summary>
        /// 勝者のエントリー番号を取得します。
        /// </summary>
        public EntryNumber EntryNumberOfWinner { get; private set; }

        /// <summary>
        /// スコアを取得します。
        /// </summary>
        public GameScore GameScore { get; private set; }

        /// <summary>
        /// 試合結果の新しいインスタンスを生成します。
        /// </summary>
        public GameResult()
        {
            this.GameStatus = GameStatus.NotReadied;
        }

        /// <summary>
        /// 試合状況を準備完了に変更します。
        /// </summary>
        public void Ready()
        {
            this.GameStatus = GameStatus.None;
        }

        /// <summary>
        /// 試合状況をNotPlayed に変更します。
        /// </summary>
        public void NotPlayed()
        {
            this.GameStatus = GameStatus.NotPlayed;
        }

        /// <summary>
        /// 試合状況を準備未完了に変更します。
        /// </summary>
        public void NotReadied()
        {
            this.GameStatus = GameStatus.NotReadied;
        }

        /// <summary>
        /// 試合結果を更新します。
        /// </summary>
        /// <param name="gameStatus">試合状況。</param>
        /// <param name="playerClassificationOfWinner">勝者の選手区分。</param>
        /// <param name="entryNumberOfWinner">勝者のエントリー番号。</param>
        /// <param name="gameScore">スコア。</param>
        /// <returns>勝者が変更されている場合は true。それ以外の場合は false。</returns>
        public bool UpdateGameResult(
            GameStatus gameStatus,
            PlayerClassification playerClassificationOfWinner,
            EntryNumber entryNumberOfWinner,
            GameScore gameScore)
        {
            var breforeEntryNumberOfWinner = this.EntryNumberOfWinner;

            this.GameStatus = gameStatus;
            this.PlayerClassificationOfWinner = playerClassificationOfWinner;
            this.EntryNumberOfWinner = entryNumberOfWinner;
            this.GameScore = gameScore;

            return breforeEntryNumberOfWinner != entryNumberOfWinner;
        }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public int GameId { get; private set; }

        /// <summary>
        /// 外部キー。
        /// </summary>
        public Game Game { get; private set; }
    }
}
