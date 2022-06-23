using System;
using DG.Tweening;
using UnityEngine;

public class TarotCardPullAnim : MonoBehaviour
{
	private void Awake()
	{
		TarotCardPullAnim.Ins = this;
		this.GlowMat = UnityEngine.Object.Instantiate<Material>(this.GlowMat);
		this.DisappearSFX = CustomSoundLookUp.disappear;
		this.TheFoolSFX = CustomSoundLookUp.fool;
		this.PullSFX = CustomSoundLookUp.pull;
		TarotCardPullAnim.currentCard = 0;
		TarotCardPullAnim.currentCardTex = -1;
		TarotCardPullAnim.foolTimer = 1f;
	}

	private void TheFool()
	{
		int num = UnityEngine.Random.Range(0, 3);
		if (num == 0)
		{
			this.TheFoolSFX = CustomSoundLookUp.fool;
		}
		else if (num == 1)
		{
			this.TheFoolSFX = CustomSoundLookUp.fool2;
		}
		else if (num == 2)
		{
			this.TheFoolSFX = CustomSoundLookUp.fool3;
		}
		TarotCardsBehaviour.Ins.myAudioHub.PlaySoundWithWildPitch(this.TheFoolSFX, 0.5f, 1.5f);
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material = this.GlowMat;
		this.ChangeTexBF();
		for (float num2 = 0f; num2 < 0.3f; num2 += 0.01f)
		{
			GameManager.TimeSlinger.FireTimer(num2, new Action(this.GlowIn), 0);
		}
		GameManager.TimeSlinger.FireTimer(0.16f, new Action(this.ChangeTexF), 0);
		for (float num3 = 0.3f; num3 < 0.62f; num3 += 0.01f)
		{
			GameManager.TimeSlinger.FireTimer(num3, new Action(this.GlowOut), 0);
		}
		GameManager.TimeSlinger.FireTimer(0.48f, new Action(this.ChangeTexAF), 0);
		TarotCardPullAnim.foolTimer = 1f;
	}

	private void GlowIn()
	{
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0f, 0.4f, 0f, 1f) * TarotCardPullAnim.foolTimer / 10f);
		TarotCardPullAnim.foolTimer += 1f;
	}

	private void GlowOut()
	{
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material.SetColor("_EmissionColor", new Color(0f, 0.4f, 0f, 1f) * TarotCardPullAnim.foolTimer / 10f);
		TarotCardPullAnim.foolTimer -= 1f;
	}

	public void DoPull()
	{
		TarotCardsBehaviour.Ins.myAudioHub.PlaySound(this.PullSFX);
		GameManager.TimeSlinger.FireTimer(0.16f, new Action(this.ChangeTex), 0);
		int num = UnityEngine.Random.Range(0, 103);
		if (num < 50)
		{
			TarotCardPullAnim.currentCardTex = UnityEngine.Random.Range(0, 7);
		}
		else if (num >= 50 && num < 85)
		{
			TarotCardPullAnim.currentCardTex = UnityEngine.Random.Range(7, 14);
		}
		else if (num >= 85 && num < 100)
		{
			TarotCardPullAnim.currentCardTex = UnityEngine.Random.Range(14, 20);
		}
		else
		{
			TarotCardPullAnim.currentCardTex = UnityEngine.Random.Range(20, 22);
		}
		this.cards[TarotCardPullAnim.currentCard].transform.DOLocalMoveX(-0.6f, 0.4f, false);
		this.cards[TarotCardPullAnim.currentCard].transform.DOLocalMoveZ(-0.3f, 0.4f, false);
		this.cards[TarotCardPullAnim.currentCard].transform.DOLocalMoveY(-0.4f, 0.2f, false).OnComplete(delegate
		{
			this.cards[TarotCardPullAnim.currentCard].transform.DOLocalMoveY(0.4f, 0.4f, false);
			this.cards[TarotCardPullAnim.currentCard].transform.DOLocalMoveX(-0.8f, 0.4f, false);
		});
		this.cards[TarotCardPullAnim.currentCard].transform.DOLocalRotate(new Vector3(0f, -20f, -180f), 0.4f, RotateMode.Fast);
		if (UnityEngine.Random.Range(0, 100) < 20)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (DevTools.Ins != null && DevTools.Ins.alwaysFool)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (TarotCardPullAnim.currentCardTex == 2 && TarotManager.TimeController == 60)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (TarotCardPullAnim.currentCardTex == 3 && TarotManager.TimeController == 5)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (TarotCardPullAnim.currentCardTex == 8 && (StateManager.BeingHacked || !ComputerPowerHook.Ins.PowerOn || EnvironmentManager.PowerState == POWER_STATE.OFF || EnemyManager.State == ENEMY_STATE.BREATHER || EnemyManager.State == ENEMY_STATE.CULT || EnemyManager.State == ENEMY_STATE.DOLL_MAKER || EnemyManager.State == ENEMY_STATE.HITMAN || EnemyManager.State == ENEMY_STATE.POILCE || EnemyManager.State == ENEMY_STATE.BOMB_MAKER))
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (TarotCardPullAnim.currentCardTex == 14 && EnemyManager.State != ENEMY_STATE.IDLE)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (TarotCardPullAnim.currentCardTex == 19 && (EnvironmentManager.PowerBehaviour.LockedOut || StateManager.BeingHacked || EnvironmentManager.PowerState == POWER_STATE.OFF || TarotManager.HermitActive))
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (TarotCardPullAnim.currentCardTex == 16 && EnvironmentManager.PowerState == POWER_STATE.OFF)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (TarotCardPullAnim.currentCardTex == 11 && TarotManager.CurSpeed == playerSpeedMode.QUICK)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (TarotCardPullAnim.currentCardTex == 12 && TarotManager.CurSpeed == playerSpeedMode.WEAK)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (TarotCardPullAnim.currentCardTex == 15 && (DataManager.LeetMode || ModsManager.Nightmare))
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else if (TarotCardPullAnim.currentCardTex == 7 && CurrencyManager.CurrentCurrency <= 0f)
		{
			GameManager.TimeSlinger.FireTimer(1f, new Action(this.TheFool), 0);
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(1.25f, new Action(TarotManager.Ins.PullCardAtLoc), 0);
		}
		GameManager.TimeSlinger.FireTimer(1.75f, new Action(this.Popoff), 0);
	}

	private void ChangeTex()
	{
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material.SetTexture("_MainTex", this.cardsTex[TarotCardPullAnim.currentCardTex]);
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material.SetTexture("_EmissionMap", this.cardsTex[TarotCardPullAnim.currentCardTex]);
	}

	private void ChangeTexBF()
	{
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material.SetTexture("_MainTex", this.cardsTex[TarotCardPullAnim.currentCardTex]);
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material.SetTexture("_EmissionMap", null);
	}

	private void ChangeTexF()
	{
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material.SetTexture("_MainTex", this.theFool);
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material.SetTexture("_EmissionMap", null);
	}

	private void ChangeTexAF()
	{
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material.SetTexture("_MainTex", this.theFool);
		this.cards[TarotCardPullAnim.currentCard].GetComponent<Renderer>().material.SetTexture("_EmissionMap", this.theFool);
	}

	private void Popoff()
	{
		TarotCardsBehaviour.Ins.myAudioHub.PlaySound(this.DisappearSFX);
		this.cards[TarotCardPullAnim.currentCard].transform.DOScale(new Vector3(0f, 0f, 0f), 0.2f);
		TarotCardPullAnim.currentCard++;
		TarotCardPullAnim.currentCardTex = -1;
	}

	public GameObject[] cards;

	public static int currentCard = 0;

	public static int currentCardTex = -1;

	public Texture2D[] cardsTex;

	public Texture2D theFool;

	public Material GlowMat;

	public static float foolTimer = 1f;

	[HideInInspector]
	public AudioFileDefinition TheFoolSFX;

	[HideInInspector]
	public AudioFileDefinition PullSFX;

	[HideInInspector]
	public AudioFileDefinition DisappearSFX;

	public static TarotCardPullAnim Ins;
}
