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
            Clients.Caller.setSessions(MvcApplication.Manager.GetGameSessions());
        }

        public void CreateSession(string gameName)
        {
            string id = MvcApplication.Manager.CreateNewSession(gameName);
            Clients.All.setSessions(MvcApplication.Manager.GetGameSessions());
            Clients.Caller.redirectToGame(id);
        }
    }
}