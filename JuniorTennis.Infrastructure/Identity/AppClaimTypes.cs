using System.Collections.Generic;

namespace JuniorTennis.Mvc.Features.Identity
{
    /// <summary>
    /// 認証要求種別の定数クラス。
    /// </summary>
    public static class AppClaimTypes
    {
        /// <summary>
        /// 受領
        /// </summary>
        public const string Receipt = "Receipt";

        /// <summary>
        /// 大会作成
        /// </summary>
        public const string CreateTournament = "CreateTournament";

        /// <summary>
        /// お知らせ作成
        /// </summary>
        public const string CreateNews = "CreateNews";

        /// <summary>
        /// 認証要求種別の一覧を取得します。
        /// </summary>
        public static readonly List<string> ClaimTypes = new List<string> { Receipt, CreateTournament, CreateNews };
    }
}
