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

        [HttpPost("test")]
        public async Task<IActionResult> RedisTest()
        {
            var redisTestStatus = await _raffleRepo.SetGetTest("test-key", "hello-redis");
            return Ok(redisTestStatus);
        }
    }
}