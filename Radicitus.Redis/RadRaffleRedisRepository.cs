using System.Collections.Generic;
using System.Threading.Tasks;
using Radicitus.Models.Dtos;
using StackExchange.Redis;

namespace Radicitus.Redis
{
    public class RadRaffleRedisRepository : IRedisRaffleRepository
    {
        private readonly ConnectionMultiplexer _connection;

        public RadRaffleRedisRepository(string connection)
        {
            _connection = ConnectionMultiplexer.Connect(connection);
        }

        public async Task AddConnectedUserToSet(string connectionId, string raffleId, string user)
        {
            await _connection.GetDatabase().StringSetAsync($"{connectionId}:{raffleId}", user);
            await _connection.GetDatabase().SetAddAsync($"Connections:{raffleId}", connectionId);
        }

        public async Task RemoveConnectedUserFromSet(string connectionId, string raffleId)
        {
            await _connection.GetDatabase().KeyDeleteAsync($"{connectionId}:{raffleId}");
            await _connection.GetDatabase().SetRemoveAsync($"Connections:{raffleId}", connectionId);
        }

        public async Task<IEnumerable<ConnectedUser>> GetConnectedUsersForRaffle(string raffleId)
        {
            var connectionIds = await _connection.GetDatabase().SetMembersAsync($"Connections:{raffleId}");
            var users = new List<ConnectedUser>();
            foreach (var id in connectionIds)
            {
                var name = await _connection.GetDatabase().StringGetAsync($"{id}:{raffleId}");
                users.Add(new ConnectedUser
                {
                    ConnectionId = id,
                    Name = name
                });
            }
            return users;
        }

        public async Task<string> GetConnectedUserName(string connectionId, string raffleId)
        {
            var ret = await _connection.GetDatabase().StringGetAsync($"{connectionId}:{raffleId}");
            return ret;
        }
    }
}
