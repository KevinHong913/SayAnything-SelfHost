# SayAnything-SelfHost
### Xamarin Project for cross platform app Say Anything
#### Santa Clara University COEN 268 Final Project
This is the backend hosting part of the project. It is using Microsoft SignalR Library to create an SignalR Server to play the
popular board game Say Anything on your phone.

## SelfHost Documentation
This document is a step by step and detail function spec for the application backend server.
The server is a SignalR self host server.

## How to use
### Server
1. Downlaod Server Program from Github: https://github.com/KevinHong913/SayAnything-SelfHost
2. Open the solution and restore nuget packages
3. Run solution
4. Server will now listen to http://localhost:8080

### Client
##### Reference to https://github.com/KevinHong913/Xamarin-Forms-and-SignalR-Example
1. Install SignalR Client from Nuget
`Microsoft ASP.NET SignalR Client`
2. Copy `SignalRClient` class into your solution
3. Remove the `UserData` class if you already have one.
	```csharp
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
	```
3. In your main `App` class, add public field
	```csharp
		public SignalRClient SignalRClient = new SignalRClient("http://localhost:8080");
	```
4. Inside `public App()`, start the SignalR connection and add reconnect function 
	```
	//show an error if the connection doesn't succeed for some reason
	SignalRClient.Start().ContinueWith(task =>
	{
		if (task.IsFaulted)
			MainPage.DisplayAlert("Error", "An error occurred when trying to connect to SignalR: " + task.Exception.InnerExceptions[0].Message, "OK");
	});

	//try to reconnect every 10 seconds, just in case
	Device.StartTimer(TimeSpan.FromSeconds(10), () =>
	{
		if (!SignalRClient.IsConnectedOrConnecting)
			SignalRClient.Start();
		return true;
	});
	```
	
5. If you want to have a label showing the connection state (disconnect, connecting, or connected), add
	```csharp
	var connectionLabel = new Label {
		HorizontalOptions = LayoutOptions.CenterAndExpand,
		BindingContext = SignalRClient
	};
	connectionLabel.SetBinding (Label.TextProperty, "ConnectionState");
	```
	
6. If you want to call the SignalRClient object elsewhere in your application. You can use
	```csharp
	var app = Application.Current as App;
	app.SignalRClient.GetPlayerList(); // use get player list as example
	```
	
  * To invoke an event to the server
		```csharp
		addPlayerBtn.Clicked += (Object sender, EventArgs args) => {
			SignalRClient.AddNewPlayer(usernameStack.UsernameTextbox.Text);
		};
		```
		
  * To listen to an event from server
		```csharp
		SignalRClient.OnUserAdded += (userList) =>{
			Debug.WriteLine(userList.Count);
			for (int i = 0; i < userList.Count; ++i)
			{
				listView.AddText("After Add: " + userList[i].Username);
			}
		};
		```



## Protocol for each steps

### 1. Main Menu:

##### Get current player lists

* Client => Invoke: GetPlayerList ( ), return list
    ```csharp
    List<UserData> list = SignalRClient.GetPlayerList();
    ```
##### Join Button
* Client => Invoke: AddNewPlayer (username)
    ```csharp
    SignalRClient.AddNewPlayer(username);
    ```
* Client => Listen to: OnUserAdded
    ```csharp
    SignalRClient.OnUserAdded += (userList) => {
    	/* Do something */
    };
    ```
### 2. Room:
##### Leave Button
* Client => Invoke: DeletePlayer (username)
    ```csharp
    SignalRClient.DeletePlayer(username);
    ```
* Client => Listen to: OnUserDeleted

    ```csharp
    SignalRClient.OnUserDeleted += (userList) =>
    {
	/* Do something */
    };
    ```

##### Start Button
* Client => Invoke: StartGame ( ), return `(UserList, QuestionNum)`

Host: IsHost will be set to true

    ```csharp
    SignalRClient.StartGame();
    ```
    
* Client => Listen to: OnGameStarted

    ```csharp
    SignalRClient.OnGameStarted += (userList, questionNum) =>
    {
        /* Do something */
    };
    ```
* Question List (Client)
    
    `public string[] QuestionList = new string[3] { "Q1", "Q2", "Q3"};`

### 3a. USER: user answer page
##### Submit Button
* Client => Invoke: AnswerSubmit(username, answer)

    ```csharp
    SignalRClient.SubmitAnswer(answer, answer);
    ```

### 3b. USER & HOST: Wait for other user
##### Waiting for other player to submit
* Client => Listen to: OnAnswerSubmitted (userList, isFinished)
    
    ```csharp
    SignalRClient.OnAnswerSubmitted += (userList, isFinished) =>
    {
	/* Do something */
    };
    ```
##### NOTE: isFinished is true if submit count == (usercount - 1), so please do not let the host submit anser
----- End of Current progress -----

### 4a. USER: Submit Bet
##### Submit
* Client => Invoke: SubmitBet (username, bet number)
    
    ```csharp
    ChatHubProxy.Invoke("SubmitBet", username, betNum);
    ```

### 4b. HOST: Choice answer
##### Submit
* Client => Invoke: SubmitBet (username, bet number)
    
    ```csharp
    ChatHubProxy.Invoke("SubmitBet", username, betNum);
    ```

#### NOTE: Bet number is the index number of the user list.

### 5. Answer Page:
##### Waiting for other player to bet
* Client => Listen to: BetSubmitted (userList, isFinished)
    
    ```csharp
    SignalRClient.OnBetSubmitted += (userList, isFinished) =>
    {
	/* Do something */
    }
    ```
### 6. Score Board:
##### Go to scoreboard
* Client => Invoke: GetScore(), return user list with score updated

    ```csharp
    List<UserData> list = SignalRClient.GetScore();
    ```

### 7. Count down:
##### Count Down 10 second, then go to next round
* Client => Invoke: RoundFinish ( )
* (SERVER will count if all player finished, then restart round)
* (If round number == player number, game will end and will jump to Step 2)
##### After all user invoke RoundFinish
* Client => Listen To: OnGameEnd

    ```csharp
    SignalRClient.OnGameEnd += (gameEnd) =>
    {
	/* Do something */
	/* True if game end, false if only round end and will continue the game */
    }
    ```
