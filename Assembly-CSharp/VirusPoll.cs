using System;
using System.Collections.Generic;
using UnityEngine;

public class VirusPoll
{
	public void BeginVote()
	{
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("Virus Poll");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Shall the player get some viruses? Or Shall the player get rid of the viruses?");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!INSTALL");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!CLEAN");
		this.voteIsLive = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd), 0);
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == "INSTALL" || text == "CLEAN"))
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
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Virus Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == "INSTALL")
			{
				num++;
			}
			else if (keyValuePair.Value == "CLEAN")
			{
				num2++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("INSTALL: " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("CLEAN: " + num2.ToString());
		if (num == 0 && num2 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted, Story of my life!");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (num > num2)
		{
			if (num > 10)
			{
				num = 10;
			}
			if (num == 1)
			{
				if (UnityEngine.Random.Range(0, 100) > 90 && !GameManager.HackerManager.theSwan.isActivatedBefore)
				{
					GameManager.HackerManager.theSwan.ActivateTheSwan();
					this.myDOSTwitch.myTwitchIRC.SendMsg("Player's computer got infected by TH3SW4N!");
				}
				else
				{
					GameManager.HackerManager.virusManager.ForceVirus();
					this.myDOSTwitch.myTwitchIRC.SendMsg("Player's computer got 1 virus installed!");
				}
			}
			else
			{
				for (int i = 0; i < num; i++)
				{
					GameManager.HackerManager.virusManager.ForceVirus();
				}
				this.myDOSTwitch.myTwitchIRC.SendMsg("Player's computer got " + num.ToString() + " viruses installed!");
			}
		}
		else if (num2 > num)
		{
			if (GameManager.HackerManager.virusManager.getVirusCount > 0)
			{
				this.myDOSTwitch.myTwitchIRC.SendMsg("Opening VWipe... Cleaning viruses...");
				GameManager.HackerManager.virusManager.ClearVirus();
			}
			else
			{
				this.myDOSTwitch.myTwitchIRC.SendMsg("The player has no viruses. VWipe won't be opened.");
			}
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

	public DOSTwitch myDOSTwitch;

	private Dictionary<string, string> currentVotes;

	private bool voteIsLive;
}
