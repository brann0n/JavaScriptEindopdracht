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

					game.game.CreateNewPlayerObjects();
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
				//Clients.Caller.startGame(game.clientIds);

				//remove the empty player objects: no need for new players to connect after the game has started
				game.game.Players.RemoveAll(n => n.connid == "");
				Clients.Caller.startGame(game.game.Players);
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
				if(sGame.game.FullDeck.Count == 0)
				{
					//new game, select first player from list.
					sGame.game = game;
					sGame.game.CurrentPlayer = sGame.game.Players[0].id;
				}
				else
				{
					sGame.game = game;
				}
               
                sGame.UpdateAll();
            }
            else
            {
                Clients.Caller.endSession("unkown game id was passed to the server.");
            }
        }

		public void ConfirmCardGame(UnoGame game, bool success)
		{
			if (success)
			{
				string oldPlayer = game.CurrentPlayer;
				int newPlayerId = game.Players.FindIndex(n => n.id == oldPlayer) + 1;
				if(game.Players.Count > newPlayerId)
				{
					game.CurrentPlayer = game.Players[newPlayerId].id;
				}
				else
				{
					game.CurrentPlayer = game.Players[0].id;
				}		
				
				PushGame(game);
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