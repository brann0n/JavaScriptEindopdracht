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
		public void SubscribeToHost(string hostId, string clientId)
		{
			string connId = Context.ConnectionId;
			//find the session in memory
			ServerGameSession game = MvcApplication.Manager.FindSession(hostId);
			if (game != null)
			{
				//check if game is at max players
				if (game.game.Players.Where(n => n.connid != "").Count() < game.MaxClients)
				{

					if (game.game.Players.First(n => n.id == clientId).connid != "")
					{
						//game slot is in use
						Clients.Caller.endSession("This place has already been taken by another player.");
					}
					else
					{
						game.game.Players.First(n => n.id == clientId).connid = connId;
						game.UpdateAll();
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

		public void PostCard(string cardName)
		{
			string connId = Context.ConnectionId;
			ServerGameSession sGame = MvcApplication.Manager.FindSessionByClientConnectionId(connId);
			if (sGame != null)
			{
				UnoGame game = sGame.game;
				//get the player object
				PlayerObject player = game.Players.FirstOrDefault(n => n.connid == connId);
				if (player != null)
				{
					//get the card object
					CardObject card = player.cards.FirstOrDefault(n => n.name == cardName);
					if (card != null)
					{
						//send the game to the host, and let the host process it
						//sGame.UpdateAll(); //temp disabled for development purposes
					}
					else
					{
						//maybe not end the session, but there is some cheating going on
						Clients.Caller.endSession("You do not have this card");
					}
				}
				else
				{
					Clients.Caller.endSession("You are not a member of this game");
				}
			}
			else
			{
				Clients.Caller.endSession("You are not a member of a game");
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
			if (game != null)
			{
				game.game.Players.First(n => n.connid == Context.ConnectionId).connid = "";
			}

			return base.OnDisconnected(stopCalled);
		}
	}
}