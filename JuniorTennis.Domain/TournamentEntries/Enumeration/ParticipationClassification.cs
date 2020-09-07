using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 出場区分を定義します。
    /// </summary>
    public class ParticipationClassification : Enumeration
    {
        /// <summary>
        /// 本戦。
        /// </summary>
        public static readonly ParticipationClassification Main = new ParticipationClassification(1, "本戦");

        /// <summary>
        /// 予選。
        /// </summary>
        public static readonly ParticipationClassification Qualifying = new ParticipationClassification(2, "予選");

        /// <summary>
        /// 不出場。
        /// </summary>
        public static readonly ParticipationClassification NotParticipate = new ParticipationClassification(3, "不出場");

        /// <summary>
        /// 出場区分の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public ParticipationClassification(int id, string name) : base(id, name) { }
    }
}
