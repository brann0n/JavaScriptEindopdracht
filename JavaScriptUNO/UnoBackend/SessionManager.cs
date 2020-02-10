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

        public string CreateNewSession(string GameName)
        {
            string id = Guid.NewGuid().ToString();
            Sessions.Add(new ServerGameSession
            {
                GameName = GameName,
                MaxClients = 8,
                GameId = id
            });
            return id;
        }

        public List<GameSession> GetGameSessions()
        {
            List<GameSession> fndSessions = new List<GameSession>();
            foreach(var s in Sessions)
            {
                fndSessions.Add(new GameSession {
                    GameId = s.GameId,
                    GameName = s.GameName,
                    PlayerCount = s.clientIds.Count,
                    PlayerTotal = s.MaxClients
                });
            }

            return fndSessions;
        }
    }
}