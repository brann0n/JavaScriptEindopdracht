using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                if (string.IsNullOrEmpty(game.GameConnectionId) || game.GameConnectionId == Context.ConnectionId)
                {
                    game.GameConnectionId = Context.ConnectionId;
                    Clients.Caller.setGameMode("Awaiting players...");
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

        public void StartGame()
        {
            ServerGameSession game = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
            if (game != null)
            {
                game.GameStarted = true;
                //call the host with all the current player id's
                Clients.Caller.startGame(game.clientIds);
            }
            else
            {
                Clients.Caller.endSession("unkown game id was passed to the server.");
            }
        }

        public void PushGame(UnoGame game)
        {
            ServerGameSession sGame = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
            if (sGame != null)
            {
                sGame.game = game;
                sGame.UpdateAll();
            }
            else
            {
                Clients.Caller.endSession("unkown game id was passed to the server.");
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
            //find game by connection id -> dbe1b95e-ea84-40f9-a570-455ca8edf6e2
            ServerGameSession game = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
            if (game != null)
            {
                game.GameConnectionId = null;
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}