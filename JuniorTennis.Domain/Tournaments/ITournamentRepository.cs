using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuniorTennis.Domain.Tournaments
{
    public interface ITournamentRepository
    {
        Task<List<Tournament>> Find();
        Task<Tournament> Add(Tournament entity);
        Task<Tournament> FindById(int id);
        Task<Tournament> Update(Tournament entity);
        Task Delete(Tournament entity);

        Task<Tournament> FindByRegistrationYear(DateTime registrationYear);
    }
}
