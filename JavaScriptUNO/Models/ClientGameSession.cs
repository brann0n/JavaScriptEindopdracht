using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavaScriptUNO.Models
{
	/// <summary>
	/// Object used for redirecting to the client area.
	/// </summary>
	public class ClientGameSession
	{
		public string GameId { get; set; }
		public string ClientId { get; set; }
		public string GameName { get; set; }
	}
}