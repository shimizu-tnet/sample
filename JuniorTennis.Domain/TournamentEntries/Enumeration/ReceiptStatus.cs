using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.TournamentEntries
{
    /// <summary>
    /// 受領状況を定義します。
    /// </summary>
    public class ReceiptStatus : Enumeration
    {
        /// <summary>
        /// 受領。
        /// </summary>
        public static readonly ReceiptStatus Received = new ReceiptStatus(1, "受領");

        /// <summary>
        /// 未納。
        /// </summary>
        public static readonly ReceiptStatus NotReceived = new ReceiptStatus(2, "未納");

        /// <summary>
        /// 申請取消。
        /// </summary>
        public static readonly ReceiptStatus Cancel = new ReceiptStatus(3, "申請取消");

        /// <summary>
        /// 受領状況の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public ReceiptStatus(int id, string name) : base(id, name) { }
    }
}
