using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Raffle.Models;
namespace Radicitus.Redis
{
    public interface IRaffleRepository
    {
        Task<RadRaffle> GetRaffleByGuid(string guid);
        IEnumerable<RaffleNumberSelection> GetRadRafflesByRaffleGuid(string guid);
        IEnumerable<RadRaffle> GetRadRaffles();
        void PushNewWinnerForRaffle(string raffleName, string winnerName);
        IEnumerable<string> GetWinnersOfRaffles();
        Task<string> SetGetTest(string key, string value);
        void CreateRadRaffle(RadRaffle raffle);
        Task<RadRaffle> GetLatestRadRaffle();
        void PushUserNumberForRaffle(RaffleNumberSelection selection, string raffleGuid);
        void RemoveUserNumberForRaffle(RaffleNumberSelection selection, string raffleGuid);
        Task<IEnumerable<string>> GetNumbersForUserInRaffle(string guid, string username);
    }
}
