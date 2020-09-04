using JuniorTennis.Domain.Seasons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.UseCases.Seasons
{
    public class SeasonUseCase : ISeasonUseCase
    {
        private readonly ISeasonRepository repository;

        public SeasonUseCase(ISeasonRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<Season>> GetSeasons()
        {
            var seasons = await this.repository.FindAll();
            return seasons.OrderByDescending(o => o.Id).ToList();
        }

        public async Task<Season> RegisterSeason(
            string name,
            DateTime fromDate,
            DateTime toDate,
            DateTime registrationFromDate,
            int teamRegistrationFee,
            int playerRegistrationFee,
            int playerTradeFee)
        {
            var season = new Season(
                name,
                fromDate,
                toDate,
                registrationFromDate,
                new TeamRegistrationFee(teamRegistrationFee),
                new PlayerRegistrationFee(playerRegistrationFee),
                new PlayerTradeFee(playerTradeFee));

            return await this.repository.Add(season);
        }

        public async Task<Season> UpdateSeason(
            int id,
            DateTime fromDate,
            DateTime toDate,
            DateTime registrationFromDate,
            int teamRegistrationFee,
            int playerRegistrationFee,
            int playerTradeFee)
        {
            var season = await repository.FindById(id);
            season.Change(
                fromDate,
                toDate,
                registrationFromDate,
                new TeamRegistrationFee(teamRegistrationFee),
                new PlayerRegistrationFee(playerRegistrationFee),
                new PlayerTradeFee(playerTradeFee)
                );

            return await this.repository.Update(season);
        }

        public async Task<Season> GetSeason(int id) => await this.repository.FindById(id);
    }
}
