namespace JuniorTennis.Mvc.Features.TournamentEntries
{
    /// <summary>
    /// 申請選手の一覧表示用モデル。
    /// </summary>
    public class DisplayEntryPlayer
    {
        /// <summary>
        /// 選手Idを取得または設定します。
        /// </summary>
        public int PlayerId { get; set; }

        /// <summary>
        /// 登録番号を取得または設定します。
        /// </summary>
        public string PlayerCode { get; set; }

        /// <summary>
        /// 氏名を取得または設定します。
        /// </summary>
        public string PlayerName { get; set; }

        /// <summary>
        /// 氏名(カナ)を取得または設定します。
        /// </summary>
        public string PlayerNameKana { get; set; }

        /// <summary>
        /// 性別を取得または設定します。
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// カテゴリを取得または設定します。
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 生年月日を取得または設定します。
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// 年齢を取得または設定します。
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// 団体名を取得または設定します。
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// 学年を取得または設定します。
        /// </summary>
        public string SchoolYear { get; set; }

        /// <summary>
        /// 申請選手の一覧表示用モデルの新しいインスタンスを生成します。
        /// </summary>
        /// <param name="playerId">選手Id。</param>
        /// <param name="playerCode">登録番号。</param>
        /// <param name="playerName">氏名。</param>
        /// <param name="playerNameKana">氏名(カナ)</param>
        /// <param name="gender">性別。</param>
        /// <param name="category">カテゴリ。</param>
        /// <param name="birthDate">生年月日。</param>
        /// <param name="age">年齢。</param>
        /// <param name="teamName">団体名。</param>
        /// <param name="schoolYear">学年。 </param>
        public DisplayEntryPlayer(
            int playerId,
            string playerCode,
            string playerName,
            string playerNameKana,
            string gender,
            string category,
            string birthDate,
            string age,
            string teamName,
            string schoolYear)
        {
            this.PlayerId = playerId;
            this.PlayerCode = playerCode;
            this.PlayerName = playerName;
            this.PlayerNameKana = playerNameKana;
            this.Gender = gender;
            this.Category = category;
            this.BirthDate = birthDate;
            this.Age = age;
            this.TeamName = teamName;
            this.SchoolYear = schoolYear;
        }
    }
}
