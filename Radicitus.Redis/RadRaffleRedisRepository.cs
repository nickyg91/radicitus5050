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
            var raffleJson = await _connection.GetDatabase().StringGetAsync($"Raffle:{guid}");
            return JsonConvert.DeserializeObject<RadRaffle>(raffleJson);
        }

        public IEnumerable<RaffleNumberSelection> GetRadRafflesByRaffleGuid(string guid)
        {
            var numbers = _connection.GetDatabase().ListRange($"{guid}:numbers");
            foreach (var number in numbers)
            {
                yield return JsonConvert.DeserializeObject<RaffleNumberSelection>(number);
            }
        }

        public IEnumerable<RadRaffle> GetRadRaffles()
        {
            var guids = _connection.GetDatabase().ListRange("raffles");
            foreach (var guid in guids)
            {
                var raffle = _connection.GetDatabase().StringGet($"Raffle:{guid}");
                yield return JsonConvert.DeserializeObject<RadRaffle>(raffle);
            }
        }

        public void PushNewWinnerForRaffle(string raffleName, string winnerName)
        {
            _connection.GetDatabase().ListRightPush("winners", $"{raffleName} - {winnerName}");
        }

        public IEnumerable<string> GetWinnersOfRaffles()
        {
            var winners = _connection.GetDatabase().ListRange("winners");
            foreach (var winner in winners)
            {
                yield return winner;
            }
        }

        public async Task<string> SetGetTest(string key, string value)
        {
            await _connection.GetDatabase().StringSetAsync(key, value);
            return await _connection.GetDatabase().StringGetAsync(key);
        }

        public void CreateRadRaffle(RadRaffle raffle)
        {
            var serializedObject = JsonConvert.SerializeObject(raffle);
            _connection.GetDatabase().ListLeftPush("raffles", raffle.RaffleGuid.ToString());
            _connection.GetDatabase().StringSet($"Raffle:{raffle.RaffleGuid.ToString()}", serializedObject);
        }

        public async Task<RadRaffle> GetLatestRadRaffle()
        {
            var lastRaffle = await _connection.GetDatabase().ListGetByIndexAsync("raffles", 0);
            return JsonConvert.DeserializeObject<RadRaffle>(lastRaffle);
        }

        public void PushUserNumberForRaffle(RaffleNumberSelection selection, string raffleGuid)
        {
            _connection.GetDatabase().ListRightPush($"{raffleGuid}:numbers", JsonConvert.SerializeObject(selection));
            _connection.GetDatabase()
                .ListRightPush($"{selection.Name}:{raffleGuid}", selection.Number);
        }

        public void RemoveUserNumberForRaffle(RaffleNumberSelection selection, string raffleGuid)
        {
            _connection.GetDatabase().ListRemove($"{selection.Name}:{raffleGuid}", selection.Number);
        }

        public async Task<IEnumerable<string>> GetNumbersForUserInRaffle(string guid, string username)
        {
            var items = await _connection.GetDatabase().ListRangeAsync($"{username}:{guid}", 0);
            var list = new List<string>();
            foreach(var item in items)
            {
                list.Add(item);
            }
            return list;
        }

        public async Task UpdateRaffle(RadRaffle raffle)
        {
            await _connection.GetDatabase().StringSetAsync($"Raffle:{raffle.RaffleGuid.ToString()}", JsonConvert.SerializeObject(raffle));
            await _connection.GetDatabase().StringSetAsync(raffle.RaffleGuid.ToString(), JsonConvert.SerializeObject(raffle));
        }

        public async Task<IEnumerable<RaffleNumberSelection>> GetNumbersForRaffle(string guid)
        {
            var items = await _connection.GetDatabase().ListRangeAsync($"{guid}:numbers", 0);
            var numbers = new List<RaffleNumberSelection>();
            foreach(var item in items)
            {
                numbers.Add(JsonConvert.DeserializeObject<RaffleNumberSelection>(item));
            }
            return numbers;
        }
    }
}
