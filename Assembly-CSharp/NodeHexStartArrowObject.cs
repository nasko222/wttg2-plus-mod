using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class NodeHexStartArrowObject : MonoBehaviour
{
	public void Clear()
	{
		this.hideMeTween.Restart(true, -1f);
	}

	public void Present(RectTransform NewParent)
	{
		this.myRT.SetParent(NewParent);
		this.myRT.anchoredPosition = Vector2.zero;
		this.presentMeTween1.Restart(true, -1f);
		this.presentMeTween2.Restart(true, -1f);
	}

	private void Awake()
	{
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myRT = base.GetComponent<RectTransform>();
		this.presentMeTween1 = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.45f).SetEase(Ease.Linear);
		this.presentMeTween1.Pause<Tweener>();
		this.presentMeTween1.SetAutoKill(false);
		this.presentMeTween2 = DOTween.To(() => this.myRT.anchoredPosition, delegate(Vector2 x)
		{
			this.myRT.anchoredPosition = x;
		}, new Vector2(-35f, 0f), 0.15f).SetEase(Ease.OutSine);
		this.presentMeTween2.Pause<Tweener>();
		this.presentMeTween2.SetAutoKill(false);
		this.hideMeTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.myRT.SetParent(this.DefaultParent);
			this.myRT.anchoredPosition = Vector2.zero;
		});
		this.hideMeTween.Pause<Tweener>();
		this.hideMeTween.SetAutoKill(false);
	}

	public RectTransform DefaultParent;

	private CanvasGroup myCG;

	private RectTransform myRT;

	private Tweener presentMeTween1;

	private Tweener presentMeTween2;

	private Tweener hideMeTween;
}
