using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    if (!game.GameStarted)
                    {
                        game.clientIds.Add(connId);
                        game.UpdateAll();
                    }
                    else
                    {
                        Clients.Caller.endSession("game has already started.");
                    }
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

        public void Update()
        {
            ServerGameSession game = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
            if (game != null)
            {
                game.UpdateAll();
            }
            else
            {
                Clients.Caller.endSession("unkown game id was passed to the server.");
            }
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            ServerGameSession game = MvcApplication.Manager.FindSessionByClientConnectionId(Context.ConnectionId);
            if(game != null)
            {
                game.clientIds.Remove(Context.ConnectionId); //only remove from clientIds because once the game starts the players cards should stay out of rotation
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}