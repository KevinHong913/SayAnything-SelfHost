using System;
using System.Collections.Generic;

namespace SelfHost
{
	public class UserData
	{
		public string Username { get; set; }
		public bool IsHost { get; set; }
		public string Answer { get; set; }
		//public List<int> Scores { get; set; }
		public int TotalScore { get; set; }
		public int ThisRoundScore { get; set; }
		public int BettingChoice { get; set; }
		public string BettingUsername { get; set; }
		public int BetRecieved { get; set; }

		public UserData(string username)
		{
			Username = username;
			IsHost = false;
			Answer = "";
			//Scores = new List<int>();
			TotalScore = 0;
			ThisRoundScore = 0;
			BettingChoice = -1;
			BettingUsername = "";
			BetRecieved = 0;
		}
	}
}
