using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ブロック。
    /// </summary>
    public class Block : EntityBase
    {
        /// <summary>
        /// 固定シード選手（1 シード）のシードレベル。
        /// </summary>
        public const int FirstLevelSeed = 1;

        /// <summary>
        /// 固定シード選手（2 シード）のシードレベル。
        /// </summary>
        public const int SecondLevelSeed = 2;

        /// <summary>
        /// シード選手（優先度：低）のシードレベル。
        /// </summary>
        public const int LowPriorityLevelSeed = 6;

        #region properties
        /// <summary>
        /// ドロー表 ID を取得します。
        /// </summary>
        public int DrawTableId { get; private set; }

        /// <summary>
        /// ブロック番号を取得します。
        /// </summary>
        public BlockNumber BlockNumber { get; private set; }

        /// <summary>
        /// 出場区分を取得します。
        /// </summary>
        public ParticipationClassification ParticipationClassification { get; private set; }

        /// <summary>
        /// 本戦かどうかを示します。
        /// </summary>
        public bool IsMain => this.ParticipationClassification == ParticipationClassification.Main;

        /// <summary>
        /// 試合日を取得します。
        /// </summary>
        public GameDate GameDate { get; private set; }

        /// <summary>
        /// 試合一覧を取得します。
        /// </summary>
        public Games Games { get; private set; }

        /// <summary>
        /// 初期化済みかどうかを示します。
        /// </summary>
        public bool Initialized => (this.Games?.Count() ?? 0) != 0;

        /// <summary>
        /// ブロックの画面表示用の文字列を取得します。
        /// </summary>
        public string DisplayValue => this.ParticipationClassification == ParticipationClassification.Main
            ? this.ParticipationClassification.Name
            : $"{this.ParticipationClassification.Name}ブロック{this.BlockNumber.Value}";

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
        /// ブロックの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="blockNumber">ブロック番号。</param>
        /// <param name="participationClassification">出場区分。</param>
        /// <param name="gameDate">試合日。</param>
        /// <param name="game">試合一覧。</param>
        /// <param name="drawSettings">ドロー設定。</param>
        public Block(
            BlockNumber blockNumber,
            ParticipationClassification participationClassification,
            GameDate gameDate,
            DrawSettings drawSettings)
        {
            this.BlockNumber = blockNumber;
            this.ParticipationClassification = participationClassification;
            this.GameDate = gameDate;
            this.Games = null;
            this.DrawSettings = drawSettings;
        }

        /// <summary>
        /// ブロックの新しいインスタンスを生成します。
        /// </summary>
        private Block() { }
        #endregion constructors

        #region methods
        /// <summary>
        /// 試合日を更新します。
        /// </summary>
        /// <param name="gameDate">試合日。</param>
        public void UpdateGameDate(GameDate gameDate)
        {
            this.GameDate = gameDate;
        }

        /// <summary>
        /// ブロック勝者のエントリー番号を取得します。
        /// </summary>
        /// <param name="numberOfGamesPerBlock">ブロックあたりの試合数。</param>
        /// <returns>エントリー番号。</returns>
        public EntryNumber GetEntryNumberOfWinner(int numberOfGamesPerBlock)
        {
            return this.Games
                .Where(o => o.GameNumber.Value == numberOfGamesPerBlock)
                .Where(o => o.IsDone)
                .Select(o => o.GameResult)
                .Select(o => o.EntryNumberOfWinner)
                .FirstOrDefault();
        }

        /// <summary>
        /// 試合一覧を更新します。
        /// </summary>
        /// <param name="game">試合。</param>
        /// <param name="isWinnerChanged">勝者変更フラグ。</param>
        public void Update(Game game, bool isWinnerChanged)
        {
            var firstGameNumber = 0;
            var numberOfGames = 0;

            foreach (var roundNumber in Enumerable.Range(1, this.DrawSettings.NumberOfRoundsPerBlock))
            {
                var numberOfGamesPerRound = this.CalculateNumberOfGamesPerRound(roundNumber);
                firstGameNumber = numberOfGames + 1;
                numberOfGames += numberOfGamesPerRound;

                var gameNumberPerRound = 0;
                foreach (var gameNumber in Enumerable.Range(firstGameNumber, numberOfGamesPerRound))
                {
                    gameNumberPerRound++;
                    if (!(game.RoundNumber.Value == roundNumber && game.GameNumber.Value == gameNumber))
                    {
                        continue;
                    }

                    var nextRoundNumber = new RoundNumber(roundNumber + 1);
                    if (nextRoundNumber.Value > this.DrawSettings.NumberOfRoundsPerBlock)
                    {
                        return;
                    }

                    var nextGameNumber = this.CalculateNextGameNumber(numberOfGames, gameNumberPerRound);
                    var nextGame = this.AddNextGameIfNotExists(nextRoundNumber, nextGameNumber);

                    if (!isWinnerChanged)
                    {
                        return;
                    }

                    nextGame.UnassignOpponent(game.GameNumber);

                    var winner = game.GetWinner();
                    if (winner == null)
                    {
                        return;
                    }

                    if (winner.PlayerClassification == PlayerClassification.Bye)
                    {
                        winner.UpdateBlockNumber(this.BlockNumber);
                    }

                    winner.UpdateGameNumber(nextGameNumber);
                    nextGame.UpdateOpponent(
                            winner.PlayerClassification,
                            winner.EntryNumber,
                            winner.SeedNumber,
                            winner.TeamCodes,
                            winner.TeamAbbreviatedNames,
                            winner.PlayerCodes,
                            winner.PlayerNames,
                            game.GameNumber,
                            game.GameNumber.NextDrawNumber
                        );

                    if (nextGame.Opponents.Count(o => o.IsAssigned) == Game.maxOpponentsCount)
                    {
                        nextGame.GameResult.Ready();
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// 試合一覧を更新します。
        /// </summary>
        public void Update()
        {
            var gameNumbersMap = new Dictionary<GameNumber, GameNumber>();
            var firstGameNumber = 0;
            var numberOfGames = 0;

            foreach (var roundNumber in Enumerable.Range(1, this.DrawSettings.NumberOfRoundsPerBlock))
            {
                var numberOfGamesPerRound = this.CalculateNumberOfGamesPerRound(roundNumber);
                firstGameNumber = numberOfGames + 1;
                numberOfGames += numberOfGamesPerRound;

                var gameNumberPerRound = 0;
                foreach (var gameNumber in Enumerable.Range(firstGameNumber, numberOfGamesPerRound))
                {
                    gameNumberPerRound++;

                    var nextRoundNumber = new RoundNumber(roundNumber + 1);
                    if (nextRoundNumber.Value > this.DrawSettings.NumberOfRoundsPerBlock)
                    {
                        return;
                    }

                    var game = this.Games.FirstOrDefault(o => o.RoundNumber.Value == roundNumber && o.GameNumber.Value == gameNumber);
                    if (game == null)
                    {
                        continue;
                    }

                    if (!(game.IsWalkover || game.IsNotPlayed))
                    {
                        // Walkover でも NotPlayed でもない場合は、
                        // 自動進出させる必要がないので次の週へ
                        continue;
                    }

                    var nextGameNumber = this.CalculateNextGameNumber(numberOfGames, gameNumberPerRound);
                    var nextGame = this.AddNextGameIfNotExists(nextRoundNumber, nextGameNumber);
                    if (nextGame.IsDone)
                    {
                        continue;
                    }

                    nextGame.UnassignOpponent(game.GameNumber);

                    var winner = game.GetWinner();
                    if (winner == null)
                    {
                        continue;
                    }

                    winner.UpdateBlockNumber(this.BlockNumber);
                    winner.UpdateGameNumber(nextGameNumber);
                    nextGame.UpdateOpponent(
                            winner.PlayerClassification,
                            winner.EntryNumber,
                            winner.SeedNumber,
                            winner.TeamCodes,
                            winner.TeamAbbreviatedNames,
                            winner.PlayerCodes,
                            winner.PlayerNames,
                            game.GameNumber,
                            game.GameNumber.NextDrawNumber
                        );

                    if (nextGame.Opponents.Count(o => o.IsAssigned) == Game.maxOpponentsCount)
                    {
                        nextGame.GameResult.Ready();
                    }

                    if (nextGame.Opponents.Count(o => o.IsBye) == Game.maxOpponentsCount)
                    {
                        nextGame.GameResult.NotPlayed();
                    }

                    var gameStatus = game.IsWalkover
                        ? GameStatus.Walkover
                        : GameStatus.NotPlayed;
                    var gameScore = game.IsWalkover
                        ? new GameScore(GameStatus.Walkover.Name)
                        : new GameScore(GameStatus.NotPlayed.Name);
                    game.GameResult.UpdateGameResult(
                        gameStatus,
                        winner.PlayerClassification,
                        winner.EntryNumber,
                        gameScore
                    );
                }
            }
        }

        /// <summary>
        /// 試合一覧を初期化します。
        /// </summary>
        public void InitializeGames()
        {
            this.Games = new Games();
        }

        private int CalculateNumberOfGamesPerRound(int roundNumber)
        {
            return this.DrawSettings.NumberOfDraws.Value / (int)Math.Pow(2, roundNumber);
        }

        private GameNumber CalculateNextGameNumber(int numberOfGames, int gameNumberPerRound)
        {
            return new GameNumber(numberOfGames + (int)Math.Ceiling(gameNumberPerRound / 2.0));
        }

        private Game AddNextGameIfNotExists(RoundNumber roundNumber, GameNumber nextGameNumber)
        {
            var nextGame = this.Games
                .Where(o => o.GameNumber == nextGameNumber)
                .FirstOrDefault();

            if (nextGame == null)
            {
                nextGame = new Game(nextGameNumber, roundNumber, this.DrawSettings);

                var opponent = new Opponent(drawNumber: new DrawNumber(1), seedLevel: null, assignOrder: null);
                opponent.UpdateBlockNumber(this.BlockNumber);
                opponent.UpdateGameNumber(nextGameNumber);
                nextGame.AssignOpponent(opponent);

                opponent = new Opponent(drawNumber: new DrawNumber(2), seedLevel: null, assignOrder: null);
                opponent.UpdateBlockNumber(this.BlockNumber);
                opponent.UpdateGameNumber(nextGameNumber);
                nextGame.AssignOpponent(opponent);

                this.Games.Add(nextGame);
            }

            return nextGame;
        }
        #endregion methods

        /// <summary>
        /// 外部キー。
        /// </summary>
        public DrawTable DrawTable { get; private set; }
    }
}
