using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Raffle.Models;
using StackExchange.Redis;

namespace Radicitus.Redis
{
    public class RadRaffleRedisRepository : IRaffleRepository
    {
        private readonly ConnectionMultiplexer _connection;

        public RadRaffleRedisRepository(ConnectionMultiplexer connection)
        {
            _connection = connection;
        }

        public async Task<RadRaffle> GetRaffleBuGuid(string guid)
        {
            var raffleJson = await _connection.GetDatabase().StringGetAsync(guid);
            return JsonConvert.DeserializeObject<RadRaffle>(raffleJson);
        }

        public async Task<List<RaffleNumber>> GetRadRafflesByRaffleGuid(string guid)
        {
            throw new NotImplementedException();
        }

        public List<RadRaffle> GetRadRaffles()
        {
            throw new NotImplementedException();
        }

        public void PushNewWinnerForRaffle(string raffleName, string winnerName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetWinnersOfRaffles()
        {
            throw new NotImplementedException();
        }
    }
}
