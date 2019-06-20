using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Radicitus.Models;
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
            var numbers = _connection.GetDatabase().HashGetAll($"{guid}:numbers");
            foreach (var number in numbers)
            {
                yield return new RaffleNumberSelection
                {
                    Name = number.Value,
                    Number = int.Parse(number.Name)
                };
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

        public void PushUserNumberForRaffle(RaffleNumberSelection selection, string raffleGuid, bool isRemoved)
        {
            if (!isRemoved)
            {
                _connection.GetDatabase().HashSet($"{raffleGuid}:numbers", new HashEntry[]
                {
                    new HashEntry(selection.Number, selection.Name)
                });
                _connection.GetDatabase()
                    .SetAdd($"{selection.Name}:{raffleGuid}", selection.Number);
            }
            else
            {
                _connection.GetDatabase().HashDelete($"{raffleGuid}:numbers", selection.Number);
                _connection.GetDatabase()
                    .SetRemove($"{selection.Name}:{raffleGuid}", selection.Number);
            }
        }

        public void RemoveUserNumberForRaffle(RaffleNumberSelection selection, string raffleGuid)
        {
            _connection.GetDatabase().SetRemove($"{selection.Name}:{raffleGuid}", selection.Number);
            _connection.GetDatabase()
                .SetRemove($"{selection.Name}:{raffleGuid}", selection.Number);
        }

        public async Task<IEnumerable<string>> GetNumbersForUserInRaffle(string guid, string username)
        {
            var items = await _connection.GetDatabase().SetMembersAsync($"{username}:{guid}");
            var list = new List<string>();
            foreach (var item in items)
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
            foreach (var item in items)
            {
                numbers.Add(JsonConvert.DeserializeObject<RaffleNumberSelection>(item));
            }
            return numbers;
        }

        public async Task AddConnectedUserToSet(string connectionId, string raffleGuid, string user)
        {
            await _connection.GetDatabase().StringSetAsync($"{connectionId}:{raffleGuid}", user);
            await _connection.GetDatabase().SetAddAsync($"Connections:{raffleGuid}", connectionId);
        }

        public async Task RemoveConnectedUserFromSet(string connectionId, string raffleGuid)
        {
            await _connection.GetDatabase().KeyDeleteAsync($"{connectionId}:{raffleGuid}");
            await _connection.GetDatabase().SetRemoveAsync($"Connections:{raffleGuid}", connectionId);
        }

        public async Task<IEnumerable<ConnectedUser>> GetConnectedUsersForRaffle(string raffleGuid)
        {
            var connectionIds = await _connection.GetDatabase().SetMembersAsync($"Connections:{raffleGuid}");
            var users = new List<ConnectedUser>();
            foreach (var id in connectionIds)
            {
                var name = await _connection.GetDatabase().StringGetAsync($"{id}:{raffleGuid}");
                users.Add(new ConnectedUser
                {
                    ConnectionId = id,
                    Name = name
                });
            }
            return users;
        }

        public async Task<string> GetConnectedUserName(string connectionId, string raffleGuid)
        {
            var ret = await _connection.GetDatabase().StringGetAsync($"{connectionId}:{raffleGuid}");
            return ret;
        }
    }
}
