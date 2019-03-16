using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Radicitus.Redis;
using Raffle.Models;

namespace Radicitus.Raffle.Hubs
{
    public class RaffleHub : Hub
    {
        private readonly IRaffleRepository _repo;
        public RaffleHub(IRaffleRepository repo)
        {
            _repo = repo;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var raffleGuid = httpContext.Request.Query["raffleGuid"];
            var username = httpContext.Request.Query["username"];
            await Groups.AddToGroupAsync(Context.ConnectionId, raffleGuid);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task BroadcastSelectedNumbersToRaffleGroup(RaffleNumberSelection selection)
        {
            var httpContext = Context.GetHttpContext();
            var raffleGuid = httpContext.Request.Query["raffleGuid"];
            _repo.PushUserNumberForRaffle(selection, raffleGuid);
            //await Clients.Group(raffleGuid).SendAsync("SendNumbers", selection);
            await Clients.GroupExcept(raffleGuid, Context.ConnectionId).SendAsync("SendNumbers", selection);
        }
    }
}
