﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavaScriptUNO.Models
{
	/// <summary>
	/// Player object, mirror of the JS object, contains all the player info.
	/// </summary>
	public class PlayerObject
	{
		public string id { get; set; }
		public string name { get; set; }
		public string connid { get; set; }
		public List<CardObject> cards { get; set; }
		public bool reportedUno { get; set; }
		public bool hasUno { get; set; }
		public string playerIp { get; set; }
		public bool UserPicked { get; set; } = false;
		public PlayerObject()
		{
			cards = new List<CardObject>();
			reportedUno = false;
			hasUno = false;
		}

		public override string ToString()
		{
			if (!string.IsNullOrEmpty(name))
			{
				return name;
			}

			return $"Player {id}";
		}
	}
}