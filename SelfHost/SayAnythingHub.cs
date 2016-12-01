using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;
using System.Collections.Generic;

namespace SelfHost
{
	[HubName("SayAnything")]
	public class SayAnything : Hub
	{
		//public void SendMessage(string username, string message)
		//{
		//	UserData usr = new UserData(username);
		//	Debug.WriteLine("Stock update for {0} new price {1}", usr.Username, usr.Message);
		//	//Clients.All.MessageReceived(username, message);
		//	Clients.All.MessageReceived(usr);
		//}

		[HubMethodName("GetPlayerList")]
		public List<UserData> GetPlayers()
		{
			Debug.WriteLine("[Invoke] GetPlayerList");
			return Program.gameData.UserList;
		}

		// Client listen to UserAdded
		[HubMethodName("NewPlayer")]
		public void NewPlayer(string username)
		{
			Debug.WriteLine("[Invoke] NewPlater");
			Program.gameData.AddUser(new UserData(username));

			Clients.All.UserAdded(Program.gameData.UserList);
			Debug.WriteLine("[Broadcast] UserAdded - User " + username + " added. Total User: " + Program.gameData.UserList.Count);

		}
	}
}
