using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavaScriptUNO.Models
{
	public class SpecialCardActions
	{
		public bool sendColorWheel { get; set; }
		public int cardDrawAmount { get; set; }
		public bool skipNextPerson { get; set; }
		public bool reverseOrder { get; set; }
	}
}