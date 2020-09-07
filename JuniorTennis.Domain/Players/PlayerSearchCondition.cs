using JuniorTennis.Domain.QueryConditions;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System.Linq;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 選手検索の条件。
    /// </summary>
    public class PlayerSearchCondition : SearchCondition<Player>
    {
        /// <summary>
        /// 登録選手検索の条件の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="playerName">氏名。</param>
        public PlayerSearchCondition(string playerName, int teamId)
        {
            this.AddFilter(o => teamId != o.TeamId);

            if (!string.IsNullOrWhiteSpace(playerName))
            {
                this.AddFilter(o => ((string)(object)o.PlayerFamilyName + (string)(object)o.PlayerFirstName).Contains(playerName)
                || ((string)(object)o.PlayerFamilyNameKana + (string)(object)o.PlayerFirstNameKana).Contains(playerName));
            }

            this.AddSort(SortDirection.Descending, player => player.Id);
        }

        /// <summary>
        /// 選手検索の条件の新しいインスタンスを生成します(協会選手一覧　検索)。
        /// </summary>
        /// <param name="pageIndex">ページ番号。</param>
        /// <param name="displayCount">表示件数。</param>
        /// <param name="categoryIds">カテゴリーid一覧。</param>
        /// <param name="genderIds">性別id一覧。</param>
        /// <param name="playerName">氏名。</param>
        /// <param name="teamName">団体名。</param>
        public PlayerSearchCondition(int pageIndex, int displayCount, int[] categoryIds, int[] genderIds, string playerName, string teamName)
        {            
            if (!string.IsNullOrWhiteSpace(playerName))
            {
                this.AddFilter(o => ((string)(object)o.PlayerFamilyName + (string)(object)o.PlayerFirstName).Contains(playerName)
                || ((string)(object)o.PlayerFamilyNameKana + (string)(object)o.PlayerFirstNameKana).Contains(playerName));
            }

            if (!string.IsNullOrWhiteSpace(teamName))
            {
                this.AddFilter(o => ((string)(object)o.Team.TeamName).Contains(teamName));
            }

            if (categoryIds.Any())
            {
                var categorys = Enumeration.GetAll<Category>()
                    .Where(o => categoryIds.Contains(o.Id));
                this.AddFilter(o => categorys.Contains(o.Category));
            }

            if (genderIds.Any())
            {
                var genders = Enumeration.GetAll<Gender>()
                    .Where(o => genderIds.Contains(o.Id));
                this.AddFilter(o => genders.Contains(o.Gender));
            }

            this.AddSort(SortDirection.Descending, player => player.Id);
            this.Paginate(pageIndex, displayCount);
        }

        /// <summary>
        /// 選手検索の条件の新しいインスタンスを生成します(協会選手一覧　ダウンロード)。
        /// </summary>
        /// <param name="categoryIds">カテゴリーid一覧。</param>
        /// <param name="genderIds">性別id一覧。</param>
        /// <param name="playerName">氏名。</param>
        /// <param name="teamName">団体名。</param>
        public PlayerSearchCondition(int[] categoryIds, int[] genderIds, string playerName, string teamName)
        {
            if (!string.IsNullOrWhiteSpace(playerName))
            {
                this.AddFilter(o => ((string)(object)o.PlayerFamilyName + (string)(object)o.PlayerFirstName).Contains(playerName)
                || ((string)(object)o.PlayerFamilyNameKana + (string)(object)o.PlayerFirstNameKana).Contains(playerName));
            }

            if (!string.IsNullOrWhiteSpace(teamName))
            {
                this.AddFilter(o => ((string)(object)o.Team.TeamName).Contains(teamName));
            }

            if (categoryIds.Any())
            {
                var categorys = Enumeration.GetAll<Category>()
                    .Where(o => categoryIds.Contains(o.Id));
                this.AddFilter(o => categorys.Contains(o.Category));
            }

            if (genderIds.Any())
            {
                var genders = Enumeration.GetAll<Gender>()
                    .Where(o => genderIds.Contains(o.Id));
                this.AddFilter(o => genders.Contains(o.Gender));
            }

            this.AddSort(SortDirection.Descending, player => player.Id);
        }
    }
}
