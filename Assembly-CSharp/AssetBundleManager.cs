using System;
using ASoft.WTTG2;
using TMPro;
using UnityEngine;

public static class AssetBundleManager
{
	public static void LoadAssetBundles()
	{
		if (AssetBundleManager.loaded)
		{
			return;
		}
		if ((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds > 2147483646)
		{
			TitleManager.wttg2plus_modText.GetComponent<TextMeshProUGUI>().text = string.Concat(new object[]
			{
				"This beta version of WTTG2+ has expired. Please update to the latest version of WTTG2+"
			});
			return;
		}
		AssetDownloader.Exec();
		AssetDownloader.ValidateAll();
		AssetBundleManager.WTTG2PlusProps = AssetBundle.LoadFromFile("WTTG2_Data\\Resources\\WTTG2Plus.assets");
		AssetBundleManager.loaded = true;
		AssetBundleManager.ProceedLoadingObjects();
		AssetBundleManager.ProceedLoadingSprites();
		TitleManager.UnloadBox();
	}

	public static void ProceedLoadingObjects()
	{
		CustomObjectLookUp.BombMakerHallwayJump = AssetBundleManager.WTTG2PlusProps.LoadAsset<GameObject>("BombMakerHallwayJump.prefab");
		CustomObjectLookUp.BombMakerApartmentJump = AssetBundleManager.WTTG2PlusProps.LoadAsset<GameObject>("BombMakerApartmentJump.prefab");
		CustomObjectLookUp.BombMakerPCKill = AssetBundleManager.WTTG2PlusProps.LoadAsset<GameObject>("BombMakerPCKill.prefab");
		CustomObjectLookUp.BombMakerPresence = AssetBundleManager.WTTG2PlusProps.LoadAsset<GameObject>("BombMakerPresence.prefab");
		CustomObjectLookUp.PackageBox = AssetBundleManager.WTTG2PlusProps.LoadAsset<GameObject>("PackageBox.prefab");
		CustomObjectLookUp.Router = AssetBundleManager.WTTG2PlusProps.LoadAsset<GameObject>("Router.prefab");
		CustomObjectLookUp.TarotCards = AssetBundleManager.WTTG2PlusProps.LoadAsset<GameObject>("TarotCards.prefab");
	}

	public static void ProceedLoadingSprites()
	{
		CustomSpriteLookUp.freddy = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("freddy.png").texture;
		CustomSpriteLookUp.greenclock = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("greenclock.png");
		CustomSpriteLookUp.greenkey = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("greenkey.png");
		CustomSpriteLookUp.keyitem = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("keyitem.png");
		CustomSpriteLookUp.redclock = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("redclock.png");
		CustomSpriteLookUp.redkey = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("redkey.png");
		CustomSpriteLookUp.remoteVPNlvl2 = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("remoteVPNlvl2.png");
		CustomSpriteLookUp.remoteVPNlvl3 = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("remoteVPNlvl3.png");
		CustomSpriteLookUp.speeditem = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("speeditem.png");
		CustomSpriteLookUp.sulphur = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("sulphur.png");
		CustomSpriteLookUp.router = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("router.png");
		CustomSpriteLookUp.logo = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("logo.png");
		CustomSpriteLookUp.tarotcard = AssetBundleManager.WTTG2PlusProps.LoadAsset<Sprite>("tarotcard.png");
	}

	public static void ProceedLoadingAFD()
	{
		CustomSoundLookUp.alarm = AFDManager.Ins.AddSwanAudio("alarm", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("alarm.wav"));
		CustomSoundLookUp.bblaugh = AFDManager.Ins.AddPlayerAFD("bblaugh", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("bblaugh.wav"), 0.5f);
		CustomSoundLookUp.beans = AFDManager.Ins.AddWebsiteAFD("beans", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("beans.wav"), true);
		CustomSoundLookUp.beep = AFDManager.Ins.AddSwanAudio("beep", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("beep.wav"));
		CustomSoundLookUp.beware = AFDManager.Ins.AddWebsiteAFD("beware", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("beware.wav"), true);
		CustomSoundLookUp.blue = AFDManager.Ins.AddPlayerAFD("blue", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("blue.wav"), 0.25f);
		CustomSoundLookUp.bombmaker = AFDManager.Ins.AddPlayerAFD("bombmaker", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("bombmaker.wav"), 1f);
		CustomSoundLookUp.bombmakertalk = AFDManager.Ins.AddPlayerAFD("bombmakertalk", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("bombmakertalk.wav"), 1f);
		CustomSoundLookUp.bruh = AFDManager.Ins.AddPlayerAFD("bruh", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("bruh.wav"), 0.5f);
		CustomSoundLookUp.cannabisworld = AFDManager.Ins.AddWebsiteAFD("cannabisworld", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("cannabisworld.wav"), false);
		CustomSoundLookUp.challenger = AFDManager.Ins.AddPlayerAFD("challenger", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("challenger.wav"), 0.8f);
		CustomSoundLookUp.chungus = AFDManager.Ins.AddPlayerAFD("chungus", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("chungus.wav"), 0.35f);
		CustomSoundLookUp.clown = AFDManager.Ins.AddWebsiteAFD("clown", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("clown.wav"), true);
		CustomSoundLookUp.coffin = AFDManager.Ins.AddPlayerAFD("coffin", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("coffin.wav"), 0.15f);
		CustomSoundLookUp.conga = AFDManager.Ins.AddPlayerAFD("conga", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("conga.wav"), 0.5f);
		CustomSoundLookUp.cool = AFDManager.Ins.AddWebsiteAFD("cool", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("cool.wav"), true);
		CustomSoundLookUp.crab = AFDManager.Ins.AddPlayerAFD("crab", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("crab.wav"), 0.15f);
		CustomSoundLookUp.dispatch = AFDManager.Ins.AddWebsiteAFD("dispatch", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("dispatch.wav"), true);
		CustomSoundLookUp.dogdoin = AFDManager.Ins.AddPlayerAFD("dogdoin", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("dogdoin.wav"), 0.5f);
		CustomSoundLookUp.dream = AFDManager.Ins.AddPlayerAFD("dream", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("dream.wav"), 0.35f);
		CustomSoundLookUp.elevator = AFDManager.Ins.AddPlayerAFD("elevator", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("elevator.wav"), 0.35f);
		CustomSoundLookUp.enigma = AFDManager.Ins.AddWebsiteAFD("enigma", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("enigma.wav"), false);
		CustomSoundLookUp.explosion = AFDManager.Ins.AddPlayerAFD("explosion", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("explosion.wav"), 0.65f);
		CustomSoundLookUp.failsafe = AFDManager.Ins.AddSwanAudio("failsafe", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("failsafe.wav"));
		CustomSoundLookUp.fbi = AFDManager.Ins.AddPlayerAFD("fbi", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("fbi.wav"), 0.5f);
		CustomSoundLookUp.foeoe = AFDManager.Ins.AddWebsiteAFD("foeoe", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("foeoe.wav"), false);
		CustomSoundLookUp.forsaken = AFDManager.Ins.AddWebsiteAFD("forsaken", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("forsaken.wav"), true);
		CustomSoundLookUp.freedom = AFDManager.Ins.AddWebsiteAFD("freedom", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("freedom.wav"), false);
		CustomSoundLookUp.funky = AFDManager.Ins.AddWebsiteAFD("funky", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("funky.wav"), true);
		CustomSoundLookUp.gflaugh = AFDManager.Ins.AddPlayerAFD("gflaugh", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("gflaugh.wav"), 0.8f);
		CustomSoundLookUp.gfpresence = AFDManager.Ins.AddPlayerAFD("gfpresence", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("gfpresence.wav"), 0.8f);
		CustomSoundLookUp.gfscream = AFDManager.Ins.AddPlayerAFD("gfscream", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("gfscream.wav"), 1f);
		CustomSoundLookUp.gnome = AFDManager.Ins.AddPlayerAFD("gnome", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("gnome.wav"), 0.5f);
		CustomSoundLookUp.hailsatan = AFDManager.Ins.AddWebsiteAFD("hailsatan", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("hailsatan.wav"), true);
		CustomSoundLookUp.illuminati = AFDManager.Ins.AddWebsiteAFD("illuminati", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("illuminati.wav"), false);
		CustomSoundLookUp.jebaited = AFDManager.Ins.AddPlayerAFD("jebaited", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("jebaited.wav"), 0.3f);
		CustomSoundLookUp.kappa = AFDManager.Ins.AddPlayerAFD("kappa", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("kappa.wav"), 0.15f);
		CustomSoundLookUp.keyboard = AFDManager.Ins.AddPlayerAFD("keyboard", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("keyboard.wav"), 0.35f);
		CustomSoundLookUp.legion = AFDManager.Ins.AddWebsiteAFD("legion", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("legion.wav"), false);
		CustomSoundLookUp.manipulator = AFDManager.Ins.AddPlayerAFD("manipulator", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("manipulator.wav"), 1f);
		CustomSoundLookUp.mlg = AFDManager.Ins.AddPlayerAFD("mlg", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("mlg.wav"), 0.5f);
		CustomSoundLookUp.nuclear = AFDManager.Ins.AddWebsiteAFD("nuclear", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("nuclear.wav"), false);
		CustomSoundLookUp.nyancat = AFDManager.Ins.AddPlayerAFD("nyancat", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("nyancat.wav"), 0.25f);
		CustomSoundLookUp.owl = AFDManager.Ins.AddPlayerAFD("owl", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("owl.wav"), 0.5f);
		CustomSoundLookUp.party = AFDManager.Ins.AddLoopAFD("party", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("party.wav"));
		CustomSoundLookUp.polishcow = AFDManager.Ins.AddPlayerAFD("polishcow", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("polishcow.wav"), 0.3f);
		CustomSoundLookUp.reset = AFDManager.Ins.AddSwanAudio("reset", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("reset.wav"));
		CustomSoundLookUp.rosesdestruction = AFDManager.Ins.AddWebsiteAFD("rosesdestruction", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("rosesdestruction.wav"), false);
		CustomSoundLookUp.routerreset = AFDManager.Ins.AddPlayerAFD("routerreset", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("routerreset.wav"), 1f);
		CustomSoundLookUp.routerjammed = AFDManager.Ins.AddPlayerAFD("routerjammed", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("routerjammed.wav"), 1f);
		CustomSoundLookUp.shutdown = AFDManager.Ins.AddPlayerAFD("shutdown", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("shutdown.wav"), 0.5f);
		CustomSoundLookUp.startup = AFDManager.Ins.AddPlayerAFD("startup", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("startup.wav"), 0.5f);
		CustomSoundLookUp.stickbug = AFDManager.Ins.AddPlayerAFD("stickbug", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("stickbug.wav"), 0.4f);
		CustomSoundLookUp.swamp = AFDManager.Ins.AddPlayerAFD("swamp", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("swamp.wav"), 0.5f);
		CustomSoundLookUp.systemFailure = AFDManager.Ins.AddSwanAudio("systemFailure", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("systemFailure.wav"));
		CustomSoundLookUp.takedownman = AFDManager.Ins.AddWebsiteAFD("takedownman", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("takedownman.wav"), true);
		CustomSoundLookUp.tango = AFDManager.Ins.AddWebsiteAFD("tango", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("tango.wav"), true);
		CustomSoundLookUp.testical = AFDManager.Ins.AddWebsiteAFD("testical", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("testical.wav"), false);
		CustomSoundLookUp.theart = AFDManager.Ins.AddWebsiteAFD("theart", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("theart.wav"), false);
		CustomSoundLookUp.thomas = AFDManager.Ins.AddPlayerAFD("thomas", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("thomas.wav"), 0.35f);
		CustomSoundLookUp.triangle = AFDManager.Ins.AddPlayerAFD("triangle", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("triangle.wav"), 0.35f);
		CustomSoundLookUp.vacation = AFDManager.Ins.AddPlayerAFD("vacation", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("vacation.wav"), 0.35f);
		CustomSoundLookUp.virus = AFDManager.Ins.AddPlayerAFD("virus", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("virus.wav"), 0.5f);
		CustomSoundLookUp.whisper = AFDManager.Ins.AddWebsiteAFD("whisper", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("whisper.wav"), true);
		CustomSoundLookUp.xfiles = AFDManager.Ins.AddPlayerAFD("xfiles", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("xfiles.wav"), 0.5f);
		CustomSoundLookUp.xor = AFDManager.Ins.AddPlayerAFD("xor", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("xor.wav"), 1f);
		CustomSoundLookUp.youreuseless = AFDManager.Ins.AddPlayerAFD("youreuseless", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("youreuseless.wav"), 1f);
		CustomSoundLookUp.encrypta = AFDManager.Ins.AddWebsiteAFD("encrypta", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("encrypta.wav"), false);
		CustomSoundLookUp.envision = AFDManager.Ins.AddWebsiteAFD("envision", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("envision.wav"), false);
		CustomSoundLookUp.enigmaold = AFDManager.Ins.AddWebsiteAFD("enigmaold", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("enigmaold.wav"), false);
		CustomSoundLookUp.morse = AFDManager.Ins.AddWebsiteAFD("morse", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("morse.wav"), false);
		CustomSoundLookUp.existance = AFDManager.Ins.AddWebsiteAFD("existance", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("existance.wav"), true);
		CustomSoundLookUp.leettf = AFDManager.Ins.AddWebsiteAFD("leettf", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("leettf.wav"), false);
		CustomSoundLookUp.theend = AFDManager.Ins.AddWebsiteAFD("theend", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("theend.wav"), true);
		CustomSoundLookUp.imaginary = AFDManager.Ins.AddWebsiteAFD("imaginary", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("imaginary.wav"), true);
		CustomSoundLookUp.idiot = AFDManager.Ins.AddWebsiteAFD("idiot", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("idiot.wav"), true);
		CustomSoundLookUp.livehere = AFDManager.Ins.AddWebsiteAFD("livehere", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("livehere.wav"), false);
		CustomSoundLookUp.timechange = AFDManager.Ins.AddPlayerAFD("timechange", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("timechange.wav"), 1f);
		CustomSoundLookUp.pull = AFDManager.Ins.AddPlayerAFD("pull", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("pull.wav"), 1f);
		CustomSoundLookUp.disappear = AFDManager.Ins.AddPlayerAFD("disappear", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("disappear.wav"), 1f);
		CustomSoundLookUp._static = AFDManager.Ins.AddPlayerAFD("_static", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("static.wav"), 0.6f);
		CustomSoundLookUp.fool = AFDManager.Ins.AddPlayerAFD("fool", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("fool.wav"), 1f);
		CustomSoundLookUp.fool2 = AFDManager.Ins.AddPlayerAFD("fool2", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("thefool2.wav"), 1f);
		CustomSoundLookUp.fool3 = AFDManager.Ins.AddPlayerAFD("fool3", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("thefool3.wav"), 1f);
		CustomSoundLookUp.trolled = AFDManager.Ins.AddPlayerAFD("trolled", AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("trolled.wav"), 0.5f);
		CustomSoundLookUp.hackermans = AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>("hackermans.wav");
		AssetBundleManager.LoadDeepWebRadioSamples();
	}

	public static void PrepAssetBundles()
	{
		AssetDownloader.Init();
		TitleManager.AddTextHook();
	}

	private static void LoadDeepWebRadioSamples()
	{
		for (int i = 1; i <= CustomRadioLookUp.tracks.Length; i++)
		{
			CustomRadioLookUp.tracks[i - 1] = AssetBundleManager.WTTG2PlusProps.LoadAsset<AudioClip>(i.ToString() + ".ogg");
		}
	}

	public static bool loaded;

	public static AssetBundle WTTG2PlusProps;
}
