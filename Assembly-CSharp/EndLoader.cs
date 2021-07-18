using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLoader : MonoBehaviour
{
	private IEnumerator loadWorld(int worldID)
	{
		AsyncOperation result = SceneManager.LoadSceneAsync(worldID, LoadSceneMode.Additive);
		while (!result.isDone)
		{
			yield return new WaitForEndOfFrame();
		}
		this.ClearLoading();
		yield break;
	}

	private void ClearLoading()
	{
		this.skullTween.Kill(false);
		DOTween.To(() => this.loadingScreenCG.alpha, delegate(float x)
		{
			this.loadingScreenCG.alpha = x;
		}, 0f, 1f).SetEase(Ease.Linear);
	}

	private void Awake()
	{
		this.skullTween = DOTween.To(() => this.skullCanvasGroup.alpha, delegate(float x)
		{
			this.skullCanvasGroup.alpha = x;
		}, 1f, 0.75f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
		this.skullTween.SetAutoKill(false);
		this.skullTween.Pause<Tweener>();
	}

	private void Start()
	{
		if (!this.debugMode)
		{
			this.skullTween.Restart(true, -1f);
			GameManager.TimeSlinger.FireTimer(4f, delegate()
			{
				base.StartCoroutine(this.loadWorld(this.endingWorldID));
			}, 0);
		}
		else
		{
			this.loadingScreenCG.alpha = 0f;
		}
	}

	[SerializeField]
	private CanvasGroup loadingScreenCG;

	[SerializeField]
	private CanvasGroup skullCanvasGroup;

	[SerializeField]
	private int endingWorldID;

	[SerializeField]
	private bool debugMode;

	private Tweener skullTween;
}
