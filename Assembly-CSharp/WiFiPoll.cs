using System;
using System.Collections.Generic;
using UnityEngine;

public class WiFiPoll
{
	public void BeginVote()
	{
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("WiFi Poll");
		if (!ModsManager.SBGlitch)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("Shall the player get a random WiFi unlocked? Or shall the current WiFi get locked?");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!LOCK");
			this.myDOSTwitch.myTwitchIRC.SendMsg("!UNLOCK");
			this.voteIsLive = true;
			GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd), 0);
			return;
		}
		if (DataManager.LeetMode)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("Oops, Probe/Inject Skip is enabled, WiFi poll will have 25% chance of infecting the WiFi with D05_DR41N3R!");
			if (UnityEngine.Random.Range(0, 100) > 75)
			{
				if (GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() == null || GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer)
				{
					this.myDOSTwitch.myTwitchIRC.SendMsg("Cannot infect WiFi at the moment, thread queued.");
					GameManager.TimeSlinger.FireTimer(20f, new Action(this.tryInfectTheWiFi), 0);
				}
				else
				{
					this.infectTheWiFi();
				}
			}
			else
			{
				this.myDOSTwitch.myTwitchIRC.SendMsg("No WiFi will be affected by D05_DR41N3R");
			}
			this.myDOSTwitch.setPollInactive();
			return;
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("Oops, Probe/Inject Skip is enabled, WiFi poll will always yield infecting the WiFi with D05_DR41N3R!");
		if (GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() == null)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("Cannot infect WiFi at the moment, thread queued.");
			GameManager.TimeSlinger.FireTimer(20f, new Action(this.tryInfectTheWiFi), 0);
		}
		else
		{
			this.infectTheWiFi();
		}
		this.myDOSTwitch.setPollInactive();
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "LOCK" || text == "UNLOCK"))
			{
				this.currentVotes.Add(userName, text);
			}
		}
	}

	private void PollEnd()
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		this.voteIsLive = false;
		this.myDOSTwitch.myTwitchIRC.SendMsg("The WiFi Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "LOCK")
			{
				num++;
			}
			else if (keyValuePair.Value == "UNLOCK")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("LOCK: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("UNLOCK: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			WiFiPoll.resetWiFiStats();
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted, Story of my life!");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (num > num2)
		{
			if (UnityEngine.Random.Range(0, 100) < 10)
			{
				WiFiPoll.resetWiFiStats();
				this.myDOSTwitch.myTwitchIRC.SendMsg("Error!!! - D05_DR41N3R Infected this poll!!! Infecting a WiFi instead of locking it.");
				if (GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() == null || GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer)
				{
					this.myDOSTwitch.myTwitchIRC.SendMsg("Cannot infect WiFi at the moment, thread queued.");
					GameManager.TimeSlinger.FireTimer(20f, new Action(this.tryInfectTheWiFi), 0);
				}
				else
				{
					this.infectTheWiFi();
				}
			}
			else if (GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() == null)
			{
				this.myDOSTwitch.myTwitchIRC.SendMsg("Cannot lock WiFi at the moment, thread queued.");
				GameManager.TimeSlinger.FireTimer(20f, new Action(this.tryLockTheWiFi), 0);
			}
			else
			{
				this.lockTheWiFi();
			}
		}
		else if (num2 > num)
		{
			WiFiPoll.resetWiFiStats();
			int index;
			do
			{
				index = UnityEngine.Random.Range(0, GameManager.ManagerSlinger.WifiManager.GetCurrentWifiNetworks().Count - 1);
			}
			while (GameManager.ManagerSlinger.WifiManager.GetCurrentWifiNetworks()[index].networkSecurity == WIFI_SECURITY.NONE);
			WiFiPoll.interactedWiFiDefinition = GameManager.ManagerSlinger.WifiManager.GetCurrentWifiNetworks()[index];
			WiFiPoll.interactedWiFiNetworkSecurity = GameManager.ManagerSlinger.WifiManager.GetCurrentWifiNetworks()[index].networkSecurity;
			GameManager.ManagerSlinger.WifiManager.GetCurrentWifiNetworks()[index].networkSecurity = WIFI_SECURITY.NONE;
			this.myDOSTwitch.myTwitchIRC.SendMsg("Unlocking " + GameManager.ManagerSlinger.WifiManager.GetCurrentWifiNetworks()[index].networkName + "...");
			WifiMenuBehaviour.Ins.refreshNetworks();
			WiFiPoll.interactedWiFiType = WiFiPoll.WiFiInteractionType.UNLOCKED;
		}
		else
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("There is a tie! RE-VOTE!");
			GameManager.TimeSlinger.FireTimer(10f, new Action(this.BeginVote), 0);
			flag = true;
		}
		if (!flag)
		{
			this.myDOSTwitch.setPollInactive();
		}
	}

	private void lockTheWiFi()
	{
		WiFiPoll.resetWiFiStats();
		WiFiPoll.interactedWiFiNetworkStrength = GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().networkStrength;
		WiFiPoll.interactedWiFiDefinition = GameManager.ManagerSlinger.WifiManager.getCurrentWiFi();
		GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().networkStrength = -3;
		this.myDOSTwitch.myTwitchIRC.SendMsg("Locking " + GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().networkName + "...");
		if (GameManager.ManagerSlinger.WifiManager.IsOnline)
		{
			GameManager.ManagerSlinger.WifiManager.DisconnectFromWifi();
		}
		WifiMenuBehaviour.Ins.refreshNetworks();
		WiFiPoll.interactedWiFiType = WiFiPoll.WiFiInteractionType.LOCKED;
	}

	private void tryLockTheWiFi()
	{
		if (GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() == null)
		{
			GameManager.TimeSlinger.FireTimer(20f, new Action(this.tryLockTheWiFi), 0);
			return;
		}
		this.lockTheWiFi();
	}

	public static void resetWiFiStats()
	{
		if (!(WiFiPoll.interactedWiFiDefinition != null))
		{
			return;
		}
		switch (WiFiPoll.interactedWiFiType)
		{
		case WiFiPoll.WiFiInteractionType.LOCKED:
			WiFiPoll.interactedWiFiDefinition.networkStrength = WiFiPoll.interactedWiFiNetworkStrength;
			Debug.Log(WiFiPoll.interactedWiFiDefinition.ToString() + " was re-unlocked");
			return;
		case WiFiPoll.WiFiInteractionType.UNLOCKED:
			WiFiPoll.interactedWiFiDefinition.networkSecurity = WiFiPoll.interactedWiFiNetworkSecurity;
			Debug.Log(WiFiPoll.interactedWiFiDefinition.ToString() + " was re-locked");
			return;
		case WiFiPoll.WiFiInteractionType.NONE:
			return;
		default:
			return;
		}
	}

	private void infectTheWiFi()
	{
		GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer = true;
		this.myDOSTwitch.myTwitchIRC.SendMsg("Infecting " + GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().networkName + "...");
	}

	private void tryInfectTheWiFi()
	{
		if (GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() == null || GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer)
		{
			GameManager.TimeSlinger.FireTimer(20f, new Action(this.tryInfectTheWiFi), 0);
			return;
		}
		this.infectTheWiFi();
	}

	public DOSTwitch myDOSTwitch;

	private Dictionary<string, string> currentVotes;

	private bool voteIsLive;

	private static short interactedWiFiNetworkStrength;

	private static WIFI_SECURITY interactedWiFiNetworkSecurity;

	private static WifiNetworkDefinition interactedWiFiDefinition;

	private static WiFiPoll.WiFiInteractionType interactedWiFiType = WiFiPoll.WiFiInteractionType.NONE;

	public enum WiFiInteractionType
	{
		LOCKED,
		UNLOCKED,
		NONE
	}
}
