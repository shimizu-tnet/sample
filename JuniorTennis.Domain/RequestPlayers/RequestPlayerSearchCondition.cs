using JuniorTennis.Domain.QueryConditions;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System.Linq;

namespace JuniorTennis.Domain.RequestPlayers
{
    /// <summary>
    /// 選手検索の条件。
    /// </summary>
    public class RequestPlayerSearchCondition : SearchCondition<RequestPlayer>
    {
        /// <summary>
        /// 登録選手検索の条件の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="playerName">氏名。</param>
        /// <param name="categoryIds">カテゴリーid一覧。</param>
        /// <param name="genderIds">性別id一覧。</param>
        /// <param name="seasonId">年度id。</param>
        public RequestPlayerSearchCondition(string playerName, int[] categoryIds, int[] genderIds, int seasonId)
        {
            this.AddFilter(o => seasonId == o.SeasonId);

            if (!string.IsNullOrEmpty(playerName))
            {
                this.AddFilter(o => ((string)(object)o.Player.PlayerFamilyName + (string)(object)o.Player.PlayerFirstName).Contains(playerName)
                || ((string)(object)o.Player.PlayerFamilyNameKana + (string)(object)o.Player.PlayerFirstNameKana).Contains(playerName));
            }

            if (categoryIds?.Any() ?? false)
            {
                var categorys = Enumeration.GetAll<Category>()
                    .Where(o => categoryIds.Contains(o.Id));
                this.AddFilter(o => categorys.Contains(o.Player.Category));
            }

            if (genderIds?.Any() ?? false)
            {
                var genders = Enumeration.GetAll<Gender>()
                    .Where(o => genderIds.Contains(o.Id));
                this.AddFilter(o => genders.Contains(o.Player.Gender));
            }

            this.AddSort(SortDirection.Descending, player => player.Id);
        }
    }
}
