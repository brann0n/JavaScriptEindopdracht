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
        public UnoGame game { get; set; }
        public int MaxClients { get; set; }
        public bool GameStarted { get; set; }

        private IHubContext clientHubContext;
        private IHubContext hostHubContext;

        public ServerGameSession()
        {
            clientIds = new List<string>();
            //game = new UnoGame();
            //assign the hub contexts to the interfaces.
            clientHubContext = GlobalHost.ConnectionManager.GetHubContext<ClientHub>();
            hostHubContext = GlobalHost.ConnectionManager.GetHubContext<HostHub>();
        }

        public void UpdateAll()
        {
            UpdateClients();
            UpdateHost();
        }

        public void UpdateClients()
        {
            if (game != null)
                clientHubContext.Clients.Clients(clientIds).doRefresh(game);
        }

        public void UpdateHost()
        {
            hostHubContext.Clients.Client(GameConnectionId).updatePlayerCount(clientIds.Count, MaxClients);
            if (game != null)
                hostHubContext.Clients.Client(GameConnectionId).doRefresh(game);
        }
    }
}