using System;
using System.Collections.Generic;
using UnityEngine;

public class TrollPoll
{
	public void BeginVote()
	{
		int num = UnityEngine.Random.Range(0, 15);
		int num2 = UnityEngine.Random.Range(0, 15);
		do
		{
			num2 = UnityEngine.Random.Range(0, 15);
		}
		while (num == num2);
		TrollPoll.firstTrollSound = (TrollPoll.TROLL_SOUNDS)num;
		TrollPoll.secondTrollSound = (TrollPoll.TROLL_SOUNDS)num2;
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("Troll Poll");
		if (!ModsManager.Trolling)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("Memes and Music are disabled by the player! Sadge");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (ModsManager.Nightmare)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("Troll Poll is disabled in nightmare mode! Sadge");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("Which of these sounds should start playing?");
		this.myDOSTwitch.myTwitchIRC.SendMsg("!" + TrollPoll.firstTrollSound.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("!" + TrollPoll.secondTrollSound.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("!NO");
		this.voteIsLive = true;
		GameManager.TimeSlinger.FireTimer(60f, new Action(this.PollEnd), 0);
	}

	public void CastVote(string userName, string theVote)
	{
		if (this.voteIsLive && theVote.Contains("!"))
		{
			string text = theVote.Replace("!", string.Empty);
			if (!this.currentVotes.ContainsKey(userName) && (text == TrollPoll.firstTrollSound.ToString() || text == TrollPoll.secondTrollSound.ToString() || text == "NO"))
			{
				this.currentVotes.Add(userName, text);
			}
		}
	}

	private void PollEnd()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		bool flag = false;
		this.voteIsLive = false;
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Troll Poll Has Ended!");
		this.myDOSTwitch.myTwitchIRC.SendMsg("Tallying Results!");
		foreach (KeyValuePair<string, string> keyValuePair in this.currentVotes)
		{
			if (keyValuePair.Value == TrollPoll.firstTrollSound.ToString())
			{
				num++;
			}
			else if (keyValuePair.Value == TrollPoll.secondTrollSound.ToString())
			{
				num2++;
			}
			else if (keyValuePair.Value == "NO")
			{
				num3++;
			}
		}
		this.myDOSTwitch.myTwitchIRC.SendMsg("The Results Are In!");
		this.myDOSTwitch.myTwitchIRC.SendMsg(TrollPoll.firstTrollSound.ToString() + ": " + num.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg(TrollPoll.secondTrollSound.ToString() + ": " + num2.ToString());
		this.myDOSTwitch.myTwitchIRC.SendMsg("NO: " + num3.ToString());
		if (num == 0 && num2 == 0 && num3 == 0)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No one voted, Story of my life!");
			this.myDOSTwitch.setPollInactive();
			return;
		}
		if (num > num2 && num > num3)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg(TrollPoll.firstTrollSound.ToString() + " will play now.");
			this.triggerSoundTroll(TrollPoll.firstTrollSound);
		}
		else if (num2 > num && num2 > num3)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg(TrollPoll.secondTrollSound.ToString() + " will play now.");
			this.triggerSoundTroll(TrollPoll.secondTrollSound);
		}
		else if (num3 > num && num3 > num2)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("No sounds will play.");
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

	public void triggerSoundTroll(TrollPoll.TROLL_SOUNDS theSound)
	{
		if (TrollPoll.isTrollPlaying)
		{
			return;
		}
		switch (theSound)
		{
		case TrollPoll.TROLL_SOUNDS.VACATION:
			TrollPoll.trollAudio = CustomSoundLookUp.vacation;
			break;
		case TrollPoll.TROLL_SOUNDS.TRIANGLE:
			TrollPoll.trollAudio = CustomSoundLookUp.triangle;
			break;
		case TrollPoll.TROLL_SOUNDS.POLISHCOW:
			TrollPoll.trollAudio = CustomSoundLookUp.polishcow;
			break;
		case TrollPoll.TROLL_SOUNDS.NYANCAT:
			TrollPoll.trollAudio = CustomSoundLookUp.nyancat;
			break;
		case TrollPoll.TROLL_SOUNDS.STICKBUG:
			TrollPoll.trollAudio = CustomSoundLookUp.stickbug;
			break;
		case TrollPoll.TROLL_SOUNDS.JEBAITED:
			TrollPoll.trollAudio = CustomSoundLookUp.jebaited;
			break;
		case TrollPoll.TROLL_SOUNDS.KEYBOARDCAT:
			TrollPoll.trollAudio = CustomSoundLookUp.keyboard;
			break;
		case TrollPoll.TROLL_SOUNDS.RUNNING:
			TrollPoll.trollAudio = CustomSoundLookUp.dream;
			break;
		case TrollPoll.TROLL_SOUNDS.CHUNGUS:
			TrollPoll.trollAudio = CustomSoundLookUp.chungus;
			break;
		case TrollPoll.TROLL_SOUNDS.BLUE:
			TrollPoll.trollAudio = CustomSoundLookUp.blue;
			break;
		case TrollPoll.TROLL_SOUNDS.COFFIN:
			TrollPoll.trollAudio = CustomSoundLookUp.coffin;
			break;
		case TrollPoll.TROLL_SOUNDS.CRAB:
			TrollPoll.trollAudio = CustomSoundLookUp.crab;
			break;
		case TrollPoll.TROLL_SOUNDS.THOMAS:
			TrollPoll.trollAudio = CustomSoundLookUp.thomas;
			break;
		case TrollPoll.TROLL_SOUNDS.ELEVATOR:
			TrollPoll.trollAudio = CustomSoundLookUp.elevator;
			break;
		case TrollPoll.TROLL_SOUNDS.KAPPA:
			TrollPoll.trollAudio = CustomSoundLookUp.kappa;
			break;
		}
		TrollPoll.isTrollPlaying = true;
		GameManager.AudioSlinger.PlaySound(TrollPoll.trollAudio);
		GameManager.TimeSlinger.FireTimer((DataManager.LeetMode || ModsManager.Nightmare) ? 30f : 110f, new Action(this.stopPlayingSound), 0);
	}

	private void stopPlayingSound()
	{
		GameManager.AudioSlinger.KillSound(TrollPoll.trollAudio);
		TrollPoll.isTrollPlaying = false;
	}

	public DOSTwitch myDOSTwitch;

	private Dictionary<string, string> currentVotes;

	private bool voteIsLive;

	private static TrollPoll.TROLL_SOUNDS firstTrollSound = TrollPoll.TROLL_SOUNDS.VACATION;

	private static TrollPoll.TROLL_SOUNDS secondTrollSound = TrollPoll.TROLL_SOUNDS.TRIANGLE;

	public static AudioFileDefinition trollAudio;

	public static bool isTrollPlaying;

	public enum TROLL_SOUNDS
	{
		VACATION,
		TRIANGLE,
		POLISHCOW,
		NYANCAT,
		STICKBUG,
		JEBAITED,
		KEYBOARDCAT,
		RUNNING,
		CHUNGUS,
		BLUE,
		COFFIN,
		CRAB,
		THOMAS,
		ELEVATOR,
		KAPPA
	}
}
