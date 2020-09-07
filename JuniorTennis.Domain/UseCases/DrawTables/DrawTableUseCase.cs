using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.Ranking;
using JuniorTennis.Domain.Repositoies;
using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.DrawTables
{
    public class DrawTableUseCase : IDrawTableUseCase
    {
        private readonly IDrawTableRepository drawTableRepository;
        private readonly ITournamentRepository tournamentRepository;
        private readonly ITournamentEntryRepository tournamentEntryRepository;
        private readonly IRankingRepository rankingRepository;
        public DrawTableUseCase(
            IDrawTableRepository drawTablerepository,
            ITournamentRepository tournamentRepository,
            ITournamentEntryRepository tournamentEntryRepository,
            IRankingRepository rankingRepository)
        {
            this.drawTableRepository = drawTablerepository;
            this.tournamentRepository = tournamentRepository;
            this.tournamentEntryRepository = tournamentEntryRepository;
            this.rankingRepository = rankingRepository;
        }

        public async Task<IEnumerable<DrawTable>> GetDrawTables()
        {
            throw new Exception();
        }

        public async Task<DrawTable> GetDrawTable(DrawTableRepositoryDto dto, bool asNoTracking = false)
        {
            return await this.drawTableRepository.FindByDtoAsync(dto, asNoTracking);
        }

        public async Task<DrawTable> GetDrawTableWithoutRelevantData(int tournamentId, string tennisEventId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId);

            return await this.GetDrawTable(dto);
        }

        public async Task<bool> ExistsDrawTable(int tournamentId, string tennisEventId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId);
            return await this.drawTableRepository.ExistsByDtoAsync(dto);
        }

        public async Task<DrawTable> RegisterDrawTable(DrawTable drawTable)
        {
            return await this.drawTableRepository.AddAsync(drawTable);
        }

        public async Task<DrawTable> UpdateDrawTable(DrawTable drawTable)
        {
            return await this.drawTableRepository.UpdateAsync(drawTable);
        }

        public async Task DeleteDrawTable(DrawTable drawTable)
        {
            await this.drawTableRepository.DeleteAsync(drawTable);
        }

        public async Task<List<EntryDetail>> RetrievePlayers(int tournamentId, string tennisEventId)
        {
            var tournamentEntries = await this.tournamentEntryRepository.FindByIdAsync(tournamentId, tennisEventId);
            var entryDetails = tournamentEntries
                .Select(o => o.EntryDetail)
                .Select(o => new EntryDetail(
                    o.EntryNumber,
                    o.ParticipationClassification,
                    o.SeedNumber,
                    o.EntryPlayers,
                    o.CanParticipationDates,
                    o.ReceiptStatus,
                    UsageFeatures.DrawTable))
                .ToList();

            return entryDetails;
        }

        public async Task StartEditingTheDrawTable(int tournamentId, string tennisEventId, int tournamentFromatId)
        {
            var editingDto = new DrawTableRepositoryDto(tournamentId, tennisEventId, EditStatus.Editing);
            var editingDrawTable = await this.GetDrawTable(editingDto);
            if (editingDrawTable != null)
            {
                await this.drawTableRepository.DeleteAsync(editingDrawTable);
            }

            var draftDto = new DrawTableRepositoryDto(tournamentId, tennisEventId, EditStatus.Draft)
            {
                IncludeEntryDetails = true,
                IncludeEntryPlayers = true,
                IncludeQualifyingDrawSettings = true,
                IncludeMainDrawSettings = true,
                IncludeBlocks = true,
                IncludeGames = true,
                IncludeGameResult = true,
                IncludeOpponents = true,
            };
            var draftDrawTable = await this.GetDrawTable(draftDto);
            if (draftDrawTable == null || editingDrawTable.TournamentFormat.Id != tournamentFromatId)
            {
                editingDrawTable = await this.CreateInitialDrawTable(tournamentId, tennisEventId, tournamentFromatId);
                await this.drawTableRepository.AddAsync(editingDrawTable);
                return;
            }

            editingDrawTable = await this.drawTableRepository.FindByDtoAsync(draftDto, asNoTracking: true);
            editingDrawTable.AsEditing();
            await this.drawTableRepository.AddAsync(editingDrawTable);
        }

        private async Task<DrawTable> CreateInitialDrawTable(int tournamentId, string tennisEventId, int tournamentFromatId)
        {
            var tournament = await this.tournamentRepository.FindById(tournamentId);
            var tennisEvent = TennisEvent.FromId(tennisEventId);
            var tournamentFormat = Enumeration.FromValue<TournamentFormat>(tournamentFromatId);
            var entryDetails = await this.RetrievePlayers(tournamentId, tennisEventId);
            var mainDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(1),
                numberOfDraws: new NumberOfDraws(0),
                numberOfEntries: new NumberOfEntries(0),
                numberOfWinners: new NumberOfWinners(0),
                tournamentGrade: TournamentGrade.A);
            var qualifyingDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(1),
                numberOfDraws: new NumberOfDraws(0),
                numberOfEntries: new NumberOfEntries(0),
                numberOfWinners: new NumberOfWinners(0),
                tournamentGrade: TournamentGrade.A);
            var blocks = new List<Block>();
            blocks.Add(new Block(
                blockNumber: new BlockNumber(0),
                participationClassification: ParticipationClassification.Main,
                gameDate: null,
                mainDrawSettings
                ));
            if (tournamentFormat == TournamentFormat.WithQualifying)
            {
                blocks.Add(new Block(
                    blockNumber: new BlockNumber(1),
                    participationClassification: ParticipationClassification.Qualifying,
                    gameDate: null,
                    qualifyingDrawSettings
                    ));
            }

            var drawTable = new DrawTable(
                tournament,
                tennisEvent,
                tournamentFormat,
                eligiblePlayersType: EligiblePlayersType.AllPlayers,
                entryDetails,
                mainDrawSettings,
                qualifyingDrawSettings,
                blocks,
                editStatus: EditStatus.Editing);

            return drawTable;
        }

        public async Task<DrawTable> UpdateEligiblePlayersType(DrawTable drawTable, EligiblePlayersType eligiblePlayersType)
        {
            drawTable.UpdateEligiblePlayersType(eligiblePlayersType);
            return await this.UpdateDrawTable(drawTable);
        }

        public string GetEntryDetailsJson(
            Tournament tournament,
            DrawTable drawTable,
            EligiblePlayersType eligiblePlayersType,
            ParticipationClassification participationClassification)
        {
            var entryDetailsJson = drawTable.EntryDetails
                .Where(o => o.ReceiptStatus != ReceiptStatus.Cancel)
                .OrderByDescending(o => o.TotalPoint)
                .Select(o => new
                {
                    entryNumber = o.EntryNumber.Value,
                    participationClassificationId = o.ParticipationClassification.Id,
                    isDa = o.ParticipationClassification == ParticipationClassification.Main,
                    seedNumber = o.SeedNumber.Value,
                    isParticipate = o.ParticipationClassification != ParticipationClassification.NotParticipate,
                    teamNames = o.EntryPlayers.Select(o => o.TeamName.Value),
                    teamCodes = o.EntryPlayers.Select(o => o.TeamCode.Value),
                    teamAbbreviatedNames = o.EntryPlayers.Select(o => o.TeamAbbreviatedName.Value),
                    playerCodes = o.EntryPlayers.Select(o => o.PlayerCode.Value),
                    playerNames = o.EntryPlayers.Select(o => o.PlayerName.Value),
                    playerFamilyNames = o.EntryPlayers.Select(o => o.PlayerFamilyName.Value),
                    playerFirstNames = o.EntryPlayers.Select(o => o.PlayerFirstName.Value),
                    points = o.EntryPlayers.Select(o => o.Point.Value),
                    totalPoint = o.TotalPoint,
                    rank = drawTable.EntryDetails.Count(p => p.TotalPoint > o.TotalPoint) + 1,
                    canParticipationDates = tournament.HoldingDates.Select(p => new
                    {
                        value = p.Value,
                        text = p.DisplayValue,
                        isParticipate = o.CanParticipationDates.Any(q => q.Value == p.Value),
                    }),
                    receiptStatusId = o.ReceiptStatus.Id,
                    fromQualifying = o.FromQualifying,
                    blockNumber = o.BlockNumber?.Value,
                });

            if (eligiblePlayersType != null && eligiblePlayersType == EligiblePlayersType.RecievedPlayers)
            {
                entryDetailsJson = entryDetailsJson.Where(o => o.receiptStatusId == ReceiptStatus.Received.Id);
            }

            if (participationClassification != null)
            {
                entryDetailsJson = entryDetailsJson.Where(o => o.participationClassificationId == participationClassification.Id);
            }
            else
            {
                entryDetailsJson = entryDetailsJson.Where(o => !o.fromQualifying);
            }

            return JsonSerializer.Serialize(entryDetailsJson);
        }

        public async Task<string> GetDrawSettingsJson(int tournamentId, string tennisEventId, int participationClassificationId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeQualifyingDrawSettings = true,
                IncludeMainDrawSettings = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var participationClassification = Enumeration.FromValue<ParticipationClassification>(participationClassificationId);
            return this.GetDrawSettingsJson(drawTable, participationClassification);
        }

        public string GetDrawSettingsJson(DrawTable drawTable, ParticipationClassification participationClassification)
        {
            return drawTable.GetDrawSettings(participationClassification).ToJson();
        }

        public async Task<string> GetGameDatesJson(int tournamentId, string tennisEventId)
        {
            var tournament = await this.tournamentRepository.FindById(tournamentId);
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeBlocks = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            return this.GetGameDatesJson(tournament, drawTable);
        }

        public string GetGameDatesJson(Tournament tournament, DrawTable drawTable)
        {
            return JsonSerializer.Serialize(drawTable.Blocks.Select(o => new
            {
                blockNumber = o.BlockNumber.Value,
                participationClassification = o.ParticipationClassification,
                text = o.DisplayValue,
                gameDates = tournament.HoldingDates.Select(p => new
                {
                    value = p.Value,
                    text = p.DisplayValue,
                    selected = p.ElementValue == (o.GameDate?.ElementValue ?? string.Empty),
                }),
            }));
        }

        public async Task<string> GetBlankDrawsJson(int tournamentId, string tennisEventId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeOpponents = true,
            };
            var drawTable = await this.drawTableRepository.FindByDtoAsync(dto);
            var json = drawTable.Blocks
                .SelectMany(o => o.Games
                .Where(p => p.RoundNumber.Value == 1)
                .SelectMany(p => p.Opponents
                .Where(p => !p.IsAssigned)
                .Select(q => new
                {
                    blockName = o.DisplayValue,
                    drawNumber = q.DrawNumber.Value,
                })));

            return JsonSerializer.Serialize(json);
        }

        public async Task<string> GetGameOfSameTeamsJson(int tournamentId, string tennisEventId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeOpponents = true,
            };
            var drawTable = await this.drawTableRepository.FindByDtoAsync(dto);
            var json = drawTable.Blocks
                .SameTeamsGames()
                .Select(o => new
                {
                    blockName = o.BlockName,
                    drawNumber = o.DrawNumber,
                    teamAbbreviatedNames = o.TeamAbbreviatedNames,
                    playerNames = o.PlayerNames,
                });

            return JsonSerializer.Serialize(json);
        }

        public async Task UpdateGameDates(int tournamentId, string tennisEventId, IEnumerable<(int blockNumber, DateTime gameDate)> gameDates)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeBlocks = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var gameDates_ = gameDates.Select(o => (new BlockNumber(o.blockNumber), new GameDate(o.gameDate)));

            await this.UpdateGameDates(drawTable, gameDates_);
        }

        public async Task UpdateGameDates(DrawTable drawTable, IEnumerable<(BlockNumber blockNumber, GameDate gameDate)> gameDates)
        {
            drawTable.Blocks.UpdateGameDates(gameDates);
            await this.UpdateDrawTable(drawTable);
        }

        public async Task<int> GetNumberOfEntries(int tournamentId, string tennisEventId)
        {
            var entries = await this.tournamentEntryRepository.FindByIdAsync(tournamentId, tennisEventId);
            return entries.Count();
        }

        /// <summary>
        /// ドロー表を取得または設定します。
        /// </summary>
        private DrawTable DrawTable { get; set; }

        /// <summary>
        /// シード選手の一覧を取得または設定します。
        /// </summary>
        private Queue<EntryDetail> SeedPlayersQueue { get; set; }

        /// <summary>
        /// 一般選手の一覧を取得または設定します。
        /// </summary>
        private Queue<EntryDetail> GeneralPlayersQueue { get; set; }

        /// <summary>
        /// BYE の一覧を取得または設定します。
        /// </summary>
        private Queue<int> Byes { get; set; }

        /// <summary>
        /// シードレベルの一覧を取得または設定します。
        /// </summary>
        private List<int> SeedLevels { get; set; }

        public async Task ExecuteDrawingMain(int tournamentId, string tennisEventId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeEntryDetails = true,
                IncludeEntryPlayers = true,
                IncludeMainDrawSettings = true,
                IncludeOpponents = true,
            };
            this.DrawTable = await this.GetDrawTable(dto);

            // 試合一覧を初期化
            this.DrawTable.Blocks.InitializeMainGames();
            await this.UpdateDrawTable(this.DrawTable);

            this.DrawTable = await this.GetDrawTable(dto);
            var drawSettings = this.DrawTable.MainDrawSettings;
            this.SeedLevels = this.GetSeedLevels(this.DrawTable.Blocks.GetMainBlock());
            this.SeedPlayersQueue = this.DrawTable
                .EntryDetails
                .ExtractEntryDetailsQueue(ParticipationClassification.Main, isSeed: true);
            this.GeneralPlayersQueue = this.DrawTable
                .EntryDetails
                .ExtractEntryDetailsQueue(ParticipationClassification.Main, isSeed: false);
            this.Byes = new Queue<int>(Enumerable.Range(1, drawSettings.NumberOfByes));

            this.AssignSeedPlayerToSeedSlot(ParticipationClassification.Main);
            this.AssignByeToSeedPlayersOpponent(ParticipationClassification.Main);
            this.AssignByesToBlankSlot(ParticipationClassification.Main);
            this.AssignGeneralPlayersToBlankSlotInRandomOrder(ParticipationClassification.Main);
            this.DrawTable.Blocks.UpdateGames(ParticipationClassification.Main);

            await this.UpdateDrawTable(this.DrawTable);
        }

        public async Task ExecuteDrawingQualifying(int tournamentId, string tennisEventId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeEntryDetails = true,
                IncludeEntryPlayers = true,
                IncludeQualifyingDrawSettings = true,
                IncludeOpponents = true,
            };
            this.DrawTable = await this.GetDrawTable(dto);

            // 試合一覧を初期化
            this.DrawTable.Blocks.InitializeQualifyingGames();
            await this.UpdateDrawTable(this.DrawTable);

            this.DrawTable = await this.GetDrawTable(dto);
            var drawSettings = this.DrawTable.QualifyingDrawSettings;
            this.SeedLevels = new List<int>() { Block.FirstLevelSeed, Block.SecondLevelSeed };
            this.SeedPlayersQueue = this.DrawTable
                .EntryDetails
                .ExtractEntryDetailsQueue(ParticipationClassification.Qualifying, isSeed: true);
            this.GeneralPlayersQueue = this.DrawTable
                .EntryDetails
                .ExtractEntryDetailsQueue(ParticipationClassification.Qualifying, isSeed: false);
            this.Byes = new Queue<int>(Enumerable.Range(1, (int)drawSettings.NumberOfByes));

            this.AssignSeedPlayerToSeedSlot(ParticipationClassification.Qualifying);
            this.AssignByeToSeedPlayersOpponent(ParticipationClassification.Qualifying);
            this.AssignByesToBlankSlot(ParticipationClassification.Qualifying);
            this.AssignGeneralPlayersToBlankSlotInRandomOrder(ParticipationClassification.Qualifying);
            this.DrawTable.Blocks.UpdateGames(ParticipationClassification.Qualifying);

            await this.UpdateDrawTable(this.DrawTable);
        }

        /// <summary>
        /// シード枠にシード選手を割り当てます。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        private void AssignSeedPlayerToSeedSlot(ParticipationClassification participationClassification)
        {
            foreach (var seedLevel in this.SeedLevels)
            {
                // シード枠の割当
                var opponents = this.DrawTable.Blocks.FindOpponentsBySeedLevel(participationClassification, seedLevel);

                if (seedLevel > Block.FirstLevelSeed)
                {
                    // シードレベル 1 以降の場合、ランダムに選手を割り当てる。
                    this.SeedPlayersQueue = new Queue<EntryDetail>(this.SeedPlayersQueue.OrderBy(o => Guid.NewGuid()));
                }

                foreach (var opponent in opponents)
                {
                    // シード選手を割当
                    if (this.SeedPlayersQueue.Any())
                    {
                        var player = this.SeedPlayersQueue.Dequeue();
                        opponent.UpdateOpponent(
                            PlayerClassification.Seed,
                            player.EntryNumber,
                            player.SeedNumber,
                            new TeamCodes(player.EntryPlayers.Select(o => o.TeamCode)),
                            new TeamAbbreviatedNames(player.EntryPlayers.Select(o => o.TeamAbbreviatedName)),
                            new PlayerCodes(player.EntryPlayers.Select(o => o.PlayerCode)),
                            new PlayerNames(player.EntryPlayers.Select(o => o.PlayerName)));

                        continue;
                    }

                    // 空いたシード枠に一般選手を割当
                    if (this.GeneralPlayersQueue.Any())
                    {
                        var player = this.GeneralPlayersQueue.Dequeue();
                        opponent.UpdateOpponent(
                            PlayerClassification.General,
                            player.EntryNumber,
                            player.SeedNumber,
                            new TeamCodes(player.EntryPlayers.Select(o => o.TeamCode)),
                            new TeamAbbreviatedNames(player.EntryPlayers.Select(o => o.TeamAbbreviatedName)),
                            new PlayerCodes(player.EntryPlayers.Select(o => o.PlayerCode)),
                            new PlayerNames(player.EntryPlayers.Select(o => o.PlayerName)));

                        continue;
                    }

                    // 空いたシード枠にBYEを割り当て
                    if (this.Byes.Any())
                    {
                        this.Byes.Dequeue();
                        opponent.UpdateOpponent(
                            PlayerClassification.Bye,
                            new EntryNumber(0),
                            new SeedNumber(0),
                            teamCodes: null,
                            teamAbbreviatedNames: null,
                            playerCodes: null,
                            playerNames: null);

                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// BYE をシード選手の対戦相手に割り当てます。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        private void AssignByeToSeedPlayersOpponent(ParticipationClassification participationClassification)
        {
            foreach (var seedLevel in this.SeedLevels)
            {
                // シード選手枠
                var seedOpponents = this.DrawTable.Blocks.FindOpponentsBySeedLevel(participationClassification, seedLevel);

                // シード選手が割り当てられている試合番号一覧
                var gameNumbers = seedOpponents
                    .Where(opponet => opponet.PlayerClassification == PlayerClassification.Seed)
                    .Select(opponet => opponet.GameNumber);

                // 一般選手枠
                var opponents = this.DrawTable.Blocks.FindOpponentsBySeedLevel(participationClassification, 0);

                var opponentsOfSeed = opponents
                    .Where(o => gameNumbers.Contains(o.GameNumber))
                    .OrderBy(o => o.AssignOrder.Value)
                    .ThenByDescending(o => o.BlockNumber.Value);

                foreach (var opponent in opponentsOfSeed)
                {
                    // シード選手の対戦相手にBYEを割り当て
                    if (this.Byes.Any())
                    {
                        this.Byes.Dequeue();
                        opponent.UpdateOpponent(
                            PlayerClassification.Bye,
                            new EntryNumber(0),
                            new SeedNumber(0),
                            teamCodes: null,
                            teamAbbreviatedNames: null,
                            playerCodes: null,
                            playerNames: null);

                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// BYE をブロック番号の大きい方から順に割り当てます。
        /// ブロック内での割り当て位置はランダムとします。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        private void AssignByesToBlankSlot(ParticipationClassification participationClassification)
        {
            var blocks = this.DrawTable.Blocks.GetBlocks(participationClassification);
            var maxBlockNumber = blocks.Max(o => o.BlockNumber.Value);
            var minBlockNumber = blocks.Min(o => o.BlockNumber.Value);

            // 割り当て可能な試合が存在する限り繰り返す
            while (this.DrawTable.Blocks.HasByeAssignableGames(participationClassification))
            {
                // ブロック番号を大きい方から順に繰り返す
                for (var blockNumber = maxBlockNumber; blockNumber >= minBlockNumber; blockNumber--)
                {
                    if (!this.Byes.Any())
                    {
                        return;
                    }

                    // ブロックを取得
                    var block = this.DrawTable.Blocks.First(o => o.BlockNumber.Value == blockNumber);

                    // BYE を割り当て可能な試合をランダムに取得
                    var game = block.Games
                        .Where(o => o.CanAssign)
                        .Where(o => !o.HasBye)
                        .OrderBy(o => Guid.NewGuid())
                        .FirstOrDefault();
                    if (game == null)
                    {
                        continue;
                    }

                    // 対戦枠をランダムに取得
                    var opponent = game.Opponents
                        .OrderBy(o => Guid.NewGuid())
                        .First();

                    // BYEを割り当てる
                    this.Byes.Dequeue();
                    opponent.UpdateOpponent(
                        PlayerClassification.Bye,
                        new EntryNumber(0),
                        new SeedNumber(0),
                        teamCodes: null,
                        teamAbbreviatedNames: null,
                        playerCodes: null,
                        playerNames: null);
                }
            }
        }

        /// <summary>
        /// 空き枠に一般選手をランダムに割り当てます。
        /// </summary>
        /// <param name="participationClassification">出場区分。</param>
        private void AssignGeneralPlayersToBlankSlotInRandomOrder(ParticipationClassification participationClassification)
        {
            // 一般選手枠の割当
            var opponents = this.DrawTable.Blocks.FindBlankOpponentsInRandomOrder(participationClassification);
            foreach (var opponent in opponents)
            {
                // シード選手を割当
                if (this.SeedPlayersQueue.Any())
                {
                    var player = this.SeedPlayersQueue.Dequeue();
                    opponent.UpdateOpponent(
                        PlayerClassification.Seed,
                        player.EntryNumber,
                        player.SeedNumber,
                        new TeamCodes(player.EntryPlayers.Select(o => o.TeamCode)),
                        new TeamAbbreviatedNames(player.EntryPlayers.Select(o => o.TeamAbbreviatedName)),
                        new PlayerCodes(player.EntryPlayers.Select(o => o.PlayerCode)),
                        new PlayerNames(player.EntryPlayers.Select(o => o.PlayerName)));

                    continue;
                }

                // BYEを割り当て
                if (this.Byes.Any())
                {
                    this.Byes.Dequeue();
                    opponent.UpdateOpponent(
                        PlayerClassification.Bye,
                        new EntryNumber(0),
                        new SeedNumber(0),
                        teamCodes: null,
                        teamAbbreviatedNames: null,
                        playerCodes: null,
                        playerNames: null);

                    continue;
                }

                // 一般選手を割り当て
                if (this.GeneralPlayersQueue.Any())
                {
                    var player = this.GeneralPlayersQueue.Dequeue();
                    opponent.UpdateOpponent(
                        PlayerClassification.General,
                        player.EntryNumber,
                        player.SeedNumber,
                        new TeamCodes(player.EntryPlayers.Select(o => o.TeamCode)),
                        new TeamAbbreviatedNames(player.EntryPlayers.Select(o => o.TeamAbbreviatedName)),
                        new PlayerCodes(player.EntryPlayers.Select(o => o.PlayerCode)),
                        new PlayerNames(player.EntryPlayers.Select(o => o.PlayerName)));

                    continue;
                }
            }
        }

        /// <summary>
        /// 重複を排除したシードレベル一覧を取得します。
        /// </summary>
        /// <param name="block">ブロック。</param>
        /// <returns>シードレベル一覧。</returns>
        private List<int> GetSeedLevels(Block block)
        {
            return block.Games
                .SelectMany(o => o.Opponents)
                .Select(o => o.SeedLevel.Value)
                .Where(o => o != 0)
                .Distinct()
                .OrderBy(o => o)
                .ToList();
        }

        public async Task<string> GetBlocksJson(int tournamentId, string tennisEventId, int participationClassificationId, int? blockNumber = null)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var participationClassification = Enumeration.FromValue<ParticipationClassification>(participationClassificationId);
            var BlockNumber = blockNumber.HasValue ? new BlockNumber(blockNumber.Value) : null;
            return this.GetBlocksJson(drawTable, participationClassification, BlockNumber);
        }

        public string GetBlocksJson(DrawTable drawTable, ParticipationClassification participationClassification, BlockNumber blockNumber)
        {
            return drawTable.Blocks.ToJson(participationClassification, blockNumber);
        }

        public async Task InitializeDrawTable(int tournamentId, string tennisEventId, int participationClassificationId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeEntryDetails = true,
                IncludeEntryPlayers = true,
                IncludeQualifyingDrawSettings = true,
                IncludeMainDrawSettings = true,
                IncludeBlocks = true,
                IncludeGames = true,
                IncludeGameResult = true,
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var participationClassification = Enumeration.FromValue<ParticipationClassification>(participationClassificationId);
            var drawSettings = drawTable.GetDrawSettings(participationClassification);
            var blocks = drawTable.Blocks.GetBlocks(participationClassification);
            var drawNumberSettings = DrawNumberSettingsRepository.FindByNumberOfDraws(drawSettings.NumberOfDraws.Value);
            foreach (var block in blocks)
            {
                block.InitializeGames();
                var opponents = new Queue<Opponent>();
                foreach (var drawNumberSetting in drawNumberSettings)
                {
                    opponents.Enqueue(new Opponent(
                        new DrawNumber(drawNumberSetting.DrawNumber),
                        new SeedLevel(0),
                        new AssignOrder(drawNumberSetting.AssignOrder)));
                }

                foreach (var gameNumber in Enumerable.Range(1, block.Games.CalculateNumberOfGames(drawSettings.NumberOfDraws)))
                {
                    var game = new Game(
                        new GameNumber(gameNumber),
                        new RoundNumber(1),
                        drawSettings);

                    var opponent = opponents.Dequeue();
                    opponent.UpdateBlockNumber(block.BlockNumber);
                    opponent.UpdateGameNumber(game.GameNumber);
                    game.AssignOpponent(opponent);

                    opponent = opponents.Dequeue();
                    opponent.UpdateBlockNumber(block.BlockNumber);
                    opponent.UpdateGameNumber(game.GameNumber);
                    game.AssignOpponent(opponent);

                    block.Games.Add(game);
                }
            }

            await this.UpdateDrawTable(drawTable);
        }

        public async Task ExecuteSeedFrameSetting(int tournamentId, string tennisEventId, int participationClassificationId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeMainDrawSettings = true,
                IncludeQualifyingDrawSettings = true,
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var participationClassification = Enumeration.FromValue<ParticipationClassification>(participationClassificationId);
            var drawSettings = drawTable.GetDrawSettings(participationClassification);
            var blocks = drawTable.Blocks.GetBlocks(participationClassification);
            var drawNumberSettings = DrawNumberSettingsRepository.FindByNumberOfDraws(drawSettings.NumberOfDraws.Value);
            foreach (var block in blocks)
            {
                var opponents = block.Games.SelectMany(o => o.Opponents);
                foreach (var drawNumberSetting in drawNumberSettings)
                {
                    if (drawNumberSetting.PlayerClassificationId != PlayerClassification.Seed.Id)
                    {
                        continue;
                    }

                    opponents
                        .First(o => o.DrawNumber.Value == drawNumberSetting.DrawNumber)
                        .AsSeedFrame(new SeedLevel(drawNumberSetting.SeedLevel));
                }
            }

            await this.UpdateDrawTable(drawTable);
        }

        public async Task ExecuteSeedFrameRemove(int tournamentId, string tennisEventId, int participationClassificationId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeMainDrawSettings = true,
                IncludeQualifyingDrawSettings = true,
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var participationClassification = Enumeration.FromValue<ParticipationClassification>(participationClassificationId);
            var opponents = drawTable.Blocks.GetBlocks(participationClassification)
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.FramePlayerClassification == PlayerClassification.Seed);

            foreach (var opponent in opponents)
            {
                opponent.AsGeneralFrame();
            }

            await this.UpdateDrawTable(drawTable);
        }

        public async Task ExecuteByeFrameSetting(int tournamentId, string tennisEventId, int participationClassificationId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeMainDrawSettings = true,
                IncludeQualifyingDrawSettings = true,
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var participationClassification = Enumeration.FromValue<ParticipationClassification>(participationClassificationId);
            var drawSettings = drawTable.GetDrawSettings(participationClassification);
            var blocks = drawTable.Blocks.GetBlocks(participationClassification);
            var drawNumberSettings = DrawNumberSettingsRepository.FindByNumberOfDraws(drawSettings.NumberOfDraws.Value);
            foreach (var block in blocks)
            {
                var opponents = block.Games.SelectMany(o => o.Opponents);
                foreach (var drawNumberSetting in drawNumberSettings)
                {
                    if (drawNumberSetting.PlayerClassificationId != PlayerClassification.Bye.Id)
                    {
                        continue;
                    }

                    opponents
                        .First(o => o.DrawNumber.Value == drawNumberSetting.DrawNumber)
                        .AsByeFrame();
                }
            }

            await this.UpdateDrawTable(drawTable);
        }

        public async Task ExecuteByeFrameRemove(int tournamentId, string tennisEventId, int participationClassificationId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeMainDrawSettings = true,
                IncludeQualifyingDrawSettings = true,
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var participationClassification = Enumeration.FromValue<ParticipationClassification>(participationClassificationId);
            var opponents = drawTable.Blocks.GetBlocks(participationClassification)
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.FramePlayerClassification == PlayerClassification.Bye);

            foreach (var opponent in opponents)
            {
                opponent.AsGeneralFrame();
            }

            await this.UpdateDrawTable(drawTable);
        }

        public async Task IntakeQualifyingWinners(int tournamentId, string tennisEventId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeEntryDetails = true,
                IncludeEntryPlayers = true,
                IncludeQualifyingDrawSettings = true,
                IncludeGameResult = true,
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var qualifyingWinners = this.GetQualifyingWinners(drawTable);
            var qualifyingOpponents = this.GetQualifyingOpponents(drawTable);
            var finalists = this.CreateFinalists(qualifyingWinners, qualifyingOpponents);

            drawTable.EntryDetails.RemoveQualifyingWinners();
            drawTable.EntryDetails.AddFinalists(finalists);

            await this.UpdateDrawTable(drawTable);
        }

        /// <summary>
        /// 予選ブロックの勝者一覧を取得します。
        /// </summary>
        /// <param name="drawTable">ドロー表。</param>
        /// <returns>選手一覧。</returns>
        private IEnumerable<EntryDetail> GetQualifyingWinners(DrawTable drawTable)
        {
            var drawSettings = drawTable.QualifyingDrawSettings;
            var numberOfGamesPerBlock = drawSettings.NumberOfGamesPerBlock;
            var entryNumberOfQualifyingWinners = drawTable.Blocks
                .GetQualifyingBlocks()
                .Select(o => o.GetEntryNumberOfWinner(numberOfGamesPerBlock));
            var qualifyingWinners = drawTable.EntryDetails
                .Where(o => o.ParticipationClassification == ParticipationClassification.Qualifying)
                .Where(o => entryNumberOfQualifyingWinners.Contains(o.EntryNumber));

            return qualifyingWinners;
        }

        /// <summary>
        /// 予選ブロックの対戦者一覧を取得します。
        /// </summary>
        /// <param name="drawTable">ドロー表。</param>
        /// <returns>対戦者一覧。</returns>
        private IEnumerable<Opponent> GetQualifyingOpponents(DrawTable drawTable)
        {
            var qualifyingOpponents = drawTable.Blocks
                   .GetQualifyingBlocks()
                   .SelectMany(o => o.Games)
                   .Where(o => o.RoundNumber == new RoundNumber(1))
                   .SelectMany(o => o.Opponents);

            return qualifyingOpponents;
        }

        /// <summary>
        /// 予選勝者から本戦進出者を作成します。
        /// </summary>
        /// <param name="entryDetails">選手一覧。</param>
        /// <param name="opponents">対戦者一覧。</param>
        /// <returns>選手一覧。</returns>
        private IEnumerable<EntryDetail> CreateFinalists(
            IEnumerable<EntryDetail> entryDetails,
            IEnumerable<Opponent> opponents)
        {
            var finalists = entryDetails.Select((entry, index) =>
            {
                var opponent = opponents.FirstOrDefault(o => o.EntryNumber == entry.EntryNumber);

                return new EntryDetail(
                    entry.EntryNumber,
                    ParticipationClassification.Main,
                    new SeedNumber(0),
                    entry.EntryPlayers,
                    entry.CanParticipationDates,
                    entry.ReceiptStatus,
                    usageFeatures: UsageFeatures.DrawTable,
                    fromQualifying: true,
                    opponent.BlockNumber
                );
            });

            return finalists.ToList();
        }

        public async Task CancelDrawing(int tournamentId, string tennisEventId, int participationClassificationId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeEntryDetails = true,
                IncludeEntryPlayers = true,
                IncludeMainDrawSettings = true,
                IncludeQualifyingDrawSettings = true,
                IncludeBlocks = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var participationClassification = Enumeration.FromValue<ParticipationClassification>(participationClassificationId);

            this.CancelDrawing(drawTable, participationClassification);
        }

        public void CancelDrawing(DrawTable drawTable, ParticipationClassification participationClassification)
        {
            drawTable.Blocks.InitializeMainGames();

            if (participationClassification == ParticipationClassification.Qualifying)
            {
                drawTable.Blocks.InitializeQualifyingGames();
                drawTable.EntryDetails.RemoveQualifyingWinners();
            }
        }

        public async Task<string> GetGemeResultsJson(int tournamentId, string tennisEventId, int participationClassificationId, int blockNumber)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeGameResult = true,
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var participationClassification = Enumeration.FromValue<ParticipationClassification>(participationClassificationId);
            var blocks = participationClassification == ParticipationClassification.Main
                 ? new List<Block>() { drawTable.Blocks.GetMainBlock() }.AsEnumerable()
                 : drawTable.Blocks.GetQualifyingBlocks().AsEnumerable();

            var games = blocks
                .Where(o => o.ParticipationClassification == participationClassification)
                .Where(o => o.BlockNumber.Value == blockNumber)
                .SelectMany(o => o.Games);

            var json = games.Select(o =>
            {
                var nextGame = games.FirstOrDefault(p => p.Opponents.Any(q => q.FromGameNumber == o.GameNumber));

                return new
                {
                    gameNumber = o.GameNumber?.Value,
                    gameStatus = o.GameResult?.GameStatus?.Id,
                    playerClassificationOfWinner = o.GameResult?.PlayerClassificationOfWinner?.Id,
                    entryNumberOfWinner = o.GameResult?.EntryNumberOfWinner?.Value,
                    gameScore = o.GameResult?.GameScore?.Value,
                    isDoneNextGame = nextGame?.IsDone ?? false,
                };
            });

            return JsonSerializer.Serialize(json);
        }

        public async Task UpdateGameStatus(int tournamentId, string tennisEventId, BlockNumber blockNumber, GameNumber gameNumber, GameResult gameResult)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeQualifyingDrawSettings = true,
                IncludeMainDrawSettings = true,
                IncludeBlocks = true,
                IncludeGames = true,
                IncludeGameResult = true,
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);
            var block = drawTable.Blocks.FirstOrDefault(o => o.BlockNumber == blockNumber);
            var game = block.Games.FirstOrDefault(o => o.GameNumber == gameNumber);

            var isWinnerChanged = game.GameResult.UpdateGameResult(
                gameResult.GameStatus,
                gameResult.PlayerClassificationOfWinner,
                gameResult.EntryNumberOfWinner,
                gameResult.GameScore
            );

            var drawSettings = drawTable.GetDrawSettings(block.ParticipationClassification);
            block.Update(game, isWinnerChanged);
            block.Update();

            await this.UpdateDrawTable(drawTable);
        }

        public async Task DrawFramePlayerClassificationChange(
            int tournamentId,
            string tennisEventId,
            int blockNumber,
            int drawNumber,
            int playerClassificationId)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);

            var targetOpponent = drawTable.Blocks
                .First(o => o.BlockNumber == new BlockNumber(blockNumber))
                .Games.SelectMany(o => o.Opponents)
                .First(o => o.DrawNumber == new DrawNumber(drawNumber));

            if (playerClassificationId == PlayerClassification.Seed.Id)
            {
                targetOpponent.AsSeedFrame(isManual: true);
            }
            else if (playerClassificationId == PlayerClassification.General.Id)
            {
                targetOpponent.AsGeneralFrame(isManual: true);
            }
            else
            {
                targetOpponent.AsByeFrame(isManual: true);
            }

            await this.UpdateDrawTable(drawTable);
        }

        public async Task UnassignPlayer(int tournamentId, string tennisEventId, int blockNumber, int drawNumber)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);

            var targetOpponent = drawTable.Blocks
                .First(o => o.BlockNumber == new BlockNumber(blockNumber))
                .Games.SelectMany(o => o.Opponents)
                .First(o => o.DrawNumber == new DrawNumber(drawNumber));

            targetOpponent.UpdateOpponent(
                playerClassification: null,
                entryNumber: null,
                seedNumber: null,
                teamCodes: null,
                teamAbbreviatedNames: null,
                playerCodes: null,
                playerNames: null,
                fromGameNumber: null);

            await this.UpdateDrawTable(drawTable);
        }

        public async Task AssignPlayersToDraw(int tournamentId, string tennisEventId, int fromEntryNumber, int toBlockNumber, int toDrawNumber)
        {
            var dto = new DrawTableRepositoryDto(tournamentId, tennisEventId)
            {
                IncludeEntryDetails = true,
                IncludeEntryPlayers = true,
                IncludeGameResult = true,
                IncludeOpponents = true,
            };
            var drawTable = await this.GetDrawTable(dto);

            // ドラッグされた選手を取得する
            var fromEntryDetail = drawTable.EntryDetails
                .FirstOrDefault(o => o.EntryNumber == new EntryNumber(fromEntryNumber));
            if (fromEntryDetail == null)
            {
                throw new InvalidOperationException("対象の選手が存在しません。");
            }

            // ドラッグ先に割り当てられている選手の対戦枠の割り当てを解除する
            var toGame = drawTable
                .Blocks.First(o => o.BlockNumber == new BlockNumber(toBlockNumber))
                .Games.FirstOrDefault(o => o.Opponents.Any(p => p.DrawNumber == new DrawNumber(toDrawNumber)));
            var toOpponent = drawTable
                .Blocks.First(o => o.BlockNumber == new BlockNumber(toBlockNumber))
                .Games.SelectMany(o => o.Opponents)
                .First(o => o.DrawNumber == new DrawNumber(toDrawNumber));
            toGame.UnassignOpponent(toOpponent);

            // ドラッグされた選手の対戦枠の割り当てを解除する
            var fromGame = drawTable
                .Blocks.SelectMany(o => o.Games)
                .FirstOrDefault(o => o.Opponents.Any(p => p.EntryNumber == fromEntryDetail.EntryNumber));
            var fromOpponent = drawTable
                .Blocks.SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .FirstOrDefault(o => o.EntryNumber == fromEntryDetail.EntryNumber);
            fromGame?.UnassignOpponent(fromOpponent);

            // ドラッグ先にドラッグされた選手の対戦枠を割り当てる
            toGame.UpdateOpponent(
                fromEntryDetail.SeedNumber.IsSeed ? PlayerClassification.Seed : PlayerClassification.General,
                fromEntryDetail.EntryNumber,
                fromEntryDetail.SeedNumber,
                new TeamCodes(fromEntryDetail.EntryPlayers.Select(o => o.TeamCode)),
                new TeamAbbreviatedNames(fromEntryDetail.EntryPlayers.Select(o => o.TeamAbbreviatedName)),
                new PlayerCodes(fromEntryDetail.EntryPlayers.Select(o => o.PlayerCode)),
                new PlayerNames(fromEntryDetail.EntryPlayers.Select(o => o.PlayerName)),
                drawNumber: new DrawNumber(toDrawNumber)
            );

            await this.UpdateDrawTable(drawTable);
        }

        public async Task UpdateToDraft(int tournamentId, string tennisEventId)
        {
            // ドラフトのドロー表を取得
            var draftDto = new DrawTableRepositoryDto(tournamentId, tennisEventId, EditStatus.Draft);
            var draftDrawTable = await this.drawTableRepository.FindByDtoAsync(draftDto);

            // ドラフトのドロー表を削除
            if (draftDrawTable != null)
            {
                await this.drawTableRepository.DeleteAsync(draftDrawTable);
            }

            // 編集中からドラフトを作成
            var editingDto = new DrawTableRepositoryDto(tournamentId, tennisEventId, EditStatus.Editing)
            {
                IncludeEntryDetails = true,
                IncludeEntryPlayers = true,
                IncludeQualifyingDrawSettings = true,
                IncludeMainDrawSettings = true,
                IncludeBlocks = true,
                IncludeGames = true,
                IncludeGameResult = true,
                IncludeOpponents = true,
            };
            draftDrawTable = await this.drawTableRepository.FindByDtoAsync(editingDto, asNoTracking: true);
            draftDrawTable.AsDraft();
            await this.drawTableRepository.AddAsync(draftDrawTable);
        }

        public async Task UpdateToPublish(int tournamentId, string tennisEventId)
        {
            // 公開済みのドロー表を取得
            var publishedDto = new DrawTableRepositoryDto(tournamentId, tennisEventId, EditStatus.Published);
            var publishedDrawTable = await this.drawTableRepository.FindByDtoAsync(publishedDto);

            // 公開済みのドロー表を削除
            if (publishedDrawTable != null)
            {
                await this.drawTableRepository.DeleteAsync(publishedDrawTable);
            }

            // ドラフトから公開済みを作成
            var editingDto = new DrawTableRepositoryDto(tournamentId, tennisEventId, EditStatus.Draft)
            {
                IncludeEntryDetails = true,
                IncludeEntryPlayers = true,
                IncludeQualifyingDrawSettings = true,
                IncludeMainDrawSettings = true,
                IncludeBlocks = true,
                IncludeGames = true,
                IncludeGameResult = true,
                IncludeOpponents = true,
            };
            publishedDrawTable = await this.drawTableRepository.FindByDtoAsync(editingDto, asNoTracking: true);
            publishedDrawTable.AsPublished();
            await this.drawTableRepository.AddAsync(publishedDrawTable);
        }
    }
}
