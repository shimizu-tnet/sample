using JuniorTennis.SeedWork;

namespace JuniorTennis.Domain.RequestPlayers
{
    /// <summary>
    /// 申請種別を定義します。
    /// </summary>
    public class RequestType : Enumeration
    {
        /// <summary>
        /// 新規登録申請。
        /// </summary>
        public static readonly RequestType NewRegistration = new RequestType(1, "新規登録申請");

        /// <summary>
        /// 継続登録申請。
        /// </summary>
        public static readonly RequestType ContinuedRegistration = new RequestType(2, "継続登録申請");

        /// <summary>
        /// 所属変更申請。
        /// </summary>
        public static readonly RequestType TransferRegistration = new RequestType(3, "所属変更申請");

        /// <summary>
        /// 申請種別の新しいインスタンスを生成します。
        /// </summary>
        public RequestType(int id, string name) : base(id, name) { }
    }
}
