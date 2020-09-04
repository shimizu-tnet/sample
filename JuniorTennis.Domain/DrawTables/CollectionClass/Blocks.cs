using JuniorTennis.Domain.Repositoies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ブロック一覧。
    /// </summary>
    public class Blocks : IList<Block>
    {
        /// <summary>
        /// ブロック一覧を格納します。
        /// </summary>
        private readonly List<Block> Values;

        #region constructors
        /// <summary>
        /// ブロックの新しいインスタンスを生成します。
        /// </summary>
        public Blocks()
        {
            this.Values = new List<Block>();
        }

        /// <summary>
        /// ブロック一覧の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="blocks">ブロック一覧。</param>
        public Blocks(IEnumerable<Block> blocks)
        {
            this.Values = blocks.ToList();
        }
        #endregion constructors

        #region methods
        /// <summary>
        /// 試合日を更新します。
        /// </summary>
        /// <param name="gameDates">試合日一覧。</param>
        public void UpdateGameDates(IEnumerable<(BlockNumber blockNumber, GameDate gameDate)> gameDates)
        {
            gameDates.ToList().ForEach(o =>
            {
                if (Values.Any(p => p.BlockNumber == o.blockNumber))
                {
                    Values.First(p => p.BlockNumber == o.blockNumber).UpdateGameDate(o.gameDate);
                }
            });
        }

        /// <summary>
        /// ブロック数の変更を適用します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <param name="drawSettings">ドロー設定。</param>
        public void ChangeNumberOfBlocks(ParticipationClassification participationClassification, DrawSettings drawSettings)
        {
            if (participationClassification == ParticipationClassification.Main)
            {
                return;
            }

            this.AddIfLenssThen(drawSettings);
            this.RemoveIfGreaterThen(drawSettings);
        }

        /// <summary>
        /// ドロー数の変更を適用します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        public void ChangeNumberOfDraws(ParticipationClassification participationClassification)
        {
            if (participationClassification == ParticipationClassification.Main)
            {
                this.InitializeMainGames();
            }
            else
            {
                this.InitializeQualifyingGames();
            }
        }

        /// <summary>
        /// 予選のブロック数を取得します。
        /// </summary>
        /// <returns>予選のブロック数。</returns>
        private int QualifyingCount()
        {
            return this.Values.Count(o => o.ParticipationClassification == ParticipationClassification.Qualifying);
        }

        /// <summary>
        /// 予選ブロック数が指定されたブロック数に満たない場合、要素を追加します。
        /// </summary>
        /// <param name="drawSettings">ドロー設定。</param>
        public void AddIfLenssThen(DrawSettings drawSettings)
        {
            while (this.QualifyingCount() < drawSettings.NumberOfBlocks.Value)
            {
                var blockNumber = this.Values.Max(o => o.BlockNumber.Value) + 1;
                var block = new Block(
                    new BlockNumber(blockNumber),
                    ParticipationClassification.Qualifying,
                    gameDate: null,
                    new Games(),
                    drawSettings
                );
                block.Games.InitializeQualifyingGames(block);

                this.Values.Add(block);
            }
        }

        /// <summary>
        /// 予選ブロック数が指定されたブロック数を超える場合、要素を末尾から順に削除します。
        /// </summary>
        /// <param name="drawSettings">ドロー設定。</param>
        public void RemoveIfGreaterThen(DrawSettings drawSettings)
        {
            while (this.QualifyingCount() > drawSettings.NumberOfBlocks.Value)
            {
                this.Pop();
            }
        }

        /// <summary>
        /// 指定した出場区分のブロックの一覧を取得します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>ブロック一覧。</returns>
        public IEnumerable<Block> GetBlocks(ParticipationClassification participationClassification)
        {
            return this.Where(o => o.ParticipationClassification == participationClassification);
        }

        /// <summary>
        /// 本戦ブロックを取得します。
        /// </summary>
        /// <returns>本戦ブロック。</returns>
        public Block GetMainBlock()
        {
            var block = this.Values.First(o => o.ParticipationClassification == ParticipationClassification.Main);
            return block;
        }

        /// <summary>
        /// 予選ブロックの一覧を取得します。
        /// </summary>
        /// <returns>予選ブロックの一覧。</returns>
        public List<Block> GetQualifyingBlocks()
        {
            var blocks = this.Values.Where(o => o.ParticipationClassification == ParticipationClassification.Qualifying);
            return blocks.ToList();
        }

        /// <summary>
        /// シードレベルで抽出した対戦者の一覧を取得します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <param name="seedLevel">シードレベル。</param>
        /// <returns>対戦者の一覧。</returns>
        public IEnumerable<Opponent> FindOpponentsBySeedLevel(ParticipationClassification participationClassification, int seedLevel)
        {
            var opponents = this.Values
                .Where(o => o.ParticipationClassification == participationClassification)
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.SeedLevel.Value == seedLevel)
                .OrderBy(o => o.AssignOrder.Value)
                .ThenBy(o => o.BlockNumber.Value);

            return opponents;
        }

        /// <summary>
        /// 対戦相手の割り当てを解除します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <param name="entryNumber">エントリー番号。</param>
        public void UnassignOpponent(ParticipationClassification participationClassification, EntryNumber entryNumber)
        {
            var game = this.Values
                .Where(o => o.ParticipationClassification == participationClassification)
                .SelectMany(o => o.Games)
                .First(o => o.Opponents.Any(p => p.EntryNumber == entryNumber));

            var opponent = this.Values
                .Where(o => o.ParticipationClassification == participationClassification)
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .FirstOrDefault(o => o.EntryNumber == entryNumber);

            if (opponent == null)
            {
                return;
            }

            game.UnassignOpponent(opponent);
        }

        /// <summary>
        /// 奇数、偶数別にランダムに並び替えた未割り当ての対戦者の一覧を取得します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>ランダムに並び替えた未割り当ての対戦者の一覧。</returns>
        public IEnumerable<Opponent> FindBlankOpponentsInRandomOrder(ParticipationClassification participationClassification)
        {
            var opponents = this.Values
                .Where(o => o.ParticipationClassification == participationClassification)
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.EntryNumber is null)
                .OrderBy(o => o.DrawNumber.Value % 2 == 0)
                .ThenBy(o => Guid.NewGuid());

            return opponents;
        }

        /// <summary>
        /// 予選の試合一覧を初期化します。
        /// </summary>
        public void InitializeQualifyingGames()
        {
            foreach (var block in this.GetQualifyingBlocks())
            {
                block.InitializeGames();
                block.Games.InitializeQualifyingGames(block);
            }
        }

        /// <summary>
        /// 本戦の試合一覧の新しいインスタンスを生成します。
        /// </summary>
        public void InitializeMainGames()
        {
            var block = this.GetMainBlock();
            var drawNumberSettings = DrawNumberSettingsRepository.FindByNumberOfDraws(block.DrawSettings.NumberOfDraws.Value);
            block.InitializeGames();
            block.Games.InitializeMainGames(block, drawNumberSettings);
        }

        private IEnumerable<(int drawNumber, int playerClassificationId, int seedLevel, int assignOrder)> GetDrawNumbers(NumberOfDraws numberOfDraws)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @$"Data\{this.GetDrawNumbersFileName(numberOfDraws)}");
            var csv = File.ReadAllLines(path, Encoding.UTF8);
            var list = csv
                .Select(o => o.Split(','))
                .Select(o => (drawNumber: int.Parse(o[0]), playerClassificationId: int.Parse(o[1]), seedLevel: int.Parse(o[2]), seedNumber: int.Parse(o[3])));

            return list;
        }

        private string GetDrawNumbersFileName(NumberOfDraws numberOfDraws)
        {
            switch (numberOfDraws.Value)
            {
                case 4:
                    return "drawNumbers_0004.csv";
                case 8:
                    return "drawNumbers_0008.csv";
                case 16:
                    return "drawNumbers_0016.csv";
                case 32:
                    return "drawNumbers_0032.csv";
                case 64:
                    return "drawNumbers_0064.csv";
                case 128:
                    return "drawNumbers_0128.csv";
                case 256:
                    return "drawNumbers_0256.csv";
                case 512:
                    return "drawNumbers_0512.csv";
                case 1024:
                    return "drawNumbers_1024.csv";
                default:
                    throw new ArgumentException("不正なドロー数です。");
            }
        }

        /// <summary>
        /// ブロックを JSON 文字列に変換します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <param name="blockNumber">ブロック番号。</param>
        /// <returns>JSON 文字列。</returns>
        public string ToJson(ParticipationClassification participationClassification, BlockNumber blockNumber)
        {
            var blocks = participationClassification == ParticipationClassification.Qualifying
                ? this.GetQualifyingBlocks().AsEnumerable()
                : new List<Block>() { this.GetMainBlock() }.AsEnumerable();

            if (blockNumber != null)
            {
                blocks = blocks.Where(o => o.BlockNumber == blockNumber);
            }

            var json = blocks.SelectMany(o => o.Games.SelectMany(p => p.Opponents.Select(q => new
            {
                blockNumber = o.BlockNumber?.Value,
                gameNumber = p.GameNumber?.Value,
                drawNumber = q.DrawNumber?.Value,
                playerClassification = q.PlayerClassification?.Id,
                seedLevel = q.SeedLevel?.Value,
                assignOrder = q.AssignOrder?.Value,
                entryNumber = q.EntryNumber?.Value,
                seedNumber = q.SeedNumber?.Value,
                playerCodes = q.PlayerCodes?.Select(o => o.Value),
                playerNames = q.PlayerNames?.Select(o => o.Value),
            })));

            return JsonSerializer.Serialize(json);
        }

        /// <summary>
        /// ブロック一覧から最後の要素を取り除き、その要素を返します。
        /// このメソッドは配列の長さを変化させます。
        /// </summary>
        /// <returns>ブロック一覧の最後の要素。</returns>
        private Block Pop()
        {
            var block = this.Values.ElementAt(this.Values.Count - 1);
            this.Values.RemoveAt(this.Values.Count - 1);
            return block;
        }

        /// <summary>
        /// 試合一覧を更新します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        public void UpdateGames(ParticipationClassification participationClassification)
        {
            foreach (var block in this.Values.Where(o => o.ParticipationClassification == participationClassification))
            {
                block.Update();
            }
        }

        /// <summary>
        /// BYE を割り当て可能な試合があるかどうかを示します。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        /// <returns>割り当て可能な場合は true。それ以外の場合は false。</returns>
        public bool HasByeAssignableGames(ParticipationClassification participationClassification)
        {
            return this.GetBlocks(participationClassification)
                .SelectMany(o => o.Games)
                .Where(o => o.CanAssign)
                .Where(o => !o.HasBye)
                .Any();
        }
        #endregion methods

        #region IList
        public Block this[int index]
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

        public void Add(Block item)
        {
            this.Values.Add(item);
        }

        public void Clear()
        {
            this.Values.Clear();
        }

        public bool Contains(Block item)
        {
            return this.Values.Contains(item);
        }

        public void CopyTo(Block[] array, int arrayIndex)
        {
            this.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Block> GetEnumerator()
        {
            return this.Values.GetEnumerator();
        }

        public int IndexOf(Block item)
        {
            return this.Values.IndexOf(item);
        }

        public void Insert(int index, Block item)
        {
            this.Values.Insert(index, item);
        }

        public bool Remove(Block item)
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
