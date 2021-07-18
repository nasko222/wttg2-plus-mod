using System;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPoll
{
	public void BeginVote()
	{
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("Page Loading Speed Poll");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Should the websites load faster, or slower for 5 minutes?");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!FASTER");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!SLOWER");
		this.voteIsLive = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd), 0);
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "FASTER" || text == "SLOWER"))
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
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Page Loading Speed Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "FASTER")
			{
				num++;
			}
			else if (keyValuePair.Value == "SLOWER")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("FASTER: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("SLOWER: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted, Story of my life!");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (num > num2)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("Sites will load faster for the next 5 minutes!");
			this.FireManipulator(TWITCH_NET_SPEED.FAST);
		}
		else if (num2 > num)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("Sites will load slower for the next 5 minutes!");
			this.FireManipulator(TWITCH_NET_SPEED.SLOW);
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

	private void FireManipulator(TWITCH_NET_SPEED nET_SPEED)
	{
		if (SpeedPoll.speedManipulatorActive)
		{
			return;
		}
		SpeedPoll.speedManipulatorData = nET_SPEED;
		SpeedPoll.speedManipulatorActive = true;
		GameManager.TimeSlinger.FireTimer(300f, new Action(this.DisableManipulator), 0);
	}

	private void DisableManipulator()
	{
		SpeedPoll.speedManipulatorActive = false;
	}

	public static void FireFastManipulator(float time)
	{
		SpeedPoll.speedManipulatorData = TWITCH_NET_SPEED.FAST;
		SpeedPoll.speedManipulatorActive = true;
		GameManager.TimeSlinger.FireTimer(time, new Action(SpeedPoll.DevDisableManipulator), 0);
		Debug.Log("manipulator " + time.ToString());
	}

	public static void FireSlowManipulator(float time)
	{
		SpeedPoll.speedManipulatorData = TWITCH_NET_SPEED.SLOW;
		SpeedPoll.speedManipulatorActive = true;
		GameManager.TimeSlinger.FireTimer(time, new Action(SpeedPoll.DevDisableManipulator), 0);
		Debug.Log("manipulator " + time.ToString());
	}

	public static void DevDisableManipulator()
	{
		SpeedPoll.speedManipulatorActive = false;
	}

	public static void DevEnableManipulator(TWITCH_NET_SPEED nET_SPEED)
	{
		if (SpeedPoll.speedManipulatorActive)
		{
			return;
		}
		SpeedPoll.speedManipulatorData = nET_SPEED;
		SpeedPoll.speedManipulatorActive = true;
		GameManager.TimeSlinger.FireTimer(600f, new Action(SpeedPoll.DevDisableManipulator), 0);
	}

	public DOSTwitch myDOSTwitch;

	private Dictionary<string, string> currentVotes;

	private bool voteIsLive;

	public static bool speedManipulatorActive;

	public static TWITCH_NET_SPEED speedManipulatorData;
}
