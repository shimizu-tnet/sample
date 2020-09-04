namespace JuniorTennis.Domain.DrawTables
{
    /// <summary>
    /// ドロー表リポジトリ DTO。
    /// </summary>
    public class DrawTableRepositoryDto
    {
        public int TournamentId { get; private set; }
        public string TennisEventId { get; private set; }
        public EditStatus EditStatus { get; private set; }
        public bool IncludeEntryDetails { get; set; }
        public bool IncludeEntryPlayers { get; set; }
        public bool IncludeQualifyingDrawSettings { get; set; }
        public bool IncludeMainDrawSettings { get; set; }
        public bool IncludeBlocks { get; set; }
        public bool IncludeGames { get; set; }
        public bool IncludeGameResult { get; set; }
        public bool IncludeOpponents { get; set; }

        /// <summary>
        /// ドロー表リポジトリ DTO の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        public DrawTableRepositoryDto(int tournamentId, string tennisEventId)
        {
            this.TournamentId = tournamentId;
            this.TennisEventId = tennisEventId;
            this.EditStatus = EditStatus.Editing;
        }

        /// <summary>
        /// ドロー表リポジトリ DTO の新しいインスタンスを生成します。
        /// </summary>
        /// <param name="tournamentId">大会 ID。</param>
        /// <param name="tennisEventId">種目 ID。</param>
        /// <param name="editStatus">編集状態。</param>
        public DrawTableRepositoryDto(int tournamentId, string tennisEventId, EditStatus editStatus)
        {
            this.TournamentId = tournamentId;
            this.TennisEventId = tennisEventId;
            this.EditStatus = editStatus;
        }
    }
}
