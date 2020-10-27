using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavaScriptUNO.Models
{
    public class UnoGame
    {
        public List<CardObject> Deck { get; set; }
        public List<CardObject> FullDeck { get; set; }
        public string PathPrefix { get; set; }
        public List<PlayerObject> Players { get; set; }
        public List<CardObject> StockPile { get; set; }
		public string CurrentPlayer { get; set; }
        public bool DirectionClockwise { get; set; }

        public UnoGame()
        {
            Deck = new List<CardObject>();
            FullDeck = new List<CardObject>();
            Players = new List<PlayerObject>();
            StockPile = new List<CardObject>();
            DirectionClockwise = false;
		}

		public void CreateNewPlayerObjects()
		{
			//create 8 player object presets
			for(int i = 0; i < 8; i++)
			{
				PlayerObject pObject = new PlayerObject();
				pObject.id = Guid.NewGuid().ToString();
				pObject.connid = "";
				Players.Add(pObject);
			}
		}

    }
}