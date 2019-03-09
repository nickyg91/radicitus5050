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
        Task<List<RaffleNumber>> GetRadRafflesByRaffleGuid(string guid);
        List<RadRaffle> GetRadRaffles();
        void PushNewWinnerForRaffle(string raffleName, string winnerName);
        List<string> GetWinnersOfRaffles();
        Task<string> SetGetTest(string key, string value);
        void CreateRadRaffle(RadRaffle raffle);
        Task<RadRaffle> GetLatestRadRaffle();
    }
}
