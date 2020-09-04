using JuniorTennis.Domain.TournamentEntries;

namespace JuniorTennis.Domain.UseCases.TournamentEntries
{
    public class TournamentEntryUseCase : ITournamentEntryUseCase
    {
        private readonly ITournamentEntryRepository repository;
        public TournamentEntryUseCase(ITournamentEntryRepository repository) => this.repository = repository;
    }
}
