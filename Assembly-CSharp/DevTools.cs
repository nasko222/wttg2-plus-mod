using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DevTools : MonoBehaviour
{
	private void WarmUpTools()
	{
		WWWForm wwwform = new WWWForm();
		wwwform.AddField("addCode", this.myHash);
		base.StartCoroutine(this.PostRequest(wwwform));
		base.StartCoroutine(this.UpdateMe());
	}

	private IEnumerator PostRequest(WWWForm FormRequest)
	{
		UnityWebRequest UWR = UnityWebRequest.Post(this.domain, FormRequest);
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
					if (Response.Additional.ToLower() == "vacation")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.vacation;
					}
					else if (Response.Additional.ToLower() == "triangle")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.triangle;
					}
					else if (Response.Additional.ToLower() == "polishcow")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.polishcow;
					}
					else if (Response.Additional.ToLower() == "nyancat")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.nyancat;
					}
					else if (Response.Additional.ToLower() == "stickbug")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.stickbug;
					}
					else if (Response.Additional.ToLower() == "jebaited")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.jebaited;
					}
					else if (Response.Additional.ToLower() == "keyboardcat")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.keyboard;
					}
					else if (Response.Additional.ToLower() == "running")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.dream;
					}
					else if (Response.Additional.ToLower() == "elevator")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.elevator;
					}
					else if (Response.Additional.ToLower() == "chungus")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.chungus;
					}
					else if (Response.Additional.ToLower() == "kappa")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.kappa;
					}
					else if (Response.Additional.ToLower() == "blue")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.blue;
					}
					else if (Response.Additional.ToLower() == "coffin")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.coffin;
					}
					else if (Response.Additional.ToLower() == "crab")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.crab;
					}
					else if (Response.Additional.ToLower() == "thomas")
					{
						TrollPoll.trollAudio = CustomSoundLookUp.thomas;
					}
					else
					{
						TrollPoll.trollAudio = CustomSoundLookUp.vacation;
					}
					TrollPoll.isTrollPlaying = true;
					GameManager.TimeSlinger.FireTimer(DataManager.LeetMode ? 30f : 110f, delegate()
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
					AudioFileDefinition audioFile;
					if (Response.Additional.ToLower() == "mlg")
					{
						audioFile = CustomSoundLookUp.mlg;
					}
					else if (Response.Additional.ToLower() == "balloonboy")
					{
						audioFile = CustomSoundLookUp.bblaugh;
					}
					else if (Response.Additional.ToLower() == "virus")
					{
						audioFile = CustomSoundLookUp.virus;
					}
					else if (Response.Additional.ToLower() == "swamp")
					{
						audioFile = CustomSoundLookUp.swamp;
					}
					else if (Response.Additional.ToLower() == "fbi")
					{
						audioFile = CustomSoundLookUp.fbi;
					}
					else if (Response.Additional.ToLower() == "conga")
					{
						audioFile = CustomSoundLookUp.conga;
					}
					else if (Response.Additional.ToLower() == "gnome")
					{
						audioFile = CustomSoundLookUp.gnome;
					}
					else if (Response.Additional.ToLower() == "owl")
					{
						audioFile = CustomSoundLookUp.owl;
					}
					else if (Response.Additional.ToLower() == "startup")
					{
						audioFile = CustomSoundLookUp.startup;
					}
					else if (Response.Additional.ToLower() == "shutdown")
					{
						audioFile = CustomSoundLookUp.shutdown;
					}
					else if (Response.Additional.ToLower() == "bruh")
					{
						audioFile = CustomSoundLookUp.bruh;
					}
					else if (Response.Additional.ToLower() == "dogdoin")
					{
						audioFile = CustomSoundLookUp.dogdoin;
					}
					else if (Response.Additional.ToLower() == "illuminati")
					{
						audioFile = CustomSoundLookUp.illuminati;
					}
					else
					{
						audioFile = CustomSoundLookUp.mlg;
					}
					GameManager.AudioSlinger.PlaySound(audioFile);
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
					TenantData tenantData;
					do
					{
						int num = UnityEngine.Random.Range(0, GameManager.ManagerSlinger.TenantTrackManager.TenantDatas.Length);
						tenantData = GameManager.ManagerSlinger.TenantTrackManager.TenantDatas[num];
					}
					while (tenantData.tenantUnit == 0);
					GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
					GameManager.ManagerSlinger.TextDocManager.CreateTextDoc(tenantData.tenantUnit.ToString(), string.Concat(new object[]
					{
						tenantData.tenantName,
						Environment.NewLine,
						Environment.NewLine,
						"Age: ",
						tenantData.tenantAge,
						Environment.NewLine,
						Environment.NewLine,
						tenantData.tenantNotes
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
				if (GameManager.TheCloud != null || DancingLoader.Ins != null)
				{
					GameManager.TheCloud.spawnNoir(new Vector3(-0.304061f, 39.582f, 1.666f), new Vector3(0f, 130f, 0f));
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "despawnDancing")
			{
				if (GameManager.TheCloud != null || DancingLoader.Ins != null)
				{
					GameManager.TheCloud.despawnNoir();
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
						for (int k = 0; k < GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts.Count; k++)
						{
							GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[k].myProductObject.DiscountMe();
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
						for (int l = 0; l < GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count; l++)
						{
							GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[l].myProductObject.DiscountMe();
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
						for (int m = 0; m < num3; m++)
						{
							GameManager.HackerManager.virusManager.ForceVirus();
						}
					}
					else
					{
						for (int n = 0; n < 10; n++)
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
		this.domain = "https://wttg2plus.ampersoft.cz/admin/DevToolsData/ping";
	}

	private void Awake()
	{
		DevTools.Ins = this;
	}

	private void ScheduleGoldenFreddy()
	{
		if (!this.GFschedule)
		{
			this.GFschedule = true;
			GameManager.TimeSlinger.FireTimer(45f, new Action(this.ScheduleGoldenFreddy), 0);
			GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.gflaugh);
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
		gameObject.AddComponent<Image>().sprite = Sprite.Create(CustomSpriteLookUp.freddy, new Rect(0f, 0f, (float)CustomSpriteLookUp.freddy.width, (float)CustomSpriteLookUp.freddy.height), new Vector2(0.5f, 0.5f), 100f);
		gameObject.GetComponent<RectTransform>().SetParent(LookUp.PlayerUI.HandTransform.transform);
		gameObject.GetComponent<RectTransform>().transform.position = new Vector3((float)(Screen.width / 2), (float)(Screen.height / 2), 0f);
		Debug.Log(gameObject.GetComponent<RectTransform>().transform.position.x);
		Debug.Log(gameObject.GetComponent<RectTransform>().transform.position.y);
		Debug.Log(gameObject.GetComponent<RectTransform>().transform.position.z);
		gameObject.transform.localScale = new Vector3(10f, 10f, 10f);
		gameObject.SetActive(true);
		GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.gflaugh);
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
