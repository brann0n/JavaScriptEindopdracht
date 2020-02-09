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
        public void Hello()
        {
            Clients.All.hello();
        }

        public void GetSessions()
        {
            List<GameSesssion> s = new List<GameSesssion>();
            s.Add(new GameSesssion {
                GameName = "Dikke",
                PlayerCount = 1,
                PlayerTotal = 8
            });

            s.Add(new GameSesssion
            {
                GameName = "Was",
                PlayerCount = 2,
                PlayerTotal = 8
            });

            s.Add(new GameSesssion
            {
                GameName = "Bever",
                PlayerCount = 3,
                PlayerTotal = 8
            });

            Clients.Caller.setSessions(s);
        }
    }
}