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
        public List<int> clientIds { get; set; }
        public int MaxClients { get; set; }

        //add functions for the cards and such
    }
}