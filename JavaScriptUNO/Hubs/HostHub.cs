using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace JavaScriptUNO.Hubs
{
    public class HostHub : Hub
    {
        public void StartGame()
        {
            Clients.Caller.setGameMode("Playing");
        }
    }
}