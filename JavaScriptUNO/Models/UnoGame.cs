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
        public static int MAX_PLAYER_SIZE = 32;
        public readonly int PlayerCount;
        public UnoGame(int playerCount)
        {
            Deck = new List<CardObject>();
            FullDeck = new List<CardObject>();
            Players = new List<PlayerObject>();
            StockPile = new List<CardObject>();
            DirectionClockwise = true;
            PlayerCount = playerCount;
        }

        public void CreateNewPlayerObjects()
        {
            //create 8 player object presets
            for (int i = 0; i < PlayerCount; i++)
            {
                Players.Add(new PlayerObject
                {
                    id = Guid.NewGuid().ToString(),
                    connid = ""
                });
            }
        }

        public bool HasConnectedPlayers()
        {
            return Players.Where(n => n.connid != "").Count() != 0;
        }

        public bool HasAvailablePlayerSpots()
        {
            return Players.Where(n => n.connid == "").Count() != 0;
        }

    }
}