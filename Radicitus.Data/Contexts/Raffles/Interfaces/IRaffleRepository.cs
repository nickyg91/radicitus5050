using Radicitus.Data.Contexts.Raffles.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radicitus.Data.Contexts.Raffles.Interfaces
{
    public interface IRaffleRepository
    {
        IEnumerable<RadRaffle> GetRaffles();
        Task<RadRaffle> GetRaffleById(int id);
        IEnumerable<RaffleNumber> GetRaffleNumbersByRaffleId(int id);
        IEnumerable<RaffleNumber> GetRaffleNumbersByIdAndName(int id, string name);
        IEnumerable<RadRaffle> GetRafflesByAmountAndPage(int amount, int page);
    }
}
