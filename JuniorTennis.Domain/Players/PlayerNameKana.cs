using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 氏名(カナ)。
    /// </summary>
    public class PlayerNameKana : ValueObject
    {
        /// <summary>
        /// 氏名(カナ)を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 姓(カナ)と名(カナ)から氏名(カナ)のインスタンスを生成します。
        /// </summary>
        public PlayerNameKana(PlayerFamilyNameKana playerFamilyNameKana, PlayerFirstNameKana playerFirstNameKana)
        {
            this.Value = $"{playerFamilyNameKana.Value} {playerFirstNameKana.Value}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
