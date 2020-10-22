using Radicitus.Data.Contexts.Raffles.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radicitus.Data.Contexts.Raffles.Interfaces
{
    public interface IRaffleRepository
    {
        IEnumerable<RadRaffle> GetRaffles();
        Task<RadRaffle> GetRaffleById(int id);
    }
}
