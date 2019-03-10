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

        public async Task<RadRaffle> GetRaffleByGuid(string guid)
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
            _connection.GetDatabase().ListRightPush("winners", $"{raffleName} - {winnerName}");
        }

        public List<string> GetWinnersOfRaffles()
        {
            throw new NotImplementedException();
        }

        public async Task<string> SetGetTest(string key, string value)
        {
            await _connection.GetDatabase().StringSetAsync(key, value);
            return await _connection.GetDatabase().StringGetAsync(key);
        }

        public void CreateRadRaffle(RadRaffle raffle)
        {
            throw new NotImplementedException();
        }

        public async Task<RadRaffle> GetLatestRadRaffle()
        {
            var raffleLength = await _connection.GetDatabase().ListLengthAsync("raffles");
            var lastRaffle = await _connection.GetDatabase().ListGetByIndexAsync("raffles", raffleLength);
            return JsonConvert.DeserializeObject<RadRaffle>(lastRaffle);
        }

        public void PushUserNumberForRaffle(RaffleNumberSelection selection, string raffleGuid)
        {
            _connection.GetDatabase()
                .ListRightPush($"{selection.Name}:{raffleGuid}", selection.Number);
        }

        public void RemoveUserNumberForRaffle(RaffleNumberSelection selection, string raffleGuid)
        {
            _connection.GetDatabase().ListRemove($"{selection.Name}:{raffleGuid}", selection.Number);
        }
    }
}
