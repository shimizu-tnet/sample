using JuniorTennis.Domain.Tournaments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.TournamentEntries
{
    public class TournamentEntryUseCase : ITournamentEntryUseCase
    {
        private readonly ITournamentRepository tournamentRepository;

        public TournamentEntryUseCase(ITournamentRepository tournamentRepository) => this.tournamentRepository = tournamentRepository;

        public async Task<List<ApplicationTournamentsDto>> GetApplicationTournaments()
        {
            var tournament = await this.tournamentRepository.SearchAsync(new ApplicationTournamentsSearchCondition());
            var applicableTournaments = tournament
                .Select(o => new ApplicationTournamentsDto
                {
                    Id = o.Id.ToString(),
                    Name = o.TournamentName.Value
                })
                .ToList();

            return applicableTournaments;
        }

        public async Task<SelectedTournamentDto> GetSelectedTournament(int tournamentId)
        {
            if (tournamentId == 0)
            {
                return new SelectedTournamentDto();
            }

            var tournament = await this.tournamentRepository.FindById(tournamentId);
            var tennisEvents = tournament.TennisEvents.Select(o => new TennisEventDto
            {
                Id = o.TennisEventId,
                Name = o.DisplayTournamentEvent
            })
            .ToList();

            var selectedTournament = new SelectedTournamentDto
            {
                TennisEvents = tennisEvents,
                HoldingPeriod = tournament.HoldingPeriod.DisplayValue,
                ApplicationPeriod = tournament.ApplicationPeriod.DisplayValue
            };

            return selectedTournament;
        }
    }
}
