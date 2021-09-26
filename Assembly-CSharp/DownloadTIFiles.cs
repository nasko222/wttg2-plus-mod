using System;
using System.IO;
using LutoniteMods;
using UnityEngine;

public static class DownloadTIFiles
{
	public static void startDownloadingFiles()
	{
		DownloadTIFiles.CheckIfDirectoryExists("WTTG2_Data\\Resources\\custom_audio");
		DownloadTIFiles.CheckIfDirectoryExists("WTTG2_Data\\Resources\\custom_tex");
		DownloadTIFiles.CheckIfDirectoryExists("WTTG2_Data\\Resources\\custom_source");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\dream.wav", "dream.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\triangle.wav", "triangle.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\nyancat.wav", "nyancat.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\polishcow.wav", "polishcow.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\stickbug.wav", "stickbug.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\vacation.wav", "vacation.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\keyboard.wav", "keyboard.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\jebaited.wav", "jebaited.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\stal.wav", "stal.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\chungus.wav", "chungus.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\gnome.wav", "gnome.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\rickroll.wav", "rickroll.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\hackermans.wav", "hackermans.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\party.wav", "party.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\blue.wav", "blue.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\coffin.wav", "coffin.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\crab.wav", "crab.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\thomas.wav", "thomas.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\alarm.wav", "alarm.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\beep.wav", "beep.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\reset.wav", "reset.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\failsafe.wav", "failsafe.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\systemFailure.wav", "systemFailure.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\gflaugh.wav", "gflaugh.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\gfpresence.wav", "gfpresence.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\gfscream.wav", "gfscream.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\mlg.wav", "mlg.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\bblaugh.wav", "bblaugh.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\virus.wav", "virus.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\swamp.wav", "swamp.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\fbi.wav", "fbi.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\xor.wav", "xor.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\challenger.wav", "challenger.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\conga.wav", "conga.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\forsaken.wav", "forsaken.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\legion.wav", "legion.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\tango.wav", "tango.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\takedownman.wav", "takedownman.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\testical.wav", "testical.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\hailsatan.wav", "hailsatan.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\illuminati.wav", "illuminati.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\enigma.wav", "enigma.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\theart.wav", "theart.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\cannabisworld.wav", "cannabisworld.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\nuclear.wav", "nuclear.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\rosesdestruction.wav", "rosesdestruction.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\freedom.wav", "freedom.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_audio\\funky.wav", "funky.wav");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_tex\\freddy.png", "freddy.png");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_tex\\remoteVPNlvl2.png", "remoteVPNlvl2.png");
		DownloadTIFiles.CheckIfFileExists("WTTG2_Data\\Resources\\custom_tex\\remoteVPNlvl3.png", "remoteVPNlvl3.png");
		DownloadTIFiles.CheckSourceCodeExistance("bathroomcams");
		DownloadTIFiles.CheckSourceCodeExistance("bathroomcamsaccess");
		DownloadTIFiles.CheckSourceCodeExistance("bathroomcamscams");
		DownloadTIFiles.CheckSourceCodeExistance("blackhatpost");
		DownloadTIFiles.CheckSourceCodeExistance("blackhatpostsubmit");
		DownloadTIFiles.CheckSourceCodeExistance("bleedforme");
		DownloadTIFiles.CheckSourceCodeExistance("bleedformeerror");
		DownloadTIFiles.CheckSourceCodeExistance("burnedatthestake");
		DownloadTIFiles.CheckSourceCodeExistance("cannabisworld");
		DownloadTIFiles.CheckSourceCodeExistance("cannabisworldorder");
		DownloadTIFiles.CheckSourceCodeExistance("cheapsurgery");
		DownloadTIFiles.CheckSourceCodeExistance("cheapsurgerycontact");
		DownloadTIFiles.CheckSourceCodeExistance("darkbook");
		DownloadTIFiles.CheckSourceCodeExistance("deathlog");
		DownloadTIFiles.CheckSourceCodeExistance("decryptyou");
		DownloadTIFiles.CheckSourceCodeExistance("doctormurder");
		DownloadTIFiles.CheckSourceCodeExistance("doscoingenerator");
		DownloadTIFiles.CheckSourceCodeExistance("doscoingeneratorerror");
		DownloadTIFiles.CheckSourceCodeExistance("enigma");
		DownloadTIFiles.CheckSourceCodeExistance("eurofirearms");
		DownloadTIFiles.CheckSourceCodeExistance("eurofirearmsorder");
		DownloadTIFiles.CheckSourceCodeExistance("eurofirearmsproducts");
		DownloadTIFiles.CheckSourceCodeExistance("euthanasialegion");
		DownloadTIFiles.CheckSourceCodeExistance("euthanasialegionabout");
		DownloadTIFiles.CheckSourceCodeExistance("euthanasialegionorder");
		DownloadTIFiles.CheckSourceCodeExistance("euthanasialegionproducts");
		DownloadTIFiles.CheckSourceCodeExistance("euthanasialegionservice");
		DownloadTIFiles.CheckSourceCodeExistance("familydrugshop");
		DownloadTIFiles.CheckSourceCodeExistance("familydrugshopabout");
		DownloadTIFiles.CheckSourceCodeExistance("familydrugshopfaq");
		DownloadTIFiles.CheckSourceCodeExistance("familydrugshoporder");
		DownloadTIFiles.CheckSourceCodeExistance("finalthoughts");
		DownloadTIFiles.CheckSourceCodeExistance("finalthoughtscontact");
		DownloadTIFiles.CheckSourceCodeExistance("fleshtrade");
		DownloadTIFiles.CheckSourceCodeExistance("forsakengifts");
		DownloadTIFiles.CheckSourceCodeExistance("forsakengiftsgifts");
		DownloadTIFiles.CheckSourceCodeExistance("forsakengiftsorder");
		DownloadTIFiles.CheckSourceCodeExistance("funnymonke");
		DownloadTIFiles.CheckSourceCodeExistance("gravethieves");
		DownloadTIFiles.CheckSourceCodeExistance("greetmysisters");
		DownloadTIFiles.CheckSourceCodeExistance("greetmysistersorder");
		DownloadTIFiles.CheckSourceCodeExistance("greetmysisterssisters");
		DownloadTIFiles.CheckSourceCodeExistance("hailsatan");
		DownloadTIFiles.CheckSourceCodeExistance("hailsatancontact");
		DownloadTIFiles.CheckSourceCodeExistance("hailsatanfaq");
		DownloadTIFiles.CheckSourceCodeExistance("hailsatanlinks");
		DownloadTIFiles.CheckSourceCodeExistance("hailsatanmembers");
		DownloadTIFiles.CheckSourceCodeExistance("hailsatanpics");
		DownloadTIFiles.CheckSourceCodeExistance("hailsatanvids");
		DownloadTIFiles.CheckSourceCodeExistance("hiddencams");
		DownloadTIFiles.CheckSourceCodeExistance("hiddencamsorder");
		DownloadTIFiles.CheckSourceCodeExistance("hiddenquestions");
		DownloadTIFiles.CheckSourceCodeExistance("hiddenquestions1");
		DownloadTIFiles.CheckSourceCodeExistance("hiddenquestions2");
		DownloadTIFiles.CheckSourceCodeExistance("hiddenquestions3");
		DownloadTIFiles.CheckSourceCodeExistance("hotburners");
		DownloadTIFiles.CheckSourceCodeExistance("hotburnersorder");
		DownloadTIFiles.CheckSourceCodeExistance("illuminati");
		DownloadTIFiles.CheckSourceCodeExistance("itersanguinis");
		DownloadTIFiles.CheckSourceCodeExistance("legion");
		DownloadTIFiles.CheckSourceCodeExistance("masterclass");
		DownloadTIFiles.CheckSourceCodeExistance("masterclassdogeater");
		DownloadTIFiles.CheckSourceCodeExistance("masterclassjessica");
		DownloadTIFiles.CheckSourceCodeExistance("masterclassjimmy");
		DownloadTIFiles.CheckSourceCodeExistance("masterclassreviews");
		DownloadTIFiles.CheckSourceCodeExistance("masterclasssakura");
		DownloadTIFiles.CheckSourceCodeExistance("necroconvert");
		DownloadTIFiles.CheckSourceCodeExistance("necroconvertabout");
		DownloadTIFiles.CheckSourceCodeExistance("necroconvertservices");
		DownloadTIFiles.CheckSourceCodeExistance("nucleardream");
		DownloadTIFiles.CheckSourceCodeExistance("nudeyoutubers");
		DownloadTIFiles.CheckSourceCodeExistance("organmart");
		DownloadTIFiles.CheckSourceCodeExistance("organmartorder");
		DownloadTIFiles.CheckSourceCodeExistance("organmartproducts");
		DownloadTIFiles.CheckSourceCodeExistance("passportsrus");
		DownloadTIFiles.CheckSourceCodeExistance("passportsrusorder");
		DownloadTIFiles.CheckSourceCodeExistance("rentahacker");
		DownloadTIFiles.CheckSourceCodeExistance("rentahackererror");
		DownloadTIFiles.CheckSourceCodeExistance("rentahackerproducts");
		DownloadTIFiles.CheckSourceCodeExistance("rosesdestruction");
		DownloadTIFiles.CheckSourceCodeExistance("shadowwebportal");
		DownloadTIFiles.CheckSourceCodeExistance("shithole");
		DownloadTIFiles.CheckSourceCodeExistance("shitholeolder");
		DownloadTIFiles.CheckSourceCodeExistance("steroidqueen");
		DownloadTIFiles.CheckSourceCodeExistance("steroidqueenorder");
		DownloadTIFiles.CheckSourceCodeExistance("steroidqueenproducts");
		DownloadTIFiles.CheckSourceCodeExistance("takedownman");
		DownloadTIFiles.CheckSourceCodeExistance("tangodown");
		DownloadTIFiles.CheckSourceCodeExistance("tangodownhire");
		DownloadTIFiles.CheckSourceCodeExistance("tangodownpayment");
		DownloadTIFiles.CheckSourceCodeExistance("tangodownresults");
		DownloadTIFiles.CheckSourceCodeExistance("testicalmutilation");
		DownloadTIFiles.CheckSourceCodeExistance("theart");
		DownloadTIFiles.CheckSourceCodeExistance("thebutcher");
		DownloadTIFiles.CheckSourceCodeExistance("thebutcherbeheading");
		DownloadTIFiles.CheckSourceCodeExistance("thebutcherbleeding");
		DownloadTIFiles.CheckSourceCodeExistance("thebutchergutting");
		DownloadTIFiles.CheckSourceCodeExistance("thebutcherhanging");
		DownloadTIFiles.CheckSourceCodeExistance("thebutcherprep");
		DownloadTIFiles.CheckSourceCodeExistance("thebutcherskinning");
		DownloadTIFiles.CheckSourceCodeExistance("theinvestigation");
		DownloadTIFiles.CheckSourceCodeExistance("theinvestigationerror");
		DownloadTIFiles.CheckSourceCodeExistance("themuhgel");
		DownloadTIFiles.CheckSourceCodeExistance("themuhgelorder");
		DownloadTIFiles.CheckSourceCodeExistance("vengeanceangel");
		DownloadTIFiles.CheckSourceCodeExistance("vengeanceangelfollowers");
		DownloadTIFiles.CheckSourceCodeExistance("vengeanceangelfreedom");
		DownloadTIFiles.CheckSourceCodeExistance("vengeanceangelpower");
		DownloadTIFiles.CheckSourceCodeExistance("vengeanceangelresults");
		DownloadTIFiles.CheckSourceCodeExistance("vengeanceangelwill");
		DownloadTIFiles.CheckSourceCodeExistance("warehouserenter");
		DownloadTIFiles.CheckSourceCodeExistance("warehouserenterrent");
		DownloadTIFiles.CheckSourceCodeExistance("weedpost");
		DownloadTIFiles.CheckSourceCodeExistance("weedpostorder");
		DownloadTIFiles.CheckSourceCodeExistance("zeroday");
		DownloadTIFiles.CheckSourceCodeExistance("zerodaycode");
		DownloadTIFiles.CheckSourceCodeExistance("zerodayexploits");
		DownloadTIFiles.CheckSourceCodeExistance("zerodayleaks");
		DownloadTIFiles.CheckSourceCodeExistance("zerodayrant");
		DownloadTIFiles.CheckSourceCodeExistance("zerodayscripts");
		DownloadTIFiles.CheckBrowserAssetsCorrectSize(260492275L);
		DownloadTIFiles.Freddy = DownloadTIFiles.LoadPNG("WTTG2_Data\\Resources\\custom_tex\\freddy.png");
		DownloadTIFiles.RemoteVPNLevel2 = DownloadTIFiles.LoadNewSprite("WTTG2_Data\\Resources\\custom_tex\\remoteVPNlvl2.png", 100f, SpriteMeshType.Tight);
		DownloadTIFiles.RemoteVPNLevel3 = DownloadTIFiles.LoadNewSprite("WTTG2_Data\\Resources\\custom_tex\\remoteVPNlvl3.png", 100f, SpriteMeshType.Tight);
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
		DownloadTIFiles.Conga = WavUtility.ToAudioClip("conga.wav");
		DownloadTIFiles.ForsakenGifts = WavUtility.ToAudioClip("forsaken.wav");
		DownloadTIFiles.Legion = WavUtility.ToAudioClip("legion.wav");
		DownloadTIFiles.TangoDown = WavUtility.ToAudioClip("tango.wav");
		DownloadTIFiles.TakedownMan = WavUtility.ToAudioClip("takedownman.wav");
		DownloadTIFiles.TesticalMutilation = WavUtility.ToAudioClip("testical.wav");
		DownloadTIFiles.HailSatan = WavUtility.ToAudioClip("hailsatan.wav");
		DownloadTIFiles.Illuminati = WavUtility.ToAudioClip("illuminati.wav");
		DownloadTIFiles.Enigma = WavUtility.ToAudioClip("enigma.wav");
		DownloadTIFiles.TheArt = WavUtility.ToAudioClip("theart.wav");
		DownloadTIFiles.Cannabisworld = WavUtility.ToAudioClip("cannabisworld.wav");
		DownloadTIFiles.NuclearDream = WavUtility.ToAudioClip("nuclear.wav");
		DownloadTIFiles.RosesDestruction = WavUtility.ToAudioClip("rosesdestruction.wav");
		DownloadTIFiles.Freedom = WavUtility.ToAudioClip("freedom.wav");
		DownloadTIFiles.Funky = WavUtility.ToAudioClip("funky.wav");
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

	public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 100f, SpriteMeshType spriteType = SpriteMeshType.Tight)
	{
		Texture2D texture2D = DownloadTIFiles.LoadPNG(FilePath);
		return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f), PixelsPerUnit, 0u, spriteType);
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
}
