using System;
using UnityEngine;

public class TarotManager : MonoBehaviour
{
	public void playTarot(TAROT_CARDS tarot)
	{
		if (UnityEngine.Random.Range(0, 100) < 13)
		{
			this.TheFool();
			return;
		}
		if (tarot == TAROT_CARDS.THE_PRO)
		{
			KeyPoll.DevEnableManipulator(KEY_CUE_MODE.ENABLED);
		}
		else if (tarot == TAROT_CARDS.THE_NOOB)
		{
			KeyPoll.DevEnableManipulator(KEY_CUE_MODE.DISABLED);
		}
		else if (tarot == TAROT_CARDS.THE_RICH)
		{
			SpeedPoll.DevEnableManipulator(TWITCH_NET_SPEED.FAST);
		}
		else if (tarot == TAROT_CARDS.THE_POOR)
		{
			SpeedPoll.DevEnableManipulator(TWITCH_NET_SPEED.SLOW);
		}
		else if (tarot == TAROT_CARDS.THE_IMMUNE)
		{
			if (EnemyManager.State == ENEMY_STATE.IDLE)
			{
				EnemyManager.State = ENEMY_STATE.LOCKED;
				GameManager.TimeSlinger.FireTimer(600f, delegate()
				{
					EnemyManager.State = ENEMY_STATE.IDLE;
				}, 0);
			}
			else
			{
				this.TheFool();
			}
		}
		else if (tarot == TAROT_CARDS.THE_SUN)
		{
			if (TarotManager.TimeController == 30)
			{
				TarotManager.TimeController = 60;
			}
			else if (TarotManager.TimeController == 5)
			{
				TarotManager.TimeController = 30;
			}
			GameManager.TimeSlinger.FireTimer(600f, delegate()
			{
				TarotManager.TimeController = 30;
			}, 0);
		}
		else if (tarot == TAROT_CARDS.THE_MOON)
		{
			if (TarotManager.TimeController == 30)
			{
				TarotManager.TimeController = 5;
			}
			else if (TarotManager.TimeController == 60)
			{
				TarotManager.TimeController = 30;
			}
			GameManager.TimeSlinger.FireTimer(600f, delegate()
			{
				TarotManager.TimeController = 30;
			}, 0);
		}
		else if (tarot == TAROT_CARDS.THE_UNDERTAKER)
		{
			TarotManager.BreatherUndertaker = true;
			GameManager.TimeSlinger.FireTimer(600f, delegate()
			{
				TarotManager.BreatherUndertaker = false;
			}, 0);
		}
		else if (tarot == TAROT_CARDS.THE_HERMIT)
		{
			TarotManager.HermitActive = true;
			GameManager.TimeSlinger.FireTimer(600f, delegate()
			{
				TarotManager.HermitActive = false;
			}, 0);
		}
		else if (tarot == TAROT_CARDS.THE_HACKER)
		{
			CurrencyManager.AddCurrency(UnityEngine.Random.Range(3.33f, 166.6f));
			GameManager.HackerManager.WhiteHatSound();
		}
		else if (tarot == TAROT_CARDS.THE_DEVIL)
		{
			CurrencyManager.RemoveCurrency(UnityEngine.Random.Range(CurrencyManager.CurrentCurrency / 3f, CurrencyManager.CurrentCurrency / 2f));
			GameManager.HackerManager.BlackHatSound();
		}
		else if (tarot == TAROT_CARDS.THE_CURSED)
		{
			Debug.Log("spawns bomb or doll maker");
		}
		else if (tarot == TAROT_CARDS.THE_GAMBLER)
		{
			Debug.Log("discount me");
		}
		else if (tarot == TAROT_CARDS.THE_DEAD)
		{
			Debug.Log("lucas");
		}
		else if (tarot == TAROT_CARDS.THE_POPULAR)
		{
			Debug.Log("popular");
		}
		Debug.Log(tarot + " missing action");
	}

	private void Update()
	{
		if (TarotManager.HermitActive)
		{
			this.HermitTrap();
		}
	}

	private void HermitTrap()
	{
		if (StateManager.PlayerLocation != PLAYER_LOCATION.MAIN_ROON && StateManager.PlayerLocation != PLAYER_LOCATION.UNKNOWN)
		{
			ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).transform.position = new Vector3(0.10953f, 40.51757f, -1.304061f);
			Debug.Log("teleport back to the room");
		}
	}

	private void TheFool()
	{
		Debug.Log("THE_FOOL");
	}

	public void playTarot(int tarot)
	{
		this.playTarot((TAROT_CARDS)tarot);
	}

	private void Awake()
	{
		TarotManager.Ins = this;
		this.cards = UnityEngine.Object.Instantiate<GameObject>(CustomObjectLookUp.TarotCards);
		this.cards.transform.position = new Vector3(1.603f, 40.68f, 2.489f);
		this.cards.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
		this.cards.transform.Rotate(new Vector3(0f, -20f, 180f));
	}

	public static bool HermitActive = false;

	public static int TimeController = 30;

	public static bool BreatherUndertaker = false;

	public static TarotManager Ins;

	[HideInInspector]
	private GameObject cards;
}
