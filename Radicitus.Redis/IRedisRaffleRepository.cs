using System.Collections.Generic;
using System.Threading.Tasks;
using Radicitus.Models.Dtos;

namespace Radicitus.Redis
{
    public interface IRedisRaffleRepository
    {
        Task AddConnectedUserToSet(string connectionId, string raffleId, string user);
        Task RemoveConnectedUserFromSet(string connectionId, string raffleId);
        Task<IEnumerable<ConnectedUser>> GetConnectedUsersForRaffle(string raffleId);
        Task<string> GetConnectedUserName(string connectionId, string raffleId);
    }
}
