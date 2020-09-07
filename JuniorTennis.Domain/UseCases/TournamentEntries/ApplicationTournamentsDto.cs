namespace JuniorTennis.Domain.UseCases.TournamentEntries
{
    /// <summary>
    /// 申込期間中の大会のIdと名前を取得用 DTO。
    /// </summary>
    public class ApplicationTournamentsDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
