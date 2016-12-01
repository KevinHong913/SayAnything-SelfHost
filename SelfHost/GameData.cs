using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SelfHost
{
	public class GameData
	{
		private List<UserData> userList;
		private int questionNum;
		private int hostIndex;
		private int answerCount;

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
			return userList.Count == answerCount;
		}

	}
}
