using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 申請者を定義します。
    /// </summary>
    public class Applicant : Enumeration
    {
        /// <summary>
        /// 団体。
        /// </summary>
        public static readonly Applicant Team = new Applicant(1, "団体");

        /// <summary>
        /// 協会。
        /// </summary>
        public static readonly Applicant Association = new Applicant(2, "協会");

        /// <summary>
        /// 申請者の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public Applicant(int id, string name) : base(id, name) { }
    }
}
