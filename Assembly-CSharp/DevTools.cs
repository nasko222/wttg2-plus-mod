using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DevTools : MonoBehaviour
{
	private void WarmUpTools()
	{
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("addCode", this.myHash);
		base.StartCoroutine(this.PostRequest(wwwform));
		base.StartCoroutine(this.UpdateMe());
		base.StartCoroutine(this.loadEndingWorld());
	}

	private IEnumerator PostRequest(WWWForm FormRequest)
	{
		UnityWebRequest UWR = UnityWebRequest.Post(this.domain + "Data/ping.php", FormRequest);
		yield return UWR.SendWebRequest();
		if (!UWR.isNetworkError)
		{
			this.iAmLive = false;
			try
			{
				this.handleResponse(DevResponse.CreateFromJSON(UWR.downloadHandler.text));
				yield break;
			}
			catch
			{
				yield break;
			}
		}
		this.iAmLive = true;
		Debug.Log("Network Error: " + UWR.error);
		yield break;
	}

	private IEnumerator UpdateMe()
	{
		yield return new WaitForSecondsRealtime(this.UpdateTickCount);
		base.StartCoroutine(this.UpdateMe());
		WWWForm formRequest = new WWWForm();
		base.StartCoroutine(this.PostRequest(formRequest));
		yield break;
	}

	public void Start()
	{
		this.GFschedule = false;
		this.UpdateTickCount = 10f;
		this.iAmLive = false;
		this.myHash = SystemInfo.deviceUniqueIdentifier.Substring(0, 16);
		GameManager.TimeSlinger.FireTimer(5f, new Action(this.WarmUpTools), 0);
	}

	private void handleResponse(DevResponse Response)
	{
		if (Response.GameHash == this.myHash && !this.iAmLive)
		{
			if (Response.Action == "updateTickCount")
			{
				if (Response.Additional != "" && float.Parse(Response.Additional) <= 30f && float.Parse(Response.Additional) >= 1f)
				{
					Debug.Log("Update tick count to " + Response.Additional + " seconds!");
					this.UpdateTickCount = float.Parse(Response.Additional);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "openWindow")
			{
				if (KitchenWindowHook.Ins != null && !KitchenWindowHook.Ins.isOpen)
				{
					KitchenWindowHook.Ins.OpenWindow();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "closeWindow")
			{
				if (KitchenWindowHook.Ins != null && KitchenWindowHook.Ins.isOpen)
				{
					KitchenWindowHook.Ins.CloseWindow();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "toggleLock")
			{
				if (LookUp.Doors != null)
				{
					LookUp.Doors.MainDoor.ToggleLock();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "trippower")
			{
				if (EnvironmentManager.PowerBehaviour != null)
				{
					EnvironmentManager.PowerBehaviour.ForceTwitchPowerOff();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "killTrollPoll" && ModsManager.Trolling)
			{
				if (GameManager.AudioSlinger != null && !DevTools.InsanityMode)
				{
					GameManager.AudioSlinger.KillSound(TrollPoll.trollAudio);
					TrollPoll.isTrollPlaying = false;
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "playTrollPoll")
			{
				if (GameManager.AudioSlinger != null && Response.Additional != "" && !TrollPoll.isTrollPlaying && ModsManager.Trolling)
				{
					TrollPoll.trollAudio = LookUp.SoundLookUp.JumpHit1;
					if (Response.Additional.ToLower() == "vacation")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.vacationMusic;
					}
					else if (Response.Additional.ToLower() == "triangle")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.triangleMusic;
					}
					else if (Response.Additional.ToLower() == "polishcow")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.polishCowMusic;
					}
					else if (Response.Additional.ToLower() == "nyancat")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.nyanCatMusic;
					}
					else if (Response.Additional.ToLower() == "stickbug")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.stickBugMusic;
					}
					else if (Response.Additional.ToLower() == "jebaited")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.jebaitedSong;
					}
					else if (Response.Additional.ToLower() == "keyboardcat")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.keyboardCatMusic;
					}
					else if (Response.Additional.ToLower() == "running")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.dreamRunningMusic;
					}
					else if (Response.Additional.ToLower() == "stal")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.minecraftStalMusic;
					}
					else if (Response.Additional.ToLower() == "chungus")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.bigChungusMusic;
					}
					else if (Response.Additional.ToLower() == "gnome")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.gnomedLOL;
					}
					else if (Response.Additional.ToLower() == "rickroll")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.rickRolled;
					}
					else if (Response.Additional.ToLower() == "blue")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.blueMusic;
					}
					else if (Response.Additional.ToLower() == "coffin")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.coffinDance;
					}
					else if (Response.Additional.ToLower() == "crab")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.crabRave;
					}
					else if (Response.Additional.ToLower() == "thomas")
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.thomasDankEngine;
					}
					else
					{
						TrollPoll.trollAudio.AudioClip = DownloadTIFiles.vacationMusic;
					}
					TrollPoll.trollAudio.MyAudioHub = AUDIO_HUB.PLAYER_HUB;
					TrollPoll.trollAudio.MyAudioLayer = AUDIO_LAYER.PLAYER;
					TrollPoll.trollAudio.Loop = false;
					TrollPoll.trollAudio.LoopCount = 0;
					TrollPoll.trollAudio.Volume = 0.333f;
					TrollPoll.isTrollPlaying = true;
					GameManager.TimeSlinger.FireTimer(DataManager.LeetMode ? 30f : 300f, delegate()
					{
						TrollPoll.isTrollPlaying = false;
					}, 0);
					GameManager.AudioSlinger.PlaySound(TrollPoll.trollAudio);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "trollLockPick")
			{
				if (LookUp.Doors != null)
				{
					LookUp.Doors.MainDoor.AudioHub.PlaySound(LookUp.SoundLookUp.DoorKnobSFX);
					if (ModsManager.EasyModeActive)
					{
						LookUp.Doors.MainDoor.AudioHub.PlaySoundCustomDelay(LookUp.SoundLookUp.DoorKnobSFX, 1f);
						LookUp.Doors.MainDoor.AudioHub.PlaySoundCustomDelay(LookUp.SoundLookUp.DoorKnobSFX, 2f);
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "trollScanner")
			{
				if (PoliceScannerBehaviour.Ins != null && EnemyManager.PoliceManager != null)
				{
					EnemyManager.PoliceManager.TrollPoliceScanner();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "shortTroll")
			{
				if (ModsManager.Trolling && GameManager.AudioSlinger != null)
				{
					AudioFileDefinition jumpHit = LookUp.SoundLookUp.JumpHit1;
					jumpHit.Volume = 0.666f;
					jumpHit.Loop = false;
					if (Response.Additional.ToLower() == "mlg")
					{
						jumpHit.AudioClip = DownloadTIFiles.MLGAirhorn;
					}
					else if (Response.Additional.ToLower() == "balloonboy")
					{
						jumpHit.AudioClip = DownloadTIFiles.BalloonBoy;
					}
					else if (Response.Additional.ToLower() == "virus")
					{
						jumpHit.AudioClip = DownloadTIFiles.YourComputerHasVirus;
					}
					else if (Response.Additional.ToLower() == "swamp")
					{
						jumpHit.AudioClip = DownloadTIFiles.WhatAreUDoingInMySwamp;
					}
					else if (Response.Additional.ToLower() == "swan")
					{
						jumpHit.AudioClip = DownloadTIFiles.SwanFailsafe;
					}
					else if (Response.Additional.ToLower() == "fbi")
					{
						jumpHit.AudioClip = DownloadTIFiles.FBIOpenUp;
					}
					else if (Response.Additional.ToLower() == "conga")
					{
						jumpHit.AudioClip = DownloadTIFiles.Conga;
					}
					else
					{
						jumpHit.AudioClip = DownloadTIFiles.MLGAirhorn;
					}
					GameManager.AudioSlinger.PlaySound(jumpHit);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "doomersElevator")
			{
				if (GameManager.WorldManager != null)
				{
					ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = new Vector3(-28.10953f, 40.51757f, -6.304061f);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "doomersOutside")
			{
				if (GameManager.WorldManager != null)
				{
					ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = new Vector3(-0.10953f, 0.51757f, -6.304061f);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "doomersApartment")
			{
				if (GameManager.WorldManager != null)
				{
					ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = new Vector3(6.10953f, 40.51757f, 1.304061f);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "doomersHome")
			{
				if (GameManager.WorldManager != null)
				{
					ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = new Vector3(0.10953f, 40.51757f, -1.304061f);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "doomersHallway")
			{
				if (GameManager.WorldManager != null)
				{
					ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = new Vector3(18.10953f, 40.51757f, -6.304061f);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "breatherOn")
			{
				this.forceBreather = true;
				this.iAmLive = true;
			}
			else if (Response.Action == "breatherOff")
			{
				this.forceBreather = false;
				this.iAmLive = true;
			}
			else if (Response.Action == "dollMaker")
			{
				if (EnemyManager.DollMakerManager != null)
				{
					EnemyManager.DollMakerManager.ForceMarker();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "giveTenant")
			{
				if (GameManager.ManagerSlinger.TenantTrackManager != null)
				{
					TenantDefinition tenantDefinition;
					do
					{
						int num = UnityEngine.Random.Range(0, GameManager.ManagerSlinger.TenantTrackManager.Tenants.Length);
						tenantDefinition = GameManager.ManagerSlinger.TenantTrackManager.Tenants[num];
					}
					while (tenantDefinition.tenantUnit == 0);
					GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					GameManager.ManagerSlinger.TextDocManager.CreateTextDoc(tenantDefinition.tenantUnit.ToString(), string.Concat(new object[]
					{
						tenantDefinition.tenantName,
						Environment.NewLine,
						Environment.NewLine,
						"Age: ",
						tenantDefinition.tenantAge,
						Environment.NewLine,
						Environment.NewLine,
						tenantDefinition.tenantNotes
					}));
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "diffIncrease")
			{
				if (EnemyManager.HitManManager != null && Response.Additional != "")
				{
					int num2 = int.Parse(Response.Additional);
					if (num2 <= 8)
					{
						for (int i = 0; i < num2; i++)
						{
							GameManager.TheCloud.ForceKeyDiscover();
						}
					}
					else
					{
						for (int j = 0; j < 8; j++)
						{
							GameManager.TheCloud.ForceKeyDiscover();
						}
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "noircult")
			{
				if (EnemyManager.CultManager != null)
				{
					EnemyManager.CultManager.attemptSpawn();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "spawnAdam")
			{
				if (AdamLOLHook.Ins != null)
				{
					AdamLOLHook.Ins.Spawn();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "deSpawnAdam")
			{
				if (AdamLOLHook.Ins != null)
				{
					AdamLOLHook.Ins.DeSpawn();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "spawnDancing")
			{
				if (DevTools.Ins != null)
				{
					this.spawnNoir(new Vector3(-0.304061f, 39.582f, 1.666f), new Vector3(0f, 130f, 0f));
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "despawnDancing")
			{
				if (DevTools.Ins != null)
				{
					this.despawnNoir();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "GoldenFreddy")
			{
				if (DevTools.Ins != null && !this.GFschedule && !ModsManager.Nightmare)
				{
					this.ScheduleGoldenFreddy();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "XOR")
			{
				if (DevTools.Ins != null && GameManager.TheCloud != null && !ModsManager.Nightmare)
				{
					ModsManager.Nightmare = true;
					GameManager.TheCloud.TenTwentyMode();
					GameManager.TimeSlinger.FireTimer(22f, new Action(this.ScheduleGoldenFreddy), 0);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "INSANITY")
			{
				if (DevTools.Ins != null && !DevTools.InsanityMode)
				{
					this.despawnNoir();
					for (int k = 0; k < 20; k++)
					{
						this.instantinateNoir(new Vector3(UnityEngine.Random.Range(-5f, 5f), 39.582f, UnityEngine.Random.Range(-5f, 5f)), new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f));
					}
					DevTools.InsanityMode = true;
					if (TrollPoll.isTrollPlaying)
					{
						GameManager.AudioSlinger.KillSound(TrollPoll.trollAudio);
					}
					else
					{
						TrollPoll.isTrollPlaying = true;
					}
					TrollPoll.trollAudio = LookUp.SoundLookUp.JumpHit1;
					TrollPoll.trollAudio.AudioClip = DownloadTIFiles.crazyparty;
					TrollPoll.trollAudio.MyAudioHub = AUDIO_HUB.PLAYER_HUB;
					TrollPoll.trollAudio.MyAudioLayer = AUDIO_LAYER.PLAYER;
					TrollPoll.trollAudio.Loop = true;
					TrollPoll.trollAudio.LoopCount = 3600;
					TrollPoll.trollAudio.Volume = 1f;
					EnemyManager.State = ENEMY_STATE.CULT;
					GameManager.TimeSlinger.FireTimer(30f, delegate()
					{
						CultComputerJumper.Ins.AddLightsOffJump();
					}, 0);
					GameManager.AudioSlinger.PlaySoundWithCustomDelay(TrollPoll.trollAudio, 0.4f);
					EnvironmentManager.PowerBehaviour.ForcePowerOff();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "casual")
			{
				if (EnemyManager.BreatherManager != null && EnemyManager.CultManager != null && EnemyManager.DollMakerManager != null && EnemyManager.HitManManager != null && EnemyManager.PoliceManager != null && EnemyManager.State == ENEMY_STATE.IDLE)
				{
					EnemyManager.State = ENEMY_STATE.LOCKED;
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "noCasual")
			{
				if (EnemyManager.BreatherManager != null && EnemyManager.CultManager != null && EnemyManager.DollMakerManager != null && EnemyManager.HitManManager != null && EnemyManager.PoliceManager != null && EnemyManager.State == ENEMY_STATE.LOCKED)
				{
					EnemyManager.State = ENEMY_STATE.IDLE;
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "chatdev")
			{
				if (ModsManager.DOSTwitchActive && Response.Additional != "")
				{
					GameManager.GetDOSTwitch().ChatDevUsername = Response.Additional;
					GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("DevTools: " + Response.Additional + " is the new chat developer!");
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "sendChatMessage")
			{
				if (ModsManager.DOSTwitchActive && Response.Additional != "")
				{
					GameManager.GetDOSTwitch().myTwitchIRC.SendMsg("DEV: " + Response.Additional);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "killp")
			{
				if (EnemyManager.BreatherManager != null && EnemyManager.CultManager != null && EnemyManager.DollMakerManager != null && EnemyManager.HitManManager != null && EnemyManager.PoliceManager != null && Response.Additional != "" && EnemyManager.State == ENEMY_STATE.IDLE)
				{
					if (Response.Additional == "lucas")
					{
						HitmanComputerJumper.Ins.AddComputerJump();
						EnemyManager.State = ENEMY_STATE.HITMAN;
					}
					else if (Response.Additional == "police")
					{
						EnemyManager.PoliceManager.triggerDevSwat();
						EnemyManager.State = ENEMY_STATE.POILCE;
					}
					else if (Response.Additional == "noir")
					{
						CultComputerJumper.Ins.AddLightsOffJump();
						EnemyManager.State = ENEMY_STATE.CULT;
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "zeroDiscount")
			{
				if (GameManager.ManagerSlinger.ProductsManager != null)
				{
					WindowManager.Get(SOFTWARE_PRODUCTS.ZERODAY).Launch();
					if (!ZeroDayProductObject.isDiscountOn)
					{
						for (int l = 0; l < GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts.Count; l++)
						{
							GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[l].myProductObject.DiscountMe();
						}
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "shadowDiscount")
			{
				if (GameManager.ManagerSlinger.ProductsManager != null)
				{
					WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
					if (!ShadowProductObject.isDiscountOn)
					{
						for (int m = 0; m < GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count; m++)
						{
							GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[m].myProductObject.DiscountMe();
						}
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "speedManipulator")
			{
				if (GameManager.BehaviourManager.AnnBehaviour != null && Response.Additional != "")
				{
					if (Response.Additional.ToLower() == "slower")
					{
						SpeedPoll.DevEnableManipulator(TWITCH_NET_SPEED.SLOW);
					}
					else if (Response.Additional.ToLower() == "faster")
					{
						SpeedPoll.DevEnableManipulator(TWITCH_NET_SPEED.FAST);
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "keyManipulator")
			{
				if (GameManager.BehaviourManager.AnnBehaviour != null && Response.Additional != "")
				{
					if (Response.Additional.ToLower() == "enabled")
					{
						KeyPoll.DevEnableManipulator(KEY_CUE_MODE.ENABLED);
					}
					else if (Response.Additional.ToLower() == "disabled")
					{
						KeyPoll.DevEnableManipulator(KEY_CUE_MODE.DISABLED);
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "addCoins")
			{
				if (Response.Additional != "" && float.Parse(Response.Additional) <= 1000f && float.Parse(Response.Additional) > 0f)
				{
					CurrencyManager.AddCurrency(float.Parse(Response.Additional));
					GameManager.HackerManager.WhiteHatSound();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "subCoins")
			{
				if (Response.Additional != "" && float.Parse(Response.Additional) <= 1000f && float.Parse(Response.Additional) > 0f)
				{
					CurrencyManager.RemoveCurrency(float.Parse(Response.Additional));
					GameManager.HackerManager.BlackHatSound();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "setloan")
			{
				if (GameManager.ManagerSlinger != null && Response.Additional != "")
				{
					DOSCoinPoll.moneyLoan = (int)float.Parse(Response.Additional);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "keyDoc")
			{
				if (Response.Additional != "" && GameManager.AudioSlinger != null && GameManager.ManagerSlinger != null && GameManager.ManagerSlinger.TextDocManager != null)
				{
					string masterKey = GameManager.TheCloud.MasterKey;
					if (int.Parse(Response.Additional) == 1)
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Key1.txt", "1 - " + masterKey.Substring(0, 12));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
					else if (int.Parse(Response.Additional) == 2)
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Key2.txt", "2 - " + masterKey.Substring(12, 12));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
					else if (int.Parse(Response.Additional) == 3)
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Key3.txt", "3 - " + masterKey.Substring(24, 12));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
					else if (int.Parse(Response.Additional) == 4)
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Key4.txt", "4 - " + masterKey.Substring(36, 12));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
					else if (int.Parse(Response.Additional) == 5)
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Key5.txt", "5 - " + masterKey.Substring(48, 12));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
					else if (int.Parse(Response.Additional) == 6)
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Key6.txt", "6 - " + masterKey.Substring(60, 12));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
					else if (int.Parse(Response.Additional) == 7)
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Key7.txt", "7 - " + masterKey.Substring(72, 12));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
					else if (int.Parse(Response.Additional) == 8)
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Key8.txt", "8 - " + masterKey.Substring(84, 12));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "doc")
			{
				if (Response.Additional != "" && GameManager.AudioSlinger != null && GameManager.ManagerSlinger != null && GameManager.ManagerSlinger.TextDocManager != null)
				{
					string[] array = Response.Additional.Split(new char[]
					{
						':'
					});
					GameManager.ManagerSlinger.TextDocManager.CreateTextDoc(array[0], array[1]);
					GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "wikiDoc")
			{
				if (Response.Additional != "" && GameManager.AudioSlinger != null && GameManager.ManagerSlinger != null && GameManager.ManagerSlinger.TextDocManager != null)
				{
					if (Response.Additional == "1")
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Wiki1.txt", GameManager.TheCloud.GetWikiURL(0));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
					else if (Response.Additional == "2")
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Wiki2.txt", GameManager.TheCloud.GetWikiURL(1));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
					else if (Response.Additional == "3")
					{
						GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("Wiki3.txt", GameManager.TheCloud.GetWikiURL(2));
						GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "clearNotes")
			{
				if (GameManager.BehaviourManager.NotesBehaviour != null)
				{
					GameManager.BehaviourManager.NotesBehaviour.ClearNotes();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "shutdownPC")
			{
				if (!StateManager.BeingHacked && ComputerPowerHook.Ins != null)
				{
					ComputerPowerHook.Ins.ShutDownComputer();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "vwipe")
			{
				if (GameManager.HackerManager.virusManager != null)
				{
					GameManager.HackerManager.virusManager.ClearVirus();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "dosdrainer")
			{
				if (GameManager.ManagerSlinger.WifiManager != null && GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() != null)
				{
					GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer = true;
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "swan")
			{
				if (GameManager.HackerManager != null && GameManager.HackerManager.theSwan != null)
				{
					GameManager.HackerManager.theSwan.ActivateTheSwan();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "virus")
			{
				if (GameManager.HackerManager.virusManager != null && Response.Additional != "")
				{
					int num3 = int.Parse(Response.Additional);
					if (num3 <= 10)
					{
						for (int n = 0; n < num3; n++)
						{
							GameManager.HackerManager.virusManager.ForceVirus();
						}
					}
					else
					{
						for (int num4 = 0; num4 < 10; num4++)
						{
							GameManager.HackerManager.virusManager.ForceVirus();
						}
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "whitehat")
			{
				if (GameManager.HackerManager != null && InventoryManager.GetProductCount(SOFTWARE_PRODUCTS.BACKDOOR) > 0)
				{
					InventoryManager.RemoveProduct(SOFTWARE_PRODUCTS.BACKDOOR);
					float setAMT;
					if (UnityEngine.Random.Range(0, 10) > 7)
					{
						setAMT = UnityEngine.Random.Range(3.5f, 133.7f);
					}
					else if (UnityEngine.Random.Range(0, 100) > 90)
					{
						setAMT = 3.5f;
					}
					else
					{
						setAMT = UnityEngine.Random.Range(3.5f, 33.7f);
					}
					GameManager.HackerManager.WhiteHatSound();
					CurrencyManager.AddCurrency(setAMT);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "itemwhitehat")
			{
				if (GameManager.ManagerSlinger.ProductsManager != null)
				{
					InventoryManager.RemoveProduct(SOFTWARE_PRODUCTS.BACKDOOR);
					WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
					if (!ProductsManager.ownsWhitehatScanner && !PoliceScannerBehaviour.Ins.ownPoliceScanner)
					{
						WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
						ProductsManager.ownsWhitehatScanner = true;
						GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 5].myProductObject.shipItem();
					}
					else if (!ProductsManager.ownsWhitehatDongle2 && InventoryManager.WifiDongleLevel == WIFI_DONGLE_LEVEL.LEVEL1)
					{
						WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
						ProductsManager.ownsWhitehatDongle2 = true;
						GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 9].myProductObject.shipItem();
					}
					else if (!ProductsManager.ownsWhitehatDongle3 && InventoryManager.WifiDongleLevel == WIFI_DONGLE_LEVEL.LEVEL2)
					{
						WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
						ProductsManager.ownsWhitehatDongle3 = true;
						GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 8].myProductObject.shipItem();
					}
					else
					{
						GameManager.HackerManager.WhiteHatSound();
						CurrencyManager.AddCurrency(2.5f);
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "hack")
			{
				if (GameManager.HackerManager != null)
				{
					GameManager.HackerManager.ForceNormalHack();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "hackpog")
			{
				if (GameManager.HackerManager != null)
				{
					GameManager.HackerManager.ForcePogHack();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "blackhat")
			{
				if (GameManager.HackerManager != null)
				{
					GameManager.HackerManager.ForceTwitchHack();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "drainBackdoor")
			{
				if (GameManager.HackerManager != null)
				{
					do
					{
						InventoryManager.RemoveProduct(SOFTWARE_PRODUCTS.BACKDOOR);
					}
					while (InventoryManager.GetProductCount(SOFTWARE_PRODUCTS.BACKDOOR) > 0);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "wifiDoc")
			{
				if (GameManager.ManagerSlinger.WifiManager != null && GameManager.ManagerSlinger.TextDocManager != null && GameManager.AudioSlinger != null)
				{
					int index;
					do
					{
						index = UnityEngine.Random.Range(0, 42);
					}
					while (GameManager.ManagerSlinger.WifiManager.GetAllWifiNetworks()[index].networkSecurity == WIFI_SECURITY.NONE);
					GameManager.ManagerSlinger.TextDocManager.CreateTextDoc(GameManager.ManagerSlinger.WifiManager.GetAllWifiNetworks()[index].networkName, GameManager.ManagerSlinger.WifiManager.GetAllWifiNetworks()[index].networkPassword);
					GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "disconnectWifi")
			{
				if (GameManager.ManagerSlinger.WifiManager != null)
				{
					GameManager.ManagerSlinger.WifiManager.DisconnectFromWifi();
				}
				this.iAmLive = true;
			}
			Debug.Log(string.Concat(new string[]
			{
				"Executed DevResponse: ",
				Response.Action,
				" with additional information(",
				Response.Additional,
				")"
			}));
			WWWForm wwwform = new WWWForm();
			wwwform.AddField("resetJson", "true");
			base.StartCoroutine(this.PostRequest(wwwform));
		}
	}

	public DevTools()
	{
		this.myHash = string.Empty;
		this.domain = "http://naskogdps17.7m.pl/wttg/dev/";
	}

	private void Awake()
	{
		DevTools.Ins = this;
	}

	private IEnumerator loadEndingWorld()
	{
		AsyncOperation result = SceneManager.LoadSceneAsync(8, LoadSceneMode.Additive);
		Debug.Log("Loaded Scene 8");
		while (!result.isDone)
		{
			yield return new WaitForEndOfFrame();
		}
		UnityEngine.Object.Destroy(GameObject.Find("SecretManager").gameObject);
		UnityEngine.Object.Destroy(GameObject.Find("SecretCanvas").gameObject);
		Debug.Log("Destroyed Scene 8");
		GameObject original = GameObject.Find("CultMaleSecret");
		this.dancingNoir = UnityEngine.Object.Instantiate<GameObject>(original, new Vector3(0f, 0f, 0f), Quaternion.identity);
		SceneManager.UnloadSceneAsync(8);
		Debug.Log("Unloaded Scene 8");
		yield break;
	}

	public void spawnNoir(Vector3 Pos, Vector3 Rot)
	{
		this.dancingNoir.transform.localPosition = Pos;
		this.dancingNoir.transform.localRotation = Quaternion.Euler(Rot);
		this.dancingNoirSpawned = true;
	}

	public void despawnNoir()
	{
		this.dancingNoir.transform.localPosition = Vector3.zero;
		this.dancingNoir.transform.localRotation = Quaternion.Euler(Vector3.zero);
		this.dancingNoirSpawned = false;
	}

	public void instantinateNoir(Vector3 Pos, Vector3 Rot)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.dancingNoir);
		gameObject.transform.localPosition = Pos;
		gameObject.transform.localRotation = Quaternion.Euler(Rot);
	}

	private void ScheduleGoldenFreddy()
	{
		if (!this.GFschedule)
		{
			this.GFschedule = true;
			GameManager.TimeSlinger.FireTimer(45f, new Action(this.ScheduleGoldenFreddy), 0);
			AudioFileDefinition jumpHit = LookUp.SoundLookUp.JumpHit1;
			jumpHit.AudioClip = DownloadTIFiles.GFPresence;
			jumpHit.Volume = 0.8f;
			jumpHit.Loop = false;
			GameManager.AudioSlinger.PlaySound(jumpHit);
			return;
		}
		if (StateManager.PlayerState != PLAYER_STATE.PEEPING)
		{
			GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(150f, 350f), new Action(this.ScheduleGoldenFreddy), 0);
			this.SpawnGF();
			return;
		}
		GameManager.TimeSlinger.FireTimer(UnityEngine.Random.Range(5f, 20f), new Action(this.ScheduleGoldenFreddy), 0);
	}

	private void SpawnGF()
	{
		GameObject gameObject = new GameObject();
		gameObject.AddComponent<Image>().sprite = Sprite.Create(DownloadTIFiles.Freddy, new Rect(0f, 0f, (float)DownloadTIFiles.Freddy.width, (float)DownloadTIFiles.Freddy.height), new Vector2(0.5f, 0.5f), 100f);
		gameObject.GetComponent<RectTransform>().SetParent(LookUp.PlayerUI.HandTransform.transform);
		gameObject.GetComponent<RectTransform>().transform.position = new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f);
		Debug.Log(gameObject.GetComponent<RectTransform>().transform.position.x);
		Debug.Log(gameObject.GetComponent<RectTransform>().transform.position.y);
		Debug.Log(gameObject.GetComponent<RectTransform>().transform.position.z);
		gameObject.transform.localScale = new Vector3(10f, 10f, 10f);
		gameObject.SetActive(true);
		AudioFileDefinition jumpHit = LookUp.SoundLookUp.JumpHit1;
		jumpHit.AudioClip = DownloadTIFiles.GFLaugh;
		jumpHit.Volume = 1f;
		jumpHit.Loop = false;
		GameManager.AudioSlinger.PlaySound(jumpHit);
		GameManager.TimeSlinger.FireTimer(0.85f, delegate()
		{
			UnityEngine.Object.Destroy(gameObject);
		}, 0);
	}

	public string myHash;

	public string domain;

	public float UpdateTickCount;

	public bool iAmLive;

	public DevResponse DevResponse;

	public bool forceBreather;

	public static DevTools Ins;

	private GameObject dancingNoir;

	public bool dancingNoirSpawned;

	public static bool InsanityMode;

	private bool GFschedule;
}
