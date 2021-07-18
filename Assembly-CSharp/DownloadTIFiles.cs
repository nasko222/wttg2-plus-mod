using System;
using System.IO;
using System.Net;
using LutoniteMods;
using UnityEngine;

public static class DownloadTIFiles
{
	public static void startDownloadingFiles()
	{
		if (!Directory.Exists("WTTG2_Data\\Resources\\custom_audio"))
		{
			Directory.CreateDirectory("WTTG2_Data\\Resources\\custom_audio");
		}
		if (!Directory.Exists("WTTG2_Data\\Resources\\custom_tex"))
		{
			Directory.CreateDirectory("WTTG2_Data\\Resources\\custom_tex");
		}
		WebClient webClient = new WebClient();
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\dream.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/dream.wav", "WTTG2_Data\\Resources\\custom_audio\\dream.wav");
			Debug.Log("dream does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\triangle.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/triangle.wav", "WTTG2_Data\\Resources\\custom_audio\\triangle.wav");
			Debug.Log("triangle does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\nyancat.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/nyancat.wav", "WTTG2_Data\\Resources\\custom_audio\\nyancat.wav");
			Debug.Log("nyancat does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\polishcow.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/polishcow.wav", "WTTG2_Data\\Resources\\custom_audio\\polishcow.wav");
			Debug.Log("polishcow does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\stickbug.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/stickbug.wav", "WTTG2_Data\\Resources\\custom_audio\\stickbug.wav");
			Debug.Log("stickbug does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\vacation.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/vacation.wav", "WTTG2_Data\\Resources\\custom_audio\\vacation.wav");
			Debug.Log("vacation does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\keyboard.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/keyboard.wav", "WTTG2_Data\\Resources\\custom_audio\\keyboard.wav");
			Debug.Log("keyboard does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\jebaited.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/jebaited.wav", "WTTG2_Data\\Resources\\custom_audio\\jebaited.wav");
			Debug.Log("jebaited does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\stal.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/stal.wav", "WTTG2_Data\\Resources\\custom_audio\\stal.wav");
			Debug.Log("stal does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\chungus.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/chungus.wav", "WTTG2_Data\\Resources\\custom_audio\\chungus.wav");
			Debug.Log("chungus does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\gnome.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/gnome.wav", "WTTG2_Data\\Resources\\custom_audio\\gnome.wav");
			Debug.Log("gnome does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\rickroll.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/rickroll.wav", "WTTG2_Data\\Resources\\custom_audio\\rickroll.wav");
			Debug.Log("rickroll does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\hackermans.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/hackermans.wav", "WTTG2_Data\\Resources\\custom_audio\\hackermans.wav");
			Debug.Log("hackermans does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\diarrhea.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/diarrhea.wav", "WTTG2_Data\\Resources\\custom_audio\\diarrhea.wav");
			Debug.Log("diarrhea does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\party.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/party.wav", "WTTG2_Data\\Resources\\custom_audio\\party.wav");
			Debug.Log("party does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\blue.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/blue.wav", "WTTG2_Data\\Resources\\custom_audio\\blue.wav");
			Debug.Log("blue does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\coffin.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/coffin.wav", "WTTG2_Data\\Resources\\custom_audio\\coffin.wav");
			Debug.Log("coffin does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\crab.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/crab.wav", "WTTG2_Data\\Resources\\custom_audio\\crab.wav");
			Debug.Log("crab does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\thomas.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/troll/thomas.wav", "WTTG2_Data\\Resources\\custom_audio\\thomas.wav");
			Debug.Log("thomas does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\alarm.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/alarm.wav", "WTTG2_Data\\Resources\\custom_audio\\alarm.wav");
			Debug.Log("[CONTENT] alarm does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\beep.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/beep.wav", "WTTG2_Data\\Resources\\custom_audio\\beep.wav");
			Debug.Log("[CONTENT] beep does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\reset.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/reset.wav", "WTTG2_Data\\Resources\\custom_audio\\reset.wav");
			Debug.Log("[CONTENT] reset does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\failsafe.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/failsafe.wav", "WTTG2_Data\\Resources\\custom_audio\\failsafe.wav");
			Debug.Log("[CONTENT] failsafe does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\systemFailure.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/systemFailure.wav", "WTTG2_Data\\Resources\\custom_audio\\systemFailure.wav");
			Debug.Log("[CONTENT] systemFailure does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\gflaugh.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/gflaugh.wav", "WTTG2_Data\\Resources\\custom_audio\\gflaugh.wav");
			Debug.Log("[CONTENT] gflaugh does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\gfpresence.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/gfpresence.wav", "WTTG2_Data\\Resources\\custom_audio\\gfpresence.wav");
			Debug.Log("[CONTENT] gfpresence does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\gfscream.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/gfscream.wav", "WTTG2_Data\\Resources\\custom_audio\\gfscream.wav");
			Debug.Log("[CONTENT] gfscream does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\mlg.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/mlg.wav", "WTTG2_Data\\Resources\\custom_audio\\mlg.wav");
			Debug.Log("[CONTENT] mlg does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\bblaugh.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/bblaugh.wav", "WTTG2_Data\\Resources\\custom_audio\\bblaugh.wav");
			Debug.Log("[CONTENT] bblaugh does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\virus.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/virus.wav", "WTTG2_Data\\Resources\\custom_audio\\virus.wav");
			Debug.Log("[CONTENT] virus does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\swamp.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/swamp.wav", "WTTG2_Data\\Resources\\custom_audio\\swamp.wav");
			Debug.Log("[CONTENT] swampu does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\fbi.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/fbi.wav", "WTTG2_Data\\Resources\\custom_audio\\fbi.wav");
			Debug.Log("[CONTENT] fbi open up does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\xor.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/xor.wav", "WTTG2_Data\\Resources\\custom_audio\\xor.wav");
			Debug.Log("[CONTENT] XOR open up does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_audio\\challenger.wav"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/challenger.wav", "WTTG2_Data\\Resources\\custom_audio\\challenger.wav");
			Debug.Log("[CONTENT] challenger open up does not exist, downloading...");
		}
		if (!File.Exists("WTTG2_Data\\Resources\\custom_tex\\freddy.png"))
		{
			webClient.DownloadFile("http://naskogdps17.7m.pl/wttg/content/freddy.png", "WTTG2_Data\\Resources\\custom_tex\\freddy.png");
			Debug.Log("[CONTENT] freddy does not exist, downloading...");
		}
		DownloadTIFiles.triangleMusic = WavUtility.ToAudioClip("triangle.wav");
		DownloadTIFiles.dreamRunningMusic = WavUtility.ToAudioClip("dream.wav");
		DownloadTIFiles.nyanCatMusic = WavUtility.ToAudioClip("nyancat.wav");
		DownloadTIFiles.polishCowMusic = WavUtility.ToAudioClip("polishcow.wav");
		DownloadTIFiles.stickBugMusic = WavUtility.ToAudioClip("stickbug.wav");
		DownloadTIFiles.vacationMusic = WavUtility.ToAudioClip("vacation.wav");
		DownloadTIFiles.keyboardCatMusic = WavUtility.ToAudioClip("keyboard.wav");
		DownloadTIFiles.jebaitedSong = WavUtility.ToAudioClip("jebaited.wav");
		DownloadTIFiles.minecraftStalMusic = WavUtility.ToAudioClip("stal.wav");
		DownloadTIFiles.bigChungusMusic = WavUtility.ToAudioClip("chungus.wav");
		DownloadTIFiles.gnomedLOL = WavUtility.ToAudioClip("gnome.wav");
		DownloadTIFiles.rickRolled = WavUtility.ToAudioClip("rickroll.wav");
		DownloadTIFiles.hackermansAudio = WavUtility.ToAudioClip("hackermans.wav");
		DownloadTIFiles.diarrheaSounds = WavUtility.ToAudioClip("diarrhea.wav");
		DownloadTIFiles.crazyparty = WavUtility.ToAudioClip("party.wav");
		DownloadTIFiles.blueMusic = WavUtility.ToAudioClip("blue.wav");
		DownloadTIFiles.coffinDance = WavUtility.ToAudioClip("coffin.wav");
		DownloadTIFiles.crabRave = WavUtility.ToAudioClip("crab.wav");
		DownloadTIFiles.thomasDankEngine = WavUtility.ToAudioClip("thomas.wav");
		DownloadTIFiles.SwanAlarm = WavUtility.ToAudioClip("alarm.wav");
		DownloadTIFiles.SwanBeep = WavUtility.ToAudioClip("beep.wav");
		DownloadTIFiles.SwanFailsafe = WavUtility.ToAudioClip("failsafe.wav");
		DownloadTIFiles.SwanReset = WavUtility.ToAudioClip("reset.wav");
		DownloadTIFiles.SwanFailure = WavUtility.ToAudioClip("systemFailure.wav");
		DownloadTIFiles.GFPresence = WavUtility.ToAudioClip("gfpresence.wav");
		DownloadTIFiles.GFLaugh = WavUtility.ToAudioClip("gflaugh.wav");
		DownloadTIFiles.GFScream = WavUtility.ToAudioClip("gfscream.wav");
		DownloadTIFiles.MLGAirhorn = WavUtility.ToAudioClip("mlg.wav");
		DownloadTIFiles.BalloonBoy = WavUtility.ToAudioClip("bblaugh.wav");
		DownloadTIFiles.YourComputerHasVirus = WavUtility.ToAudioClip("virus.wav");
		DownloadTIFiles.WhatAreUDoingInMySwamp = WavUtility.ToAudioClip("swamp.wav");
		DownloadTIFiles.FBIOpenUp = WavUtility.ToAudioClip("fbi.wav");
		DownloadTIFiles.XOR = WavUtility.ToAudioClip("xor.wav");
		DownloadTIFiles.Challenger = WavUtility.ToAudioClip("challenger.wav");
		DownloadTIFiles.Freddy = DownloadTIFiles.LoadPNG("WTTG2_Data\\Resources\\custom_tex\\freddy.png");
	}

	public static Texture2D LoadPNG(string filePath)
	{
		Texture2D texture2D = null;
		if (File.Exists(filePath))
		{
			byte[] data = File.ReadAllBytes(filePath);
			texture2D = new Texture2D(2, 2);
			texture2D.LoadImage(data);
		}
		return texture2D;
	}

	public static AudioClip triangleMusic;

	public static AudioClip dreamRunningMusic;

	public static AudioClip polishCowMusic;

	public static AudioClip stickBugMusic;

	public static AudioClip nyanCatMusic;

	public static AudioClip vacationMusic;

	public static AudioClip keyboardCatMusic;

	public static AudioClip jebaitedSong;

	public static AudioClip minecraftStalMusic;

	public static AudioClip bigChungusMusic;

	public static AudioClip gnomedLOL;

	public static AudioClip rickRolled;

	public static AudioClip hackermansAudio;

	public static AudioClip diarrheaSounds;

	public static AudioClip crazyparty;

	public static AudioClip blueMusic;

	public static AudioClip coffinDance;

	public static AudioClip crabRave;

	public static AudioClip SwanAlarm;

	public static AudioClip SwanBeep;

	public static AudioClip SwanFailsafe;

	public static AudioClip SwanReset;

	public static AudioClip SwanFailure;

	public static AudioClip thomasDankEngine;

	public static AudioClip GFPresence;

	public static AudioClip GFLaugh;

	public static Texture2D Freddy;

	public static AudioClip MLGAirhorn;

	public static AudioClip BalloonBoy;

	public static AudioClip YourComputerHasVirus;

	public static AudioClip WhatAreUDoingInMySwamp;

	public static AudioClip GFScream;

	public static AudioClip FBIOpenUp;

	public static AudioClip XOR;

	public static AudioClip Challenger;
}
