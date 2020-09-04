using JuniorTennis.SeedWork;
using System.Collections.Generic;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 氏名。
    /// </summary>
    public class PlayerName : ValueObject
    {
        /// <summary>
        /// 氏名を取得します。
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// 姓と名から氏名のインスタンスを生成します。
        /// </summary>
        public PlayerName(PlayerFamilyName playerFamilyName, PlayerFirstName playerFirstName)
        {
            this.Value = $"{playerFamilyName.Value} {playerFirstName.Value}";
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.Value;
        }
    }
}
