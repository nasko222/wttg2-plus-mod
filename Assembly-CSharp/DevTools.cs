using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

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
		this.createTrollBlockers();
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
			else if (Response.Action == "enableTrollBlocker")
			{
				if (Response.Additional != "")
				{
					if (Response.Additional.ToLower() == "apartment")
					{
						this.trollBlockerApartment.SetActive(true);
					}
					else if (Response.Additional.ToLower() == "hallway")
					{
						this.trollBlockerHallway.SetActive(true);
					}
					else if (Response.Additional.ToLower() == "lobby")
					{
						this.trollBlockerLobby.SetActive(true);
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "disableTrollBlocker")
			{
				if (Response.Additional != "")
				{
					if (Response.Additional.ToLower() == "apartment")
					{
						this.trollBlockerApartment.SetActive(false);
					}
					else if (Response.Additional.ToLower() == "hallway")
					{
						this.trollBlockerHallway.SetActive(false);
					}
					else if (Response.Additional.ToLower() == "lobby")
					{
						this.trollBlockerLobby.SetActive(false);
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "changeTrollBlockerImage")
			{
				if (Response.Additional != "")
				{
					DevTools._imgURL = Response.Additional;
					base.StartCoroutine(this.setTrollTex());
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
					if (ModsManager.EasierEnemies)
					{
						LookUp.Doors.MainDoor.AudioHub.PlaySoundCustomDelay(LookUp.SoundLookUp.DoorKnobSFX, 1f);
						LookUp.Doors.MainDoor.AudioHub.PlaySoundCustomDelay(LookUp.SoundLookUp.DoorKnobSFX, 2f);
					}
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "trollScanner")
			{
				if (PoliceScannerBehaviour.Ins != null && EnemyManager.PoliceManager != null && PoliceScannerBehaviour.Ins.IsOn)
				{
					EnemyManager.PoliceManager.TrollPoliceScanner();
				}
				this.iAmLive = true;
			}
			else if (Response.Action == "trollRouter")
			{
				if (RouterBehaviour.Ins != null && EnemyManager.PoliceManager != null && RouterBehaviour.Ins.Owned)
				{
					RouterBehaviour.Ins.trollReset();
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
						audioFile = CustomSoundLookUp.xfiles;
					}
					else if (Response.Additional.ToLower() == "trolled")
					{
						audioFile = CustomSoundLookUp.trolled;
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
			else if (Response.Action == "foolOn")
			{
				this.alwaysFool = true;
				this.iAmLive = true;
			}
			else if (Response.Action == "foolOff")
			{
				this.alwaysFool = false;
				this.iAmLive = true;
			}
			else if (Response.Action == "refillTarot")
			{
				if (TarotCardsBehaviour.Ins != null)
				{
					TarotRefiller.RefillCards();
				}
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
			else if (Response.Action == "BombMaker")
			{
				if (EnemyManager.BombMakerManager != null)
				{
					EnemyManager.BombMakerManager.ReleaseTheBombMakerInstantly();
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
				if (GameManager.TheCloud != null && !GameManager.TheCloud.IsGFActive && !ModsManager.Nightmare)
				{
					GameManager.TheCloud.ScheduleGoldenFreddy();
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
					else if (Response.Additional == "bombmaker")
					{
						BombMakerDeskJumper.Ins.AddComputerJump();
						BombMakerBehaviour.Ins.StageBombMakerOutsideKill();
						EnemyManager.State = ENEMY_STATE.BOMB_MAKER;
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
					if (Response.Additional == "2")
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
					if (!ProductsManager.ownsWhitehatScanner && !PoliceScannerBehaviour.Ins.ownPoliceScanner && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 8].myProductObject.myProduct.productIsPending && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 8].myProductObject.myProduct.productIsShipped)
					{
						WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
						ProductsManager.ownsWhitehatScanner = true;
						GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 8].myProductObject.shipItem();
					}
					else if (!ProductsManager.ownsWhitehatRemoteVPN2 && RemoteVPNObject.RemoteVPNLevel == 1 && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 5].myProductObject.myProduct.productIsPending && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 5].myProductObject.myProduct.productIsShipped)
					{
						WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
						ProductsManager.ownsWhitehatRemoteVPN2 = true;
						GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 5].myProductObject.shipItem();
					}
					else if (!ProductsManager.ownsWhitehatRouter && !RouterBehaviour.Ins.Owned && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 2].myProductObject.myProduct.productIsPending && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 2].myProductObject.myProduct.productIsShipped)
					{
						WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
						ProductsManager.ownsWhitehatRouter = true;
						GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 2].myProductObject.shipItem();
					}
					else if (!ProductsManager.ownsWhitehatDongle2 && InventoryManager.WifiDongleLevel == WIFI_DONGLE_LEVEL.LEVEL1 && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 12].myProductObject.myProduct.productIsPending && !GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 12].myProductObject.myProduct.productIsShipped)
					{
						WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
						ProductsManager.ownsWhitehatDongle2 = true;
						GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count - 12].myProductObject.shipItem();
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
				if (GameManager.ManagerSlinger.WifiManager != null && GameManager.ManagerSlinger.WifiManager.IsOnline)
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

	private void createTrollBlockers()
	{
		GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		this.trollBlockerApartment = gameObject;
		this.trollBlockerHallway = gameObject2;
		this.trollBlockerLobby = gameObject3;
		this.trollBlockerApartment.transform.position = new Vector3(-5.2f, 40.5f, -0.5f);
		this.trollBlockerApartment.transform.localScale = new Vector3(2.7f, 3f, 0.1f);
		this.trollBlockerApartment.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
		this.trollBlockerHallway.transform.position = new Vector3(5.2f, 40.5f, -6.3f);
		this.trollBlockerHallway.transform.localScale = new Vector3(0.1f, 3f, 2.3f);
		this.trollBlockerLobby.transform.position = new Vector3(-1.5f, 1f, -9f);
		this.trollBlockerLobby.transform.localScale = new Vector3(3f, 3f, 0.1f);
		this.trollBlockerApartment.SetActive(false);
		this.trollBlockerHallway.SetActive(false);
		this.trollBlockerLobby.SetActive(false);
	}

	private IEnumerator setTrollTex()
	{
		UnityWebRequest www = UnityWebRequestTexture.GetTexture(DevTools._imgURL);
		yield return www.SendWebRequest();
		try
		{
			Texture texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
			this.trollBlockerApartment.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
			this.trollBlockerHallway.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
			this.trollBlockerLobby.GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
			yield break;
		}
		catch (Exception message)
		{
			Debug.Log(message);
			yield break;
		}
		yield break;
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

	public bool alwaysFool;

	private GameObject trollBlockerApartment;

	private GameObject trollBlockerHallway;

	private GameObject trollBlockerLobby;

	public static string _imgURL;
}
