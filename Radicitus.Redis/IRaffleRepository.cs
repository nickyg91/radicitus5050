using System.Collections.Generic;
using System.Threading.Tasks;
using Radicitus.Models.Dtos;

namespace Radicitus.Redis
{
    public interface IRaffleRepository
    {
        Task<Raffle> GetRaffleByGuid(string guid);
        IEnumerable<RaffleNumberSelection> GetRadRafflesByRaffleGuid(string guid);
        IEnumerable<Raffle> GetRadRaffles();
        void PushNewWinnerForRaffle(string raffleName, string winnerName);
        IEnumerable<string> GetWinnersOfRaffles();
        Task<string> SetGetTest(string key, string value);
        void CreateRadRaffle(Raffle raffle);
        Task<Raffle> GetLatestRadRaffle();
        void PushUserNumberForRaffle(RaffleNumberSelection selection, string raffleGuid, bool isRemoved);
        void RemoveUserNumberForRaffle(RaffleNumberSelection selection, string raffleGuid);
        Task<IEnumerable<string>> GetNumbersForUserInRaffle(string guid, string username);
        Task UpdateRaffle(Raffle raffle);
        Task<IEnumerable<RaffleNumberSelection>> GetNumbersForRaffle(string guid);
        Task AddConnectedUserToSet(string connectionId, string raffleGuid, string user);
        Task RemoveConnectedUserFromSet(string connectionId, string raffleGuid);
        Task<IEnumerable<ConnectedUser>> GetConnectedUsersForRaffle(string raffleGuid);
        Task<string> GetConnectedUserName(string connectionId, string raffleGuid);
    }
}
