using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JavaScriptUNO.Models;
using Microsoft.AspNet.SignalR;

namespace JavaScriptUNO.Hubs
{
    public class ClientHub : Hub
    {
        public void SubscribeToHost(string hostId)
        {
            string connId = Context.ConnectionId;
            //find the session in memory
            ServerGameSession game = MvcApplication.Manager.FindSession(hostId);
            if(game != null)
            {
                //check if game is at max players
                if(game.clientIds.Count < game.MaxClients)
                {
                    game.clientIds.Add(connId);
                }
                else
                {
                    Clients.Caller.endSession("This game is already at its max players.");
                }
            }
            else
            {
                Clients.Caller.endSession("This game does not exist");
            }
        }
    }
}