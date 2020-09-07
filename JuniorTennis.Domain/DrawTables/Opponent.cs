using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.SeedWork;
using System.Linq;

namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 対戦者。
    /// </summary>
    public class Opponent : EntityBase
    {
        #region properties
        /// <summary>
        /// ブロック番号を取得します。
        /// </summary>
        public BlockNumber BlockNumber { get; private set; }

        /// <summary>
        /// 試合番号を取得します。
        /// </summary>
        public GameNumber GameNumber { get; private set; }

        /// <summary>
        /// ドロー番号を取得します。
        /// </summary>
        public DrawNumber DrawNumber { get; private set; }

        /// <summary>
        /// <para>シードレベルを取得します。</para>
        /// <para>0 : シード以外。</para>
        /// <para>1 : 第 1 シード。</para>
        /// <para>2 : 第 2 シード。</para>
        /// <para>3 : 第 3, 4 シード。</para>
        /// <para>4 : 第 5, 6, 7, 8 シード。</para>
        /// <para>5 : 第 9, 10, 11, 12 シード。</para>
        /// <para>6 : 第 13, 14, 15, 16 シード。</para>
        /// </summary>
        public SeedLevel SeedLevel { get; private set; }

        /// <summary>
        /// 割当順を取得します。
        /// </summary>
        public AssignOrder AssignOrder { get; private set; }

        /// <summary>
        /// 対戦枠の選手区分を取得します。
        /// </summary>
        public PlayerClassification FramePlayerClassification { get; private set; }

        /// <summary>
        /// 手動で変更された対戦枠かどうかを示します。
        /// </summary>
        public bool IsManuallySettingFrame { get; private set; }

        /// <summary>
        /// 手動で割り当てられたかどうかを示します。
        /// </summary>
        public bool IsManuallyAssigned { get; private set; }

        /// <summary>
        /// 選手区分を取得します。
        /// </summary>
        public PlayerClassification PlayerClassification { get; private set; }

        /// <summary>
        /// エントリー番号を取得します。
        /// </summary>
        public EntryNumber EntryNumber { get; private set; }

        /// <summary>
        /// シード番号を取得します。
        /// </summary>
        public SeedNumber SeedNumber { get; private set; }

        /// <summary>
        /// 団体登録番号一覧を取得します。
        /// </summary>
        public TeamCodes TeamCodes { get; private set; }

        /// <summary>
        /// 団体名略称一覧を取得します。
        /// </summary>
        public TeamAbbreviatedNames TeamAbbreviatedNames { get; private set; }

        /// <summary>
        /// 登録番号一覧を取得します。
        /// </summary>
        public PlayerCodes PlayerCodes { get; private set; }

        /// <summary>
        /// 氏名一覧を取得します。
        /// </summary>
        public PlayerNames PlayerNames { get; private set; }

        /// <summary>
        /// 一つ前の試合番号を取得します。
        /// </summary>
        public GameNumber FromGameNumber { get; private set; }

        /// <summary>
        /// 当該対戦枠に対戦者が割り当てられているかどうかを示します。
        /// </summary>
        public bool IsAssigned => this.EntryNumber != null;

        /// <summary>
        /// 当該対戦者が BYE かどうかを示します。
        /// </summary>
        public bool IsBye => this.PlayerClassification == PlayerClassification.Bye;
        #endregion properties

        #region constructors
        /// <summary>
        /// 対戦者の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="drawNumber">ドロー番号。</param>
        /// <param name="seedLevel">シードレベル。</param>
        /// <param name="assignOrder">割当順。</param>
        public Opponent(
            DrawNumber drawNumber,
            SeedLevel seedLevel,
            AssignOrder assignOrder)
        {
            this.DrawNumber = drawNumber;
            this.SeedLevel = seedLevel;
            this.AssignOrder = assignOrder;
            this.FramePlayerClassification = PlayerClassification.General;
            this.IsManuallySettingFrame = false;
            this.IsManuallyAssigned = false;
            this.PlayerClassification = null;
            this.EntryNumber = null;
            this.SeedNumber = null;
            this.TeamCodes = null;
            this.TeamAbbreviatedNames = null;
            this.PlayerCodes = null;
            this.PlayerNames = null;
        }

        private Opponent(
            BlockNumber blockNumber,
            GameNumber gameNumber,
            DrawNumber drawNumber,
            SeedLevel seedLevel,
            AssignOrder assignOrder,
            PlayerClassification playerClassification,
            EntryNumber entryNumber,
            SeedNumber seedNumber,
            TeamCodes teamCodes,
            TeamAbbreviatedNames teamAbbreviatedNames,
            PlayerCodes playerCodes,
            PlayerNames playerNames,
            GameNumber fromGameNumber)
        {
            this.BlockNumber = blockNumber;
            this.GameNumber = gameNumber;
            this.DrawNumber = drawNumber;
            this.SeedLevel = seedLevel;
            this.AssignOrder = assignOrder;
            this.FramePlayerClassification = PlayerClassification.General;
            this.IsManuallySettingFrame = false;
            this.IsManuallyAssigned = false;
            this.PlayerClassification = playerClassification;
            this.EntryNumber = entryNumber;
            this.SeedNumber = seedNumber;
            this.TeamCodes = teamCodes;
            this.TeamAbbreviatedNames = teamAbbreviatedNames;
            this.PlayerCodes = playerCodes;
            this.PlayerNames = playerNames;
            this.FromGameNumber = fromGameNumber;
        }
        #endregion constructors

        #region methods
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
        /// <param name="isManuallyAssigned">手動で割り当てフラグ。</param>
        public void UpdateOpponent(
            PlayerClassification playerClassification,
            EntryNumber entryNumber,
            SeedNumber seedNumber,
            TeamCodes teamCodes,
            TeamAbbreviatedNames teamAbbreviatedNames,
            PlayerCodes playerCodes,
            PlayerNames playerNames,
            GameNumber fromGameNumber = null,
            bool isManuallyAssigned = false)
        {
            this.PlayerClassification = playerClassification;
            this.EntryNumber = entryNumber;
            this.SeedNumber = seedNumber;
            this.TeamCodes = teamCodes;
            this.TeamAbbreviatedNames = teamAbbreviatedNames;
            this.PlayerCodes = playerCodes;
            this.PlayerNames = playerNames;
            this.FromGameNumber = fromGameNumber;
            this.IsManuallyAssigned = isManuallyAssigned;

            if (this.Game != null && this.Game.Opponents.Count(o => o.IsAssigned) == Game.maxOpponentsCount)
            {
                this.Game.GameResult.Ready();
            }
        }

        /// <summary>
        /// 試合番号を更新します。
        /// </summary>
        /// <param name="blockNumber">ブロック番号。</param>
        public void UpdateBlockNumber(BlockNumber blockNumber)
        {
            this.BlockNumber = blockNumber;
        }

        /// <summary>
        /// 試合番号を更新します。
        /// </summary>
        /// <param name="gameNumber">試合番号。</param>
        public void UpdateGameNumber(GameNumber gameNumber)
        {
            this.GameNumber = gameNumber;
        }

        /// <summary>
        /// 対戦者を複製します。
        /// </summary>
        /// <returns>対戦者。</returns>
        public Opponent Clone()
        {
            return new Opponent(
                this.BlockNumber,
                this.GameNumber,
                this.DrawNumber,
                this.SeedLevel,
                this.AssignOrder,
                this.PlayerClassification,
                this.EntryNumber,
                this.SeedNumber,
                this.TeamCodes,
                this.TeamAbbreviatedNames,
                this.PlayerCodes,
                this.PlayerNames,
                this.GameNumber
            );
        }

        /// <summary>
        /// 対戦者（BYE）を作成します。
        /// </summary>
        /// <returns>対戦者。</returns>
        public static Opponent CreateBye()
        {
            return new Opponent(
                blockNumber: null,
                gameNumber: null,
                new DrawNumber(0),
                new SeedLevel(0),
                new AssignOrder(0),
                PlayerClassification.Bye,
                new EntryNumber(0),
                new SeedNumber(0),
                teamCodes: null,
                teamAbbreviatedNames: null,
                playerCodes: null,
                playerNames: null,
                fromGameNumber: null
            );
        }

        /// <summary>
        /// シード枠に変更します。
        /// </summary>
        /// <param name="seedLevel">アサインレベル。</param>
        /// <param name="isManual">手動設定フラグ。</param>
        public void AsSeedFrame(SeedLevel seedLevel = null, bool isManual = false)
        {
            this.FramePlayerClassification = PlayerClassification.Seed;
            this.IsManuallySettingFrame = isManual;
            this.SeedLevel = seedLevel ?? new SeedLevel(Block.LowPriorityLevelSeed);
        }

        /// <summary>
        /// 一般枠に変更します。
        /// </summary>
        /// <param name="isManual">手動設定フラグ。</param>
        public void AsGeneralFrame(bool isManual = false)
        {
            this.FramePlayerClassification = PlayerClassification.General;
            this.IsManuallySettingFrame = isManual;
            this.SeedLevel = new SeedLevel(0);
        }

        /// <summary>
        /// BYE 枠に変更します。
        /// </summary>
        /// <param name="isManual">手動設定フラグ。</param>
        public void AsByeFrame(bool isManual = false)
        {
            this.FramePlayerClassification = PlayerClassification.Bye;
            this.IsManuallySettingFrame = isManual;
            this.SeedLevel = new SeedLevel(0);
        }
        #endregion methods

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
