using System;
using DG.Tweening;
using UnityEngine;

public class SecretManager : MonoBehaviour
{
	public void HypeTextEnd()
	{
		if (DancingLoader.Ins == null)
		{
			this.currentHypeAni.DORewind();
			this.currentHypeAni.gameObject.SetActive(false);
			float duration = UnityEngine.Random.Range(0.5f, 8f);
			GameManager.TimeSlinger.FireTimer(duration, new Action(this.rollHypeText), 0);
		}
	}

	private void rollHypeText()
	{
		if (DancingLoader.Ins == null)
		{
			int num = UnityEngine.Random.Range(0, this.HypeAnis.Length);
			this.currentHypeAni = this.HypeAnis[num];
			this.currentHypeAni.gameObject.SetActive(true);
			this.currentHypeAni.DOPlay();
		}
	}

	private void gameWasPaused()
	{
		this.PauseScreenCG.alpha = 1f;
	}

	private void gameWasUnPaused()
	{
		this.PauseScreenCG.alpha = 0f;
	}

	private void Awake()
	{
		if (DancingLoader.Ins == null)
		{
			GameManager.PauseManager.GamePaused += this.gameWasPaused;
			GameManager.PauseManager.GameUnPaused += this.gameWasUnPaused;
		}
	}

	private void Start()
	{
		if (SecretController.Ins != null && DancingLoader.Ins == null)
		{
			GameManager.TimeSlinger.FireTimer(0.5f, delegate()
			{
				PauseManager.UnLockPause();
				SecretController.Ins.Release();
				GameManager.AudioSlinger.PlaySound(this.SecretMusic);
				this.rollHypeText();
			}, 0);
			GameManager.TimeSlinger.FireTimer(3f, delegate()
			{
				if (SteamSlinger.Ins != null)
				{
					SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.GOD_GAMER);
				}
			}, 0);
		}
	}

	[SerializeField]
	private AudioFileDefinition SecretMusic;

	[SerializeField]
	private DOTweenAnimation[] HypeAnis;

	[SerializeField]
	private CanvasGroup PauseScreenCG;

	private DOTweenAnimation currentHypeAni;
}
