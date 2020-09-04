using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 試合一覧。
    /// </summary>
    public class Games : IList<Game>
    {
        /// <summary>
        /// シード選手の割当順の開始。
        /// </summary>
        private const int seedAssignOrderStartingNumber = 1;

        /// <summary>
        /// 一般選手の割当順の開始。
        /// </summary>
        private const int generalAssignOrderStartingNumber = 3;

        /// <summary>
        /// 試合一覧を格納します。
        /// </summary>
        private readonly List<Game> Values;

        /// <summary>
        /// 試合一覧を取得します。
        /// </summary>
        /// <returns></returns>
        public List<Game> ToList() => Values.ToList();

        /// <summary>
        /// 終了済みの試合数を取得します。
        /// </summary>
        public int DoneCount => this.Values
            .Where(o => GameStatus.Done.Equals(o.GameResult.GameStatus))
            .Count();

        #region constructors
        /// <summary>
        /// 試合の新しいインスタンスを生成します。
        /// </summary>
        public Games()
        {
            this.Values = new List<Game>();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// 試合結果を設定します。
        /// </summary>
        /// <param name="game">試合。</param>
        /// <param name="gameResult">試合結果。</param>
        public void SetGameResult(Game game, GameResult gameResult) =>
            this.Values[this.Values.FindIndex(o => o.GameNumber == game.GameNumber)]
            .SetGameResult(gameResult);

        /// <summary>
        /// 試合を削除します。
        /// </summary>
        /// <param name="games">試合一覧</param>
        public void Remove(IEnumerable<Game> games)
        {
            foreach (var item in games.ToList())
            {
                this.Values.Remove(item);
            }
        }

        /// <summary>
        /// 予選の試合一覧を初期化します。
        /// </summary>
        /// <param name="block">ブロック。</param>
        public void InitializeQualifyingGames(Block block)
        {
            this.Clear();

            var seedAssignOrder = seedAssignOrderStartingNumber;
            var generalAssignOrder = generalAssignOrderStartingNumber;
            var drawNumbers = Enumerable.Range(1, block.DrawSettings.NumberOfDraws.Value)
                .Select(o =>
                {
                    if (this.IsFirstDraw(o))
                    {
                        return (
                            drawNumber: new DrawNumber(o),
                            seedLevel: new SeedLevel(Block.FirstLevelSeed),
                            assignOrder: new AssignOrder(seedAssignOrder++)
                        );
                    }

                    if (this.IsLastDraw(o, block.DrawSettings.NumberOfDraws))
                    {
                        return (
                            drawNumber: new DrawNumber(o),
                            seedLevel: new SeedLevel(Block.SecondLevelSeed),
                            assignOrder: new AssignOrder(seedAssignOrder++)
                        );
                    }

                    return (
                        drawNumber: new DrawNumber(o),
                        seedLevel: new SeedLevel(0),
                        assignOrder: new AssignOrder(generalAssignOrder++)
                    );
                });

            var opponents = new Queue<Opponent>();
            foreach (var drawNumber in drawNumbers)
            {
                opponents.Enqueue(new Opponent(
                    drawNumber.drawNumber,
                    drawNumber.seedLevel,
                    drawNumber.assignOrder));
            }

            foreach (var gameNumber in Enumerable.Range(1, this.CalculateNumberOfGames(block.DrawSettings.NumberOfDraws)))
            {
                var game = new Game(
                    new GameNumber(gameNumber),
                    new RoundNumber(1),
                    block.DrawSettings);

                var opponent = opponents.Dequeue();
                opponent.UpdateBlockNumber(block.BlockNumber);
                opponent.UpdateGameNumber(game.GameNumber);
                game.AssignOpponent(opponent);

                opponent = opponents.Dequeue();
                opponent.UpdateBlockNumber(block.BlockNumber);
                opponent.UpdateGameNumber(game.GameNumber);
                game.AssignOpponent(opponent);

                this.Add(game);
            }
        }

        /// <summary>
        /// 本戦の試合一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="block">ブロック。</param>
        /// <param name="drawNumberSettings">ドロー枠初期設定情報一覧。</param>
        public void InitializeMainGames(Block block, IEnumerable<DrawNumberSettingsDto> drawNumberSettings)
        {
            this.Clear();

            var opponents = new Queue<Opponent>();
            foreach (var drawNumberSetting in drawNumberSettings)
            {
                opponents.Enqueue(new Opponent(
                    new DrawNumber(drawNumberSetting.DrawNumber),
                    new SeedLevel(drawNumberSetting.SeedLevel),
                    new AssignOrder(drawNumberSetting.AssignOrder)));
            }

            foreach (var gameNumber in Enumerable.Range(1, this.CalculateNumberOfGames(block.DrawSettings.NumberOfDraws)))
            {
                var game = new Game(
                    new GameNumber(gameNumber),
                    new RoundNumber(1),
                    block.DrawSettings);

                var opponent = opponents.Dequeue();
                opponent.UpdateBlockNumber(block.BlockNumber);
                opponent.UpdateGameNumber(game.GameNumber);
                game.AssignOpponent(opponent);

                opponent = opponents.Dequeue();
                opponent.UpdateBlockNumber(block.BlockNumber);
                opponent.UpdateGameNumber(game.GameNumber);
                game.AssignOpponent(opponent);

                this.Add(game);
            }
        }

        /// <summary>
        /// 現在のドロー番号が一覧の最初のドローかどうかを示します。
        /// </summary>
        /// <param name="currentDrawNumber">現在のドロー番号。</param>
        /// <returns>最初、または最後の場合 true。それ以外の場合 false。</returns>
        private bool IsFirstDraw(int currentDrawNumber)
        {
            return currentDrawNumber == 1;
        }

        /// <summary>
        /// 現在のドロー番号が一覧の最後のドローかどうかを示します。
        /// </summary>
        /// <param name="currentDrawNumber">現在のドロー番号。</param>
        /// <param name="numberOfDraws">ドロー数。</param>
        /// <returns>最初、または最後の場合 true。それ以外の場合 false。</returns>
        private bool IsLastDraw(int currentDrawNumber, NumberOfDraws numberOfDraws)
        {
            return currentDrawNumber == numberOfDraws.Value;
        }

        /// <summary>
        /// ドロー数から試合数を算出します。
        /// </summary>
        /// <param name="numberOfDraws">ドロー数。</param>
        /// <returns>試合数。</returns>
        public int CalculateNumberOfGames(NumberOfDraws numberOfDraws)
        {
            return numberOfDraws.Value / 2;
        }
        #endregion methods

        #region IList
        public Game this[int index]
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

        public void Add(Game item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(Game item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(Game[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Game> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(Game item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, Game item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(Game item)
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
