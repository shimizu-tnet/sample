using JuniorTennis.Domain.Teams;
using System.Threading.Tasks;
using System.Collections.Generic;
using JuniorTennis.Domain.Accounts;
using JuniorTennis.Domain.RequestTeams;
using JuniorTennis.Domain.Seasons;
using JuniorTennis.Domain.UseCases.Shared;
using System.Linq;

namespace JuniorTennis.Domain.UseCases.Teams
{
    public interface ITeamUseCase
    {
        /// <summary>
        /// 団体一覧を取得します。
        /// </summary>
        /// <param name="pageIndex">現在のページ番号。</param>
        /// <param name="displayCount">ページ当たりの表示件数。</param>
        /// <param name="teamTypes">団体種別一覧。</param>
        /// <param name="teamName">団体名。</param>
        /// <returns>団体一覧。</returns>
        Task<Pagable<Team>> SearchTeam(int pageIndex, int displayCount, int[] teamTypes, string teamName);

        /// <summary>
        /// 団体を取得します。
        /// </summary>
        /// <param name="teamCode">団体コード。</param>
        /// <returns>団体。</returns>
        Task<Team> GetTeam(string teamCode);

        /// <summary>
        /// JPINの団体番号を更新します。
        /// </summary>
        /// <param name="teamJpin">団体登録番号。</param>
        /// <param name="teamJpin">JPINの団体番号。</param>
        /// <returns>更新後の団体。</returns>
        Task<Team> UpdateTeamJpin(string teamCode, string teamJpin);

        /// <summary>
        /// 団体新規登録申請
        /// </summary>
        /// <param name="dto">団体登録用dto。</param>
        /// <returns>登録後の団体。</returns>
        Task<Team> RequestTeamNewRegistration(RequestTeamNewRegistrationDto dto);

        /// <summary>
        /// 団体申請後の通知メールを送信します。
        /// </summary>
        /// <param name="mailAddress">メールアドレス</param>
        /// <returns>Task</returns>
        Task SendRequestTeamNewRegistrationMail(string mailAddress);

        /// <summary>
        /// 認証コードと紐づく認証情報を取得します
        /// </summary>
        /// <param name="authorizationCode">認証コード</param>
        /// <returns>認証情報。</returns>
        Task<AuthorizationLink> GetAuthorizationLinkByCode(string authorizationCode);

        /// <summary>
        /// 団体情報変更
        /// </summary>
        /// <param name="dto">団体更新用dto。</param>
        /// <returns>更新後の団体。</returns>
        Task<Team> UpdateTeam(UpdateTeamDto dto);

        /// <summary>
        /// 団体申請情報取得
        /// </summary>
        /// <param name="teamId">団体Id。</param>
        /// <returns>団体申請情報dto。</returns>
        Task<GetRequestTeamStateDto> GetRequestTeamState(int teamId);

        /// <summary>
        /// 団体継続申請
        /// </summary>
        /// <param name="dto">団体申請情報dto。</param>
        /// <returns>登録団体。</returns>
        Task<RequestTeam> AddRequestTeam(AddRequestTeamDto dto);

        /// <summary>
        /// 認証情報を認証テーブルに登録します
        /// </summary>
        /// <param name="uniqueKey">ユニークキー</param>
        /// <returns>Task。</returns>
        Task<AuthorizationLink> AddAuthorizationLink(string uniqueKey);

        /// <summary>
        /// 団体メールアドレス再設定の認証メールを送信
        /// </summary>
        /// <param name="authorizationCode">認証コード。</param>
        /// <param name="mailAddress">送信先メールアドレス。</param>
        /// <param name="domainUrl">ドメインURL。</param>
        /// <returns>Task。</returns>
        Task SendChangeMailAddressVerifyMail(AuthorizationCode authorizationCode, string mailAddress, string domainUrl);

        /// <summary>
        /// 登録団体一覧を取得します。
        /// </summary>
        /// <returns>登録団体一覧。</returns>
        Task<List<RequestTeam>> GetRequestTeams();

        /// <summary>
        /// 登録団体の受領状態を更新します。
        /// </summary>
        /// <param name="approveType">受領登録or取消登録。</param>
        /// <param name="selectedRequestTeamIds">更新対象の登録団体のid一覧。</param>
        /// <param name="selectedSeasonId">選択年度。</param>
        /// <param name="domainUrl">ドメインURL。</param>
        /// <returns>Task。</returns>
        Task UpdateRequestTeamsApproveState(int approveType, List<int> selectedRequestTeamIds, int selectedSeasonId, string domainUrl);

        /// <summary>
        /// 新規団体番号を発行します。
        /// </summary>
        /// <param name="teamType">団体種別。</param>
        /// <returns>新規団体番号。</returns>
        Task<string> CreateTeamCode(TeamType teamType);

        /// <summary>
        /// 新規団体アカウントパスワード設定要求メールを送信します。
        /// </summary>
        /// <param name="team">団体。</param>
        /// <param name="domainUrl">ドメインURL。</param>
        /// <returns>Task。</returns>
        Task SendRequestRegisterPasswordMail(Team team, string domainUrl);

        /// <summary>
        /// 年度一覧を取得します
        /// </summary>
        /// <returns>年度一覧。</returns>
        Task<List<Season>> GetSeasons();

        /// <summary>
        /// 登録団体一覧を取得します。
        /// </summary>
        /// <param name="pageIndex">現在のページ番号。</param>
        /// <param name="displayCount">ページ当たりの表示件数。</param>
        /// <param name="seasonId">年度id。</param>
        /// <param name="teamCode">団体番号。</param>
        /// <param name="reservationNumber">予約番号。</param>
        /// <param name="approveState">受領状態。</param>
        /// <returns>登録団体一覧。</returns>
        Task<Pagable<RequestTeam>> SearchRequestTeams(int pageIndex, int displayCount, int seasonId, string teamCode, string reservationNumber, int approveState);
    }
}
