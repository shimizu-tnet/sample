using JuniorTennis.Domain.Externals;
using JuniorTennis.Domain.Players;
using JuniorTennis.Domain.RequestPlayers;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.ReservationNumbers;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.Domain.UseCases.Shared;
using JuniorTennis.Domain.Utils;
using JuniorTennis.SeedWork;
using JuniorTennis.SeedWork.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Players
{
    public class PlayerUseCase : IPlayerUseCase
    {
        /// <summary>
        /// 継続登録申請の検索対象となる年度の範囲。
        /// </summary>
        private const int RequestContinuedPlayersSearchPeriod = 3;
        /// <summary>
        /// 登録可能な上限年齢。
        /// </summary>
        private const int RegisterableUpperLimitAge = 17;

        private readonly IMailSender mailSender;
        private readonly IPlayerRepository playerRepository;
        private readonly IRequestPlayerRepository requestPlayerRepository;
        private readonly IReservationNumberRepository reservationNumberRepository;
        private readonly ISeasonRepository seasonRepository;
        private readonly ITeamRepository teamRepository;

        public PlayerUseCase(
            IMailSender mailSender,
            IPlayerRepository playerRepository,
            IRequestPlayerRepository requestPlayerRepository,
            IReservationNumberRepository reservationNumberRepository,
            ISeasonRepository seasonRepository,
            ITeamRepository teamRepository)
        {
            this.mailSender = mailSender;
            this.playerRepository = playerRepository;
            this.requestPlayerRepository = requestPlayerRepository;
            this.reservationNumberRepository = reservationNumberRepository;
            this.seasonRepository = seasonRepository;
            this.teamRepository = teamRepository;
        }

        public async Task<List<Player>> GetTeamPlayersThisSeason(int teamId)
        {
            var season = await this.seasonRepository.FindByDate(DateTime.Today);
            var requestPlayers = await this.requestPlayerRepository.FindAllByTeamIdAndSeasonId(teamId, season.Id);
            var players = requestPlayers.Select(o => o.Player).ToList();
            return players;
        }

        public async Task<Player> GetPlayer(string playerCode)
        {
            return await this.playerRepository.FindByPlayerCodeAsync(new PlayerCode(playerCode));
        }

        public async Task<Player> UpdatePlayerTelephoneNumber(string playerCode, string telephoneNumber)
        {
            var updatePlayerCode = new PlayerCode(playerCode);
            var player = await this.playerRepository.FindByPlayerCodeAsync(updatePlayerCode);
            if (player == null)
            {
                throw new NotFoundException(playerCode, typeof(Player));
            }

            player.ChangeTelephoneNumber(telephoneNumber);
            var changed = await this.playerRepository.UpdateAsync(player);
            return changed;
        }

        public async Task<List<Player>> GetUnrequestedPlayers(int teamId)
        {
            return await this.playerRepository.FindUnrequestedAllByTeamIdWithoutPlayerCode(teamId);
        }

        public async Task RequestPlayersNewRegistration(List<int> playerIds, int teamId, string domainUrl)
        {
            var team = await this.teamRepository.FindByIdAsync(teamId);
            var season = await this.seasonRepository.FindByDate(DateTime.Today);
            var reservationNumber = await ReservationNumberFactory.Create(this.reservationNumberRepository);
            var branchNumber = 1;
            foreach (var playerId in playerIds)
            {
                var player = await this.playerRepository.FindByIdAsync(playerId);
                var requestPlayer = new RequestPlayer(
                    playerId,
                    teamId,
                    season.Id,
                    reservationNumber,
                    branchNumber,
                    player.Category,
                    RequestType.NewRegistration,
                    ApproveState.Unapproved,
                    DateTime.Now,
                    season.GetPlayerRegistrationFee(),
                    null);
                await this.requestPlayerRepository.UpdateAsync(requestPlayer);
                branchNumber++;
            }
            await this.SendRequestPlayersNewRegistrationMail(team.RepresentativeEmailAddress, domainUrl);
        }

        public async Task SendRequestPlayersNewRegistrationMail(string mailAddress, string domainUrl)
        {
            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/requestPlayers_mail.json"));
                var json = sr.ReadToEnd();
                json = json.Replace("\r\n", "");
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                var subject = JsonConverter.ToString(jsonElement.GetProperty("subject"));
                var htmlMessage = JsonConverter.ToMailBodyString(jsonElement.GetProperty("body"));
                await this.mailSender.SendEmailAsync(mailAddress, subject, htmlMessage);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task DeleteUnrequestedPlayer(int playerId)
        {
            var player = await this.playerRepository.FindByIdAsync(playerId);
            await this.playerRepository.DeleteAsync(player);
        }

        public async Task<bool> ExistsDuplicatedPlayer(string playerFamilyName, string playerFirstName, DateTime birthDate)
        {
            return await this.playerRepository.ExistsByNameAndBirtDateAsync(
                new PlayerFamilyName(playerFamilyName),
                new PlayerFirstName(playerFirstName),
                new BirthDate(birthDate));
        }

        public async Task AddPlayer(AddPlayerDto dto)
        {
            var player = new Player(
                dto.TeamId,
                null,
                new PlayerFamilyName(dto.PlayerFamilyName),
                new PlayerFirstName(dto.PlayerFirstName),
                new PlayerFamilyNameKana(dto.PlayerFamilyNameKana),
                new PlayerFirstNameKana(dto.PlayerFirstNameKana),
                null,
                Enumeration.FromValue<Category>(dto.Category),
                Enumeration.FromValue<Gender>(dto.Gender),
                new BirthDate(dto.BirthDate),
                dto.TelephoneNumber);
            await this.playerRepository.AddAsync(player);
        }

        public async Task<List<RequestPlayer>> GetRequestState(int teamId)
        {
            var season = await this.seasonRepository.FindByDate(DateTime.Today);
            return await this.requestPlayerRepository.FindRequestingAsync(teamId, season.Id);
        }

        public async Task<List<Player>> SearchPlayers(string playerName, int[] categoryIds, int[] genderIds)
        {
            var season = await this.seasonRepository.FindByDate(DateTime.Today);
            var condition = new RequestPlayerSearchCondition(playerName, categoryIds, genderIds, season.Id);
            var requestPlayers = await this.requestPlayerRepository.SearchAsync(condition);
            var players = requestPlayers.Select(o => o.Player).ToList();
            return players;
        }

        public async Task<List<RequestContinuedPlayersDto>> GetRequestContinuedPlayers(int teamId)
        {
            // 過去3年分の年度を取得
            var seasonIds = new List<int>();
            var thisSeasonId = 0;
            for (var i = 0; i < RequestContinuedPlayersSearchPeriod; i++)
            {
                var day = new DateTime(DateTime.Now.Year - i, DateTime.Now.Month, DateTime.Now.Day);
                var season = await this.seasonRepository.FindByDate(day);
                seasonIds.Add(season.Id);
                if (i == 0)
                {
                    thisSeasonId = season.Id;
                }
            }

            // 新しい順に登録選手を取得
            var requestPlayers = new List<RequestPlayer>();
            foreach (var seasonId in seasonIds)
            {
                var list = await this.requestPlayerRepository.FindAllByTeamIdAndSeasonId(teamId, seasonId);
                requestPlayers.AddRange(list);
            }

            // 前々年度まで登録していた17歳以下の選手を取得
            var requestContinuedPlayers = new List<RequestContinuedPlayersDto>();
            foreach (var requestPlayer in requestPlayers)
            {
                var playerId = requestPlayer.PlayerId;
                if (!requestContinuedPlayers.Any(o => o.PlayerId == playerId))
                {
                    continue;
                }
                var player = requestPlayer.Player;
                var seasonAge = player.GetSeasonAge();
                if (seasonAge > RegisterableUpperLimitAge)
                {
                    continue;
                }

                var requestContinuedPlayer = new RequestContinuedPlayersDto();
                requestContinuedPlayer.PlayerId = playerId;
                requestContinuedPlayer.PlayerName = new PlayerName(player.PlayerFamilyName, player.PlayerFirstName);
                requestContinuedPlayer.PlayerNameKana = new PlayerNameKana(player.PlayerFamilyNameKana, player.PlayerFirstNameKana);
                requestContinuedPlayer.Gender = player.Gender;
                requestContinuedPlayer.BirthDate = player.BirthDate;

                if (requestPlayer.SeasonId == thisSeasonId)
                {
                    // 今年度の登録が済の場合
                    requestContinuedPlayer.IsRequested = true;
                    requestContinuedPlayer.Category = player.Category;
                }
                else
                {
                    // 別団体の所属歴がある場合は対象外とする
                    if (await this.requestPlayerRepository.ExistsInOtherTeamAsync(teamId, playerId))
                    {
                        continue;
                    }
                    // 今年度の登録が未の場合、年齢基準のカテゴリーと過去に登録していたカテゴリーの大きい方を渡す
                    requestContinuedPlayer.Category = player.GetAvailableLowestCategory();
                }
                requestContinuedPlayers.Add(requestContinuedPlayer);
            }

            return requestContinuedPlayers;
        }

        public async Task AddRequestPlayers(List<AddRequestPlayersDto> dtos, int teamId)
        {
            var team = await this.teamRepository.FindByIdAsync(teamId);
            var season = await this.seasonRepository.FindByDate(DateTime.Today);
            var reservationNumber = await ReservationNumberFactory.Create(this.reservationNumberRepository);
            var branchNumber = 1;
            foreach (var dto in dtos)
            {
                var requestPlayer = new RequestPlayer(
                                    dto.PlayerId,
                                    teamId,
                                    season.Id,
                                    reservationNumber,
                                    branchNumber,
                                    Enumeration.FromValue<Category>(dto.CategoryId),
                                    RequestType.ContinuedRegistration,
                                    ApproveState.Unapproved,
                                    DateTime.Now,
                                    season.GetPlayerRegistrationFee(),
                                    null);
                await this.requestPlayerRepository.AddAsync(requestPlayer);
                branchNumber++;
            }
            await this.SendRequestPlayersContinuedRegistrationMail(team.RepresentativeEmailAddress);
        }

        public async Task SendRequestPlayersContinuedRegistrationMail(string mailAddress)
        {
            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/requestPlayersContinued_mail.json"));
                var json = sr.ReadToEnd();
                json = json.Replace("\r\n", "");
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                var subject = JsonConverter.ToString(jsonElement.GetProperty("subject"));
                var htmlMessage = JsonConverter.ToMailBodyString(jsonElement.GetProperty("body"));
                await this.mailSender.SendEmailAsync(mailAddress, subject, htmlMessage);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<List<Player>> SearchOtherTeamPlayers(string playerName, int teamId)
        {
            var condition = new PlayerSearchCondition(playerName, teamId);
            var players = await this.playerRepository.SearchAsync(condition);
            return players;
        }

        public async Task<List<Player>> GetTransferPlayers(List<int> playerIds)
        {
            var players = new List<Player>();
            if (playerIds == null)
            {
                return players;
            }

            players = await this.playerRepository.FindAllByIdsAsync(playerIds);
            return players;
        }

        public async Task AddRequestTransferPlayers(List<int> transferPlayerIds, int teamId)
        {
            var team = await this.teamRepository.FindByIdAsync(teamId);
            var season = await this.seasonRepository.FindByDate(DateTime.Today);
            var reservationNumber = await ReservationNumberFactory.Create(this.reservationNumberRepository);
            var branchNumber = 1;
            var players = await this.playerRepository.FindAllByIdsAsync(transferPlayerIds);
            var requestPlayers = await this.requestPlayerRepository.FindAllByPlayerIdsAndSeasonId(transferPlayerIds, season.Id);
            foreach (var player in players)
            {
                var isRegisteredThisSeason = requestPlayers.Any(o => o.PlayerId == player.Id && o.SeasonId == season.Id);
                var requestPlayer = new RequestPlayer(
                                    player.Id,
                                    teamId,
                                    season.Id,
                                    reservationNumber,
                                    branchNumber,
                                    player.Category,
                                    RequestType.TransferRegistration,
                                    ApproveState.Unapproved,
                                    DateTime.Now,
                                    season.GetPlayerTradeFee(isRegisteredThisSeason),
                                    null);
                await this.requestPlayerRepository.AddAsync(requestPlayer);
                branchNumber++;
            }
            await this.SendRequestPlayersTransferMail(team.RepresentativeEmailAddress);
        }

        public async Task SendRequestPlayersTransferMail(string mailAddress)
        {
            try
            {
                using var sr = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data/mail/requestPlayersTransfer_mail.json"));
                var json = sr.ReadToEnd();
                json = json.Replace("\r\n", "");
                var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
                var subject = JsonConverter.ToString(jsonElement.GetProperty("subject"));
                var htmlMessage = JsonConverter.ToMailBodyString(jsonElement.GetProperty("body"));
                await this.mailSender.SendEmailAsync(mailAddress, subject, htmlMessage);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public async Task<List<Season>> GetSeasons() => await seasonRepository.FindAll();

        public async Task<Pagable<Player>> SearchPlayerPagedList(int pageIndex, int displayCount, int seasonId, int[] categoryIds, int[] genderIds, string playerName, string teamName)
        {
            var condition = new PlayerSearchCondition(pageIndex, displayCount, categoryIds, genderIds, playerName, teamName);
            return await this.playerRepository.SearchPagedListAsync(condition, seasonId);
        }

        public async Task<List<Player>> SearchPlayerList(int seasonId, int[] categoryIds, int[] genderIds, string playerName, string teamName)
        {
            var condition = new PlayerSearchCondition(categoryIds, genderIds, playerName, teamName);
            return await this.playerRepository.SearchListAsync(condition, seasonId);
        }
    }
}
