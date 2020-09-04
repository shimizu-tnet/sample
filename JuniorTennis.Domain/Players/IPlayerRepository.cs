using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 選手管理リポジトリ。
    /// </summary>
    public interface IPlayerRepository
    {
        /// <summary>
        /// 登録番号に紐づく選手を取得します。
        /// </summary>
        /// <param name="playerCode">登録番号。</param>
        /// <returns>選手。</returns>
        Task<Player> FindByPlayerCodeAsync(PlayerCode playerCode);

        /// <summary>
        /// 仮登録の選手一覧を取得します。
        /// </summary>
        /// <param name="teamId">団体id。</param>
        /// <returns>選手。</returns>
        Task<List<Player>> FindUnrequestedAllByTeamIdWithoutPlayerCode(int teamId);

        /// <summary>
        /// 選手idに紐づく選手を取得します。
        /// </summary>
        /// <param name="playerId">選手id。</param>
        /// <returns>選手。</returns>
        Task<Player> FindByIdAsync(int playerId);

        /// <summary>
        /// 氏名と誕生日に紐づく選手の存在を判定します。
        /// </summary>
        /// <param name="playerFamilyName">姓。</param>
        /// <param name="playerFirstName">名。</param>
        /// <param name="birthDate">誕生日。</param>
        /// <returns>選手。</returns>
        Task<bool> ExistsByNameAndBirtDateAsync(PlayerFamilyName playerFamilyName, PlayerFirstName playerFirstName, BirthDate birthDate);

        /// <summary>
        /// 選手を更新します。
        /// </summary>
        /// <param name="player">選手。</param>
        /// <returns>更新後の選手。</returns>
        Task<Player> UpdateAsync(Player player);

        /// <summary>
        /// 選手を新規登録します。
        /// </summary>
        /// <param name="player">選手。</param>
        /// <returns>登録後の選手。</returns>
        Task<Player> AddAsync(Player player);

        /// <summary>
        /// 選手を削除します。
        /// </summary>
        /// <param name="player">選手。</param>
        Task DeleteAsync(Player player);
    }
}
