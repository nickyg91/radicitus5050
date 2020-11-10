using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Radicitus.Data.Contexts.Raffles.Entities;
using Radicitus.Data.Contexts.Raffles.Implementations;
using Radicitus.Models;
using Radicitus.Models.Dtos;
using Radicitus.Redis;

namespace Radicitus.Raffle.Hubs
{
    public class RaffleHub : Hub
    {
        private readonly IRedisRaffleRepository _repo;
        private readonly RaffleRepository _dbRepo;
        public RaffleHub(IRedisRaffleRepository repo, RaffleRepository dbRepo)
        {
            _repo = repo;
            _dbRepo = dbRepo;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var raffleId = httpContext.Request.Query["raffleId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, raffleId);
            var connectedUsers = await _repo.GetConnectedUsersForRaffle(raffleId);
            await Clients.Caller.SendAsync("PopulateConnectedUsers", connectedUsers);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var raffleId = httpContext.Request.Query["raffleId"];
            var userLeaving = await _repo.GetConnectedUserName(Context.ConnectionId, raffleId);
            await Clients.GroupExcept(raffleId, Context.ConnectionId).SendAsync("UserLeft", new ConnectedUser { ConnectionId = Context.ConnectionId, Name = userLeaving });
            await _repo.RemoveConnectedUserFromSet(Context.ConnectionId, raffleId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task BroadcastSelectedNumbersToRaffleGroup(RaffleNumberSelection selection)
        {
            var httpContext = Context.GetHttpContext();
            var raffleId = int.Parse(httpContext.Request.Query["raffleId"]);
            if (selection.IsRemoved)
            {
                var raffleNumber = _dbRepo.GetRaffleNumbersByIdAndName(raffleId, selection.Name)
                    .FirstOrDefault(x => x.Number == selection.Number);
                if (raffleNumber != null)
                {
                    _dbRepo.Remove(raffleNumber);
                    await _dbRepo.SaveChangesAsync();
                }
            }
            else
            {
                var raffleNumber = new Radicitus.Data.Contexts.Raffles.Entities.RaffleNumber
                {
                    Name = selection.Name,
                    Number = selection.Number,
                    RaffleId = raffleId
                };
                _dbRepo.Add(raffleNumber);
                await _dbRepo.SaveChangesAsync();
            }
            
            await Clients.GroupExcept(raffleId.ToString(), Context.ConnectionId).SendAsync("SendNumbers", selection);
        }

        public async Task UserConnectedToRaffle(string user)
        {
            var httpContext = Context.GetHttpContext();
            var raffleId = httpContext.Request.Query["raffleId"];
            await _repo.AddConnectedUserToSet(Context.ConnectionId, raffleId, user);
            await Clients.GroupExcept(raffleId, Context.ConnectionId).SendAsync("UserConnected", new ConnectedUser { ConnectionId = Context.ConnectionId, Name = user });
        }
    }
}
