using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
	private void stageLoading()
	{
		this.myAudioHub.PlaySound(this.loadingMusic);
		DOTween.To(() => this.contentCanvasGroup.alpha, delegate(float x)
		{
			this.contentCanvasGroup.alpha = x;
		}, 1f, 2f).SetDelay(3f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.skullTween.Restart(true, -1f);
			GameManager.WorldManager.StageGame();
		});
	}

	private void gameIsLive()
	{
		GameManager.StageManager.TheGameIsLive -= this.gameIsLive;
		if (!this.debugMode)
		{
			this.myAudioHub.MuffleHub(0f, 1.75f);
			DOTween.To(() => this.contentCanvasGroup.alpha, delegate(float x)
			{
				this.contentCanvasGroup.alpha = x;
			}, 0f, 1f).SetDelay(1f).SetEase(Ease.Linear).OnComplete(delegate
			{
				DOTween.To(() => this.loadingScreenCanvasGroup.alpha, delegate(float x)
				{
					this.loadingScreenCanvasGroup.alpha = x;
				}, 0f, 0.75f).SetEase(Ease.Linear).OnComplete(delegate
				{
					this.skullTween.Kill(false);
					this.loadingScreenGameObject.SetActive(false);
				});
			});
		}
		else
		{
			this.loadingScreenGameObject.SetActive(false);
		}
	}

	private void Awake()
	{
		LoadingScreenManager.Ins = this;
		this.myAudioHub = base.GetComponent<AudioHubObject>();
		this.skullTween = DOTween.To(() => this.skullCanvasGroup.alpha, delegate(float x)
		{
			this.skullCanvasGroup.alpha = x;
		}, 1f, 0.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		this.skullTween.SetAutoKill(false);
		this.skullTween.Pause<Tweener>();
		GameManager.StageManager.TheGameIsLive += this.gameIsLive;
	}

	private void Start()
	{
		if (!this.debugMode)
		{
			this.stageLoading();
		}
	}

	public static LoadingScreenManager Ins;

	[SerializeField]
	private bool debugMode;

	[SerializeField]
	private GameObject loadingScreenGameObject;

	[SerializeField]
	private CanvasGroup loadingScreenCanvasGroup;

	[SerializeField]
	private CanvasGroup contentCanvasGroup;

	[SerializeField]
	private CanvasGroup skullCanvasGroup;

	[SerializeField]
	private CanvasGroup proTipCG;

	[SerializeField]
	private TextMeshProUGUI proTipText;

	[SerializeField]
	private AudioFileDefinition loadingMusic;

	[SerializeField]
	private string[] tips;

	private AudioHubObject myAudioHub;

	private Tweener skullTween;
}
