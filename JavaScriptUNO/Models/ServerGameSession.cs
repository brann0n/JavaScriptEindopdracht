using JavaScriptUNO.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace JavaScriptUNO.Models
{
	/// <summary>
	/// Full Game controller, this object handles communication between host and client.
	/// </summary>
	public class ServerGameSession
	{
		public string GameId { get; set; }
		public string GameName { get; set; }
		public string GameConnectionId { get; set; }
		//public List<string> clientIds { get; set; }
		public UnoGame game { get; set; }
		public int MaxClients { get; set; }
		public bool GameStarted { get; set; }

		private IHubContext clientHubContext;
		private IHubContext hostHubContext;
		private IHubContext sessionHubContext;

		public ServerGameSession()
		{
			game = new UnoGame();
			//assign the hub contexts to the interfaces.
			clientHubContext = GlobalHost.ConnectionManager.GetHubContext<ClientHub>();
			hostHubContext = GlobalHost.ConnectionManager.GetHubContext<HostHub>();
			sessionHubContext = GlobalHost.ConnectionManager.GetHubContext<SessionHub>();
		}

		public async Task UpdateAll()
		{
			await UpdateClients();
			await UpdateHost();
			await sessionHubContext.Clients.All.setSessions(MvcApplication.Manager.GetGameSessions());
		}

		public async Task UpdateClients()
		{
			if (game != null)
				await clientHubContext.Clients.Clients(game.Players.Where(n => n.connid != "").Select(n => n.connid).ToList()).doRefresh(game);
		}

		public async Task UpdateHost()
		{
			await hostHubContext.Clients.Client(GameConnectionId).updatePlayerCount(game.Players.Where(n => n.connid != "").Select(n => n.connid).Count(), MaxClients);
			if (game != null)
				await hostHubContext.Clients.Client(GameConnectionId).doRefresh(game);
		}

		public void MessageClients(string message)
		{
			if (game != null)
				clientHubContext.Clients.Clients(game.Players.Where(n => n.connid != "").Select(n => n.connid).ToList()).displayMessage(message);
		}

		public void MessageHost(string message)
		{
			if (game != null)
				hostHubContext.Clients.Client(GameConnectionId).displayMessage(message);
		}

		public void UpdateCurrentPlayingName(PlayerObject player)
		{
			if (game != null)
			{
				clientHubContext.Clients.Clients(game.Players.Where(n => n.connid != "").Select(n => n.connid).ToList()).setCurrentPlayer(player);
				hostHubContext.Clients.Client(GameConnectionId).setCurrentPlayer(player);
			}
		}

		public void PlayCard(string playerId, CardObject card)
		{
			//sends the players card to the host for verification.
			hostHubContext.Clients.Client(GameConnectionId).playCard(playerId, card);
		}

		public void DrawCard(string playerId)
		{
			hostHubContext.Clients.Client(GameConnectionId).drawCard(playerId);
		}

		public void UpdateColorInHost(PlayerObject player, string color, SpecialCardActions effects)
		{
			hostHubContext.Clients.Client(GameConnectionId).setPickedColor(player, color, effects);
		}

		public void ShowColorWheelInClient(SpecialCardActions effects)
		{
			PlayerObject player = game.Players.FirstOrDefault(n => n.id == game.CurrentPlayer);
			clientHubContext.Clients.Client(player.connid).displayColorWheel(effects);
		}

		public async Task GameWon(string playerId)
		{
			PlayerObject player = game.Players.FirstOrDefault(n => n.id == playerId);
			await hostHubContext.Clients.Client(GameConnectionId).showWinnerAndEndGame(player);
			await EndGameForClients();
		}

		public async Task EndGameForClients()
		{
			if (game != null)
				await clientHubContext.Clients.Clients(game.Players.Where(n => n.connid != "").Select(n => n.connid).ToList()).stopGame();
		}
	}
}