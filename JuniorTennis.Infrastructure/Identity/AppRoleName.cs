using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Mvc.Features.Identity
{
    /// <summary>
    /// アプリケーションで使用するロール名。
    /// </summary>
    public class AppRoleName : Enumeration
    {
        /// <summary>
        /// 管理者。
        /// </summary>
        public static readonly AppRoleName Administrator = new AppRoleName(1, "Administrator", "管理者");

        /// <summary>
        /// 大会作成者。
        /// </summary>
        public static readonly AppRoleName TournamentCreator = new AppRoleName(2, "TournamentCreator", "大会作成者");

        /// <summary>
        /// 記録入力者。
        /// </summary>
        public static readonly AppRoleName Recorder = new AppRoleName(3, "Recorder", "記録入力者");

        /// <summary>
        /// 団体ユーザー。
        /// </summary>
        public static readonly AppRoleName Team = new AppRoleName(4, "Team", "団体ユーザー");

        /// <summary>
        /// 開発者。
        /// </summary>
        public static readonly AppRoleName Developer = new AppRoleName(5, "Developer", "開発者");

        /// <summary>
        /// 表示名を取得します。
        /// </summary>
        public string DisplayName { get; }

        public AppRoleName(int id, string name, string displayName) : base(id, name)
        {
            this.DisplayName = displayName;
        }

        /// <summary>
        /// ロール名の一覧を取得します。
        /// </summary>
        public static IReadOnlyList<AppRoleName> GetOperatorRoles => new List<AppRoleName>()
        {
            AppRoleName.Administrator,
            AppRoleName.TournamentCreator,
            AppRoleName.Recorder
        };
    }
}
