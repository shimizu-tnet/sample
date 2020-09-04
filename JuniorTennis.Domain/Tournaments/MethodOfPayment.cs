using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.Tournaments
{
    /// <summary>
    /// 支払い方法を定義します。
    /// </summary>
    public class MethodOfPayment : Enumeration
    {
        /// <summary>
        /// 前払い。
        /// </summary>
        public static readonly MethodOfPayment PrePayment = new MethodOfPayment(1, "前払い");

        /// <summary>
        /// 後払い。
        /// </summary>
        public static readonly MethodOfPayment PostPayment = new MethodOfPayment(2, "後払い");

        /// <summary>
        /// 受領しない。
        /// </summary>
        public static readonly MethodOfPayment NotRecieve = new MethodOfPayment(3, "受領しない");

        /// <summary>
        /// その他。
        /// </summary>
        public static readonly MethodOfPayment Other = new MethodOfPayment(4, "その他");

        /// <summary>
        /// 支払い方法の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public MethodOfPayment(int id, string name) : base(id, name) { }
    }
}
