﻿using Radicitus.Data.Contexts.Raffles.Entities;
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
    }
}