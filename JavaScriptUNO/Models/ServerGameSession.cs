using JavaScriptUNO.Hubs;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavaScriptUNO.Models
{
    public class ServerGameSession
    {
        public string GameId { get; set; }
        public string GameName { get; set; }
        public string GameConnectionId { get; set; }
        public List<string> clientIds { get; set; }
        public int MaxClients { get; set; }
        public bool GameStarted { get; set; }

        private IHubContext clientHubContext;
        private IHubContext hostHubContext;

        public ServerGameSession()
        {
            clientIds = new List<string>();
            
            //assign the hub contexts to the interfaces.
            clientHubContext = GlobalHost.ConnectionManager.GetHubContext<ClientHub>();
            hostHubContext = GlobalHost.ConnectionManager.GetHubContext<HostHub>();
        }

        public void UpdateClients()
        {
            clientHubContext.Clients.Clients(clientIds).doRefresh();
        }

        public void UpdateHost()
        {
            hostHubContext.Clients.Client(GameConnectionId).doRefresh();
        }
    }
}