using JuniorTennis.Domain.Teams;
using JuniorTennis.Domain.Tournaments;
using JuniorTennis.SeedWork;
using System;

namespace JuniorTennis.Domain.Players
{
    /// <summary>
    /// 選手。
    /// </summary>
    public class Player : EntityBase
    {
        /// <summary>
        /// 団体idを取得します。
        /// </summary>
        public int TeamId { get; private set; }

        /// <summary>
        /// 団体を取得します。
        /// </summary>
        public Team Team { get; private set; }

        /// <summary>
        /// 登録番号を取得します。
        /// </summary>
        public PlayerCode PlayerCode { get; private set; }

        /// <summary>
        /// 氏名を取得します。
        /// </summary>
        public PlayerName PlayerName { get; private set; }

        /// <summary>
        /// 姓を取得します。
        /// </summary>
        public PlayerFamilyName PlayerFamilyName { get; private set; }

        /// <summary>
        /// 名を取得します。
        /// </summary>
        public PlayerFirstName PlayerFirstName { get; private set; }

        /// <summary>
        /// 氏名(カナ)を取得します。
        /// </summary>
        public PlayerNameKana PlayerNameKana { get; private set; }

        /// <summary>
        /// 姓(カナ)を取得します。
        /// </summary>
        public PlayerFamilyNameKana PlayerFamilyNameKana { get; private set; }

        /// <summary>
        /// 名(カナ)を取得します。
        /// </summary>
        public PlayerFirstNameKana PlayerFirstNameKana { get; private set; }

        /// <summary>
        /// JPINを取得します。
        /// </summary>
        public string PlayerJpin { get; private set; }

        /// <summary>
        /// カテゴリーを取得します。
        /// </summary>
        public Category Category { get; private set; }

        /// <summary>
        /// 性別を取得します。
        /// </summary>
        public Gender Gender { get; private set; }

        /// <summary>
        /// 誕生日を取得します。
        /// </summary>
        public BirthDate BirthDate { get; private set; }

        /// <summary>
        /// 電話番号を取得します。
        /// </summary>
        public string TelephoneNumber { get; private set; }

        /// <summary>
        /// 選手の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="playerCode">登録番号。</param>
        /// <param name="playerFamilyName">姓。</param>
        /// <param name="playerFirstName">名。</param>
        /// <param name="playerFamilyNameKana">姓(カナ)。</param>
        /// <param name="playerFirstNameKana">名(カナ)。</param>
        /// <param name="playerJpin">JPIN。</param>
        /// <param name="category">カテゴリー。</param>
        /// <param name="gender">性別。</param>
        /// <param name="birthDate">誕生日。</param>
        /// <param name="telephoneNumber">電話番号。</param>
        public Player(
            int teamId,
            PlayerCode playerCode,
            PlayerFamilyName playerFamilyName,
            PlayerFirstName playerFirstName,
            PlayerFamilyNameKana playerFamilyNameKana,
            PlayerFirstNameKana playerFirstNameKana,
            string playerJpin,
            Category category,
            Gender gender,
            BirthDate birthDate,
            string telephoneNumber)
        {
            this.TeamId = teamId;
            this.PlayerCode = playerCode;
            this.PlayerFamilyName = playerFamilyName;
            this.PlayerFirstName = playerFirstName;
            this.PlayerFamilyNameKana = playerFamilyNameKana;
            this.PlayerFirstNameKana = playerFirstNameKana;
            this.PlayerJpin = playerJpin;
            this.Category = category;
            this.Gender = gender;
            this.BirthDate = birthDate;
            this.TelephoneNumber = telephoneNumber;
            this.PlayerName = new PlayerName(this.PlayerFamilyName, this.PlayerFirstName);
            this.PlayerNameKana = new PlayerNameKana(this.PlayerFamilyNameKana, this.PlayerFirstNameKana);
        }

        private Player() { }

        /// <summary>
        /// 登録番号を変更します。
        /// </summary>
        /// <param name="playerCode">登録番号。</param>
        public void ChangePlayerCode(string playerCode) => this.PlayerCode = new PlayerCode(playerCode);

        /// <summary>
        /// 電話番号を変更します。
        /// </summary>
        /// <param name="telephoneNumber">電話番号。</param>
        public void ChangeTelephoneNumber(string telephoneNumber) => this.TelephoneNumber = telephoneNumber;

        /// <summary>
        /// 選手の年齢を取得します。
        /// </summary>
        public int GetSeasonAge()
        {
            var currentYear = DateTime.Today.Year;
            var birthYear = this.BirthDate.Value.Year;
            var seasonAge = currentYear - birthYear;
            return seasonAge;
        }

        /// <summary>
        /// 年齢に対応するカテゴリーを取得します。
        /// </summary>
        public Category GetCategoryBySeasonAge()
        {
            var seasonAge = GetSeasonAge();
            if (seasonAge <= 12)
            {
                return Category.Under11Or12;
            }
            else if(seasonAge <= 14)
            {
                return Category.Under13Or14;
            }
            else if(seasonAge <= 16)
            {
                return Category.Under15Or16;
            }
            else
            {
                return Category.Under17Or18;
            }
        }

        /// <summary>
        /// 選手が登録可能な最も低いカテゴリーを取得します。
        /// </summary>
        public Category GetAvailableLowestCategory()
        {
            return this.GetCategoryBySeasonAge().Id > this.Category.Id
                        ? this.Category
                        : this.GetCategoryBySeasonAge();
        }

        /// <summary>
        /// CSV1レコード分の文字列を生成します。
        /// </summary>
        public string ToCsv()
        {
            return
            (this.PlayerCode?.Value ?? "") + "," +
            this.PlayerFamilyName.Value + " " + this.PlayerFirstName.Value + "," +
            (this.Team.TeamCode?.Value ?? "") + "," +
            this.Team.TeamName.Value + "," +
            this.PlayerJpin + "," +
            this.Category.Name + "," +
            this.Gender.Name +
            "\r\n";
        }
    }
}
