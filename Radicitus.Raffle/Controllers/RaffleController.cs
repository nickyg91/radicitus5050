using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Radicitus.Data.Contexts.Raffles.Entities;
using Radicitus.Data.Contexts.Raffles.Interfaces;
using Radicitus.Models.Dtos;
using Radicitus.Models.Interfaces;
using Radicitus.Models.ReferenceMapper;

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
            //var guid = Guid.NewGuid();
            //raffle.RaffleGuid = guid;
            //raffle.DateCreated = DateTime.Now;

            //_raffleRepo.CreateRadRaffle(raffle);
            return Ok(raffle);
        }

        [HttpGet("numbers/{raffleGuid}")]
        public IActionResult GetNumbersForRaffleGrid(string raffleGuid)
        {
            //var raffleNumbers = _raffleRepo.GetRadRafflesByRaffleGuid(raffleGuid);
            return Ok();
        }

        [HttpGet("{raffleGuid}/{username}/numbers")]
        public async Task<IActionResult> GetNumbersForRaffleUser(string raffleGuid, string username)
        {
            //var raffleNumbers = await _raffleRepo.GetNumbersForUserInRaffle(raffleGuid, username);
            return Ok();
        }

        //[HttpGet("winner/{raffleGuid}")]
        //public async Task<IActionResult> GetWinner(string raffleGuid)
        //{
        //    var rand = new Random();
        //    var randomInteger = rand.Next(1, 100);
        //    var doWeHaveAWinner = _raffleRepo.GetRadRafflesByRaffleGuid(raffleGuid);
        //    if (doWeHaveAWinner.Count() < 0)
        //    {
        //        return Ok(new RaffleNumberSelection 
        //        {
        //            Name = null,
        //            Number = randomInteger
        //        });
        //    }
        //    var winner = doWeHaveAWinner.FirstOrDefault(x => x.Number == randomInteger);
        //    if (winner == null)
        //    {
        //        return Ok(new RaffleNumberSelection 
        //        {
        //            Name = null,
        //            Number = randomInteger
        //        });
        //    }
        //    var allSquares = (await _raffleRepo.GetNumbersForRaffle(raffleGuid)).ToList();
        //    var raffle = await _raffleRepo.GetRaffleByGuid(raffleGuid);
        //    var totalWinnings = (raffle.SquareWorthAmount * allSquares.Count()) / 2;
        //    _raffleRepo.PushNewWinnerForRaffle(raffle.RaffleName, winner.Name);
        //    raffle.AmountWon = totalWinnings;
        //    raffle.WinnerName = winner.Name;
        //    raffle.WinningSquare = winner.Number;
        //    await _raffleRepo.UpdateRaffle(raffle);
        //    return Ok(winner);
        //}
        
        [HttpGet("raffles")]
        public async Task<IActionResult> GetAllRaffles()
        {
            var raffles = _raffleRepo.GetRaffles().ToList();
            var mappedRaffles = raffles.Select(ReferenceMapper.MapToNewInstance<RadRaffle, Models.Dtos.Raffle, IRadRaffle>);
            
            return Ok(mappedRaffles);
        }
    }
}