using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Radicitus.Data.Contexts.Raffles.Entities;
using Radicitus.Data.Contexts.Raffles.Implementations;
using Radicitus.Models.Dtos;
using Radicitus.Models.Interfaces;
using Radicitus.Models.ReferenceMapper;
using RaffleNumber = Radicitus.Data.Contexts.Raffles.Entities.RaffleNumber;

namespace Radicitus.Raffle.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaffleController : ControllerBase
    {
        private readonly RaffleRepository _raffleRepo;

        public RaffleController(RaffleRepository raffleRepo)
        {
            _raffleRepo = raffleRepo;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateRaffle(RadicitusRaffle raffle)
        {
            raffle.DateCreatedUtc = DateTime.UtcNow;

            var raffleToCreate = ReferenceMapper.MapToNewInstance<RadicitusRaffle, RadRaffle, IRadRaffle>(raffle);
            raffleToCreate.StartDateUtc = raffle.StartDateUtc.ToUniversalTime();
            raffleToCreate.EndDateUtc = raffle.EndDateUtc.ToUniversalTime();
            _raffleRepo.Add(raffleToCreate);
            await _raffleRepo.SaveChangesAsync();

            raffle = ReferenceMapper.MapToNewInstance<RadRaffle, RadicitusRaffle, IRadRaffle>(raffleToCreate);
            return Ok(raffle);
        }

        [HttpGet("numbers/{raffleId}")]
        public IActionResult GetNumbersForRaffleGrid(int raffleId)
        {
            var raffleNumbers = _raffleRepo.GetRaffleNumbersByRaffleId(raffleId).ToList();
            var raffleDtos = raffleNumbers.Select(ReferenceMapper
                .MapToNewInstance<RaffleNumber, Models.Dtos.RaffleNumber, IRaffleNumber>).ToList();
            return Ok(raffleDtos);
        }

        [HttpGet("{id}/{username}/numbers")]
        public IActionResult GetNumbersForRaffleUser(int id, string username)
        {
            var raffleNumbers = _raffleRepo.GetRaffleNumbersByIdAndName(id, username).ToList();
            var raffleDtos = raffleNumbers
                .Select(ReferenceMapper.MapToNewInstance<RaffleNumber, Models.Dtos.RaffleNumber, IRaffleNumber>)
                .ToList();
            return Ok(raffleDtos);
        }

        [HttpGet("winner/{id}")]
        public async Task<IActionResult> GetWinner(int id)
        {
            var rand = new Random();
            var randomInteger = rand.Next(1, 100);
            var raffleNumbers = _raffleRepo.GetRaffleNumbersByRaffleId(id).ToList();
            var raffle = await _raffleRepo.GetRaffleById(id);
            if (!raffleNumbers.Any())
            {
                return Ok(new RaffleNumberSelection 
                {
                    Name = null,
                    Number = randomInteger
                });
            }
            var winner = raffleNumbers.FirstOrDefault(x => x.Number == randomInteger);
            if (winner == null)
            {
                return Ok(new RaffleNumberSelection 
                {
                    Name = null,
                    Number = randomInteger
                });
            }
            var totalWinnings = (raffle.SquareWorthAmount * raffleNumbers.Count) / 2;
            raffle.AmountWon = raffle.SquareWorthAmount * raffleNumbers.Count;
            raffle.WinnerName = winner.Name;
            raffle.WinningSquare = winner.Number;
            _raffleRepo.Update(raffle);
            await _raffleRepo.SaveChangesAsync();
            return Ok(new RaffleNumberSelection
            {
                Name = winner.Name,
                Number = winner.Number
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRaffle(int id)
        {
            var raffle = await _raffleRepo.GetRaffleById(id);
            var raffleDto = ReferenceMapper.MapToNewInstance<RadRaffle, RadicitusRaffle, IRadRaffle>(raffle);
            return Ok(raffleDto);
        }

        [HttpGet("raffles")]
        public IActionResult GetAllRaffles()
        {
            var raffles = _raffleRepo.GetRaffles().ToList();
            var mappedRaffles = raffles.Select(ReferenceMapper.MapToNewInstance<RadRaffle, RadicitusRaffle, IRadRaffle>);
            return Ok(mappedRaffles);
        }
    }
}