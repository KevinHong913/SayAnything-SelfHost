using System;
using System.Collections.Generic;

namespace SelfHost
{
	public class UserData
	{
		public string Username { get; set; }
		public bool IsHost { get; set; }
		public string Answer { get; set; }
		public List<int> Scores { get; set; }
		public int Bet { get; set; }

		public UserData(string username)
		{
			Username = username;
			IsHost = false;
			Answer = "";
			Scores = new List<int>();
			Bet = -1;
		}
	}
}
