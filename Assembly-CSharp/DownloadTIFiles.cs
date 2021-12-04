using System;
using System.IO;
using UnityEngine;

public static class DownloadTIFiles
{
	public static void startDownloadingFiles()
	{
		DownloadTIFiles.CheckBrowserAssetsCorrectSize(280756713L);
		DownloadTIFiles.Freddy = AssetBundleManager.customTex.LoadAsset<Sprite>("freddy.png").texture;
		DownloadTIFiles.RemoteVPNLevel2 = AssetBundleManager.customTex.LoadAsset<Sprite>("remoteVPNlvl2.png");
		DownloadTIFiles.RemoteVPNLevel3 = AssetBundleManager.customTex.LoadAsset<Sprite>("remoteVPNlvl3.png");
		DownloadTIFiles.SpeedItem = AssetBundleManager.customTex.LoadAsset<Sprite>("speeditem.png");
		DownloadTIFiles.KeyItem = AssetBundleManager.customTex.LoadAsset<Sprite>("keyitem.png");
		DownloadTIFiles.Sulphur = AssetBundleManager.customTex.LoadAsset<Sprite>("sulphur.png");
		DownloadTIFiles.RedKey = AssetBundleManager.customTex.LoadAsset<Sprite>("redkey.png");
		DownloadTIFiles.GreenKey = AssetBundleManager.customTex.LoadAsset<Sprite>("greenkey.png");
		DownloadTIFiles.RedClock = AssetBundleManager.customTex.LoadAsset<Sprite>("redclock.png");
		DownloadTIFiles.GreenClock = AssetBundleManager.customTex.LoadAsset<Sprite>("greenclock.png");
		DownloadTIFiles.triangleMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("triangle.wav");
		DownloadTIFiles.dreamRunningMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("dream.wav");
		DownloadTIFiles.nyanCatMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("nyancat.wav");
		DownloadTIFiles.polishCowMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("polishcow.wav");
		DownloadTIFiles.stickBugMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("stickbug.wav");
		DownloadTIFiles.vacationMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("vacation.wav");
		DownloadTIFiles.keyboardCatMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("keyboard.wav");
		DownloadTIFiles.jebaitedSong = AssetBundleManager.customAudio.LoadAsset<AudioClip>("jebaited.wav");
		DownloadTIFiles.minecraftStalMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("elevator.wav");
		DownloadTIFiles.bigChungusMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("chungus.wav");
		DownloadTIFiles.gnomedLOL = AssetBundleManager.customAudio.LoadAsset<AudioClip>("gnome.wav");
		DownloadTIFiles.rickRolled = AssetBundleManager.customAudio.LoadAsset<AudioClip>("kappa.wav");
		DownloadTIFiles.hackermansAudio = AssetBundleManager.customAudio.LoadAsset<AudioClip>("hackermans.wav");
		DownloadTIFiles.crazyparty = AssetBundleManager.customAudio.LoadAsset<AudioClip>("party.wav");
		DownloadTIFiles.blueMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("blue.wav");
		DownloadTIFiles.coffinDance = AssetBundleManager.customAudio.LoadAsset<AudioClip>("coffin.wav");
		DownloadTIFiles.crabRave = AssetBundleManager.customAudio.LoadAsset<AudioClip>("crab.wav");
		DownloadTIFiles.thomasDankEngine = AssetBundleManager.customAudio.LoadAsset<AudioClip>("thomas.wav");
		DownloadTIFiles.SwanAlarm = AssetBundleManager.customAudio.LoadAsset<AudioClip>("alarm.wav");
		DownloadTIFiles.SwanBeep = AssetBundleManager.customAudio.LoadAsset<AudioClip>("beep.wav");
		DownloadTIFiles.SwanFailsafe = AssetBundleManager.customAudio.LoadAsset<AudioClip>("failsafe.wav");
		DownloadTIFiles.SwanReset = AssetBundleManager.customAudio.LoadAsset<AudioClip>("reset.wav");
		DownloadTIFiles.SwanFailure = AssetBundleManager.customAudio.LoadAsset<AudioClip>("systemFailure.wav");
		DownloadTIFiles.GFPresence = AssetBundleManager.customAudio.LoadAsset<AudioClip>("gfpresence.wav");
		DownloadTIFiles.GFLaugh = AssetBundleManager.customAudio.LoadAsset<AudioClip>("gflaugh.wav");
		DownloadTIFiles.GFScream = AssetBundleManager.customAudio.LoadAsset<AudioClip>("gfscream.wav");
		DownloadTIFiles.MLGAirhorn = AssetBundleManager.customAudio.LoadAsset<AudioClip>("mlg.wav");
		DownloadTIFiles.BalloonBoy = AssetBundleManager.customAudio.LoadAsset<AudioClip>("bblaugh.wav");
		DownloadTIFiles.YourComputerHasVirus = AssetBundleManager.customAudio.LoadAsset<AudioClip>("virus.wav");
		DownloadTIFiles.WhatAreUDoingInMySwamp = AssetBundleManager.customAudio.LoadAsset<AudioClip>("swamp.wav");
		DownloadTIFiles.FBIOpenUp = AssetBundleManager.customAudio.LoadAsset<AudioClip>("fbi.wav");
		DownloadTIFiles.Bruh = AssetBundleManager.customAudio.LoadAsset<AudioClip>("bruh.wav");
		DownloadTIFiles.DogDoin = AssetBundleManager.customAudio.LoadAsset<AudioClip>("dogdoin.wav");
		DownloadTIFiles.IlluminatiTroll = AssetBundleManager.customAudio.LoadAsset<AudioClip>("xfiles.wav");
		DownloadTIFiles.XOR = AssetBundleManager.customAudio.LoadAsset<AudioClip>("xor.wav");
		DownloadTIFiles.Challenger = AssetBundleManager.customAudio.LoadAsset<AudioClip>("challenger.wav");
		DownloadTIFiles.Conga = AssetBundleManager.customAudio.LoadAsset<AudioClip>("conga.wav");
		DownloadTIFiles.ForsakenGifts = AssetBundleManager.customAudio.LoadAsset<AudioClip>("forsaken.wav");
		DownloadTIFiles.Legion = AssetBundleManager.customAudio.LoadAsset<AudioClip>("legion.wav");
		DownloadTIFiles.TangoDown = AssetBundleManager.customAudio.LoadAsset<AudioClip>("tango.wav");
		DownloadTIFiles.TakedownMan = AssetBundleManager.customAudio.LoadAsset<AudioClip>("takedownman.wav");
		DownloadTIFiles.TesticalMutilation = AssetBundleManager.customAudio.LoadAsset<AudioClip>("testical.wav");
		DownloadTIFiles.HailSatan = AssetBundleManager.customAudio.LoadAsset<AudioClip>("hailsatan.wav");
		DownloadTIFiles.Illuminati = AssetBundleManager.customAudio.LoadAsset<AudioClip>("illuminati.wav");
		DownloadTIFiles.Enigma = AssetBundleManager.customAudio.LoadAsset<AudioClip>("enigma.wav");
		DownloadTIFiles._41818 = AssetBundleManager.customAudio.LoadAsset<AudioClip>("foeoe.wav");
		DownloadTIFiles.TheArt = AssetBundleManager.customAudio.LoadAsset<AudioClip>("theart.wav");
		DownloadTIFiles.Cannabisworld = AssetBundleManager.customAudio.LoadAsset<AudioClip>("cannabisworld.wav");
		DownloadTIFiles.NuclearDream = AssetBundleManager.customAudio.LoadAsset<AudioClip>("nuclear.wav");
		DownloadTIFiles.RosesDestruction = AssetBundleManager.customAudio.LoadAsset<AudioClip>("rosesdestruction.wav");
		DownloadTIFiles.Freedom = AssetBundleManager.customAudio.LoadAsset<AudioClip>("freedom.wav");
		DownloadTIFiles.Funky = AssetBundleManager.customAudio.LoadAsset<AudioClip>("funky.wav");
		DownloadTIFiles.Owl = AssetBundleManager.customAudio.LoadAsset<AudioClip>("owl.wav");
		DownloadTIFiles.ManipulatorSound = AssetBundleManager.customAudio.LoadAsset<AudioClip>("manipulator.wav");
		DownloadTIFiles.ShutdownSound = AssetBundleManager.customAudio.LoadAsset<AudioClip>("shutdown.wav");
		DownloadTIFiles.StartupSound = AssetBundleManager.customAudio.LoadAsset<AudioClip>("startup.wav");
		DownloadTIFiles.DispatchBeware = AssetBundleManager.customAudio.LoadAsset<AudioClip>("beware.wav");
		DownloadTIFiles.DispatchMain = AssetBundleManager.customAudio.LoadAsset<AudioClip>("dispatch.wav");
		DownloadTIFiles.BeansAudio = AssetBundleManager.customAudio.LoadAsset<AudioClip>("beans.wav");
		DownloadTIFiles.CoolMusic = AssetBundleManager.customAudio.LoadAsset<AudioClip>("cool.wav");
		DownloadTIFiles.ClownHAHAHA = AssetBundleManager.customAudio.LoadAsset<AudioClip>("clown.wav");
		DownloadTIFiles.EvilWhisper = AssetBundleManager.customAudio.LoadAsset<AudioClip>("whisper.wav");
		DownloadTIFiles.BombmakerLaugh = AssetBundleManager.customAudio.LoadAsset<AudioClip>("bombmaker.wav");
		DownloadTIFiles.Explosion = AssetBundleManager.customAudio.LoadAsset<AudioClip>("explosion.wav");
		DownloadTIFiles.BombMakerRoamKiller = AssetBundleManager.Bombmaker.LoadAsset<GameObject>("BombMakerHallwayJump.prefab");
		DownloadTIFiles.BombMakerApartmentKiller = AssetBundleManager.Bombmaker.LoadAsset<GameObject>("BombMakerApartmentJump.prefab");
	}

	private static void CheckIfDirectoryExists(string dirPath)
	{
		if (!Directory.Exists(dirPath))
		{
			Debug.Log("FATAL ERROR: " + dirPath + " does not exist!");
		}
	}

	private static void CheckIfFileExists(string filePath, string fileName)
	{
		if (!File.Exists(filePath))
		{
			Debug.Log("FATAL ERROR: " + fileName + " does not exist!");
			Application.Quit();
		}
	}

	private static void CheckBrowserAssetsCorrectSize(long theSize)
	{
		if (!File.Exists("WTTG2_Data\\Resources\\browser_assets"))
		{
			Debug.Log("FATAL ERROR: Browser assets doesn't exist");
			Application.Quit();
		}
		if (new FileInfo("WTTG2_Data\\Resources\\browser_assets").Length != theSize)
		{
			Debug.Log("FATAL ERROR: Browser assets has the incorrect size!");
			Application.Quit();
		}
	}

	private static void CheckSourceCodeExistance(string source)
	{
		if (!File.Exists("WTTG2_Data\\Resources\\custom_source\\" + source + ".txt"))
		{
			Debug.Log("FATAL ERROR: " + source + ".txt does not exist in the sources folder");
			Application.Quit();
		}
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

	public static AudioClip Conga;

	public static AudioClip ForsakenGifts;

	public static AudioClip Legion;

	public static AudioClip TangoDown;

	public static AudioClip TakedownMan;

	public static AudioClip TesticalMutilation;

	public static AudioClip HailSatan;

	public static AudioClip Illuminati;

	public static AudioClip Enigma;

	public static AudioClip TheArt;

	public static AudioClip Cannabisworld;

	public static Sprite RemoteVPNLevel2;

	public static Sprite RemoteVPNLevel3;

	public static AudioClip NuclearDream;

	public static AudioClip RosesDestruction;

	public static AudioClip Freedom;

	public static AudioClip Funky;

	public static Sprite RedKey;

	public static Sprite GreenKey;

	public static Sprite RedClock;

	public static Sprite GreenClock;

	public static Sprite SpeedItem;

	public static Sprite KeyItem;

	public static AudioClip Owl;

	public static AudioClip ManipulatorSound;

	public static AudioClip _41818;

	public static AudioClip ShutdownSound;

	public static AudioClip StartupSound;

	public static AudioClip DispatchBeware;

	public static AudioClip DispatchMain;

	public static AudioClip BeansAudio;

	public static AudioClip CoolMusic;

	public static AudioClip ClownHAHAHA;

	public static AudioClip EvilWhisper;

	public static AudioClip Bruh;

	public static AudioClip DogDoin;

	public static AudioClip IlluminatiTroll;

	public static Sprite Sulphur;

	public static AudioClip BombmakerLaugh;

	public static AudioClip Explosion;

	public static GameObject BombMakerRoamKiller;

	public static GameObject BombMakerApartmentKiller;
}
