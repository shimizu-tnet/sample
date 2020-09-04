using JuniorTennis.SeedWork;
using System;
using System.Text.Json;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ドロー設定。
    /// </summary>
    public class DrawSettings : EntityBase
    {
        /// <summary>
        /// ブロック数を取得します。
        /// </summary>
        public NumberOfBlocks NumberOfBlocks { get; private set; }

        /// <summary>
        /// ドロー数を取得します。
        /// </summary>
        public NumberOfDraws NumberOfDraws { get; private set; }

        /// <summary>
        /// 総ドロー数を取得します。
        /// </summary>
        public int TotalNumberOfDraws => this.NumberOfBlocks.Value * this.NumberOfDraws.Value;

        /// <summary>
        /// エントリー数を取得します。
        /// </summary>
        public NumberOfEntries NumberOfEntries { get; private set; }

        /// <summary>
        /// 勝ち抜き数を取得します。
        /// </summary>
        public NumberOfWinners NumberOfWinners { get; private set; }

        /// <summary>
        /// 合計枠数を取得します。
        /// </summary>
        public int TotalNumberOfEntries => this.NumberOfEntries.Value + this.NumberOfWinners.Value;

        /// <summary>
        /// BYE 数を取得します。
        /// </summary>
        public int NumberOfByes => this.TotalNumberOfDraws - this.TotalNumberOfEntries;

        /// <summary>
        /// ブロックあたりの試合数を取得します。
        /// </summary>
        public int NumberOfGamesPerBlock => this.NumberOfDraws.Value - 1;

        /// <summary>
        /// ブロックあたりのラウンド数を取得します。
        /// </summary>
        public int NumberOfRoundsPerBlock => (int)Math.Log2(this.NumberOfDraws.Value);

        /// <summary>
        /// 大会グレードを取得します。
        /// </summary>
        public TournamentGrade TournamentGrade { get; private set; }

        /// <summary>
        /// ドロー設定の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="numberOfBlocks">ブロック数。</param>
        /// <param name="numberOfDraws">ドロー数。</param>
        /// <param name="numberOfEntries">エントリー数。</param>
        /// <param name="numberOfWinners">勝ち抜き数。</param>
        /// <param name="tournamentGrade">大会グレード。</param>
        public DrawSettings(NumberOfBlocks numberOfBlocks,
                            NumberOfDraws numberOfDraws,
                            NumberOfEntries numberOfEntries,
                            NumberOfWinners numberOfWinners,
                            TournamentGrade tournamentGrade)
        {
            this.NumberOfBlocks = numberOfBlocks;
            this.NumberOfDraws = numberOfDraws;
            this.NumberOfEntries = numberOfEntries;
            this.NumberOfWinners = numberOfWinners;
            this.TournamentGrade = tournamentGrade;
        }

        /// <summary>
        /// ドロー設定の新しいインスタンスを生成します。
        /// </summary>
        private DrawSettings() { }

        /// <summary>
        /// ドロー設定の更新を反映します。
        /// </summary>
        /// <param name="drawSettings">ドロー設定。</param>
        public void UpdateFromObject(DrawSettings drawSettings)
        {
            this.UpdateNumberOfBlocks(drawSettings.NumberOfBlocks.Value);
            this.UpdateNumberOfDraws(drawSettings.NumberOfDraws.Value);
            this.UpdateNumberOfEntries(drawSettings.NumberOfEntries.Value);
            this.UpdateNumberOfWinners(drawSettings.NumberOfWinners.Value);
            this.UpdateTournamentGrade(drawSettings.TournamentGrade.Id);
        }

        /// <summary>
        /// ブロック数を更新します。
        /// </summary>
        /// <param name="numberOfBlocks">ブロック数。</param>
        public void UpdateNumberOfBlocks(int numberOfBlocks)
        {
            this.NumberOfBlocks = new NumberOfBlocks(numberOfBlocks);
        }

        /// <summary>
        /// ドロー数を更新します。
        /// </summary>
        /// <param name="numberOfDraws">ドロー数。</param>
        public void UpdateNumberOfDraws(int numberOfDraws)
        {
            this.NumberOfDraws = new NumberOfDraws(numberOfDraws);
        }

        /// <summary>
        /// エントリー数を更新します。
        /// </summary>
        /// <param name="numberOfEntries">エントリー数。</param>
        public void UpdateNumberOfEntries(int numberOfEntries)
        {
            this.NumberOfEntries = new NumberOfEntries(numberOfEntries);
        }

        /// <summary>
        /// 勝抜き数を更新します。
        /// </summary>
        /// <param name="numberOfWinners">勝抜き数。</param>
        public void UpdateNumberOfWinners(int numberOfWinners)
        {
            this.NumberOfWinners = new NumberOfWinners(numberOfWinners);
        }

        /// <summary>
        /// 大会グレードを更新します。
        /// </summary>
        /// <param name="tournamentGradeId">大会グレード ID。</param>
        public void UpdateTournamentGrade(int tournamentGradeId)
        {
            this.TournamentGrade = Enumeration.FromValue<TournamentGrade>(tournamentGradeId);
        }

        /// <summary>
        /// JSON 文字列に変換します。
        /// </summary>
        /// <returns>JSON 文字列。</returns>
        public string ToJson()
        {
            return JsonSerializer.Serialize(new
            {
                numberOfBlocks = this.NumberOfBlocks.Value,
                numberOfDraws = this.NumberOfDraws.Value,
                totalNumberOfDraws = this.TotalNumberOfDraws,
                numberOfEntries = this.NumberOfEntries.Value,
                numberOfWinners = this.NumberOfWinners.Value,
                totalNumberOfEntries = this.TotalNumberOfEntries,
                numberOfByes = this.NumberOfByes,
                numberOfGamesPerBlock = this.NumberOfGamesPerBlock,
                numberOfRoundsPerBlock = this.NumberOfRoundsPerBlock,
                tournamentGrade = new
                {
                    id = this.TournamentGrade.Id,
                    name = this.TournamentGrade.Name,
                },
            });
        }
    }
}
