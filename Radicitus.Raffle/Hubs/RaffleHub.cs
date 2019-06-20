using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Radicitus.Models;
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
            await Groups.AddToGroupAsync(Context.ConnectionId, raffleGuid);
            var connectedUsers = await _repo.GetConnectedUsersForRaffle(raffleGuid);
            await Clients.Caller.SendAsync("PopulateConnectedUsers", connectedUsers);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var raffleGuid = httpContext.Request.Query["raffleGuid"];
            var userLeaving = await _repo.GetConnectedUserName(Context.ConnectionId, raffleGuid);
            await Clients.GroupExcept(raffleGuid, Context.ConnectionId).SendAsync("UserLeft", new ConnectedUser { ConnectionId = Context.ConnectionId, Name = userLeaving });
            await _repo.RemoveConnectedUserFromSet(Context.ConnectionId, raffleGuid);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task BroadcastSelectedNumbersToRaffleGroup(RaffleNumberSelection selection)
        {
            var httpContext = Context.GetHttpContext();
            var raffleGuid = httpContext.Request.Query["raffleGuid"];

            _repo.PushUserNumberForRaffle(selection, raffleGuid, selection.IsRemoved);
            await Clients.GroupExcept(raffleGuid, Context.ConnectionId).SendAsync("SendNumbers", selection);
        }

        public async Task UserConnectedToRaffle(string user)
        {
            var httpContext = Context.GetHttpContext();
            var raffleGuid = httpContext.Request.Query["raffleGuid"];
            await _repo.AddConnectedUserToSet(Context.ConnectionId, raffleGuid, user);
            await Clients.GroupExcept(raffleGuid, Context.ConnectionId).SendAsync("UserConnected", new ConnectedUser { ConnectionId = Context.ConnectionId, Name = user });
        }
    }
}
