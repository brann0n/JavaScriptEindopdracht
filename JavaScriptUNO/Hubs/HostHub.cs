﻿using System;
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
		/// <summary>
		/// Proxy function that creates the game object server side.
		/// </summary>
		/// <param name="gameId"></param>
		/// <returns></returns>
		public async Task InitGame(string gameId)
		{
			ServerGameSession game = MvcApplication.Manager.FindSession(gameId);
			if (game != null)
			{
				if (string.IsNullOrEmpty(game.GameConnectionId) && !game.GameStarted)
				{
					game.GameConnectionId = Context.ConnectionId;
					await Clients.Caller.setGameMode("AWAITING_PLAYERS");

					if (game.game.Players.Count == 0)
					{
						game.game.CreateNewPlayerObjects();
					}
					else
					{
						if (game.game.HasConnectedPlayers())
						{
							await Clients.Caller.setGameMode("AWAITING_PLAYERS_REFRESHED");
							await game.UpdateHost();
						}
					}
				}
				else if (string.IsNullOrEmpty(game.GameConnectionId) && game.GameStarted)
				{
					game.GameConnectionId = Context.ConnectionId;
					await Clients.Caller.setGameMode("RESUMING_GAME");
					await game.UpdateHost();
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

		/// <summary>
		/// Proxy function that starts the game
		/// </summary>
		/// <returns></returns>
		public async Task StartGame()
		{
			ServerGameSession game = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
			if (game != null)
			{
				game.GameStarted = true;

				//remove the empty player objects: no need for new players to connect after the game has started
				game.game.Players.RemoveAll(n => n.connid == "");
				await Clients.Caller.startGame(game.game.Players);

				game.UpdateCurrentPlayingName(game.game.Players[0]);
			}
			else
			{
				Clients.Caller.endSession("unkown game id was passed to the server.");
			}
		}

		/// <summary>
		/// Proxy and local function that syncs the JS and C# backend with eachother.
		/// </summary>
		/// <param name="game"></param>
		/// <returns></returns>
		public async Task PushGame(UnoGame game)
		{
			ServerGameSession sGame = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
			if (sGame != null)
			{
				if (sGame.game.FullDeck.Count == 0)
				{
					//new game, select first player from list.
					sGame.game = game;
					sGame.game.CurrentPlayer = sGame.game.Players[0].id;
				}
				else
				{
					sGame.game = game;
				}

				await sGame.UpdateAll();
			}
			else
			{
				await Clients.Caller.endSession("unkown game id was passed to the server.");
			}
		}

		/// <summary>
		/// Function that progresses the game turn depending on the card playing status
		/// </summary>
		/// <param name="game"></param>
		/// <param name="success"></param>
		public async Task ConfirmCardGame(UnoGame game, bool success)
		{
			if (success)
			{
				string currentPlayer = game.CurrentPlayer;
				game.CurrentPlayer = GetNextPlayerId(game, null);
				int drawCards = CheckUno(game, currentPlayer);
				await PushGame(game);

				//set the current playing name:

				var session = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
				session.UpdateCurrentPlayingName(game.Players.FirstOrDefault(n => n.id == game.CurrentPlayer));

				if (drawCards > 0)
				{
					Clients.Caller.drawCardFromSpecial(currentPlayer, drawCards);
				}
				else if (drawCards == -69)
				{
					Clients.Caller.gameWon(currentPlayer);
				}
			}
			else
			{
				await PushGame(game);
			}
		}

		/// <summary>
		/// Local function that is called after every card action, to determain if the current player has uno.
		/// </summary>
		/// <param name="game"></param>
		/// <param name="playerId"></param>
		/// <returns></returns>
		private int CheckUno(UnoGame game, string playerId)
		{
			PlayerObject player = game.Players.FirstOrDefault(n => n.id == playerId);
			if (player.cards.Count == 1)
			{
				if (player.reportedUno)
				{
					// allow
					player.reportedUno = false;
					Clients.Caller.displayMessage($"Player {player.name} has UNO!");
				}
				else
				{
					//give player 2 cards
					player.reportedUno = false;
					return 2;
				}
			}

			if (player.cards.Count == 0)
			{
				//player has won
				return -69; //:)
			}

			if (player.reportedUno && player.cards.Count > 1)
			{
				//false uno, draw one card
				player.reportedUno = false;
				return 1;
			}

			return 0;
		}

		/// <summary>
		/// Local function that takes the current game and gives you the next player in turn.
		/// </summary>
		/// <param name="game">current game object</param>
		/// <param name="effects">effects object</param>
		/// <returns></returns>
		private string GetNextPlayerId(UnoGame game, SpecialCardActions effects)
		{
			int currentPlayerIndex = game.Players.FindIndex(n => n.id == game.CurrentPlayer);
			if (effects == null)
			{
				if (game.DirectionClockwise)
				{
					int newPlayerId = (game.Players.Count > (currentPlayerIndex + 1)) ? currentPlayerIndex + 1 : 0;
					return game.Players[newPlayerId].id;
				}
				else
				{
					int newPlayerId = ((currentPlayerIndex - 1) >= 0) ? currentPlayerIndex - 1 : game.Players.Count - 1;
					return game.Players[newPlayerId].id;
				}
			}
			else if (effects.skipNextPerson)
			{
				if (game.DirectionClockwise)
				{
					int newPlayerId = currentPlayerIndex + 1;
					newPlayerId = (game.Players.Count > newPlayerId) ? newPlayerId : 0;
					newPlayerId++; //adds the skip
					newPlayerId = (game.Players.Count > newPlayerId) ? newPlayerId : 0;
					return game.Players[newPlayerId].id;
				}
				else
				{
					int newPlayerId = currentPlayerIndex - 1;
					newPlayerId = (newPlayerId >= 0) ? newPlayerId : game.Players.Count - 1;
					newPlayerId--; //adds the skip
					newPlayerId = (newPlayerId >= 0) ? newPlayerId : game.Players.Count - 1;
					return game.Players[newPlayerId].id;
				}
			}

			throw new Exception("Passed an effect object without enabling special effects.");
		}

		/// <summary>
		/// Proxy function that is called when an Action card is used in the game, the function decides what the game should do.
		/// </summary>
		/// <param name="game">current game object</param>
		/// <param name="effects">effects object</param>
		/// <returns></returns>
		public async Task HandleSpecialCard(UnoGame game, SpecialCardActions effects)
		{
			ServerGameSession sGame = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
			string currentPlayer = game.CurrentPlayer;

			if (effects.sendColorWheel)
			{
				//send the colorwheel update to the current client.
				//after receiving the color wheel update advance the turn
				sGame.ShowColorWheelInClient(effects);
			}
			else if (effects.cardDrawAmount != 0)
			{
				//send the NEXT client the amount of cards.
				game.CurrentPlayer = GetNextPlayerId(game, null);
				string targetPlayer = new string(game.CurrentPlayer.ToCharArray());
				game.CurrentPlayer = GetNextPlayerId(game, null);
				await PushGame(game);
				Clients.Caller.drawCardFromSpecial(targetPlayer, effects.cardDrawAmount);
			}
			else if (effects.skipNextPerson)
			{
				//the skip is handled in code
				game.CurrentPlayer = GetNextPlayerId(game, effects);

			}
			else if (effects.reverseOrder)
			{
				game.DirectionClockwise = !game.DirectionClockwise;
				if (game.Players.Count > 2)
				{
					game.CurrentPlayer = GetNextPlayerId(game, null);
				}
			}
			int drawCards = CheckUno(game, currentPlayer);
			await PushGame(game);
			//set the current playing name:
			MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId).UpdateCurrentPlayingName(game.Players.FirstOrDefault(n => n.id == game.CurrentPlayer));

			if (drawCards > 0)
			{
				Clients.Caller.drawCardFromSpecial(currentPlayer, drawCards);
			}
			else if (drawCards == -69)
			{
				Clients.Caller.gameWon(currentPlayer);
			}
		}

		/// <summary>
		/// Proxy function that is called after a user picks a color, the function checks if it needs to send cards from the draw four card.
		/// </summary>
		/// <param name="game">game object</param>
		/// <param name="effects">the effects object</param>
		/// <returns></returns>
		public async Task HandleSpecialAfterColorPick(UnoGame game, SpecialCardActions effects)
		{
			ServerGameSession sGame = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);

			if (effects.sendColorWheel)
			{
				if (effects.cardDrawAmount != 0)
				{
					//send the NEXT client the amount of cards.
					game.CurrentPlayer = GetNextPlayerId(game, null);
					string targetPlayer = new string(game.CurrentPlayer.ToCharArray());
					game.CurrentPlayer = GetNextPlayerId(game, null);
					await PushGame(game);
					Clients.Caller.drawCardFromSpecial(targetPlayer, effects.cardDrawAmount);
				}
				else
				{
					game.CurrentPlayer = GetNextPlayerId(game, null);
				}

				await PushGame(game);

				//set the current playing name:
				MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId).UpdateCurrentPlayingName(game.Players.FirstOrDefault(n => n.id == game.CurrentPlayer));
			}
		}

		/// <summary>
		/// Proxy funtion that causes an update on all connected screens.
		/// </summary>
		/// <returns></returns>
		public async Task Update()
		{
			ServerGameSession game = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
			if (game != null)
			{
				await game.UpdateAll();
			}
			else
			{
				Clients.Caller.endSession("unkown game id was passed to the server.");
			}
		}

		/// <summary>
		/// Proxy function that allows the host to send a message to all connected clients (this is only implemented in the console atm)
		/// </summary>
		/// <param name="message">the message to relay</param>
		public void RelayMessage(string message)
		{
			ServerGameSession sGame = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
			if (sGame != null)
			{
				sGame.MessageClients(message);
			}
			else
			{
				Clients.Caller.endSession("unkown game id was passed to the server.");
			}
		}

		/// <summary>
		/// Proxy function that stops the game and sends the clients back to the lobby
		/// </summary>
		/// <returns></returns>
		public async Task EndGame()
		{
			ServerGameSession sGame = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
			await sGame?.EndGameForClients();
			MvcApplication.Manager.EndGame(sGame);

			await GlobalHost.ConnectionManager.GetHubContext<SessionHub>().Clients.All.setSessions(MvcApplication.Manager.GetGameSessions());
		}

		/// <summary>
		/// Proxy function that processes the game after someone won.
		/// </summary>
		/// <param name="PlayerId">id of the player that won</param>
		/// <returns></returns>
		public async Task ProcessGameWon(string PlayerId)
		{
			ServerGameSession game = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
			if (game != null)
			{
				await game.GameWon(PlayerId);
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