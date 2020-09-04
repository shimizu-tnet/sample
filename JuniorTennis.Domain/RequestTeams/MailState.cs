using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.RequestTeams
{
    /// <summary>
    /// メール送信状態を定義します。
    /// </summary>
    public class MailState : Enumeration
    {
        /// <summary>
        /// 未送信。
        /// </summary>
        public static readonly MailState Unsent = new MailState(1, "未送信");

        /// <summary>
        /// 送信済。
        /// </summary>
        public static readonly MailState Sent = new MailState(2, "送信済");

        /// <summary>
        /// 受領状態の新しいインスタンスを生成します。
        /// </summary>
        public MailState(int id, string name) : base(id, name) { }
    }
}
