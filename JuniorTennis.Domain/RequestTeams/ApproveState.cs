using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.RequestTeams
{
    /// <summary>
    /// 受領状態を定義します。
    /// </summary>
    public class ApproveState : Enumeration
    {
        /// <summary>
        /// すべて。
        /// </summary>
        public static readonly ApproveState All = new ApproveState(0, "すべて");
        
        /// <summary>
        /// 受領。
        /// </summary>
        public static readonly ApproveState Approved = new ApproveState(1, "受領");

        /// <summary>
        /// 未納。
        /// </summary>
        public static readonly ApproveState Unapproved = new ApproveState(2, "未納");

        /// <summary>
        /// 受領状態の新しいインスタンスを生成します。
        /// </summary>
        public ApproveState(int id, string name) : base(id, name) { }
    }
}
