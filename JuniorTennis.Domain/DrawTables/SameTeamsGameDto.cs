namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// 同一団体戦表示用 DTO。
    /// </summary>
    public class SameTeamsGameDto
    {
        /// <summary>
        /// ブロック番号。
        /// </summary>
        public int BlockNumber { get; set; }

        /// <summary>
        /// ブロック名。
        /// </summary>
        public string BlockName { get; set; }

        /// <summary>
        /// ドロー番号。
        /// </summary>
        public int DrawNumber { get; set; }

        /// <summary>
        /// 団体名略称一覧
        /// </summary>
        public string[] TeamAbbreviatedNames { get; set; }

        /// <summary>
        /// 氏名一覧。
        /// </summary>
        public string[] PlayerNames { get; set; }
    }
}
