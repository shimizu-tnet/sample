using System.Collections.Generic;
using JuniorTennis.Domain.Players;
using System.Threading.Tasks;
using JuniorTennis.Domain.RequestPlayers;
using System;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.UseCases.Shared;

namespace JuniorTennis.Domain.UseCases.Players
{
    /// <summary>
    /// 選手管理ユースケース。
    /// </summary>
    public interface IPlayerUseCase
    {
        /// <summary>
        /// 今シーズン登録されている団体の選手一覧を取得します。
        /// </summary>
        /// <param name="teamId">団体id</param>
        /// <returns>選手一覧。</returns>
        Task<List<Player>> GetTeamPlayersThisSeason(int teamId);

        /// <summary>
        /// 登録番号に紐づく選手を取得します。
        /// </summary>
        /// <param name="playerCode">登録番号</param>
        /// <returns>選手。</returns>
        Task<Player> GetPlayer(string playerCode);

        /// <summary>
        /// 選手の電話番号を変更します。
        /// </summary>
        /// <param name="playerCode">登録番号</param>
        /// <param name="telephoneNumber">電話番号</param>
        /// <returns>選手。</returns>
        Task<Player> UpdatePlayerTelephoneNumber(string playerCode, string telephoneNumber);

        /// <summary>
        /// 団体の新規作成済かつ未申請の選手一覧を取得します。
        /// </summary>
        /// <param name="teamId">団体id</param>
        /// <returns>選手一覧。</returns>
        Task<List<Player>> GetUnrequestedPlayers(int teamId);

        /// <summary>
        /// 選手の新規登録申請をします。
        /// </summary>
        /// <param name="playerIds">選手idリスト</param>
        /// <param name="teamId">団体id</param>
        /// <param name="domainUrl">ドメインURL</param>
        /// <returns>選手一覧。</returns>
        Task RequestPlayersNewRegistration(List<int> playerIds, int teamId, string domainUrl);

        /// <summary>
        /// 選手新規登録申請後のメール送信をします。
        /// </summary>
        /// <param name="mailAddress">メールアドレス</param>
        /// <param name="domainUrl">ドメインURL</param>
        /// <returns>Task。</returns>
        Task SendRequestPlayersNewRegistrationMail(string mailAddress, string domainUrl);

        /// <summary>
        /// 仮登録選手を取消します。
        /// </summary>
        /// <param name="playerId">選手id</param>
        /// <returns>Task。</returns>
        Task DeleteUnrequestedPlayer(int playerId);

        /// <summary>
        /// 登録したい選手と重複した選手が存在しないか確認します。
        /// </summary>
        /// <param name="playerFamilyName">姓</param>
        /// <param name="playerFirstName">名</param>
        /// <param name="birthDate">誕生日</param>
        /// <returns>Task。</returns>
        Task<bool> ExistsDuplicatedPlayer(string playerFamilyName, string playerFirstName, DateTime birthDate);

        /// <summary>
        /// 選手を登録します。
        /// </summary>
        /// <param name="dto">選手登録dto</param>
        /// <returns>Task。</returns>
        Task AddPlayer(AddPlayerDto dto);

        /// <summary>
        /// 選手の申込状況を取得します。
        /// </summary>
        /// <param name="teamId">団体id</param>
        /// <returns>選手一覧。</returns>
        Task<List<RequestPlayer>> GetRequestState(int teamId);

        /// <summary>
        /// 選手を検索します。
        /// </summary>
        /// <param name="playerName">氏名</param>
        /// <param name="categoryIds">カテゴリーid一覧</param>
        /// <param name="genderIds">性別id一覧</param>
        /// <returns>選手一覧。</returns>
        Task<List<Player>> SearchPlayers(string playerName, int[] categoryIds, int[] genderIds);

        /// <summary>
        /// 継続登録申請選手を取得します。
        /// </summary>
        /// <param name="teamId">団体id</param>
        /// <returns>継続登録申請選手一覧。</returns>
        Task<List<RequestContinuedPlayersDto>> GetRequestContinuedPlayers(int teamId);

        /// <summary>
        /// 登録選手を登録します。
        /// </summary>
        /// <param name="dtos">選手登録dtoリスト</param>
        /// <param name="teamId">団体id</param>
        /// <returns>Task。</returns>
        Task AddRequestPlayers(List<AddRequestPlayersDto> dtos, int teamId);

        /// <summary>
        /// 継続登録申請完了後の通知メールを送信します。
        /// </summary>
        /// <param name="mailAddress">メールアドレス</param>
        /// <returns>Task。</returns>
        Task SendRequestPlayersContinuedRegistrationMail(string mailAddress);

        /// <summary>
        /// 他団体の選手を検索します。
        /// </summary>
        /// <param name="playerName">氏名</param>
        /// /// <param name="teamId">団体id</param>
        /// <returns>選手一覧。</returns>
        Task<List<Player>> SearchOtherTeamPlayers(string playerName, int teamId);

        /// <summary>
        /// 所属変更対象の選手を取得します。
        /// </summary>
        /// <param name="playerIds">選手idリスト</param>
        /// <returns>選手一覧。</returns>
        Task<List<Player>> GetTransferPlayers(List<int> playerIds);

        /// <summary>
        /// 選手所属変更を申請します。
        /// </summary>
        /// <param name="transferPlayerIds">選手idリスト</param>
        /// <param name="teamId">団体id</param>
        /// <returns>Task。</returns>
        Task AddRequestTransferPlayers(List<int> transferPlayerIds, int teamId);

        /// <summary>
        /// 選手所属変更申請完了後の通知メールを送信します。
        /// </summary>
        /// <param name="mailAddress">メールアドレス</param>
        /// <returns>Task。</returns>
        Task SendRequestPlayersTransferMail(string mailAddress);

        /// <summary>
        /// 年度一覧を取得します
        /// </summary>
        /// <returns>年度一覧。</returns>
        Task<List<Season>> GetSeasons();

        /// <summary>
        /// 選手を検索します。
        /// </summary>
        /// <param name="pageIndex">現在のページ番号。</param>
        /// <param name="displayCount">ページ当たりの表示件数。</param>
        /// <param name="seasonId">年度id</param>
        /// <param name="categoryIds">カテゴリーid一覧</param>
        /// <param name="genderIds">性別id一覧</param>
        /// <param name="playerName">氏名</param>
        /// <param name="teamName">団体名</param>
        /// <returns>選手一覧。</returns>
        Task<Pagable<Player>> SearchPlayerPagedList(int pageIndex, int displayCount, int seasonId, int[] categoryIds, int[] genderIds, string playerName, string teamName);

        /// <summary>
        /// CSVの出力対象となる選手一覧を検索します。
        /// </summary>
        /// <param name="seasonId">年度id</param>
        /// <param name="categoryIds">カテゴリーid一覧</param>
        /// <param name="genderIds">性別id一覧</param>
        /// <param name="playerName">氏名</param>
        /// <param name="teamName">団体名</param>
        /// <returns>選手一覧。</returns>
        Task<List<Player>> SearchPlayerList(int seasonId, int[] category, int[] gender, string playerName, string teamName);
    }
}
