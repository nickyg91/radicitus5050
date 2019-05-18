using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Radicitus.Redis;
using Raffle.Models;

namespace Radicitus.Raffle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaffleController : ControllerBase
    {
        private readonly IRaffleRepository _raffleRepo;

        public RaffleController(IRaffleRepository raffleRepo)
        {
            _raffleRepo = raffleRepo;
        }
        [HttpPost("create")]
        public IActionResult CreateRaffle(RadRaffle raffle)
        {
            var guid = Guid.NewGuid();
            raffle.RaffleGuid = guid;
            raffle.DateCreated = DateTime.Now;

            _raffleRepo.CreateRadRaffle(raffle);
            return Ok(raffle);
        }

        [HttpGet("numbers/{raffleGuid}")]
        public IActionResult GetNumbersForRaffleGrid(string raffleGuid)
        {
            var raffleNumbers = _raffleRepo.GetRadRafflesByRaffleGuid(raffleGuid);
            return Ok(raffleNumbers);
        }

        [HttpGet("{raffleGuid}/{username}/numbers")]
        public async Task<IActionResult> GetNumbersForRaffleUser(string raffleGuid, string username)
        {
            var raffleNumbers = await _raffleRepo.GetNumbersForUserInRaffle(raffleGuid, username);
            return Ok(raffleNumbers);
        }

        [HttpPost("test")]
        public async Task<IActionResult> RedisTest()
        {
            var redisTestStatus = await _raffleRepo.SetGetTest("test-key", "hello-redis");
            return Ok(redisTestStatus);
        }

        [HttpGet("winner/{raffleGuid}")]
        public async Task<IActionResult> GetWinner(string raffleGuid)
        {
            var rand = new Random();
            var randomInteger = rand.Next(1, 100);
            var doWeHaveAWinner = _raffleRepo.GetRadRafflesByRaffleGuid(raffleGuid);
            if (doWeHaveAWinner.Count() < 0)
            {
                return Ok(new RaffleNumberSelection 
                {
                    Name = null,
                    Number = randomInteger
                });
            }
            var winner = doWeHaveAWinner.FirstOrDefault(x => x.Number == randomInteger);
            if (winner == null)
            {
                return Ok(new RaffleNumberSelection 
                {
                    Name = null,
                    Number = randomInteger
                });
            }
            var raffle = await _raffleRepo.GetRaffleByGuid(raffleGuid);
            _raffleRepo.PushNewWinnerForRaffle(raffle.RaffleName, winner.Name);
            raffle.WinnerName = winner.Name;
            raffle.WinningSquare = winner.Number;
            await _raffleRepo.UpdateRaffle(raffle);
            return Ok(winner);
        }
        
        [HttpGet("raffles")]
        public IActionResult GetAllRaffles()
        {
            var raffles = _raffleRepo.GetRadRaffles();
            var totalWinnings = raffles.Select(x => x.AmountWon).Sum();
            
            return Ok(raffles);
        }
    }
}