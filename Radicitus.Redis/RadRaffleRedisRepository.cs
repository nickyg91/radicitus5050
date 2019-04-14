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

        public IEnumerable<RaffleNumber> GetRadRafflesByRaffleGuid(string guid)
        {
            var numbers = _connection.GetDatabase().ListRange($"{guid}:numbers");
            foreach (var number in numbers)
            {
                yield return JsonConvert.DeserializeObject<RaffleNumber>(number);
            }
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
            _connection.GetDatabase().ListLeftPush("raffles", JsonConvert.SerializeObject(raffle));
        }

        public async Task<RadRaffle> GetLatestRadRaffle()
        {
            var lastRaffle = await _connection.GetDatabase().ListGetByIndexAsync("raffles", 0);
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
