using System;
using System.Collections.Generic;

public class KeyPoll
{
	public void BeginVote()
	{
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("Key Cue Poll");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Should the key cue be enabled or disabled for 5 minutes?");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!ENABLED");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!DISABLED");
		this.voteIsLive = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd), 0);
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "ENABLED" || text == "DISABLED"))
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
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Key Cue Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "ENABLED")
			{
				num++;
			}
			else if (keyValuePair.Value == "DISABLED")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("ENABLED: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("DISABLED: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted, Story of my life!");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (num > num2)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("The Key Cue will remain enabled for the next 5 minutes!");
			this.FireManipulator(KEY_CUE_MODE.ENABLED);
		}
		else if (num2 > num)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("The Key Cue will remain disabled for the next 5 minutes!");
			this.FireManipulator(KEY_CUE_MODE.DISABLED);
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

	private void FireManipulator(KEY_CUE_MODE kEY_CUE_MODE)
	{
		if (KeyPoll.keyManipulatorData != KEY_CUE_MODE.DEFAULT)
		{
			GameManager.TimeSlinger.FireTimer(15f, delegate()
			{
				this.FireManipulator(kEY_CUE_MODE);
			}, 0);
			return;
		}
		KeyPoll.keyManipulatorData = kEY_CUE_MODE;
		GameManager.TheCloud.SpawnManipulatorIcon(300f, (kEY_CUE_MODE == KEY_CUE_MODE.ENABLED) ? CustomSpriteLookUp.greenkey : CustomSpriteLookUp.redkey, SpeedPoll.speedManipulatorActive ? 120f : 40f, 50f);
		GameManager.TimeSlinger.FireTimer(300f, new Action(this.DisableManipulator), 0);
	}

	private void DisableManipulator()
	{
		KeyPoll.keyManipulatorData = KEY_CUE_MODE.DEFAULT;
	}

	public static void DevDisableManipulator()
	{
		KeyPoll.keyManipulatorData = KEY_CUE_MODE.DEFAULT;
	}

	public static void DevEnableManipulator(KEY_CUE_MODE kEY_CUE_MODE)
	{
		if (KeyPoll.keyManipulatorData != KEY_CUE_MODE.DEFAULT)
		{
			return;
		}
		KeyPoll.keyManipulatorData = kEY_CUE_MODE;
		GameManager.TheCloud.SpawnManipulatorIcon(600f, (kEY_CUE_MODE == KEY_CUE_MODE.ENABLED) ? CustomSpriteLookUp.greenkey : CustomSpriteLookUp.redkey, SpeedPoll.speedManipulatorActive ? 120f : 40f, 50f);
		GameManager.TimeSlinger.FireTimer(600f, new Action(KeyPoll.DevDisableManipulator), 0);
	}

	public DOSTwitch myDOSTwitch;

	private Dictionary<string, string> currentVotes;

	private bool voteIsLive;

	public static KEY_CUE_MODE keyManipulatorData = KEY_CUE_MODE.DEFAULT;
}
