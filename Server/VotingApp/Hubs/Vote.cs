using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VotingApp.Hubs
{
    public class Vote : Hub
    {

        public Task JoinRoom(int voteID)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, voteID.ToString());
        }


        public Task LeaveRoom(int voteID)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, voteID.ToString());
        }

        public async Task SendOptionValues(int voteID,object result)
        {
             await Clients.Group(voteID.ToString()).SendAsync("sendToAll", result);
        }

    }
}
