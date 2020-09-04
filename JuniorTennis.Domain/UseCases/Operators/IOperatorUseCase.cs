using System.Collections.Generic;
using JuniorTennis.Domain.Operators;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Operators
{
    /// <summary>
    /// 管理ユーザー管理。
    /// </summary>
    public interface IOperatorUseCase
    {
        /// <summary>
        /// 管理ユーザーの一覧を取得します。
        /// </summary>
        /// <returns>管理ユーザー一覧。</returns>
        Task<List<Operator>> GetOperators();

        /// <summary>
        /// 管理ユーザーを登録します。
        /// </summary>
        /// <param name="name">氏名。</param>
        /// <param name="emailAddress">メールアドレス。</param>
        /// <param name="loginId">ログインID。</param>
        /// <returns>管理ユーザー。</returns>
        Task<Operator> RegisterOperator(string name, string emailAddress, string loginId);

        /// <summary>
        /// 管理ユーザー招待メールを送信します。
        /// </summary>
        /// <param name="emaliAddress">メールアドレス。</param>
        /// <param name="invitaionUrl">送付するURL</param>
        Task SendOperatorInvitaionMail(string emaliAddress, string invitaionUrl);

        /// <summary>
        /// 管理ユーザーを取得します。
        /// </summary>
        /// <param name="id">管理ユーザーId。</param>
        /// <returns>管理ユーザー。</returns>
        Task<Operator> GetOperator(int id);

        /// <summary>
        /// 管理ユーザーを更新します。
        /// </summary>
        /// <param name="id">管理ユーザーId。</param>
        /// <param name="name">氏名。</param>
        /// <param name="emailAddress">メールアドレス。</param>
        /// <returns>管理ユーザー。</returns>
        Task<Operator> UpdateOperator(int id, string name, string emailAddress);
    }
}
