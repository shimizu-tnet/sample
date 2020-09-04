using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Mvc.Features.Identity
{
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

        public static readonly List<string> ClaimTypes = new List<string> { Receipt, CreateTournament, CreateNews };
    }
}
