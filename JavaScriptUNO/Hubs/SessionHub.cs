﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JavaScriptUNO.Models;
using Microsoft.AspNet.SignalR;

namespace JavaScriptUNO.Hubs
{
	/// <summary>
	/// This class contains the server side code for managing sessions
	/// </summary>
	public class SessionHub : Hub
	{
		public static object PlayerSelectionLock = new object();

		/// <summary>
		/// Proxy function that creates a new game, and then redirects you to it.
		/// </summary>
		/// <param name="gameName"></param>
		public void CreateSession(string gameName, int playersize)
		{
			if(playersize <= UnoGame.MAX_PLAYER_SIZE)
            {
				string ip = Context.Request.Environment["server.RemoteIpAddress"].ToString();
				string id = MvcApplication.Manager.CreateNewSession(gameName, playersize, ip);
				Clients.Caller.redirectToGame(id);
			}		
		}

		/// <summary>
		/// Checks if the password belongs to a game and if the game is still available
		/// </summary>
		/// <param name="password"></param>
		/// <returns>Returns the status message, if the game exists, a redirect will happen through a seperate client call.</returns>
		public string CreateClientSessionFromPassword(string password)
        {
			ServerGameSession session = MvcApplication.Manager.FindSessionByPassword(password);
			if (session == null)
				return "Game does not exists, try again please.";

			if (!session.game.HasAvailablePlayerSpots())
				return "This game is full, you can create a new one or check if you have the correct code.";

			if (session.GameStarted)
				return "Game has already started, ask the game owner to wait longer next time.";

            lock (PlayerSelectionLock) //this is going to cause issues with more active games, players will have to wait a bit longer before they get a selectable game assigned
            {
				PlayerObject p = session.game.Players.FirstOrDefault(n => n.connid == "" && !n.UserPicked);
				p.UserPicked = true;
				string clientId = p.id; //todo prevent racecondition / deadlock if 2 players join at the same time by using a request queue of some sort.
				Clients.Caller.redirectToClientGame(clientId);
			}

			return "Game has been found, you will be redirected shortly...";
        }
	}
}