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
        //public List<string> clientIds { get; set; }
        public UnoGame game { get; set; }
        public int MaxClients { get; set; }
        public bool GameStarted { get; set; }

        private IHubContext clientHubContext;
        private IHubContext hostHubContext;
		private IHubContext sessionHubContext;

		public ServerGameSession()
        {
			game = new UnoGame();
			//assign the hub contexts to the interfaces.
			clientHubContext = GlobalHost.ConnectionManager.GetHubContext<ClientHub>();
            hostHubContext = GlobalHost.ConnectionManager.GetHubContext<HostHub>();
			sessionHubContext = GlobalHost.ConnectionManager.GetHubContext<SessionHub>();

		}

        public void UpdateAll()
        {
            UpdateClients();
            UpdateHost();
			sessionHubContext.Clients.All.setSessions(MvcApplication.Manager.GetGameSessions());
		}

        public void UpdateClients()
        {
            if (game != null)
                clientHubContext.Clients.Clients(game.Players.Where(n =>n.connid != "").Select(n => n.connid).ToList()).doRefresh(game);
        }

        public void UpdateHost()
        {
            hostHubContext.Clients.Client(GameConnectionId).updatePlayerCount(game.Players.Where(n => n.connid != "").Select(n => n.connid).Count(), MaxClients);
            if (game != null)
                hostHubContext.Clients.Client(GameConnectionId).doRefresh(game);
        }
    }
}