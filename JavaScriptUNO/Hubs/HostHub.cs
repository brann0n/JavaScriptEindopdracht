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
				if (string.IsNullOrEmpty(game.GameConnectionId) && !game.GameStarted)
				{
					game.GameConnectionId = Context.ConnectionId;
					Clients.Caller.setGameMode("AWAITING_PLAYERS");

					if (game.game.Players.Count == 0)
					{
						game.game.CreateNewPlayerObjects();
					}
					else
					{
						if (game.game.HasConnectedPlayers())
						{
							Clients.Caller.setGameMode("AWAITING_PLAYERS_REFRESHED");
							game.UpdateHost();
						}
					}
				}
				else if (string.IsNullOrEmpty(game.GameConnectionId) && game.GameStarted)
				{
					game.GameConnectionId = Context.ConnectionId;
					Clients.Caller.setGameMode("RESUMING_GAME");
					game.UpdateHost();
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

				game.UpdateCurrentPlayingName(game.game.Players[0]);
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

				sGame.UpdateAll();
			}
			else
			{
				Clients.Caller.endSession("unkown game id was passed to the server.");
			}
		}

		/// <summary>
		/// Function that progresses the game turn depending on the card playing status
		/// </summary>
		/// <param name="game"></param>
		/// <param name="success"></param>
		public void ConfirmCardGame(UnoGame game, bool success)
		{
			if (success)
			{
				game.CurrentPlayer = GetNextPlayerId(game, null);

				PushGame(game);

				//set the current playing name:
				MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId).UpdateCurrentPlayingName(game.Players.FirstOrDefault(n => n.id == game.CurrentPlayer));
			}
			else
			{
				PushGame(game);
			}
		}

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

		public void HandleSpecialCard(UnoGame game, SpecialCardActions effects)
		{
			ServerGameSession sGame = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);


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
				PushGame(game);
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

			PushGame(game);

			//set the current playing name:
			MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId).UpdateCurrentPlayingName(game.Players.FirstOrDefault(n => n.id == game.CurrentPlayer));
		}

		public void HandleSpecialAfterColorPick(UnoGame game, SpecialCardActions effects)
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
					PushGame(game);
					Clients.Caller.drawCardFromSpecial(targetPlayer, effects.cardDrawAmount);
				}
				else
				{
					game.CurrentPlayer = GetNextPlayerId(game, null);
				}

				PushGame(game);

				//set the current playing name:
				MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId).UpdateCurrentPlayingName(game.Players.FirstOrDefault(n => n.id == game.CurrentPlayer));
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

		public void EndGame()
		{
			ServerGameSession sGame = MvcApplication.Manager.FindSessionByConnectionId(Context.ConnectionId);
			sGame?.EndGameForClients();
			MvcApplication.Manager.EndGame(sGame);

			GlobalHost.ConnectionManager.GetHubContext<SessionHub>().Clients.All.setSessions(MvcApplication.Manager.GetGameSessions());
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