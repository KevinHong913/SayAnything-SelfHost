using System;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SelfHost
{
	
	class Program
	{
		public static GameData gameData { set; get; }

		static void Main(string[] args)
		{
			// This will *ONLY* bind to localhost, if you want to bind to all addresses
			// use http://*:8080 to bind to all addresses. 
			// See http://msdn.microsoft.com/en-us/library/system.net.httplistener.aspx 
			// for more information.
			gameData = new GameData();
			string url = "http://localhost:8080";
			using (WebApp.Start(url))
			{
				Console.WriteLine("Server running on {0}", url);
				Console.ReadLine();
			}
		}
	}
	class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
			app.MapSignalR();
		}
	}
		
}
