using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavaScriptUNO.Models
{
	/// <summary>
	/// Object used to render the game list on the sessions page
	/// </summary>
	public class GameSession
	{
		public string GameId { get; set; }
		public string GameName { get; set; }
		public int PlayerCount { get; set; }
		public int PlayerTotal { get; set; }
		public bool GameStarted { get; set; }
	}
}