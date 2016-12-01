using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SelfHost
{
	public class GameData
	{
		public List<UserData> UserList { set; get; }

		public GameData()
		{
			UserList = new List<UserData>();
		}

		public List<UserData> GetUserList()
		{
			return UserList;
		}

		public void AddUser(UserData user)
		{
			UserList.Add(user);
		}
		public void RemoveUser(UserData user)
		{
			UserList.Remove(user);
		}

	}
}
