using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Operators
{
    /// <summary>
    /// 管理ユーザー管理。
    /// </summary>
    public interface IOperatorRepository
    {
        /// <summary>
        /// 管理ユーザー一覧を取得します。
        /// </summary>
        /// <returns>管理ユーザー一覧。</returns>
        Task<List<Operator>> FindAllAsync();

        /// <summary>
        /// 管理ユーザーを登録します。
        /// </summary>
        /// <param name="entity">管理ユーザー。</param>
        /// <returns>管理ユーザー。</returns>
        Task<Operator> AddAsync(Operator entity);

        /// <summary>
        /// 指定したIdで管理ユーザーを取得します。
        /// </summary>
        /// <param name="id">管理ユーザーId。</param>
        /// <returns>管理ユーザー。</returns>
        Task<Operator> FindByIdAsync(int id);

        /// <summary>
        /// 管理ユーザーを更新します。
        /// </summary>
        /// <param name="entity">管理ユーザー。</param>
        /// <returns>管理ユーザー。</returns>
        Task<Operator> UpdateAsync(Operator entity);
    }
}
