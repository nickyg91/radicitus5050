using Radicitus.Data.Contexts.Raffles.Entities;
using Radicitus.Data.Contexts.Raffles.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Radicitus.Data.Contexts.Raffles.Implementations
{
    public class RaffleRepository : BaseRepository<RadicitusDbContext>, IRaffleRepository
    {
        public RaffleRepository(RadicitusDbContext context) : base(context)
        { }

        public IEnumerable<RadRaffle> GetRaffles()
        {
            return Context.Raffles.AsEnumerable();
        }

        public async Task<RadRaffle> GetRaffleById(int id)
        {
            return await Context.Raffles
                .Include(x => x.RaffleNumbers)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public IEnumerable<RaffleNumber> GetRaffleNumbersByRaffleId(int id)
        {
            return Context.RaffleNumbers.Where(x => x.RaffleId == id);
        }

        public IEnumerable<RaffleNumber> GetRaffleNumbersByIdAndName(int id, string name)
        {
            return Context.RaffleNumbers.Where(x => x.Id == id && x.Name == name);
        }

        public IEnumerable<RadRaffle> GetRafflesByAmountAndPage(int amount, int page)
        {
            return Context.Raffles.Skip(amount * (page - 1)).Take(amount);
        }

        public async Task<int> TotalRaffles()
        {
            return await Context.Raffles.CountAsync();
        }

        public async Task RemoveRaffle(int id)
        {
            var raffle = await Context.Raffles
                .Include(x => x.RaffleNumbers)
                .SingleOrDefaultAsync(x => x.Id == id);
            Context.Raffles.Remove(raffle);
        }
    }
}
