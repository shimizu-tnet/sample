using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.Tournaments;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 継続登録申請選手取得dto。
    /// </summary>
    public class RequestContinuedPlayersDto
    {
        /// <summary>
        /// 選手idを取得または設定します。
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// 氏名を取得または設定します。
        /// </summary>
        public PlayerName PlayerName { get; set; }

        /// <summary>
        /// 氏名(カナ)を取得または設定します。
        /// </summary>
        public PlayerNameKana PlayerNameKana { get; set; }

        /// <summary>
        /// カテゴリーを取得または設定します。
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// 性別を取得または設定します。
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 誕生日を取得または設定します。
        /// </summary>
        public BirthDate BirthDate { get; set; }

        /// <summary>
        /// 既に申請済みかどうかを取得または設定します。
        /// </summary>
        public bool IsRequested { get; set; }
    }
}
