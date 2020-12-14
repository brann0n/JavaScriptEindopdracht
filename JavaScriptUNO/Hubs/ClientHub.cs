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
		/// <summary>
		/// Proxy function that connects this client with a Game host
		/// </summary>
		/// <param name="hostId">id of the host</param>
		/// <param name="clientId">id of the current client (not connection id)</param>
		/// <param name="playername">the name provided by the client.</param>
		/// <returns></returns>
		public async Task SubscribeToHost(string hostId, string clientId, string playername)
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
						await Clients.Caller.endSession("This place has already been taken by another player.");
					}
					else
					{
						game.game.Players.First(n => n.id == clientId).connid = connId;
						game.game.Players.First(n => n.id == clientId).name = playername;
						await game.UpdateAll();
					}
				}
				else
				{
					await Clients.Caller.endSession("This game is already at its max players.");
				}
			}
			else
			{
				await Clients.Caller.endSession("This game does not exist");
			}
		}

		/// <summary>
		/// Proxy function that requests a card from the Draw Pile at the Game host
		/// </summary>
		public async Task DrawCardFromDeck()
		{
			string connId = Context.ConnectionId;
			ServerGameSession session = MvcApplication.Manager.FindSessionByClientConnectionId(Context.ConnectionId);

			if (session != null)
			{
				UnoGame game = session.game;
				PlayerObject player = game.Players.FirstOrDefault(n => n.connid == connId);
				await session.DrawCard(player.id);
			}
		}

		/// <summary>
		/// Proxy function that sends the pressed card to the Game host
		/// </summary>
		/// <param name="cardName"></param>
		public async Task PostCard(string cardName)
		{
			ServerGameSession sGame = MvcApplication.Manager.FindSessionByClientConnectionId(Context.ConnectionId);
			if (sGame != null)
			{
				UnoGame game = sGame.game;

				//get the player object
				PlayerObject player = game.Players.FirstOrDefault(n => n.connid == Context.ConnectionId);
				if (player != null)
				{
					//get the card object
					CardObject card = player.cards.FirstOrDefault(n => n.name == cardName);
					if (card != null)
					{
						//send the game to the host, and let the host process it
						await sGame.PlayCard(player.id, card);
					}
					else
					{
						//todo: maybe not end the session, but there is some cheating going on
						await Clients.Caller.endSession("The card you played was not in your posession.");
					}
				}
				else
				{
					await Clients.Caller.endSession("You are not a member of this game");
				}
			}
			else
			{
				await Clients.Caller.endSession("You are not a member of a game");
			}
		}

		/// <summary>
		/// Proxy function that processes the selected color and sends it to the Game host
		/// </summary>
		/// <param name="color"></param>
		/// <param name="effects"></param>
		public async Task SendColorToHost(string color, SpecialCardActions effects)
		{
			ServerGameSession session = MvcApplication.Manager.FindSessionByClientConnectionId(Context.ConnectionId);

			if (session != null)
			{
				PlayerObject player = session.game.Players.FirstOrDefault(n => n.connid == Context.ConnectionId);
				if (player != null)
				{
					switch (color)
					{
						case "green":
						case "yellow":
						case "red":
						case "blue":
							await session.UpdateColorInHost(player, color, effects);
							break;
						case "error":
						default:
							await session.UpdateColorInHost(player, null, null);
							break;
					}
				}

			}
		}

		/// <summary>
		/// Proxy function that causes a game update on every connected screen
		/// </summary>
		/// <returns></returns>
		public async Task Update()
		{
			ServerGameSession game = MvcApplication.Manager.FindSessionByClientConnectionId(Context.ConnectionId);
			if (game != null)
			{
				await game.UpdateAll();
			}
			else
			{
				await Clients.Caller.endSession("unkown game id was passed to the server.");
			}
		}

		/// <summary>
		/// Proxy function that reports uno to the Game host
		/// </summary>
		/// <returns></returns>
		public async Task ReportUno()
		{
			ServerGameSession game = MvcApplication.Manager.FindSessionByClientConnectionId(Context.ConnectionId);
			if (game != null)
			{
				PlayerObject player = game.game.Players.FirstOrDefault(n => n.connid == Context.ConnectionId);
				player.reportedUno = true;
				await game.UpdateHost();
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