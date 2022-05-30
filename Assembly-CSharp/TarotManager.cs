using System;
using UnityEngine;

public class TarotManager : MonoBehaviour
{
	private void HermitTrap()
	{
		if (StateManager.PlayerLocation != PLAYER_LOCATION.MAIN_ROON && StateManager.PlayerLocation != PLAYER_LOCATION.BATH_ROOM && StateManager.PlayerLocation != PLAYER_LOCATION.OUTSIDE && StateManager.PlayerLocation != PLAYER_LOCATION.UNKNOWN)
		{
			ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = new Vector3(0.10953f, 40.51757f, -1.304061f);
		}
	}

	private void Awake()
	{
		TarotManager.Ins = this;
	}

	public void PullTarotCard(TAROT_CARDS card)
	{
		switch (card)
		{
		case TAROT_CARDS.THE_PRO:
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				if (KeyPoll.keyManipulatorData == KEY_CUE_MODE.DEFAULT)
				{
					KeyPoll.DevEnableManipulator(KEY_CUE_MODE.ENABLED);
					return;
				}
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
				return;
			}
			else
			{
				if (!SpeedPoll.speedManipulatorActive)
				{
					SpeedPoll.DevEnableManipulator(TWITCH_NET_SPEED.FAST);
					return;
				}
				if (GameManager.ManagerSlinger.WifiManager != null && GameManager.ManagerSlinger.TextDocManager != null && GameManager.AudioSlinger != null)
				{
					int index2;
					do
					{
						index2 = UnityEngine.Random.Range(0, 42);
					}
					while (GameManager.ManagerSlinger.WifiManager.GetAllWifiNetworks()[index2].networkSecurity == WIFI_SECURITY.NONE);
					GameManager.ManagerSlinger.TextDocManager.CreateTextDoc(GameManager.ManagerSlinger.WifiManager.GetAllWifiNetworks()[index2].networkName, GameManager.ManagerSlinger.WifiManager.GetAllWifiNetworks()[index2].networkPassword);
					GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
				}
				return;
			}
			break;
		case TAROT_CARDS.THE_NOOB:
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				if (KeyPoll.keyManipulatorData == KEY_CUE_MODE.DEFAULT)
				{
					KeyPoll.DevEnableManipulator(KEY_CUE_MODE.DISABLED);
					return;
				}
				if (GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() != null)
				{
					GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer = true;
					return;
				}
				return;
			}
			else
			{
				if (!SpeedPoll.speedManipulatorActive)
				{
					SpeedPoll.DevEnableManipulator(TWITCH_NET_SPEED.SLOW);
					return;
				}
				if (GameManager.ManagerSlinger.WifiManager.getCurrentWiFi() != null)
				{
					GameManager.ManagerSlinger.WifiManager.getCurrentWiFi().affectedByDosDrainer = true;
					return;
				}
				return;
			}
			break;
		case TAROT_CARDS.THE_SUN:
			if (TarotManager.TimeController == 30)
			{
				TarotManager.TimeController = 60;
			}
			else if (TarotManager.TimeController == 5)
			{
				TarotManager.TimeController = 30;
			}
			GameManager.TimeSlinger.FireTimer(300f, delegate()
			{
				TarotManager.TimeController = 30;
			}, 0);
			GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.timechange);
			return;
		case TAROT_CARDS.THE_MOON:
			if (TarotManager.TimeController == 30)
			{
				TarotManager.TimeController = 5;
			}
			else if (TarotManager.TimeController == 60)
			{
				TarotManager.TimeController = 30;
			}
			GameManager.TimeSlinger.FireTimer(300f, delegate()
			{
				TarotManager.TimeController = 30;
			}, 0);
			GameManager.AudioSlinger.PlaySound(CustomSoundLookUp.timechange);
			return;
		case TAROT_CARDS.THE_RICH:
			CurrencyManager.AddCurrency(UnityEngine.Random.Range(3.33f, 166.6f));
			GameManager.HackerManager.WhiteHatSound();
			return;
		case TAROT_CARDS.THE_POOR:
			CurrencyManager.RemoveCurrency(UnityEngine.Random.Range(CurrencyManager.CurrentCurrency / 3f, CurrencyManager.CurrentCurrency / 2f));
			GameManager.HackerManager.BlackHatSound();
			return;
		case TAROT_CARDS.THE_CURSED:
		{
			int num = UnityEngine.Random.Range(0, 100);
			if (num < 10 && !GameManager.HackerManager.theSwan.isActivatedBefore)
			{
				GameManager.HackerManager.theSwan.ActivateTheSwan();
				return;
			}
			if (num >= 10 && num < 20 && !GameManager.TheCloud.IsGFActive)
			{
				GameManager.TheCloud.ScheduleGoldenFreddy();
				return;
			}
			for (int i = 0; i < UnityEngine.Random.Range(1, 5); i++)
			{
				GameManager.HackerManager.virusManager.ForceVirus();
			}
			return;
		}
		case TAROT_CARDS.THE_GAMBLER:
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				CurrencyManager.AddCurrency(CurrencyManager.CurrentCurrency);
				GameManager.HackerManager.WhiteHatSound();
				return;
			}
			CurrencyManager.RemoveCurrency(CurrencyManager.CurrentCurrency);
			GameManager.HackerManager.BlackHatSound();
			return;
		case TAROT_CARDS.THE_HACKER:
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				GameManager.HackerManager.ForcePogHack();
				return;
			}
			GameManager.HackerManager.ForceNormalHack();
			return;
		case TAROT_CARDS.THE_DEVIL:
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				WindowManager.Get(SOFTWARE_PRODUCTS.ZERODAY).Launch();
				if (!ZeroDayProductObject.isDiscountOn)
				{
					for (int j = 0; j < GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts.Count; j++)
					{
						GameManager.ManagerSlinger.ProductsManager.ZeroDayProducts[j].myProductObject.DiscountMe();
					}
				}
				return;
			}
			WindowManager.Get(SOFTWARE_PRODUCTS.SHADOW_MARKET).Launch();
			if (!ShadowProductObject.isDiscountOn)
			{
				for (int k = 0; k < GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts.Count; k++)
				{
					GameManager.ManagerSlinger.ProductsManager.ShadowMarketProducts[k].myProductObject.DiscountMe();
				}
			}
			return;
		case TAROT_CARDS.THE_UNDERTAKER:
			if (!TarotManager.BreatherUndertaker)
			{
				TarotManager.BreatherUndertaker = true;
				GameManager.TimeSlinger.FireTimer(600f, delegate()
				{
					TarotManager.BreatherUndertaker = false;
				}, 0);
				return;
			}
			return;
		case TAROT_CARDS.THE_QUICK:
			if (TarotManager.CurSpeed == playerSpeedMode.WEAK)
			{
				TarotManager.CurSpeed = playerSpeedMode.NORMAL;
				return;
			}
			TarotManager.CurSpeed = playerSpeedMode.QUICK;
			GameManager.TimeSlinger.FireTimer(180f, delegate()
			{
				TarotManager.CurSpeed = playerSpeedMode.NORMAL;
			}, 0);
			return;
		case TAROT_CARDS.THE_WEAK:
			if (TarotManager.CurSpeed == playerSpeedMode.QUICK)
			{
				TarotManager.CurSpeed = playerSpeedMode.NORMAL;
				return;
			}
			TarotManager.CurSpeed = playerSpeedMode.WEAK;
			GameManager.TimeSlinger.FireTimer(180f, delegate()
			{
				TarotManager.CurSpeed = playerSpeedMode.NORMAL;
			}, 0);
			return;
		case TAROT_CARDS.THE_DRUNK:
			GameManager.BehaviourManager.NotesBehaviour.ClearNotes();
			return;
		case TAROT_CARDS.THE_IMMUNE:
			if (EnemyManager.State == ENEMY_STATE.IDLE)
			{
				EnemyManager.State = ENEMY_STATE.LOCKED;
				GameManager.TimeSlinger.FireTimer(600f, delegate()
				{
					EnemyManager.State = ENEMY_STATE.IDLE;
				}, 0);
				return;
			}
			return;
		case TAROT_CARDS.THE_POPULAR:
			for (int l = 0; l < UnityEngine.Random.Range(1, 3); l++)
			{
				GameManager.TheCloud.ForceKeyDiscover();
			}
			return;
		case TAROT_CARDS.THE_HERMIT:
			if (!TarotManager.HermitActive)
			{
				TarotManager.HermitActive = true;
				GameManager.TimeSlinger.FireTimer((ModsManager.Nightmare || DataManager.LeetMode) ? 300f : 600f, delegate()
				{
					TarotManager.HermitActive = false;
				}, 0);
				return;
			}
			return;
		case TAROT_CARDS.THE_DIZZY:
			if (!TarotManager.DizzyActive)
			{
				TarotManager.DizzyActive = true;
				GameManager.TimeSlinger.FireTimer(300f, delegate()
				{
					TarotManager.DizzyActive = false;
				}, 0);
				return;
			}
			return;
		case TAROT_CARDS.THE_DEAF:
			GameManager.AudioSlinger.PlaySound(CustomSoundLookUp._static);
			return;
		case TAROT_CARDS.THE_BLIND:
			EnvironmentManager.PowerBehaviour.ForceTwitchPowerOff();
			return;
		case TAROT_CARDS.THE_ARTIST:
			GameManager.ManagerSlinger.TextDocManager.CreateTextDoc("keys.txt", string.Concat(new object[]
			{
				"- " + TarotManager.tappedSites[0] + "\n",
				"- " + TarotManager.tappedSites[1] + "\n",
				"- " + TarotManager.tappedSites[2] + "\n",
				"- " + TarotManager.tappedSites[3] + "\n",
				"- " + TarotManager.tappedSites[4] + "\n",
				"- " + TarotManager.tappedSites[5] + "\n",
				"- " + TarotManager.tappedSites[6] + "\n",
				"- " + TarotManager.tappedSites[7]
			}));
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.KeyFound);
			return;
		case TAROT_CARDS.THE_DEAD:
			EnemyManager.CultManager.triggerCloseJump();
			return;
		default:
			return;
		}
	}

	public void PullCardAtLoc()
	{
		this.PullTarotCard((TAROT_CARDS)TarotCardPullAnim.currentCardTex);
	}

	private void Update()
	{
		if (TarotManager.HermitActive)
		{
			this.HermitTrap();
		}
	}

	public static bool HermitActive;

	public static int TimeController = 30;

	public static bool BreatherUndertaker = false;

	public static TarotManager Ins;

	public static bool DizzyActive;

	public static playerSpeedMode CurSpeed = playerSpeedMode.NORMAL;

	public static string[] tappedSites;
}
