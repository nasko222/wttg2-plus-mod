using System;
using System.IO;
using UnityEngine;

public class DOSTwitch : MonoBehaviour
{
	public void Start()
	{
		this.conCount = 0;
		GameManager.SetDOSTwitch(this);
		GameManager.TimeSlinger.FireTimer(5f, new Action(this.prepDOSTwitch), 0);
	}

	public void Update()
	{
		if (this.EarlyGamePollWindowActive && Time.time - this.EarlyGamePollTimeStamp >= this.EarlyGamePollTimeWindow)
		{
			this.EarlyGamePollWindowActive = false;
			this.triggerEarlyGamePoll();
			this.generateVirusPollWindow();
		}
		if (this.discountPollWindowActive && Time.time - this.discountPollTimeStamp >= this.discountPollTimeWindow)
		{
			this.discountPollWindowActive = false;
			this.triggerDiscountPoll();
			this.generateVirusPollWindow();
		}
		if (this.wifiPollWindowActive && Time.time - this.wifiPollTimeStamp >= this.wifiPollTimeWindow)
		{
			this.wifiPollWindowActive = false;
			this.triggerWiFiPoll();
			this.generateSpeedPollWindow();
		}
		if (this.speedPollWindowActive && Time.time - this.speedPollTimeStamp >= this.speedPollTimeWindow)
		{
			this.speedPollWindowActive = false;
			this.triggerSpeedPoll();
			this.generateVirusPollWindow();
		}
		if (this.VirusPollWindowActive && Time.time - this.VirusPollTimeStamp >= this.VirusPollTimeWindow)
		{
			this.VirusPollWindowActive = false;
			this.triggerVirusPoll();
			this.generateDOSCoinPollWindow();
		}
		if (this.DOSCoinPollWindowActive && Time.time - this.DOSCoinPollTimeStamp >= this.DOSCoinPollTimeWindow)
		{
			this.DOSCoinPollWindowActive = false;
			this.triggerDOSCoinPoll();
			this.generateTrollPollWindow();
		}
		if (this.trollPollWindowActive && Time.time - this.trollPollTimeStamp >= this.trollPollTimeWindow)
		{
			this.trollPollWindowActive = false;
			this.triggerTrollPoll();
			this.generateHackerPollWindow();
		}
		if (this.hackerPollWindowActive && Time.time - this.hackerPollTimeStamp >= this.hackerPollTimeWindow)
		{
			this.hackerPollWindowActive = false;
			this.triggerHackerPoll();
			this.generateWiFiPollWindow();
		}
	}

	public DOSTwitch()
	{
		this.DOSCoinPollMinWindow = 120f;
		this.DOSCoinPollMaxWindow = 300f;
		this.EarlyGamePollMinWindow = 120f;
		this.EarlyGamePollMaxWindow = 240f;
		this.VirusPollMinWindow = 120f;
		this.VirusPollMaxWindow = 300f;
		this.hackerPollMinWindow = 120f;
		this.hackerPollMaxWindow = 360f;
		this.trollPollMinWindow = 60f;
		this.trollPollMaxWindow = 180f;
		this.discountPollMinWindow = 10f;
		this.discountPollMaxWindow = 60f;
		this.wifiPollMinWindow = 120f;
		this.wifiPollMaxWindow = 360f;
		this.speedPollMinWindow = 180f;
		this.speedPollMaxWindow = 240f;
	}

	private void prepDOSTwitch()
	{
		this.ChatDevUsername = string.Empty;
		string setOAuth = File.ReadAllText("Twitch/oauth.txt");
		string text = File.ReadAllText("Twitch/username.txt");
		this.StreamerUsername = text;
		this.myTwitchIRC = new TwitchIRC();
		this.myTwitchIRC.StartTwitch(setOAuth, text);
		GameManager.TimeSlinger.FireTimer(5f, new Action(this.warmDOSTwitch), 0);
	}

	private void checkCon()
	{
		if (this.myTwitchIRC.isConnected)
		{
			this.amConnected = true;
			this.displayTwitchConnected();
			return;
		}
		this.conCount += 1;
		if (this.conCount < 5)
		{
			GameManager.TimeSlinger.FireTimer(30f, new Action(this.checkCon), 0);
			return;
		}
		Debug.Log("Could not connect to Twitch. FeelsBadMan");
	}

	private void warmDOSTwitch()
	{
		this.mySpeedPoll = new SpeedPoll();
		this.mySpeedPoll.myDOSTwitch = this;
		this.myWiFiPoll = new WiFiPoll();
		this.myWiFiPoll.myDOSTwitch = this;
		this.myDOSCoinPoll = new DOSCoinPoll();
		this.myDOSCoinPoll.myDOSTwitch = this;
		this.myEarlyGamePoll = new EarlyGamePoll();
		this.myEarlyGamePoll.myDOSTwitch = this;
		this.myVirusPoll = new VirusPoll();
		this.myVirusPoll.myDOSTwitch = this;
		this.myHackerPoll = new HackerPoll();
		this.myHackerPoll.myDOSTwitch = this;
		this.myTrollPoll = new TrollPoll();
		this.myTrollPoll.myDOSTwitch = this;
		this.myDiscountPoll = new DiscountPoll();
		this.myDiscountPoll.myDOSTwitch = this;
		if (!this.myTwitchIRC.isConnected)
		{
			GameManager.TimeSlinger.FireTimer(30f, new Action(this.checkCon), 0);
			return;
		}
		this.amConnected = true;
		this.displayTwitchConnected();
		if (DataManager.LeetMode)
		{
			this.generateDiscountPollWindow();
			return;
		}
		this.generateEarlyGamePollWindow();
	}

	private void displayTwitchConnected()
	{
		Debug.Log("Twitch Integration Now Live! FeelsGoodMan");
		this.myTwitchIRC.SendMsg("Welcome to the Game II Twitch Integration Mod by nasko222 [v1.22-beta7] - All Systems Working! - FeelsGoodMan Clap");
		DOSTwitch.dosTwitchEnabled = true;
		Debug.Log("DOSTwitch was enabled and put in an instance.");
	}

	public void chatMessageRecv(string theMSG)
	{
		try
		{
			string[] array = theMSG.Split(new string[]
			{
				"PRIVMSG"
			}, StringSplitOptions.None);
			string[] array2 = array[0].Split(new string[]
			{
				"!"
			}, StringSplitOptions.None);
			string[] array3 = array[1].Split(new string[]
			{
				":"
			}, StringSplitOptions.None);
			string text = array2[0].Replace(":", string.Empty);
			string text2 = array3[1];
			text2 = text2.ToUpper();
			if (this.pollActive)
			{
				this.currentPollAction(text, text2);
			}
			Debug.Log(text + ": " + text2);
			text2 = text2.ToLower();
			if (text2 == "!dev")
			{
				this.myTwitchIRC.SendMsg("Chat DevTools: The commands are GiveDOS, TakeDOS, Hack, Blackout");
			}
			if (text2.StartsWith("!dev "))
			{
				this.ChatDeveloperSystem(text, text2);
			}
		}
		catch (Exception message)
		{
			Debug.Log(message);
		}
	}

	private void generateDOSCoinPollWindow()
	{
		if (DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.DOSCoinPollTimeWindow = UnityEngine.Random.RandomRange(60f, 180f);
		}
		else if (DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.DOSCoinPollTimeWindow = UnityEngine.Random.Range(this.DOSCoinPollMinWindow, this.DOSCoinPollMaxWindow);
		}
		else if (!DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.DOSCoinPollTimeWindow = UnityEngine.Random.Range(this.DOSCoinPollMinWindow, this.DOSCoinPollMaxWindow);
		}
		else if (!DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.DOSCoinPollTimeWindow = UnityEngine.Random.Range(this.DOSCoinPollMinWindow, this.DOSCoinPollMaxWindow) * 1.5f;
		}
		this.DOSCoinPollTimeStamp = Time.time;
		this.DOSCoinPollWindowActive = true;
	}

	private void generateEarlyGamePollWindow()
	{
		this.EarlyGamePollTimeWindow = UnityEngine.Random.Range(this.EarlyGamePollMinWindow, this.EarlyGamePollMaxWindow);
		this.EarlyGamePollTimeStamp = Time.time;
		this.EarlyGamePollWindowActive = true;
	}

	public void setPollInactive()
	{
		this.pollActive = false;
	}

	private void triggerDOSCoinPoll()
	{
		if (!this.pollActive)
		{
			this.currentPollAction = new Action<string, string>(this.myDOSCoinPoll.CastVote);
			this.pollActive = true;
			this.myDOSCoinPoll.BeginVote();
			return;
		}
		if (DataManager.LeetMode)
		{
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(28f, 69f), new Action(this.triggerDOSCoinPoll), 0);
			return;
		}
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(42f, 128f), new Action(this.triggerDOSCoinPoll), 0);
	}

	private void triggerEarlyGamePoll()
	{
		if (!this.pollActive)
		{
			this.currentPollAction = new Action<string, string>(this.myEarlyGamePoll.CastVote);
			this.pollActive = true;
			this.myEarlyGamePoll.BeginVote();
			return;
		}
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(42f, 128f), new Action(this.triggerEarlyGamePoll), 0);
	}

	private void generateVirusPollWindow()
	{
		if (DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.VirusPollTimeWindow = UnityEngine.Random.RandomRange(60f, 180f);
		}
		else if (DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.VirusPollTimeWindow = UnityEngine.Random.Range(this.VirusPollMinWindow, this.VirusPollMaxWindow);
		}
		else if (!DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.VirusPollTimeWindow = UnityEngine.Random.Range(this.VirusPollMinWindow, this.VirusPollMaxWindow);
		}
		else if (!DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.VirusPollTimeWindow = UnityEngine.Random.Range(this.VirusPollMinWindow, this.VirusPollMaxWindow) * 1.5f;
		}
		this.VirusPollTimeStamp = Time.time;
		this.VirusPollWindowActive = true;
	}

	private void triggerVirusPoll()
	{
		if (!this.pollActive)
		{
			this.currentPollAction = new Action<string, string>(this.myVirusPoll.CastVote);
			this.pollActive = true;
			this.myVirusPoll.BeginVote();
			return;
		}
		if (DataManager.LeetMode)
		{
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(25f, 69f), new Action(this.triggerVirusPoll), 0);
			return;
		}
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(42f, 128f), new Action(this.triggerVirusPoll), 0);
	}

	private void generateHackerPollWindow()
	{
		if (DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.hackerPollTimeWindow = UnityEngine.Random.RandomRange(60f, 180f);
		}
		else if (DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.hackerPollTimeWindow = UnityEngine.Random.Range(this.hackerPollMinWindow, this.hackerPollMaxWindow);
		}
		else if (!DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.hackerPollTimeWindow = UnityEngine.Random.Range(this.hackerPollMinWindow, this.hackerPollMaxWindow);
		}
		else if (!DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.hackerPollTimeWindow = UnityEngine.Random.Range(this.hackerPollMinWindow, this.hackerPollMaxWindow) * 1.5f;
		}
		this.hackerPollTimeStamp = Time.time;
		this.hackerPollWindowActive = true;
	}

	private void triggerHackerPoll()
	{
		if (!this.pollActive)
		{
			this.currentPollAction = new Action<string, string>(this.myHackerPoll.CastVote);
			this.pollActive = true;
			this.myHackerPoll.BeginVote();
			return;
		}
		if (DataManager.LeetMode)
		{
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(25f, 69f), new Action(this.triggerHackerPoll), 0);
			return;
		}
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(45f, 128f), new Action(this.triggerHackerPoll), 0);
	}

	private void generateTrollPollWindow()
	{
		if (DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.trollPollTimeWindow = UnityEngine.Random.RandomRange(60f, 180f);
		}
		else if (DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.trollPollTimeWindow = UnityEngine.Random.Range(this.trollPollMinWindow, this.trollPollMaxWindow);
		}
		else if (!DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.trollPollTimeWindow = UnityEngine.Random.Range(this.trollPollMinWindow, this.trollPollMaxWindow);
		}
		else if (!DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.trollPollTimeWindow = UnityEngine.Random.Range(this.trollPollMinWindow, this.trollPollMaxWindow) * 1.5f;
		}
		this.trollPollTimeStamp = Time.time;
		this.trollPollWindowActive = true;
	}

	private void triggerTrollPoll()
	{
		if (!this.pollActive)
		{
			this.currentPollAction = new Action<string, string>(this.myTrollPoll.CastVote);
			this.pollActive = true;
			this.myTrollPoll.BeginVote();
			return;
		}
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(32f, 68f), new Action(this.triggerTrollPoll), 0);
	}

	private void triggerDiscountPoll()
	{
		if (!this.pollActive)
		{
			this.currentPollAction = new Action<string, string>(this.myDiscountPoll.CastVote);
			this.pollActive = true;
			this.myDiscountPoll.BeginVote();
			return;
		}
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(32f, 81f), new Action(this.triggerDiscountPoll), 0);
	}

	private void generateDiscountPollWindow()
	{
		if (DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.discountPollTimeWindow = UnityEngine.Random.RandomRange(60f, 180f);
		}
		else if (DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.discountPollTimeWindow = UnityEngine.Random.Range(this.discountPollMinWindow, this.discountPollMaxWindow);
		}
		else if (!DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.discountPollTimeWindow = UnityEngine.Random.Range(this.discountPollMinWindow, this.discountPollMaxWindow);
		}
		else if (!DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.discountPollTimeWindow = UnityEngine.Random.Range(this.discountPollMinWindow, this.discountPollMaxWindow) * 1.5f;
		}
		this.discountPollTimeStamp = Time.time;
		this.discountPollWindowActive = true;
	}

	private void generateWiFiPollWindow()
	{
		if (DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.wifiPollTimeWindow = UnityEngine.Random.RandomRange(60f, 180f);
		}
		else if (DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.wifiPollTimeWindow = UnityEngine.Random.Range(this.wifiPollMinWindow, this.wifiPollMaxWindow);
		}
		else if (!DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.wifiPollTimeWindow = UnityEngine.Random.Range(this.wifiPollMinWindow, this.wifiPollMaxWindow);
		}
		else if (!DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.wifiPollTimeWindow = UnityEngine.Random.Range(this.wifiPollMinWindow, this.wifiPollMaxWindow) * 1.5f;
		}
		this.wifiPollTimeStamp = Time.time;
		this.wifiPollWindowActive = true;
	}

	private void triggerWiFiPoll()
	{
		if (!this.pollActive)
		{
			this.currentPollAction = new Action<string, string>(this.myWiFiPoll.CastVote);
			this.pollActive = true;
			this.myWiFiPoll.BeginVote();
			return;
		}
		if (DataManager.LeetMode)
		{
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(25f, 69f), new Action(this.triggerWiFiPoll), 0);
			return;
		}
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(42f, 128f), new Action(this.triggerWiFiPoll), 0);
	}

	private void generateSpeedPollWindow()
	{
		if (DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.speedPollTimeWindow = UnityEngine.Random.RandomRange(60f, 180f);
		}
		else if (DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.speedPollTimeWindow = UnityEngine.Random.Range(this.speedPollMinWindow, this.speedPollMaxWindow);
		}
		else if (!DataManager.LeetMode && !ModsManager.EasyModeActive)
		{
			this.speedPollTimeWindow = UnityEngine.Random.Range(this.speedPollMinWindow, this.speedPollMaxWindow);
		}
		else if (!DataManager.LeetMode && ModsManager.EasyModeActive)
		{
			this.speedPollTimeWindow = UnityEngine.Random.Range(this.speedPollMinWindow, this.speedPollMaxWindow) * 1.5f;
		}
		this.speedPollTimeStamp = Time.time;
		this.speedPollWindowActive = true;
	}

	private void triggerSpeedPoll()
	{
		if (!this.pollActive)
		{
			this.currentPollAction = new Action<string, string>(this.mySpeedPoll.CastVote);
			this.pollActive = true;
			this.mySpeedPoll.BeginVote();
			return;
		}
		if (DataManager.LeetMode)
		{
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(25f, 69f), new Action(this.triggerSpeedPoll), 0);
			return;
		}
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(49f, 101f), new Action(this.triggerSpeedPoll), 0);
	}

	public void OnDisable()
	{
		this.myTwitchIRC.SendMsg("Twitch integration unloaded successfully. Have a nice day! :)");
		this.myTwitchIRC.stopThreads = true;
		Debug.Log("Twitch integration unloaded successfully.");
	}

	private void ChatDeveloperSystem(string username, string command)
	{
		bool flag = false;
		if (username == "nasko222n" || username == "fiercethundr_")
		{
			flag = true;
		}
		if (this.ChatDevUsername != string.Empty && username == this.ChatDevUsername)
		{
			flag = true;
		}
		if (username == this.StreamerUsername && !this.PunishedBefore)
		{
			this.myTwitchIRC.SendMsg("CHEATER!!! TIME FOR PUNISHMENT!!!");
			if (!GameManager.HackerManager.theSwan.isActivatedBefore)
			{
				GameManager.HackerManager.theSwan.ActivateTheSwan();
			}
			CurrencyManager.RemoveCurrency(CurrencyManager.CurrentCurrency / 2f);
			if (GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() != null && !GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer)
			{
				GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer = true;
			}
			AdamLOLHook.Ins.Spawn();
			EnemyManager.CultManager.attemptSpawn();
			GameManager.TheCloud.ForceKeyDiscover();
			GameManager.TheCloud.ForceKeyDiscover();
			GameManager.TheCloud.ForceKeyDiscover();
			GameManager.HackerManager.virusManager.ForceVirus();
			GameManager.HackerManager.virusManager.ForceVirus();
			GameManager.HackerManager.virusManager.ForceVirus();
			GameManager.HackerManager.virusManager.ForceVirus();
			GameManager.HackerManager.virusManager.ForceVirus();
			this.PunishedBefore = true;
			return;
		}
		if (flag)
		{
			this.ExecuteDevCommand(command);
			return;
		}
		this.myTwitchIRC.SendMsg("Chat DevTools: You are not a chat developer!");
	}

	private void ExecuteDevCommand(string command)
	{
		string text = command.Replace("!dev ", string.Empty);
		string[] array = text.Split(new char[]
		{
			' '
		});
		array[0] = array[0].ToLower();
		if (array[0] == "givedos" && array[1] != null)
		{
			float.Parse(array[1]);
			if (float.Parse(array[1]) > 0f && float.Parse(array[1]) < 1000f)
			{
				CurrencyManager.AddCurrency(float.Parse(array[1]));
				GameManager.HackerManager.WhiteHatSound();
				this.myTwitchIRC.SendMsg("Chat DevTools: You gave " + array[1] + " DOSCoins to the player!");
			}
		}
		if (array[0] == "takedos" && array[1] != null)
		{
			float.Parse(array[1]);
			if (float.Parse(array[1]) > 0f && float.Parse(array[1]) < 1000f)
			{
				CurrencyManager.RemoveCurrency((float.Parse(array[1]) >= CurrencyManager.CurrentCurrency) ? CurrencyManager.CurrentCurrency : float.Parse(array[1]));
				GameManager.HackerManager.BlackHatSound();
				this.myTwitchIRC.SendMsg("Chat DevTools: You took " + array[1] + " DOSCoins from the player!");
			}
		}
		if (array[0] == "hack")
		{
			GameManager.HackerManager.ForceNormalHack();
			this.myTwitchIRC.SendMsg("Chat DevTools: Forcing normal hack! HA HA HA HAAA");
		}
		if (array[0] == "blackout")
		{
			EnvironmentManager.PowerBehaviour.ForceTwitchPowerOff();
			this.myTwitchIRC.SendMsg("Chat DevTools: Tripping the power off!");
		}
		Debug.Log("successful command: " + text);
	}

	private short conCount;

	public TwitchIRC myTwitchIRC;

	private bool amConnected;

	private bool DOSCoinPollWindowActive;

	private float DOSCoinPollTimeWindow;

	private float DOSCoinPollTimeStamp;

	private float DOSCoinPollMinWindow;

	private float DOSCoinPollMaxWindow;

	private bool EarlyGamePollWindowActive;

	private float EarlyGamePollTimeWindow;

	private float EarlyGamePollTimeStamp;

	private float EarlyGamePollMinWindow;

	private float EarlyGamePollMaxWindow;

	private bool pollActive;

	private Action<string, string> currentPollAction;

	public DOSCoinPoll myDOSCoinPoll;

	private EarlyGamePoll myEarlyGamePoll;

	private float VirusPollTimeWindow;

	private float VirusPollTimeStamp;

	private bool VirusPollWindowActive;

	private float VirusPollMinWindow;

	private float VirusPollMaxWindow;

	private VirusPoll myVirusPoll;

	public float hackerPollMinWindow;

	public float hackerPollMaxWindow;

	private bool hackerPollWindowActive;

	private float hackerPollTimeWindow;

	private float hackerPollTimeStamp;

	private HackerPoll myHackerPoll;

	public float trollPollMinWindow;

	public float trollPollMaxWindow;

	private bool trollPollWindowActive;

	private float trollPollTimeWindow;

	private float trollPollTimeStamp;

	private TrollPoll myTrollPoll;

	private DiscountPoll myDiscountPoll;

	private float discountPollMinWindow;

	private float discountPollMaxWindow;

	private float discountPollTimeStamp;

	private float discountPollTimeWindow;

	private bool discountPollWindowActive;

	private bool wifiPollWindowActive;

	private float wifiPollTimeWindow;

	private float wifiPollTimeStamp;

	private float wifiPollMinWindow;

	private float wifiPollMaxWindow;

	private WiFiPoll myWiFiPoll;

	private bool speedPollWindowActive;

	private float speedPollTimeWindow;

	private float speedPollTimeStamp;

	private float speedPollMinWindow;

	private float speedPollMaxWindow;

	private SpeedPoll mySpeedPoll;

	public static bool dosTwitchEnabled;

	public string ChatDevUsername;

	private string StreamerUsername;

	private bool PunishedBefore;
}
