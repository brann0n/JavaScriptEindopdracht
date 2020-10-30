using JavaScriptUNO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavaScriptUNO.UnoBackend
{
	public class SessionManager
	{
		public List<ServerGameSession> Sessions;
		public SessionManager()
		{
			Sessions = new List<ServerGameSession>();
		}

		public ServerGameSession FindSession(string id)
		{
			return Sessions.FirstOrDefault(n => n.GameId == id);
		}

		public ServerGameSession FindSessionByConnectionId(string id)
		{
			return Sessions.FirstOrDefault(n => n.GameConnectionId == id);
		}

		public ServerGameSession FindSessionByClientConnectionId(string id)
		{
			foreach (ServerGameSession ses in Sessions)
			{
				if (ses.game.Players.Select(n => n.connid).Contains(id))
					return ses;
			}

			return null;
		}

		public ServerGameSession FindSessionByClientId(string id)
		{
			foreach (ServerGameSession ses in Sessions)
			{
				if (ses.game.Players.Select(n => n.id).Contains(id))
					return ses;
			}

			return null;
		}

		public string CreateNewSession(string GameName)
		{
			string id = Guid.NewGuid().ToString();
			Sessions.Add(new ServerGameSession
			{
				GameName = GameName,
				MaxClients = 8,
				GameId = id,
				GameStarted = false
			});
			return id;
		}

		public List<GameSession> GetGameSessions()
		{
			List<GameSession> fndSessions = new List<GameSession>();
			foreach (ServerGameSession s in Sessions)
			{
				fndSessions.Add(new GameSession
				{
					GameId = s.GameId,
					GameName = s.GameName,
					PlayerCount = s.game.Players.Where(n => n.connid != "").Count(),
					PlayerTotal = s.MaxClients,
					GameStarted = s.GameStarted
				});
			}

			return fndSessions;
		}

		public void EndGame(ServerGameSession session)
		{
			Sessions.Remove(session);
		}
	}
}