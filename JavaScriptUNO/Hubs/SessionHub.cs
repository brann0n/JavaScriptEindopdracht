using System;
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
		/// <summary>
		/// Proxy function used to retrieve all the current sessions.
		/// </summary>
		public void GetSessions()
		{
			Clients.Caller.setSessions(MvcApplication.Manager.GetGameSessions());
		}

		/// <summary>
		/// Proxy function that creates a new game, and then redirects you to it.
		/// </summary>
		/// <param name="gameName"></param>
		public void CreateSession(string gameName)
		{
			string id = MvcApplication.Manager.CreateNewSession(gameName);
			Clients.All.setSessions(MvcApplication.Manager.GetGameSessions());
			Clients.Caller.redirectToGame(id);
		}

		/// <summary>
		/// Proxy function that creates a session for the client and uses a callback to return that id to the session page.
		/// </summary>
		/// <param name="hostSessionId">id of the game you want to connect to</param>
		/// <returns>unique client id</returns>
		public string CreateClientSession(string hostSessionId)
		{
			//no need to create a client session, just find an empty player session for the provided game
			ServerGameSession session = MvcApplication.Manager.FindSession(hostSessionId);

			return session.game.Players.FirstOrDefault(n => n.connid == "").id;
		}
	}
}