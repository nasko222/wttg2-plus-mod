using System;
using System.Collections.Generic;
using UnityEngine;

public class TrollPoll
{
	public void BeginVote()
	{
		int num = UnityEngine.Random.Range(0, 17);
		int num2 = UnityEngine.Random.Range(0, 17);
		do
		{
			num2 = UnityEngine.Random.Range(0, 17);
		}
		while (num == num2);
		TrollPoll.firstTrollSound = (TrollPoll.TROLL_SOUNDS)num;
		TrollPoll.secondTrollSound = (TrollPoll.TROLL_SOUNDS)num2;
		this.currentVotes = new Dictionary<string, string>();
		this.myDOSTwitch.myTwitchIRC.SendMsg("Troll Poll");
		if (!ModsManager.Trolling)
		{
			this.myDOSTwitch.myTwitchIRC.SendMsg("Troll Poll is disabled! Sadge");
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
		TrollPoll.trollAudio = LookUp.SoundLookUp.JumpHit1;
		switch (theSound)
		{
		case TrollPoll.TROLL_SOUNDS.VACATION:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.vacationMusic;
			break;
		case TrollPoll.TROLL_SOUNDS.TRIANGLE:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.triangleMusic;
			break;
		case TrollPoll.TROLL_SOUNDS.POLISHCOW:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.polishCowMusic;
			break;
		case TrollPoll.TROLL_SOUNDS.NYANCAT:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.nyanCatMusic;
			break;
		case TrollPoll.TROLL_SOUNDS.STICKBUG:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.stickBugMusic;
			break;
		case TrollPoll.TROLL_SOUNDS.JEBAITED:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.jebaitedSong;
			break;
		case TrollPoll.TROLL_SOUNDS.KEYBOARDCAT:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.keyboardCatMusic;
			break;
		case TrollPoll.TROLL_SOUNDS.RUNNING:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.dreamRunningMusic;
			break;
		case TrollPoll.TROLL_SOUNDS.STAL:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.minecraftStalMusic;
			break;
		case TrollPoll.TROLL_SOUNDS.CHUNGUS:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.bigChungusMusic;
			break;
		case TrollPoll.TROLL_SOUNDS.GNOME:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.gnomedLOL;
			break;
		case TrollPoll.TROLL_SOUNDS.RICKROLL:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.rickRolled;
			break;
		case TrollPoll.TROLL_SOUNDS.DIARRHEA:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.diarrheaSounds;
			break;
		case TrollPoll.TROLL_SOUNDS.BLUE:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.blueMusic;
			break;
		case TrollPoll.TROLL_SOUNDS.COFFIN:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.coffinDance;
			break;
		case TrollPoll.TROLL_SOUNDS.CRAB:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.crabRave;
			break;
		case TrollPoll.TROLL_SOUNDS.THOMAS:
			TrollPoll.trollAudio.AudioClip = DownloadTIFiles.thomasDankEngine;
			break;
		}
		TrollPoll.trollAudio.MyAudioHub = AUDIO_HUB.PLAYER_HUB;
		TrollPoll.trollAudio.MyAudioLayer = AUDIO_LAYER.PLAYER;
		TrollPoll.trollAudio.Loop = false;
		TrollPoll.trollAudio.LoopCount = 0;
		TrollPoll.trollAudio.Volume = 0.1337f;
		TrollPoll.isTrollPlaying = true;
		GameManager.AudioSlinger.PlaySound(TrollPoll.trollAudio);
		GameManager.TimeSlinger.FireTimer(DataManager.LeetMode ? 30f : 300f, new Action(this.stopPlayingSound), 0);
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
		STAL,
		CHUNGUS,
		GNOME,
		RICKROLL,
		DIARRHEA,
		BLUE,
		COFFIN,
		CRAB,
		THOMAS
	}
}
