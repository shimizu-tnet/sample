using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Teams
{
    /// <summary>
    /// 団体種別を定義します。
    /// </summary>
    public class TeamType : Enumeration
    {
        /// <summary>
        /// 学校。
        /// </summary>
        public static readonly TeamType School = new TeamType(1, "学校");

        /// <summary>
        /// 民間クラブ。
        /// </summary>
        public static readonly TeamType Club = new TeamType(2, "民間クラブ");

        /// <summary>
        /// 団体種別の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public TeamType(int id, string name) : base(id, name) { }

        /// <summary>
        /// 団体種別ごとの団体番号プレフィックスを取得します。
        /// <param name="teamType">団体種別</param>
        /// </summary>
        public static string GetTeamCodePrefix(TeamType teamType)
        {
            return teamType == TeamType.School ? "S" : "C";
        }
    }
}
