using JuniorTennis.Domain.DrawTables;
using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.Ranking;
using JuniorTennis.Domain.Repositoies;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.TournamentEntries;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Domain.UseCases.DrawTables;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace JuniorTennis.DomainTests.UseCases.DrawTables
{
    public class DrawTableUseCaseTests
    {
        [Fact]
        public async Task 予選抽選処理が正常に機能してるかテスト()
        {
            #region テストデータ作成
            // テストデータ
            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(new DateTime(2020, 6, 10), new DateTime(2020, 6, 20)),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール本文",
                "メール件名",
                1
            )
            {
                Id = 1
            };

            var qualifyingDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(4),
                numberOfDraws: new NumberOfDraws(16),
                numberOfEntries: new NumberOfEntries(59),
                numberOfWinners: new NumberOfWinners(0),
                tournamentGrade: TournamentGrade.D);

            var mainDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(1),
                numberOfDraws: new NumberOfDraws(16),
                numberOfEntries: new NumberOfEntries(12),
                numberOfWinners: new NumberOfWinners(qualifyingDrawSettings.NumberOfBlocks.Value),
                tournamentGrade: TournamentGrade.B
            );

            var random = new Random();
            var entryNumber = 1;

            var qualifyingSeedNumberLimit = qualifyingDrawSettings.NumberOfBlocks.Value * 2;
            var qualifyingEntryDetails = Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Qualifying,
                    seedNumber: new SeedNumber((entry <= qualifyingSeedNumberLimit ? entry : 0)),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var mainSeedNumberLimit
                = mainDrawSettings.NumberOfDraws.Value <= 4 ? 1
                : mainDrawSettings.NumberOfDraws.Value <= 8 ? 2
                : mainDrawSettings.NumberOfDraws.Value <= 16 ? 4
                : mainDrawSettings.NumberOfDraws.Value <= 32 ? 8
                : mainDrawSettings.NumberOfDraws.Value <= 64 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 128 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 256 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 512 ? 16
                : 1024;
            var mainEntryDetails = Enumerable
                .Range(1, mainDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Main,
                    seedNumber: new SeedNumber((entry <= mainSeedNumberLimit ? entry : 0)),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var entryDetails = Enumerable
                .Concat(qualifyingEntryDetails, mainEntryDetails)
                .ToList();

            var blocks_temp = new List<Block>();
            blocks_temp.AddRange(Enumerable
                .Range(1, mainDrawSettings.NumberOfBlocks.Value)
                .Select(o =>
                {
                    var blockNumber = new BlockNumber(o);
                    var participationClassification = ParticipationClassification.Main;
                    var gameDate = new GameDate(tournament.HoldingDates.First().Value);

                    return new Block(
                        blockNumber,
                        participationClassification,
                        gameDate,
                        mainDrawSettings
                    );
                }));
            blocks_temp.AddRange(Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfBlocks.Value)
                .Select(o =>
                {
                    var blockNumber = new BlockNumber(o);
                    var participationClassification = ParticipationClassification.Qualifying;
                    var gameDate = new GameDate(tournament.HoldingDates.First().Value);

                    return new Block(
                        blockNumber,
                        participationClassification,
                        gameDate,
                        qualifyingDrawSettings
                    );
                }));

            var blocks = new Blocks(blocks_temp);

            var drawTable = new DrawTable(
                tournament: tournament,
                tennisEvent: tournament.TennisEvents.First(),
                tournamentFormat: TournamentFormat.WithQualifying,
                eligiblePlayersType: EligiblePlayersType.AllPlayers,
                entryDetails: entryDetails,
                mainDrawSettings: mainDrawSettings,
                qualifyingDrawSettings: qualifyingDrawSettings,
                blocks: blocks,
                editStatus: EditStatus.Editing
            );
            #endregion テストデータ作成

            var entryNumbersOfSeed = drawTable.Blocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.PlayerClassification == PlayerClassification.Seed)
                .Select(o => o.EntryNumber);

            var entryNumbersOfGeneral = drawTable.Blocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.PlayerClassification == PlayerClassification.General)
                .Select(o => o.EntryNumber);

            var mockDrawTableRepository = new Mock<IDrawTableRepository>();
            var mockTournamentRepository = new Mock<ITournamentRepository>();
            var mockTournamentEntryRepository = new Mock<ITournamentEntryRepository>();
            var mockRankingRepository = new Mock<IRankingRepository>();

            mockDrawTableRepository.Setup(r => r.FindByDtoAsync(It.IsAny<DrawTableRepositoryDto>(), false))
                .ReturnsAsync(drawTable)
                .Verifiable();
            mockDrawTableRepository.Setup(r => r.UpdateAsync(drawTable))
                .ReturnsAsync(drawTable)
                .Verifiable();

            var usecase = new DrawTableUseCase(
                mockDrawTableRepository.Object,
                mockTournamentRepository.Object,
                mockTournamentEntryRepository.Object,
                mockRankingRepository.Object
            );

            // Act
            await usecase.ExecuteDrawingQualifying(1, "1_1_1");
            var drawTable2 = await usecase.GetDrawTable(default);
            var opponents = drawTable2.Blocks
                .GetQualifyingBlocks()
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents);

            // ・シード位置にシード選手が割り当てられているか
            // ・シード選手の対戦相手に優先的に BYE が割り当てられているか
            // ・残りのシード選手の対戦相手に一般選手が割り当てられているか
            var block1game1 = opponents.Where(o => o.BlockNumber.Value == 1 && o.GameNumber.Value == 1);
            var block1game1draw1 = block1game1.First(o => o.DrawNumber.Value == 1);
            var block1game1draw2 = block1game1.First(o => o.DrawNumber.Value == 2);
            Assert.Equal(block1game1draw1.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(block1game1draw1.SeedNumber, new SeedNumber(1));
            Assert.Equal(block1game1draw2.PlayerClassification, PlayerClassification.Bye);

            var block2game1 = opponents.Where(o => o.BlockNumber.Value == 2 && o.GameNumber.Value == 1);
            var block2game1draw1 = block2game1.First(o => o.DrawNumber.Value == 1);
            var block2game1draw2 = block2game1.First(o => o.DrawNumber.Value == 2);
            Assert.Equal(block2game1draw1.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(block2game1draw1.SeedNumber, new SeedNumber(2));
            Assert.Equal(block2game1draw2.PlayerClassification, PlayerClassification.Bye);

            var block3game1 = opponents.Where(o => o.BlockNumber.Value == 3 && o.GameNumber.Value == 1);
            var block3game1draw1 = block3game1.First(o => o.DrawNumber.Value == 1);
            var block3game1draw2 = block3game1.First(o => o.DrawNumber.Value == 2);
            Assert.Equal(block3game1draw1.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(block3game1draw1.SeedNumber, new SeedNumber(3));
            Assert.Equal(block3game1draw2.PlayerClassification, PlayerClassification.Bye);

            var block4game1 = opponents.Where(o => o.BlockNumber.Value == 4 && o.GameNumber.Value == 1);
            var block4game1draw1 = block4game1.First(o => o.DrawNumber.Value == 1);
            var block4game1draw2 = block4game1.First(o => o.DrawNumber.Value == 2);
            Assert.Equal(block4game1draw1.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(block4game1draw1.SeedNumber, new SeedNumber(4));
            Assert.Equal(block4game1draw2.PlayerClassification, PlayerClassification.Bye);

            var block1game8 = opponents.Where(o => o.BlockNumber.Value == 1 && o.GameNumber.Value == 8);
            var block1game8draw1 = block1game8.First(o => o.DrawNumber.Value == 15);
            var block1game8draw2 = block1game8.First(o => o.DrawNumber.Value == 16);
            Assert.Equal(block1game8draw1.PlayerClassification, PlayerClassification.General);
            Assert.Equal(block1game8draw2.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(block1game8draw2.SeedNumber, new SeedNumber[] { new SeedNumber(5), new SeedNumber(6), new SeedNumber(7), new SeedNumber(8) });

            var block2game8 = opponents.Where(o => o.BlockNumber.Value == 2 && o.GameNumber.Value == 8);
            var block2game8draw1 = block2game8.First(o => o.DrawNumber.Value == 15);
            var block2game8draw2 = block2game8.First(o => o.DrawNumber.Value == 16);
            Assert.Equal(block2game8draw1.PlayerClassification, PlayerClassification.General);
            Assert.Equal(block2game8draw2.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(block2game8draw2.SeedNumber, new SeedNumber[] { new SeedNumber(5), new SeedNumber(6), new SeedNumber(7), new SeedNumber(8) });

            var block3game8 = opponents.Where(o => o.BlockNumber.Value == 3 && o.GameNumber.Value == 8);
            var block3game8draw1 = block3game8.First(o => o.DrawNumber.Value == 15);
            var block3game8draw2 = block3game8.First(o => o.DrawNumber.Value == 16);
            Assert.Equal(block3game8draw1.PlayerClassification, PlayerClassification.General);
            Assert.Equal(block3game8draw2.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(block3game8draw2.SeedNumber, new SeedNumber[] { new SeedNumber(5), new SeedNumber(6), new SeedNumber(7), new SeedNumber(8) });

            var block4game8 = opponents.Where(o => o.BlockNumber.Value == 4 && o.GameNumber.Value == 8);
            var block4game8draw1 = block4game8.First(o => o.DrawNumber.Value == 15);
            var block4game8draw2 = block4game8.First(o => o.DrawNumber.Value == 16);
            Assert.Equal(block4game8draw1.PlayerClassification, PlayerClassification.Bye);
            Assert.Equal(block4game8draw2.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(block4game8draw2.SeedNumber, new SeedNumber[] { new SeedNumber(5), new SeedNumber(6), new SeedNumber(7), new SeedNumber(8) });
        }

        [Fact]
        public async Task 予選勝者取り込みが正常に機能してるかテスト()
        {
            // テストデータ
            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(new DateTime(2020, 6, 10), new DateTime(2020, 6, 20)),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール本文",
                "メール件名",
                1
            )
            {
                Id = 1
            };

            var qualifyingDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(4),
                numberOfDraws: new NumberOfDraws(16),
                numberOfEntries: new NumberOfEntries(59),
                numberOfWinners: new NumberOfWinners(0),
                tournamentGrade: TournamentGrade.D);

            var mainDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(1),
                numberOfDraws: new NumberOfDraws(16),
                numberOfEntries: new NumberOfEntries(12),
                numberOfWinners: new NumberOfWinners(qualifyingDrawSettings.NumberOfBlocks.Value),
                tournamentGrade: TournamentGrade.B
            );

            var random = new Random();
            var entryNumber = 1;

            var qualifyingSeedNumberLimit = qualifyingDrawSettings.NumberOfBlocks.Value * 2;
            var qualifyingEntryDetails = Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Qualifying,
                    seedNumber: new SeedNumber(entry <= qualifyingSeedNumberLimit ? entry : 0),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var mainSeedNumberLimit
                = mainDrawSettings.NumberOfDraws.Value <= 4 ? 1
                : mainDrawSettings.NumberOfDraws.Value <= 8 ? 2
                : mainDrawSettings.NumberOfDraws.Value <= 16 ? 4
                : mainDrawSettings.NumberOfDraws.Value <= 32 ? 8
                : mainDrawSettings.NumberOfDraws.Value <= 64 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 128 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 256 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 512 ? 16
                : 1024;
            var mainEntryDetails = Enumerable
                .Range(1, mainDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Main,
                    seedNumber: new SeedNumber((entry <= mainSeedNumberLimit ? entry : 0)),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var entryDetails = Enumerable
                .Concat(qualifyingEntryDetails, mainEntryDetails)
                .ToList();

            var blocks_temp = new List<Block>();
            blocks_temp.AddRange(Enumerable
                .Range(1, mainDrawSettings.NumberOfBlocks.Value)
                .Select(o =>
                {
                    var blockNumber = new BlockNumber(o);
                    var participationClassification = ParticipationClassification.Main;
                    var gameDate = new GameDate(tournament.HoldingDates.First().Value);

                    return new Block(
                        blockNumber,
                        participationClassification,
                        gameDate,
                        mainDrawSettings
                    );
                }));
            blocks_temp.AddRange(Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfBlocks.Value)
                .Select(o =>
                {
                    var blockNumber = new BlockNumber(o);
                    var participationClassification = ParticipationClassification.Qualifying;
                    var gameDate = new GameDate(tournament.HoldingDates.First().Value);

                    return new Block(
                        blockNumber,
                        participationClassification,
                        gameDate,
                        qualifyingDrawSettings
                    );
                }));

            var blocks = new Blocks(blocks_temp);

            var drawTable = new DrawTable(
                tournament: tournament,
                tennisEvent: tournament.TennisEvents.First(),
                tournamentFormat: TournamentFormat.WithQualifying,
                eligiblePlayersType: EligiblePlayersType.AllPlayers,
                entryDetails: entryDetails,
                mainDrawSettings: mainDrawSettings,
                qualifyingDrawSettings: qualifyingDrawSettings,
                blocks: blocks,
                editStatus: EditStatus.Editing
            );

            var entryNumbersOfSeed = drawTable.Blocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.PlayerClassification == PlayerClassification.Seed)
                .Select(o => o.EntryNumber);

            var entryNumbersOfGeneral = drawTable.Blocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.PlayerClassification == PlayerClassification.General)
                .Select(o => o.EntryNumber);

            var mockDrawTableRepository = new Mock<IDrawTableRepository>();
            var mockTournamentRepository = new Mock<ITournamentRepository>();
            var mockTournamentEntryRepository = new Mock<ITournamentEntryRepository>();
            var mockRankingRepository = new Mock<IRankingRepository>();

            mockDrawTableRepository.Setup(r => r.FindByDtoAsync(It.IsAny<DrawTableRepositoryDto>(), false))
                .ReturnsAsync(drawTable)
                .Verifiable();
            mockDrawTableRepository.Setup(r => r.UpdateAsync(drawTable))
                .ReturnsAsync(drawTable)
                .Verifiable();

            var usecase = new DrawTableUseCase(
                mockDrawTableRepository.Object,
                mockTournamentRepository.Object,
                mockTournamentEntryRepository.Object,
                mockRankingRepository.Object
            );

            // Act
            await usecase.ExecuteDrawingQualifying(1, "1_1_1");
            var qualifyingBlocks = drawTable.Blocks.GetQualifyingBlocks();
            var opponents = qualifyingBlocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents);

            var block1game1draw1 = opponents.First(o => o.BlockNumber.Value == 1 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block1game8draw2 = opponents.First(o => o.BlockNumber.Value == 1 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game1 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game1.AssignOpponent(block1game1draw1);
            game1.AssignOpponent(block1game8draw2);
            var gameResult1 = new GameResult();
            gameResult1.UpdateGameResult(
                GameStatus.Done,
                block1game1draw1.PlayerClassification,
                block1game1draw1.EntryNumber,
                new GameScore("123")
            );
            game1.SetGameResult(gameResult1);
            qualifyingBlocks[0].Games.Add(game1);

            var block2game1draw1 = opponents.First(o => o.BlockNumber.Value == 2 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block2game8draw2 = opponents.First(o => o.BlockNumber.Value == 2 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game2 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game2.AssignOpponent(block2game1draw1);
            game2.AssignOpponent(block2game8draw2);
            var gameResult2 = new GameResult();
            gameResult2.UpdateGameResult(
                GameStatus.Done,
                block2game1draw1.PlayerClassification,
                block2game1draw1.EntryNumber,
                new GameScore("123")
            );
            game2.SetGameResult(gameResult2);
            qualifyingBlocks[1].Games.Add(game2);

            var block3game1draw1 = opponents.First(o => o.BlockNumber.Value == 3 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block3game8draw2 = opponents.First(o => o.BlockNumber.Value == 3 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game3 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game3.AssignOpponent(block3game1draw1);
            game3.AssignOpponent(block3game8draw2);
            var gameResult3 = new GameResult();
            gameResult3.UpdateGameResult(
                GameStatus.Done,
                block3game1draw1.PlayerClassification,
                block3game1draw1.EntryNumber,
                new GameScore("123")
            );
            game3.SetGameResult(gameResult3);
            qualifyingBlocks[2].Games.Add(game3);

            var block4game1draw1 = opponents.First(o => o.BlockNumber.Value == 4 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block4game8draw2 = opponents.First(o => o.BlockNumber.Value == 4 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game4 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game4.AssignOpponent(block4game1draw1);
            game4.AssignOpponent(block4game8draw2);
            var gameResult4 = new GameResult();
            gameResult4.UpdateGameResult(
                GameStatus.Done,
                block4game1draw1.PlayerClassification,
                block4game1draw1.EntryNumber,
                new GameScore("123")
            );
            game4.SetGameResult(gameResult4);
            qualifyingBlocks[3].Games.Add(game4);

            await usecase.IntakeQualifyingWinners(1, "1_1_1");

            var mainEntryDetails2 = drawTable.EntryDetails
                .Where(o => o.ParticipationClassification == ParticipationClassification.Main)
                .OrderBy(o => o.EntryNumber.Value)
                .ToList();

            // ・予選勝者が本戦出場者の一覧に含まれているか。
            // ・本戦出場者の件数が本戦のドロー数と一致しているか。
            Assert.Contains(mainEntryDetails2, o => o.EntryNumber == block1game1draw1.EntryNumber);
            Assert.Contains(mainEntryDetails2, o => o.EntryNumber == block2game1draw1.EntryNumber);
            Assert.Contains(mainEntryDetails2, o => o.EntryNumber == block3game1draw1.EntryNumber);
            Assert.Contains(mainEntryDetails2, o => o.EntryNumber == block4game1draw1.EntryNumber);
            Assert.Equal(mainDrawSettings.NumberOfDraws.Value, mainEntryDetails2.Count());
        }

        [Fact]
        public async Task 本戦抽選処理が正常に機能してるかテスト()
        {
            // テストデータ
            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(new DateTime(2020, 6, 10), new DateTime(2020, 6, 20)),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール本文",
                "メール件名",
                1
            )
            {
                Id = 1
            };

            var qualifyingDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(4),
                numberOfDraws: new NumberOfDraws(16),
                numberOfEntries: new NumberOfEntries(59),
                numberOfWinners: new NumberOfWinners(0),
                tournamentGrade: TournamentGrade.D);

            var mainDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(1),
                numberOfDraws: new NumberOfDraws(16),
                numberOfEntries: new NumberOfEntries(9),
                numberOfWinners: new NumberOfWinners(qualifyingDrawSettings.NumberOfBlocks.Value),
                tournamentGrade: TournamentGrade.B
            );

            var random = new Random();
            var entryNumber = 1;

            var qualifyingSeedNumberLimit = qualifyingDrawSettings.NumberOfBlocks.Value * 2;
            var qualifyingEntryDetails = Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Qualifying,
                    seedNumber: new SeedNumber(entry <= qualifyingSeedNumberLimit ? entry : 0),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var mainSeedNumberLimit
                = mainDrawSettings.NumberOfDraws.Value <= 4 ? 1
                : mainDrawSettings.NumberOfDraws.Value <= 8 ? 2
                : mainDrawSettings.NumberOfDraws.Value <= 16 ? 4
                : mainDrawSettings.NumberOfDraws.Value <= 32 ? 8
                : mainDrawSettings.NumberOfDraws.Value <= 64 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 128 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 256 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 512 ? 16
                : 1024;
            var mainEntryDetails = Enumerable
                .Range(1, mainDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Main,
                    seedNumber: new SeedNumber(entry <= mainSeedNumberLimit ? entry : 0),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var entryDetails = Enumerable
                .Concat(qualifyingEntryDetails, mainEntryDetails)
                .ToList();

            var blocks_temp = new List<Block>();
            blocks_temp.AddRange(Enumerable
                .Range(1, mainDrawSettings.NumberOfBlocks.Value)
                .Select(o =>
                {
                    var blockNumber = new BlockNumber(o);
                    var participationClassification = ParticipationClassification.Main;
                    var gameDate = new GameDate(tournament.HoldingDates.First().Value);

                    return new Block(
                        blockNumber,
                        participationClassification,
                        gameDate,
                        mainDrawSettings
                    );
                }));
            blocks_temp.AddRange(Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfBlocks.Value)
                .Select(o =>
                {
                    var blockNumber = new BlockNumber(o);
                    var participationClassification = ParticipationClassification.Qualifying;
                    var gameDate = new GameDate(tournament.HoldingDates.First().Value);

                    return new Block(
                        blockNumber,
                        participationClassification,
                        gameDate,
                        qualifyingDrawSettings
                    );
                }));

            var blocks = new Blocks(blocks_temp);

            var drawTable = new DrawTable(
                tournament: tournament,
                tennisEvent: tournament.TennisEvents.First(),
                tournamentFormat: TournamentFormat.WithQualifying,
                eligiblePlayersType: EligiblePlayersType.AllPlayers,
                entryDetails: entryDetails,
                mainDrawSettings: mainDrawSettings,
                qualifyingDrawSettings: qualifyingDrawSettings,
                blocks: blocks,
                editStatus: EditStatus.Editing
            );

            var entryNumbersOfSeed = drawTable.Blocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.PlayerClassification == PlayerClassification.Seed)
                .Select(o => o.EntryNumber);

            var entryNumbersOfGeneral = drawTable.Blocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.PlayerClassification == PlayerClassification.General)
                .Select(o => o.EntryNumber);

            var mockDrawTableRepository = new Mock<IDrawTableRepository>();
            var mockTournamentRepository = new Mock<ITournamentRepository>();
            var mockTournamentEntryRepository = new Mock<ITournamentEntryRepository>();
            var mockRankingRepository = new Mock<IRankingRepository>();

            mockDrawTableRepository.Setup(r => r.FindByDtoAsync(It.IsAny<DrawTableRepositoryDto>(), false))
                .ReturnsAsync(drawTable)
                .Verifiable();
            mockDrawTableRepository.Setup(r => r.UpdateAsync(drawTable))
                .ReturnsAsync(drawTable)
                .Verifiable();

            var usecase = new DrawTableUseCase(
                mockDrawTableRepository.Object,
                mockTournamentRepository.Object,
                mockTournamentEntryRepository.Object,
                mockRankingRepository.Object
            );

            // Act
            await usecase.ExecuteDrawingQualifying(1, "1_1_1");
            var qualifyingBlocks = drawTable.Blocks.GetQualifyingBlocks();
            var opponents = qualifyingBlocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents);

            var block1game1draw1 = opponents.First(o => o.BlockNumber.Value == 1 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block1game8draw2 = opponents.First(o => o.BlockNumber.Value == 1 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game1 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game1.AssignOpponent(block1game1draw1);
            game1.AssignOpponent(block1game8draw2);
            var gameResult1 = new GameResult();
            gameResult1.UpdateGameResult(
                GameStatus.Done,
                block1game1draw1.PlayerClassification,
                block1game1draw1.EntryNumber,
                new GameScore("123")
            );
            game1.SetGameResult(gameResult1);
            qualifyingBlocks[0].Games.Add(game1);

            var block2game1draw1 = opponents.First(o => o.BlockNumber.Value == 2 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block2game8draw2 = opponents.First(o => o.BlockNumber.Value == 2 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game2 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game2.AssignOpponent(block2game1draw1);
            game2.AssignOpponent(block2game8draw2);
            var gameResult2 = new GameResult();
            gameResult2.UpdateGameResult(
                GameStatus.Done,
                block2game1draw1.PlayerClassification,
                block2game1draw1.EntryNumber,
                new GameScore("123")
            );
            game2.SetGameResult(gameResult2);
            qualifyingBlocks[1].Games.Add(game2);

            var block3game1draw1 = opponents.First(o => o.BlockNumber.Value == 3 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block3game8draw2 = opponents.First(o => o.BlockNumber.Value == 3 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game3 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game3.AssignOpponent(block3game1draw1);
            game3.AssignOpponent(block3game8draw2);
            var gameResult3 = new GameResult();
            gameResult3.UpdateGameResult(
                GameStatus.Done,
                block3game1draw1.PlayerClassification,
                block3game1draw1.EntryNumber,
                new GameScore("123")
            );
            game3.SetGameResult(gameResult3);
            qualifyingBlocks[2].Games.Add(game3);

            var block4game1draw1 = opponents.First(o => o.BlockNumber.Value == 4 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block4game8draw2 = opponents.First(o => o.BlockNumber.Value == 4 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game4 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game4.AssignOpponent(block4game1draw1);
            game4.AssignOpponent(block4game8draw2);
            var gameResult4 = new GameResult();
            gameResult4.UpdateGameResult(
                GameStatus.Done,
                block4game1draw1.PlayerClassification,
                block4game1draw1.EntryNumber,
                new GameScore("123")
            );
            game4.SetGameResult(gameResult4);
            qualifyingBlocks[3].Games.Add(game4);

            await usecase.IntakeQualifyingWinners(1, "1_1_1");
            await usecase.ExecuteDrawingMain(1, "1_1_1");

            var mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            var drawA = default(Opponent);
            var drawB = default(Opponent);

            // ・シード位置にシード選手が割り当てられているか
            // ・シード選手の対戦相手に優先的に BYE が割り当てられているか
            // ・残りのシード選手の対戦相手に一般選手が割り当てられているか
            drawA = opponents.First(o => o.DrawNumber.Value == 1);
            drawB = opponents.First(o => o.DrawNumber.Value == 2);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawA.SeedNumber, new SeedNumber(1));
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Bye);

            drawA = opponents.First(o => o.DrawNumber.Value == 3);
            drawB = opponents.First(o => o.DrawNumber.Value == 4);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 5);
            drawB = opponents.First(o => o.DrawNumber.Value == 6);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawA.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Bye);

            drawA = opponents.First(o => o.DrawNumber.Value == 7);
            drawB = opponents.First(o => o.DrawNumber.Value == 8);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 9);
            drawB = opponents.First(o => o.DrawNumber.Value == 10);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 11);
            drawB = opponents.First(o => o.DrawNumber.Value == 12);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawB.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });

            drawA = opponents.First(o => o.DrawNumber.Value == 13);
            drawB = opponents.First(o => o.DrawNumber.Value == 14);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 15);
            drawB = opponents.First(o => o.DrawNumber.Value == 16);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Bye);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawB.SeedNumber, new SeedNumber(2));
        }

        [Fact]
        public async Task 結果入力処理が正常に機能してるかテスト()
        {
            #region テストデータ作成
            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(new DateTime(2020, 6, 10), new DateTime(2020, 6, 20)),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール本文",
                "メール件名",
                1
            )
            {
                Id = 1
            };

            var qualifyingDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(4),
                numberOfDraws: new NumberOfDraws(16),
                numberOfEntries: new NumberOfEntries(59),
                numberOfWinners: new NumberOfWinners(0),
                tournamentGrade: TournamentGrade.D);

            var mainDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(1),
                numberOfDraws: new NumberOfDraws(16),
                numberOfEntries: new NumberOfEntries(12),
                numberOfWinners: new NumberOfWinners(qualifyingDrawSettings.NumberOfBlocks.Value),
                tournamentGrade: TournamentGrade.B
            );

            var random = new Random();
            var entryNumber = 1;

            var qualifyingSeedNumberLimit = qualifyingDrawSettings.NumberOfBlocks.Value * 2;
            var qualifyingEntryDetails = Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Qualifying,
                    seedNumber: new SeedNumber(entry <= qualifyingSeedNumberLimit ? entry : 0),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var mainSeedNumberLimit
                = mainDrawSettings.NumberOfDraws.Value <= 4 ? 1
                : mainDrawSettings.NumberOfDraws.Value <= 8 ? 2
                : mainDrawSettings.NumberOfDraws.Value <= 16 ? 4
                : mainDrawSettings.NumberOfDraws.Value <= 32 ? 8
                : mainDrawSettings.NumberOfDraws.Value <= 64 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 128 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 256 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 512 ? 16
                : 1024;
            var mainEntryDetails = Enumerable
                .Range(1, mainDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Main,
                    seedNumber: new SeedNumber(entry <= mainSeedNumberLimit ? entry : 0),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var entryDetails = Enumerable
                .Concat(qualifyingEntryDetails, mainEntryDetails)
                .ToList();

            var blocks_temp = new List<Block>();
            blocks_temp.AddRange(Enumerable
                .Range(1, mainDrawSettings.NumberOfBlocks.Value)
                .Select(o =>
                {
                    var blockNumber = new BlockNumber(o);
                    var participationClassification = ParticipationClassification.Main;
                    var gameDate = new GameDate(tournament.HoldingDates.First().Value);

                    return new Block(
                        blockNumber,
                        participationClassification,
                        gameDate,
                        mainDrawSettings
                    );
                }));
            blocks_temp.AddRange(Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfBlocks.Value)
                .Select(o =>
                {
                    var blockNumber = new BlockNumber(o);
                    var participationClassification = ParticipationClassification.Qualifying;
                    var gameDate = new GameDate(tournament.HoldingDates.First().Value);

                    return new Block(
                        blockNumber,
                        participationClassification,
                        gameDate,
                        qualifyingDrawSettings
                    );
                }));

            var blocks = new Blocks(blocks_temp);

            var drawTable = new DrawTable(
                tournament: tournament,
                tennisEvent: tournament.TennisEvents.First(),
                tournamentFormat: TournamentFormat.WithQualifying,
                eligiblePlayersType: EligiblePlayersType.AllPlayers,
                entryDetails: entryDetails,
                mainDrawSettings: mainDrawSettings,
                qualifyingDrawSettings: qualifyingDrawSettings,
                blocks: blocks,
                editStatus: EditStatus.Editing
            );
            #endregion テストデータ作成

            var entryNumbersOfSeed = drawTable.Blocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.PlayerClassification == PlayerClassification.Seed)
                .Select(o => o.EntryNumber);

            var entryNumbersOfGeneral = drawTable.Blocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.PlayerClassification == PlayerClassification.General)
                .Select(o => o.EntryNumber);

            var mockDrawTableRepository = new Mock<IDrawTableRepository>();
            var mockTournamentRepository = new Mock<ITournamentRepository>();
            var mockTournamentEntryRepository = new Mock<ITournamentEntryRepository>();
            var mockRankingRepository = new Mock<IRankingRepository>();

            mockDrawTableRepository.Setup(r => r.FindByDtoAsync(It.IsAny<DrawTableRepositoryDto>(), false))
                .ReturnsAsync(drawTable)
                .Verifiable();
            mockDrawTableRepository.Setup(r => r.UpdateAsync(drawTable))
                .ReturnsAsync(drawTable)
                .Verifiable();

            var usecase = new DrawTableUseCase(
                mockDrawTableRepository.Object,
                mockTournamentRepository.Object,
                mockTournamentEntryRepository.Object,
                mockRankingRepository.Object
            );

            // Act
            await usecase.ExecuteDrawingQualifying(1, "1_1_1");
            var qualifyingBlocks = drawTable.Blocks.GetQualifyingBlocks();
            var opponents = qualifyingBlocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents);

            var block1game1draw1 = opponents.First(o => o.BlockNumber.Value == 1 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block1game8draw2 = opponents.First(o => o.BlockNumber.Value == 1 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game1 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game1.AssignOpponent(block1game1draw1);
            game1.AssignOpponent(block1game8draw2);
            var gameResult1 = new GameResult();
            gameResult1.UpdateGameResult(
                GameStatus.Done,
                block1game1draw1.PlayerClassification,
                block1game1draw1.EntryNumber,
                new GameScore("123")
            );
            game1.SetGameResult(gameResult1);
            qualifyingBlocks[0].Games.Add(game1);

            var block2game1draw1 = opponents.First(o => o.BlockNumber.Value == 2 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block2game8draw2 = opponents.First(o => o.BlockNumber.Value == 2 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game2 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game2.AssignOpponent(block2game1draw1);
            game2.AssignOpponent(block2game8draw2);
            var gameResult2 = new GameResult();
            gameResult2.UpdateGameResult(
                GameStatus.Done,
                block2game1draw1.PlayerClassification,
                block2game1draw1.EntryNumber,
                new GameScore("123")
            );
            game2.SetGameResult(gameResult2);
            qualifyingBlocks[1].Games.Add(game2);

            var block3game1draw1 = opponents.First(o => o.BlockNumber.Value == 3 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block3game8draw2 = opponents.First(o => o.BlockNumber.Value == 3 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game3 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game3.AssignOpponent(block3game1draw1);
            game3.AssignOpponent(block3game8draw2);
            var gameResult3 = new GameResult();
            gameResult3.UpdateGameResult(
                GameStatus.Done,
                block3game1draw1.PlayerClassification,
                block3game1draw1.EntryNumber,
                new GameScore("123")
            );
            game3.SetGameResult(gameResult3);
            qualifyingBlocks[2].Games.Add(game3);

            var block4game1draw1 = opponents.First(o => o.BlockNumber.Value == 4 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block4game8draw2 = opponents.First(o => o.BlockNumber.Value == 4 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game4 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game4.AssignOpponent(block4game1draw1);
            game4.AssignOpponent(block4game8draw2);
            var gameResult4 = new GameResult();
            gameResult4.UpdateGameResult(
                GameStatus.Done,
                block4game1draw1.PlayerClassification,
                block4game1draw1.EntryNumber,
                new GameScore("123")
            );
            game4.SetGameResult(gameResult4);
            qualifyingBlocks[3].Games.Add(game4);

            await usecase.IntakeQualifyingWinners(1, "1_1_1");
            await usecase.ExecuteDrawingMain(1, "1_1_1");

            var mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            var drawA = default(Opponent);
            var drawB = default(Opponent);

            // ・シード位置にシード選手が割り当てられているか
            // ・シード選手の対戦相手に優先的に BYE が割り当てられているか
            // ・残りのシード選手の対戦相手に一般選手が割り当てられているか
            drawA = opponents.First(o => o.DrawNumber.Value == 1);
            drawB = opponents.First(o => o.DrawNumber.Value == 2);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawA.SeedNumber, new SeedNumber(1));
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 3);
            drawB = opponents.First(o => o.DrawNumber.Value == 4);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 5);
            drawB = opponents.First(o => o.DrawNumber.Value == 6);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawA.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 7);
            drawB = opponents.First(o => o.DrawNumber.Value == 8);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 9);
            drawB = opponents.First(o => o.DrawNumber.Value == 10);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 11);
            drawB = opponents.First(o => o.DrawNumber.Value == 12);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawB.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });

            drawA = opponents.First(o => o.DrawNumber.Value == 13);
            drawB = opponents.First(o => o.DrawNumber.Value == 14);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 15);
            drawB = opponents.First(o => o.DrawNumber.Value == 16);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawB.SeedNumber, new SeedNumber(2));

            // 第1ラウンド
            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawA = opponents.First(o => o.DrawNumber.Value == 1);
            var blockNumber = new BlockNumber(1);
            var gameNumber = new GameNumber(1);
            var gameResult = new GameResult();
            var gameStatus = GameStatus.Done;
            var gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawA.PlayerClassification, drawA.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawB = opponents.First(o => o.DrawNumber.Value == 4);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(2);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawB.PlayerClassification, drawB.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            drawA = opponents.FirstOrDefault(o => o.GameNumber.Value == 9 && o.EntryNumber == drawA.EntryNumber);
            drawB = opponents.FirstOrDefault(o => o.GameNumber.Value == 9 && o.EntryNumber == drawB.EntryNumber);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawA.SeedNumber, new SeedNumber(1));
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawA = opponents.First(o => o.DrawNumber.Value == 5);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(3);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawA.PlayerClassification, drawA.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawB = opponents.First(o => o.DrawNumber.Value == 7);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(4);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawB.PlayerClassification, drawB.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            drawA = opponents.FirstOrDefault(o => o.GameNumber.Value == 10 && o.EntryNumber == drawA.EntryNumber);
            drawB = opponents.FirstOrDefault(o => o.GameNumber.Value == 10 && o.EntryNumber == drawB.EntryNumber);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawA.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawA = opponents.First(o => o.DrawNumber.Value == 9);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(5);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawA.PlayerClassification, drawA.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawB = opponents.First(o => o.DrawNumber.Value == 12);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(6);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawB.PlayerClassification, drawB.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            drawA = opponents.FirstOrDefault(o => o.GameNumber.Value == 11 && o.EntryNumber == drawA.EntryNumber);
            drawB = opponents.FirstOrDefault(o => o.GameNumber.Value == 11 && o.EntryNumber == drawB.EntryNumber);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawB.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawA = opponents.First(o => o.DrawNumber.Value == 14);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(7);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawA.PlayerClassification, drawA.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawB = opponents.First(o => o.DrawNumber.Value == 16);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(8);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawB.PlayerClassification, drawB.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            drawA = opponents.FirstOrDefault(o => o.GameNumber.Value == 12 && o.EntryNumber == drawA.EntryNumber);
            drawB = opponents.FirstOrDefault(o => o.GameNumber.Value == 12 && o.EntryNumber == drawB.EntryNumber);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawB.SeedNumber, new SeedNumber(2));

            // 第2ラウンド
            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawA = opponents.First(o => o.GameNumber.Value == 9 && o.DrawNumber.Value == 1);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(9);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawA.PlayerClassification, drawA.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawB = opponents.First(o => o.GameNumber.Value == 10 && o.DrawNumber.Value == 1);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(10);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawB.PlayerClassification, drawB.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            drawA = opponents.FirstOrDefault(o => o.GameNumber.Value == 13 && o.DrawNumber.Value == 1);
            drawB = opponents.FirstOrDefault(o => o.GameNumber.Value == 13 && o.DrawNumber.Value == 2);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawA.SeedNumber, new SeedNumber(1));
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawB.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawA = opponents.First(o => o.GameNumber.Value == 11 && o.DrawNumber.Value == 2);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(11);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawA.PlayerClassification, drawA.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawB = opponents.First(o => o.GameNumber.Value == 12 && o.DrawNumber.Value == 2);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(12);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawB.PlayerClassification, drawB.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            drawA = opponents.FirstOrDefault(o => o.GameNumber.Value == 14 && o.DrawNumber.Value == 1);
            drawB = opponents.FirstOrDefault(o => o.GameNumber.Value == 14 && o.DrawNumber.Value == 2);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawA.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawB.SeedNumber, new SeedNumber(2));

            // 第3ラウンド
            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawA = opponents.First(o => o.GameNumber.Value == 13 && o.DrawNumber.Value == 1);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(13);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawA.PlayerClassification, drawA.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawB = opponents.First(o => o.GameNumber.Value == 14 && o.DrawNumber.Value == 2);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(14);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawB.PlayerClassification, drawB.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            drawA = opponents.FirstOrDefault(o => o.GameNumber.Value == 15 && o.DrawNumber.Value == 1);
            drawB = opponents.FirstOrDefault(o => o.GameNumber.Value == 15 && o.DrawNumber.Value == 2);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawA.SeedNumber, new SeedNumber(1));
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawB.SeedNumber, new SeedNumber(2));
        }

        [Fact]
        public async Task 結果入力処理が正常に機能してるかテストNotPlayedとかを絡める()
        {
            // テストデータ
            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(new DateTime(2020, 6, 10), new DateTime(2020, 6, 20)),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール本文",
                "メール件名",
                1
            )
            {
                Id = 1
            };

            var qualifyingDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(4),
                numberOfDraws: new NumberOfDraws(16),
                numberOfEntries: new NumberOfEntries(59),
                numberOfWinners: new NumberOfWinners(0),
                tournamentGrade: TournamentGrade.D);

            var mainDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(1),
                numberOfDraws: new NumberOfDraws(16),
                numberOfEntries: new NumberOfEntries(11),
                numberOfWinners: new NumberOfWinners(qualifyingDrawSettings.NumberOfBlocks.Value),
                tournamentGrade: TournamentGrade.B
            );

            var random = new Random();
            var entryNumber = 1;

            var qualifyingSeedNumberLimit = qualifyingDrawSettings.NumberOfBlocks.Value * 2;
            var qualifyingEntryDetails = Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Qualifying,
                    seedNumber: new SeedNumber(entry <= qualifyingSeedNumberLimit ? entry : 0),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var mainSeedNumberLimit
                = mainDrawSettings.NumberOfDraws.Value <= 4 ? 1
                : mainDrawSettings.NumberOfDraws.Value <= 8 ? 2
                : mainDrawSettings.NumberOfDraws.Value <= 16 ? 4
                : mainDrawSettings.NumberOfDraws.Value <= 32 ? 8
                : mainDrawSettings.NumberOfDraws.Value <= 64 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 128 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 256 ? 16
                : mainDrawSettings.NumberOfDraws.Value <= 512 ? 16
                : 1024;
            var mainEntryDetails = Enumerable
                .Range(1, mainDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Main,
                    seedNumber: new SeedNumber(entry <= mainSeedNumberLimit ? entry : 0),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var entryDetails = Enumerable
                .Concat(qualifyingEntryDetails, mainEntryDetails)
                .ToList();

            var blocks_temp = new List<Block>();
            blocks_temp.AddRange(Enumerable
                .Range(1, mainDrawSettings.NumberOfBlocks.Value)
                .Select(o =>
                {
                    var blockNumber = new BlockNumber(o);
                    var participationClassification = ParticipationClassification.Main;
                    var gameDate = new GameDate(tournament.HoldingDates.First().Value);

                    return new Block(
                        blockNumber,
                        participationClassification,
                        gameDate,
                        mainDrawSettings
                    );
                }));
            blocks_temp.AddRange(Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfBlocks.Value)
                .Select(o =>
                {
                    var blockNumber = new BlockNumber(o);
                    var participationClassification = ParticipationClassification.Qualifying;
                    var gameDate = new GameDate(tournament.HoldingDates.First().Value);

                    return new Block(
                        blockNumber,
                        participationClassification,
                        gameDate,
                        qualifyingDrawSettings
                    );
                }));

            var blocks = new Blocks(blocks_temp);

            var drawTable = new DrawTable(
                tournament: tournament,
                tennisEvent: tournament.TennisEvents.First(),
                tournamentFormat: TournamentFormat.WithQualifying,
                eligiblePlayersType: EligiblePlayersType.AllPlayers,
                entryDetails: entryDetails,
                mainDrawSettings: mainDrawSettings,
                qualifyingDrawSettings: qualifyingDrawSettings,
                blocks: blocks,
                editStatus: EditStatus.Editing
            );

            var entryNumbersOfSeed = drawTable.Blocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.PlayerClassification == PlayerClassification.Seed)
                .Select(o => o.EntryNumber);

            var entryNumbersOfGeneral = drawTable.Blocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents)
                .Where(o => o.PlayerClassification == PlayerClassification.General)
                .Select(o => o.EntryNumber);

            var mockDrawTableRepository = new Mock<IDrawTableRepository>();
            var mockTournamentRepository = new Mock<ITournamentRepository>();
            var mockTournamentEntryRepository = new Mock<ITournamentEntryRepository>();
            var mockRankingRepository = new Mock<IRankingRepository>();

            mockDrawTableRepository.Setup(r => r.FindByDtoAsync(It.IsAny<DrawTableRepositoryDto>(), false))
                .ReturnsAsync(drawTable)
                .Verifiable();
            mockDrawTableRepository.Setup(r => r.UpdateAsync(drawTable))
                .ReturnsAsync(drawTable)
                .Verifiable();

            var usecase = new DrawTableUseCase(
                mockDrawTableRepository.Object,
                mockTournamentRepository.Object,
                mockTournamentEntryRepository.Object,
                mockRankingRepository.Object
            );

            // Act
            await usecase.ExecuteDrawingQualifying(1, "1_1_1");
            var qualifyingBlocks = drawTable.Blocks.GetQualifyingBlocks();
            var opponents = qualifyingBlocks
                .SelectMany(o => o.Games)
                .SelectMany(o => o.Opponents);

            var block1game1draw1 = opponents.First(o => o.BlockNumber.Value == 1 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block1game8draw2 = opponents.First(o => o.BlockNumber.Value == 1 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game1 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game1.AssignOpponent(block1game1draw1);
            game1.AssignOpponent(block1game8draw2);
            var gameResult1 = new GameResult();
            gameResult1.UpdateGameResult(
                GameStatus.Done,
                block1game1draw1.PlayerClassification,
                block1game1draw1.EntryNumber,
                new GameScore("123")
            );
            game1.SetGameResult(gameResult1);
            qualifyingBlocks[0].Games.Add(game1);

            var block2game1draw1 = opponents.First(o => o.BlockNumber.Value == 2 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block2game8draw2 = opponents.First(o => o.BlockNumber.Value == 2 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game2 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game2.AssignOpponent(block2game1draw1);
            game2.AssignOpponent(block2game8draw2);
            var gameResult2 = new GameResult();
            gameResult2.UpdateGameResult(
                GameStatus.Done,
                block2game1draw1.PlayerClassification,
                block2game1draw1.EntryNumber,
                new GameScore("123")
            );
            game2.SetGameResult(gameResult2);
            qualifyingBlocks[1].Games.Add(game2);

            var block3game1draw1 = opponents.First(o => o.BlockNumber.Value == 3 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block3game8draw2 = opponents.First(o => o.BlockNumber.Value == 3 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game3 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game3.AssignOpponent(block3game1draw1);
            game3.AssignOpponent(block3game8draw2);
            var gameResult3 = new GameResult();
            gameResult3.UpdateGameResult(
                GameStatus.Done,
                block3game1draw1.PlayerClassification,
                block3game1draw1.EntryNumber,
                new GameScore("123")
            );
            game3.SetGameResult(gameResult3);
            qualifyingBlocks[2].Games.Add(game3);

            var block4game1draw1 = opponents.First(o => o.BlockNumber.Value == 4 && o.GameNumber.Value == 1 && o.DrawNumber.Value == 1);
            var block4game8draw2 = opponents.First(o => o.BlockNumber.Value == 4 && o.GameNumber.Value == 8 && o.DrawNumber.Value == 16);
            var game4 = new Game(
                new GameNumber(qualifyingDrawSettings.NumberOfGamesPerBlock),
                new RoundNumber(qualifyingDrawSettings.NumberOfRoundsPerBlock),
                qualifyingDrawSettings);
            game4.AssignOpponent(block4game1draw1);
            game4.AssignOpponent(block4game8draw2);
            var gameResult4 = new GameResult();
            gameResult4.UpdateGameResult(
                GameStatus.Done,
                block4game1draw1.PlayerClassification,
                block4game1draw1.EntryNumber,
                new GameScore("123")
            );
            game4.SetGameResult(gameResult4);
            qualifyingBlocks[3].Games.Add(game4);

            await usecase.IntakeQualifyingWinners(1, "1_1_1");
            await usecase.ExecuteDrawingMain(1, "1_1_1");

            var mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            var drawA = default(Opponent);
            var drawB = default(Opponent);

            // ・シード位置にシード選手が割り当てられているか
            // ・シード選手の対戦相手に優先的に BYE が割り当てられているか
            // ・残りのシード選手の対戦相手に一般選手が割り当てられているか
            drawA = opponents.First(o => o.DrawNumber.Value == 1);
            drawB = opponents.First(o => o.DrawNumber.Value == 2);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawA.SeedNumber, new SeedNumber(1));
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Bye);

            drawA = opponents.First(o => o.DrawNumber.Value == 3);
            drawB = opponents.First(o => o.DrawNumber.Value == 4);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 5);
            drawB = opponents.First(o => o.DrawNumber.Value == 6);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawA.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 7);
            drawB = opponents.First(o => o.DrawNumber.Value == 8);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 9);
            drawB = opponents.First(o => o.DrawNumber.Value == 10);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 11);
            drawB = opponents.First(o => o.DrawNumber.Value == 12);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawB.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });

            drawA = opponents.First(o => o.DrawNumber.Value == 13);
            drawB = opponents.First(o => o.DrawNumber.Value == 14);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            drawA = opponents.First(o => o.DrawNumber.Value == 15);
            drawB = opponents.First(o => o.DrawNumber.Value == 16);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.General);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Seed);
            Assert.Equal(drawB.SeedNumber, new SeedNumber(2));

            var blockNumber = default(BlockNumber);
            var gameNumber = default(GameNumber);
            var gameResult = default(GameResult);
            var gameStatus = default(GameStatus);
            var gameScore = default(GameScore);

            // 第1ラウンド
            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawA = opponents.First(o => o.DrawNumber.Value == 1);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(1);
            gameResult = new GameResult();
            gameStatus = GameStatus.NotPlayed;
            gameScore = new GameScore("NotPlayed");
            gameResult.UpdateGameResult(gameStatus, PlayerClassification.Bye, null, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawB = opponents.First(o => o.DrawNumber.Value == 4);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(2);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawB.PlayerClassification, drawB.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            drawA = opponents.FirstOrDefault(o => o.GameNumber.Value == 9 && o.EntryNumber == new EntryNumber(0));
            drawB = opponents.FirstOrDefault(o => o.GameNumber.Value == 9 && o.EntryNumber == drawB.EntryNumber);
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Bye);
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.General);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawA = opponents.First(o => o.DrawNumber.Value == 5);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(3);
            gameResult = new GameResult();
            gameStatus = GameStatus.Done;
            gameScore = new GameScore("64");
            gameResult.UpdateGameResult(gameStatus, drawA.PlayerClassification, drawA.EntryNumber, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            mainBlock = drawTable.Blocks.GetMainBlock();
            opponents = mainBlock.Games.SelectMany(o => o.Opponents);
            drawB = opponents.First(o => o.DrawNumber.Value == 7);
            blockNumber = new BlockNumber(1);
            gameNumber = new GameNumber(4);
            gameResult = new GameResult();
            gameStatus = GameStatus.NotPlayed;
            gameScore = new GameScore("NotPlayed");
            gameResult.UpdateGameResult(gameStatus, PlayerClassification.Bye, null, gameScore);
            await usecase.UpdateGameStatus(1, "1_1_1", blockNumber, gameNumber, gameResult);

            drawA = opponents.FirstOrDefault(o => o.GameNumber.Value == 10 && o.EntryNumber == drawA.EntryNumber);
            drawB = opponents.FirstOrDefault(o => o.GameNumber.Value == 10 && o.EntryNumber == new EntryNumber(0));
            Assert.Equal(drawA.PlayerClassification, PlayerClassification.Seed);
            Assert.Contains(drawA.SeedNumber, new SeedNumber[] { new SeedNumber(3), new SeedNumber(4) });
            Assert.Equal(drawB.PlayerClassification, PlayerClassification.Bye);
        }

        [Fact]
        public async Task ドロー表初期化及び枠の自動設定()
        {
            #region テストデータ
            var tournament = new Tournament(
                new TournamentName("ジュニア選手権"),
                TournamentType.WithDraw,
                new RegistrationYear(new DateTime(2020, 4, 1)),
                TypeOfYear.Odd,
                new AggregationMonth(new DateTime(2020, 6, 1)),
                new List<TennisEvent>() { TennisEvent.FromId("1_1_1"), TennisEvent.FromId("1_1_2") },
                new HoldingPeriod(new DateTime(2020, 6, 10), new DateTime(2020, 6, 20)),
                new List<HoldingDate>() { new HoldingDate(new DateTime(2020, 6, 12)), new HoldingDate(new DateTime(2020, 6, 13)) },
                new Venue("日本テニスコート"),
                new EntryFee(100),
                MethodOfPayment.PrePayment,
                new ApplicationPeriod(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)),
                new Outline("大会名：ジュニア選手　権場所：日本テニスコート"),
                "メール本文",
                "メール件名",
                1
            )
            {
                Id = 1
            };

            var qualifyingDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(2),
                numberOfDraws: new NumberOfDraws(8),
                numberOfEntries: new NumberOfEntries(6),
                numberOfWinners: new NumberOfWinners(0),
                tournamentGrade: TournamentGrade.D);

            var mainDrawSettings = new DrawSettings(
                numberOfBlocks: new NumberOfBlocks(1),
                numberOfDraws: new NumberOfDraws(8),
                numberOfEntries: new NumberOfEntries(6),
                numberOfWinners: new NumberOfWinners(qualifyingDrawSettings.NumberOfBlocks.Value),
                tournamentGrade: TournamentGrade.B
            );

            var random = new Random();
            var entryNumber = 1;

            var qualifyingSeedNumberLimit = qualifyingDrawSettings.NumberOfBlocks.Value * 2;
            var qualifyingEntryDetails = Enumerable
                .Range(1, qualifyingDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Qualifying,
                    seedNumber: new SeedNumber(entry <= qualifyingSeedNumberLimit ? entry : 0),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var mainSeedNumberLimit = 2;
            var mainEntryDetails = Enumerable
                .Range(1, mainDrawSettings.NumberOfEntries.Value)
                .Select((entry, index) => new EntryDetail(
                    entryNumber: new EntryNumber(entryNumber++),
                    participationClassification: ParticipationClassification.Main,
                    seedNumber: new SeedNumber((entry <= mainSeedNumberLimit ? entry : 0)),
                    entryPlayers: new EntryPlayer[] { new EntryPlayer(
                        new TeamCode($"C{entry:0000}A"),
                        new TeamName($"TEAM{entry:0000}A"),
                        new TeamAbbreviatedName($"TEAM{entry:0000}A"),
                        new PlayerCode($"CD{entry:0000}A"),
                        new PlayerFamilyName("NAME"),
                        new PlayerFirstName($"{entry:0000}A"),
                        new Point(1000 + entry))
                    },
                    canParticipationDates: tournament.HoldingDates.Select(o => new CanParticipationDate(o.Value)),
                    receiptStatus: ReceiptStatus.Received,
                    usageFeatures: UsageFeatures.DrawTable))
                .ToList();

            var entryDetails = Enumerable
                .Concat(qualifyingEntryDetails, mainEntryDetails)
                .ToList();

            var blocks_temp = new List<Block>() {
                new Block(
                    new BlockNumber(0),
                    ParticipationClassification.Main,
                    new GameDate(tournament.HoldingDates.First().Value),
                    mainDrawSettings),
                new Block(
                        new BlockNumber(1),
                        ParticipationClassification.Qualifying,
                        new GameDate(tournament.HoldingDates.First().Value),
                        qualifyingDrawSettings),
                new Block(
                        new BlockNumber(2),
                        ParticipationClassification.Qualifying,
                        new GameDate(tournament.HoldingDates.First().Value),
                        qualifyingDrawSettings),
            };
            var blocks = new Blocks(blocks_temp);

            var drawTable = new DrawTable(
                tournament: tournament,
                tennisEvent: tournament.TennisEvents.First(),
                tournamentFormat: TournamentFormat.WithQualifying,
                eligiblePlayersType: EligiblePlayersType.AllPlayers,
                entryDetails: entryDetails,
                mainDrawSettings: mainDrawSettings,
                qualifyingDrawSettings: qualifyingDrawSettings,
                blocks: blocks,
                editStatus: EditStatus.Editing
            );
            #endregion テストデータ

            Assert.Null(drawTable.Blocks[0].Games);
            Assert.Null(drawTable.Blocks[1].Games);
            Assert.Null(drawTable.Blocks[2].Games);

            var mockDrawTableRepository = new Mock<IDrawTableRepository>();
            var mockTournamentRepository = new Mock<ITournamentRepository>();
            var mockTournamentEntryRepository = new Mock<ITournamentEntryRepository>();
            var mockRankingRepository = new Mock<IRankingRepository>();

            mockDrawTableRepository.Setup(r => r.FindByDtoAsync(It.IsAny<DrawTableRepositoryDto>(), false))
                .ReturnsAsync(drawTable)
                .Verifiable();

            var usecase = new DrawTableUseCase(
                mockDrawTableRepository.Object,
                mockTournamentRepository.Object,
                mockTournamentEntryRepository.Object,
                mockRankingRepository.Object
            );

            // 予選ドローの初期化
            await usecase.InitializeDrawTable(It.IsAny<int>(), It.IsAny<string>(), ParticipationClassification.Qualifying.Id);
            Assert.Null(drawTable.Blocks[0].Games);
            Assert.NotNull(drawTable.Blocks[1].Games);
            Assert.Equal(drawTable.Blocks[1].Games.Count, qualifyingDrawSettings.NumberOfDraws.Value / 2);
            Assert.NotNull(drawTable.Blocks[2].Games);
            Assert.Equal(drawTable.Blocks[2].Games.Count, qualifyingDrawSettings.NumberOfDraws.Value / 2);

            // 本戦ドローの初期化
            await usecase.InitializeDrawTable(It.IsAny<int>(), It.IsAny<string>(), ParticipationClassification.Main.Id);
            Assert.NotNull(drawTable.Blocks[0].Games);
            Assert.Equal(drawTable.Blocks[0].Games.Count, mainDrawSettings.NumberOfDraws.Value / 2);
            Assert.NotNull(drawTable.Blocks[1].Games);
            Assert.Equal(drawTable.Blocks[1].Games.Count, qualifyingDrawSettings.NumberOfDraws.Value / 2);
            Assert.NotNull(drawTable.Blocks[2].Games);
            Assert.Equal(drawTable.Blocks[2].Games.Count, qualifyingDrawSettings.NumberOfDraws.Value / 2);

            var drawNumberSettings = DrawNumberSettingsRepository.FindByNumberOfDraws(qualifyingDrawSettings.NumberOfDraws.Value);

            // シード枠の自動設定
            await usecase.ExecuteSeedFrameSetting(It.IsAny<int>(), It.IsAny<string>(), ParticipationClassification.Qualifying.Id);

            // DrawNumber
            Assert.Equal(drawNumberSettings[0].DrawNumber, drawTable.Blocks[1].Games[0].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[1].DrawNumber, drawTable.Blocks[1].Games[0].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[2].DrawNumber, drawTable.Blocks[1].Games[1].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[3].DrawNumber, drawTable.Blocks[1].Games[1].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[4].DrawNumber, drawTable.Blocks[1].Games[2].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[5].DrawNumber, drawTable.Blocks[1].Games[2].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[6].DrawNumber, drawTable.Blocks[1].Games[3].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[7].DrawNumber, drawTable.Blocks[1].Games[3].Opponents[1].DrawNumber.Value);

            // SeedLevel
            Assert.Equal(drawNumberSettings[0].SeedLevel, drawTable.Blocks[1].Games[0].Opponents[0].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[1].SeedLevel, drawTable.Blocks[1].Games[0].Opponents[1].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[2].SeedLevel, drawTable.Blocks[1].Games[1].Opponents[0].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[3].SeedLevel, drawTable.Blocks[1].Games[1].Opponents[1].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[4].SeedLevel, drawTable.Blocks[1].Games[2].Opponents[0].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[5].SeedLevel, drawTable.Blocks[1].Games[2].Opponents[1].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[6].SeedLevel, drawTable.Blocks[1].Games[3].Opponents[0].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[7].SeedLevel, drawTable.Blocks[1].Games[3].Opponents[1].SeedLevel.Value);

            // AssignOrder
            Assert.Equal(drawNumberSettings[0].AssignOrder, drawTable.Blocks[1].Games[0].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[1].AssignOrder, drawTable.Blocks[1].Games[0].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[2].AssignOrder, drawTable.Blocks[1].Games[1].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[3].AssignOrder, drawTable.Blocks[1].Games[1].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[4].AssignOrder, drawTable.Blocks[1].Games[2].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[5].AssignOrder, drawTable.Blocks[1].Games[2].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[6].AssignOrder, drawTable.Blocks[1].Games[3].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[7].AssignOrder, drawTable.Blocks[1].Games[3].Opponents[1].AssignOrder.Value);

            // PlayerClassification
            Assert.Equal(PlayerClassification.Seed.Id, drawTable.Blocks[1].Games[0].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[0].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[1].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[1].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[2].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[2].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[3].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.Seed.Id, drawTable.Blocks[1].Games[3].Opponents[1].FramePlayerClassification.Id);

            // IsManuallySettingFrame
            Assert.False(drawTable.Blocks[1].Games[0].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[0].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[1].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[1].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[2].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[2].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[3].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[3].Opponents[1].IsManuallySettingFrame);

            // BYE枠の自動設定
            await usecase.ExecuteByeFrameSetting(It.IsAny<int>(), It.IsAny<string>(), ParticipationClassification.Qualifying.Id);

            // DrawNumber
            Assert.Equal(drawNumberSettings[0].DrawNumber, drawTable.Blocks[1].Games[0].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[1].DrawNumber, drawTable.Blocks[1].Games[0].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[2].DrawNumber, drawTable.Blocks[1].Games[1].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[3].DrawNumber, drawTable.Blocks[1].Games[1].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[4].DrawNumber, drawTable.Blocks[1].Games[2].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[5].DrawNumber, drawTable.Blocks[1].Games[2].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[6].DrawNumber, drawTable.Blocks[1].Games[3].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[7].DrawNumber, drawTable.Blocks[1].Games[3].Opponents[1].DrawNumber.Value);

            // SeedLevel
            Assert.Equal(drawNumberSettings[0].SeedLevel, drawTable.Blocks[1].Games[0].Opponents[0].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[1].SeedLevel, drawTable.Blocks[1].Games[0].Opponents[1].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[2].SeedLevel, drawTable.Blocks[1].Games[1].Opponents[0].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[3].SeedLevel, drawTable.Blocks[1].Games[1].Opponents[1].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[4].SeedLevel, drawTable.Blocks[1].Games[2].Opponents[0].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[5].SeedLevel, drawTable.Blocks[1].Games[2].Opponents[1].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[6].SeedLevel, drawTable.Blocks[1].Games[3].Opponents[0].SeedLevel.Value);
            Assert.Equal(drawNumberSettings[7].SeedLevel, drawTable.Blocks[1].Games[3].Opponents[1].SeedLevel.Value);

            // AssignOrder
            Assert.Equal(drawNumberSettings[0].AssignOrder, drawTable.Blocks[1].Games[0].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[1].AssignOrder, drawTable.Blocks[1].Games[0].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[2].AssignOrder, drawTable.Blocks[1].Games[1].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[3].AssignOrder, drawTable.Blocks[1].Games[1].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[4].AssignOrder, drawTable.Blocks[1].Games[2].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[5].AssignOrder, drawTable.Blocks[1].Games[2].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[6].AssignOrder, drawTable.Blocks[1].Games[3].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[7].AssignOrder, drawTable.Blocks[1].Games[3].Opponents[1].AssignOrder.Value);

            // PlayerClassification
            Assert.Equal(PlayerClassification.Seed.Id, drawTable.Blocks[1].Games[0].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.Bye.Id, drawTable.Blocks[1].Games[0].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[1].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[1].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[2].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[2].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.Bye.Id, drawTable.Blocks[1].Games[3].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.Seed.Id, drawTable.Blocks[1].Games[3].Opponents[1].FramePlayerClassification.Id);

            // IsManuallySettingFrame
            Assert.False(drawTable.Blocks[1].Games[0].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[0].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[1].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[1].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[2].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[2].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[3].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[3].Opponents[1].IsManuallySettingFrame);

            // シード枠の割当解除
            await usecase.ExecuteSeedFrameRemove(It.IsAny<int>(), It.IsAny<string>(), ParticipationClassification.Qualifying.Id);

            // DrawNumber
            Assert.Equal(drawNumberSettings[0].DrawNumber, drawTable.Blocks[1].Games[0].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[1].DrawNumber, drawTable.Blocks[1].Games[0].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[2].DrawNumber, drawTable.Blocks[1].Games[1].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[3].DrawNumber, drawTable.Blocks[1].Games[1].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[4].DrawNumber, drawTable.Blocks[1].Games[2].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[5].DrawNumber, drawTable.Blocks[1].Games[2].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[6].DrawNumber, drawTable.Blocks[1].Games[3].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[7].DrawNumber, drawTable.Blocks[1].Games[3].Opponents[1].DrawNumber.Value);

            // SeedLevel
            Assert.Equal(0, drawTable.Blocks[1].Games[0].Opponents[0].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[0].Opponents[1].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[1].Opponents[0].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[1].Opponents[1].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[2].Opponents[0].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[2].Opponents[1].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[3].Opponents[0].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[3].Opponents[1].SeedLevel.Value);

            // AssignOrder
            Assert.Equal(drawNumberSettings[0].AssignOrder, drawTable.Blocks[1].Games[0].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[1].AssignOrder, drawTable.Blocks[1].Games[0].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[2].AssignOrder, drawTable.Blocks[1].Games[1].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[3].AssignOrder, drawTable.Blocks[1].Games[1].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[4].AssignOrder, drawTable.Blocks[1].Games[2].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[5].AssignOrder, drawTable.Blocks[1].Games[2].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[6].AssignOrder, drawTable.Blocks[1].Games[3].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[7].AssignOrder, drawTable.Blocks[1].Games[3].Opponents[1].AssignOrder.Value);

            // PlayerClassification
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[0].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.Bye.Id, drawTable.Blocks[1].Games[0].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[1].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[1].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[2].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[2].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.Bye.Id, drawTable.Blocks[1].Games[3].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[3].Opponents[1].FramePlayerClassification.Id);

            // IsManuallySettingFrame
            Assert.False(drawTable.Blocks[1].Games[0].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[0].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[1].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[1].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[2].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[2].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[3].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[3].Opponents[1].IsManuallySettingFrame);

            // BYE枠の割当解除
            await usecase.ExecuteByeFrameRemove(It.IsAny<int>(), It.IsAny<string>(), ParticipationClassification.Qualifying.Id);

            // DrawNumber
            Assert.Equal(drawNumberSettings[0].DrawNumber, drawTable.Blocks[1].Games[0].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[1].DrawNumber, drawTable.Blocks[1].Games[0].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[2].DrawNumber, drawTable.Blocks[1].Games[1].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[3].DrawNumber, drawTable.Blocks[1].Games[1].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[4].DrawNumber, drawTable.Blocks[1].Games[2].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[5].DrawNumber, drawTable.Blocks[1].Games[2].Opponents[1].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[6].DrawNumber, drawTable.Blocks[1].Games[3].Opponents[0].DrawNumber.Value);
            Assert.Equal(drawNumberSettings[7].DrawNumber, drawTable.Blocks[1].Games[3].Opponents[1].DrawNumber.Value);

            // SeedLevel
            Assert.Equal(0, drawTable.Blocks[1].Games[0].Opponents[0].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[0].Opponents[1].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[1].Opponents[0].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[1].Opponents[1].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[2].Opponents[0].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[2].Opponents[1].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[3].Opponents[0].SeedLevel.Value);
            Assert.Equal(0, drawTable.Blocks[1].Games[3].Opponents[1].SeedLevel.Value);

            // AssignOrder
            Assert.Equal(drawNumberSettings[0].AssignOrder, drawTable.Blocks[1].Games[0].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[1].AssignOrder, drawTable.Blocks[1].Games[0].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[2].AssignOrder, drawTable.Blocks[1].Games[1].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[3].AssignOrder, drawTable.Blocks[1].Games[1].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[4].AssignOrder, drawTable.Blocks[1].Games[2].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[5].AssignOrder, drawTable.Blocks[1].Games[2].Opponents[1].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[6].AssignOrder, drawTable.Blocks[1].Games[3].Opponents[0].AssignOrder.Value);
            Assert.Equal(drawNumberSettings[7].AssignOrder, drawTable.Blocks[1].Games[3].Opponents[1].AssignOrder.Value);

            // PlayerClassification
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[0].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[0].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[1].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[1].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[2].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[2].Opponents[1].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[3].Opponents[0].FramePlayerClassification.Id);
            Assert.Equal(PlayerClassification.General.Id, drawTable.Blocks[1].Games[3].Opponents[1].FramePlayerClassification.Id);

            // IsManuallySettingFrame
            Assert.False(drawTable.Blocks[1].Games[0].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[0].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[1].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[1].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[2].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[2].Opponents[1].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[3].Opponents[0].IsManuallySettingFrame);
            Assert.False(drawTable.Blocks[1].Games[3].Opponents[1].IsManuallySettingFrame);
        }
    }
}
