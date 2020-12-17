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
			//connid could stay the same over different sessions
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
				if (ses.game.Players.Select(n => n.id).Contains(id)) //id changes per game, it is okay to search like this
					return ses;
			}

			return null;
		}

		public string CreateNewSession(string GameName, int playerSize, string ip)
		{
			string sessionPassword = CreateRandomSessionCode(5);
			var session = FindSessionByPassword(sessionPassword);
			while (session != null) //one session already has this password, chance is one in a million, but still
            {
				sessionPassword = CreateRandomSessionCode(5);
				session = FindSessionByPassword(sessionPassword);
            }

			string id = Guid.NewGuid().ToString();
			Sessions.Add(new ServerGameSession(playerSize)
			{
				GameName = GameName,
				GameId = id,
				GameStarted = false,
				GamePassword = sessionPassword,
				HostIp = ip
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

		private string CreateRandomSessionCode(int length)
        {
			var chars = "abcdefghijklmnopqrstuvwxyz0123456789";
			var stringChars = new char[length];
			var random = new Random();

			for (int i = 0; i < length; i++)
			{
				stringChars[i] = chars[random.Next(chars.Length)];
			}
			return new string(stringChars);
		}

		public ServerGameSession FindSessionByPassword(string password)
        {
			return Sessions.FirstOrDefault(n => n.GamePassword == password);
		}
	}
}