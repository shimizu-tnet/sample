using JuniorTennis.Domain.Teams;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 登録選手追加dto。
    /// </summary>
    public class AddRequestPlayersDto
    {
        /// <summary>
        /// 選手idを取得または設定します。
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// カテゴリーidを取得または設定します。
        /// </summary>
        public int CategoryId { get; set; }
    }
}
