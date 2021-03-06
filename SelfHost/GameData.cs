﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SelfHost
{
	public class GameData
	{
		private List<UserData> userList;
		private int questionNum;
		private int hostIndex;
		private int answerCount;
		private int betCount;
		public int ResetCount;
		public int RoundNumber;

		public List<UserData> UserList
		{
			set { userList = value; }
			get { return userList; }
		}

		public int QuestionNum
		{
			set { questionNum = value; }
			get { return questionNum; }
		}

		public int HostIndex
		{
			set { hostIndex = value; }
			get { return hostIndex; }
		}

		public GameData()
		{
			userList = new List<UserData>();
			questionNum = 0;
			hostIndex = 0;
			answerCount = 0;
			betCount = 0;
			ResetCount = 0;
			RoundNumber = 0;
		}

		public void SetHost()
		{
			for (int i = 0; i < userList.Count; ++i)
			{
				if (i == hostIndex)
					userList[i].IsHost = true;
				else
					userList[i].IsHost = false;
			}
		}

		public List<UserData> GetUserList()
		{
			return userList;
		}

		public void AddUser(UserData user)
		{
			userList.Add(user);
		}

		public void RemoveUser(UserData user)
		{
			userList.Remove(user);
		}

		public void RemoveUserByName(string username)
		{
			UserData deleteUser = userList.Find(x => x.Username == username); // delete the first occurance
			if (deleteUser == null)
				return;
			else
				userList.Remove(deleteUser);
		}

		public void SetAnswerByName(string username, string answer)
		{
			for (int i = 0; i < userList.Count; ++i)
			{
				if (userList[i].Username == username)
				{
					userList[i].Answer = answer;
					answerCount++;
					return;
				}
			}
		}

		public bool IsAllUserSubmit()
		{
			// host does not submit answer!!
			return answerCount == userList.Count - 1;
		}

		public void SetBetByName(string username, int betNum)
		{
			for (int i = 0; i < userList.Count; ++i)
			{
				if (userList[i].Username == username)
				{
					userList[i].BettingChoice = betNum;
					userList[i].BettingUsername = userList[betNum].Username;
					userList[betNum].BetRecieved++;
					betCount++;
					return;
				}
			}
		}

		public bool IsAllBetSubmit()
		{
			return userList.Count == betCount;
		}

		public void CalculateScore()
		{
			Debug.WriteLine("[GAME DATA] Calculate Score");
			//userList[0].Bet = 1;
			//userList[1].Bet = 1;
			//userList[2].Bet = 2;
			//userList[3].Bet = 1;
			/**
			 * player get point when host picks it,
			 * player get point when his bet is correct
			 * 
			 * 
			 * player 0 (host): 1
			 * player 1: 2
			 * player 2: 1
			 * player 3: 1
			 * 
			 * [0, 2, 1, 0]
			 **/

			// all user besides host's bet are calculated 
			int[] scoreList = Enumerable.Repeat<int>(0, userList.Count).ToArray();

			for (int i = 0; i < userList.Count; ++i)
			{
				if (userList[i].IsHost == false)
				{
					scoreList[ userList[i].BettingChoice ]++;
				}
			}
			Debug.WriteLine(scoreList.ToString());

			int[] roundScore = Enumerable.Repeat<int>(0, userList.Count).ToArray();
			int hostPick = UserList[hostIndex].BettingChoice;
			roundScore[ hostPick ]++; // host pick, +1 point
			// host got points from how many bets on the answer they pick. Max is 2 points
			roundScore[hostIndex] = scoreList[hostPick] > 2 ? 2 : scoreList[hostPick]; 
			// player that bet on the host answer will get points
			for (int i = 0; i < userList.Count; ++i)
			{
				if (userList[i].IsHost == false && userList[i].BettingChoice == hostPick)
				{
					roundScore[i]++;
				}
			}

			// insert score into player info
			for (int i = 0; i < userList.Count; ++i)
			{
				userList[i].TotalScore += roundScore[i];
				userList[i].ThisRoundScore = roundScore[i];
			}
			Debug.WriteLine("Final user score: " + String.Join("; ", userList));


		}

		public List<UserData> GetScore()
		{
			return userList;
		}

		public bool ResetRound()
		{
			RoundNumber++;

			questionNum++;
			hostIndex++;
			betCount = 0;
			answerCount = 0;
			ResetCount = 0;

			SetHost();
			for (int i = 0; i < userList.Count; ++i)
			{
				userList[i].BettingChoice = -1;
				userList[i].BettingUsername = "";
				userList[i].Answer = "";
				userList[i].ThisRoundScore = 0;
			}

			// if every player been host once, game end
			if (RoundNumber == userList.Count)
			{
				for (int i = 0; i < userList.Count; ++i)
				{
					userList[i].TotalScore = 0;
					RoundNumber = 0;
				}

				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
