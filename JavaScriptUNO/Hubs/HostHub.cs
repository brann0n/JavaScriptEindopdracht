using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JavaScriptUNO.Models;
using Microsoft.AspNet.SignalR;

namespace JavaScriptUNO.Hubs
{
    public class HostHub : Hub
    {
        public void InitGame(string gameId)
        {
            ServerGameSession game = MvcApplication.Manager.FindSession(gameId);
            if (game != null)
            {
                if (string.IsNullOrEmpty(game.GameConnectionId))
                {
                    if (!game.GameStarted)
                    {
                        game.GameConnectionId = Context.ConnectionId;
                        Clients.Caller.setGameMode("Awaiting players...");
                    }
                    else
                    {
                        Clients.Caller.endSession("game has already started.");
                    }
                }
                else
                {
                    Clients.Caller.endSession("game is already being hosted somewhere else.");
                }
            }
            else
            {
                Clients.Caller.endSession("unkown game id was passed to the server.");
            }
        }

        public void StartGame(string gameId)
        {
            ServerGameSession game = MvcApplication.Manager.FindSession(gameId);
            if (game != null)
            {
                game.GameStarted = true;

                //call the host with all the current player id's
                game.UpdateClients();
                game.UpdateHost();
            }
            else
            {
                Clients.Caller.endSession("unkown game id was passed to the server.");
            }
        }
    }
}