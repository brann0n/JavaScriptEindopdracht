﻿using JavaScriptUNO.Hubs;
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
		public string GamePassword { get; set; }
		public bool HasGameEnded { get; set; } = false;
		public string HostIp { get; set; }
		//public List<string> clientIds { get; set; }
		public UnoGame game { get; set; }
		public int MaxClients { get; set; }
		public bool GameStarted { get; set; }

		private readonly IHubContext clientHubContext;
		private readonly IHubContext hostHubContext;

		public ServerGameSession(int clientSize)
		{
			this.MaxClients = clientSize;
			game = new UnoGame(MaxClients);
			//assign the hub contexts to the interfaces.
			clientHubContext = GlobalHost.ConnectionManager.GetHubContext<ClientHub>();
			hostHubContext = GlobalHost.ConnectionManager.GetHubContext<HostHub>();
		}

		public async Task UpdateAll()
		{
			await UpdateClients();
			await UpdateHost();
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

		public async Task MessageClients(string message)
		{
			if (game != null)
				await clientHubContext.Clients.Clients(game.Players.Where(n => n.connid != "").Select(n => n.connid).ToList()).displayMessage(message);
		}

		public async Task MessageHost(string message)
		{
			if (game != null)
				await hostHubContext.Clients.Client(GameConnectionId).displayMessage(message);
		}

		public async Task UpdateCurrentPlayingName(PlayerObject player)
		{
			if (game != null)
			{
				await clientHubContext.Clients.Clients(game.Players.Where(n => n.connid != "").Select(n => n.connid).ToList()).setCurrentPlayer(player);
				await hostHubContext.Clients.Client(GameConnectionId).setCurrentPlayer(player);
			}
		}

		public async Task PlayCard(string playerId, CardObject card)
		{
			//sends the players card to the host for verification.
			await hostHubContext.Clients.Client(GameConnectionId).playCard(playerId, card);
		}

		public async Task DrawCard(string playerId)
		{
			await hostHubContext.Clients.Client(GameConnectionId).drawCard(playerId);
		}

		public async Task UpdateColorInHost(PlayerObject player, string color, SpecialCardActions effects)
		{
			await hostHubContext.Clients.Client(GameConnectionId).setPickedColor(player, color, effects);
		}

		public async Task ShowColorWheelInClient(SpecialCardActions effects)
		{
			PlayerObject player = game.Players.FirstOrDefault(n => n.id == game.CurrentPlayer);
			await clientHubContext.Clients.Client(player.connid).displayColorWheel(effects);
		}

		public async Task GameWon(string playerId)
		{
			PlayerObject player = game.Players.FirstOrDefault(n => n.id == playerId);
			await hostHubContext.Clients.Client(GameConnectionId).showWinnerAndEndGame(player);
			await EndGameForClients();
			HasGameEnded = true;
		}

		public async Task EndGameForClients()
		{
			if (game != null)
				await clientHubContext.Clients.Clients(game.Players.Where(n => n.connid != "").Select(n => n.connid).ToList()).stopGame();
		}

		public async Task KickGameAndPlayers()
        {
			await EndGameForClients();
			await hostHubContext.Clients.Client(GameConnectionId).endSession("Your game got ended by the server admin!");
		}
	}
}