using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class StackPusherGridPoperObject : MonoBehaviour
{
	public void Clear()
	{
		this.clearTween.Restart(true, -1f);
	}

	public void SetMyParent(RectTransform ParentRectTrans)
	{
		this.myRT.SetParent(ParentRectTrans);
		this.myRT.anchoredPosition = this.centerPOS;
	}

	public void PresentActive()
	{
		this.presentSelf.Restart(true, -1f);
		this.SetActive();
	}

	public void PresentInactive()
	{
		this.presentSelf.Restart(true, -1f);
		this.SetInactive();
	}

	public void SetActive()
	{
		if (this.arrowCG.alpha != 1f)
		{
			this.setActiveTween.Restart(true, -1f);
		}
	}

	public void SetInactive()
	{
		this.setInActiveTween.Restart(true, -1f);
	}

	public void PopMouseEnter()
	{
		this.mouseEnterTween.Restart(true, -1f);
	}

	public void PopMouseExit()
	{
		this.mouseExitTween.Restart(true, -1f);
	}

	private void Awake()
	{
		this.myRT = base.GetComponent<RectTransform>();
		this.myCG = base.GetComponent<CanvasGroup>();
		this.arrowCG = this.PopArrowIMG.GetComponent<CanvasGroup>();
		this.hoverCG = this.PopHoverIMG.GetComponent<CanvasGroup>();
		this.defaultParent = (RectTransform)this.myRT.parent;
		this.presentSelf = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear);
		this.presentSelf.Pause<Tweener>();
		this.presentSelf.SetAutoKill(false);
		this.setActiveTween = DOTween.To(() => this.arrowCG.alpha, delegate(float x)
		{
			this.arrowCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear);
		this.setActiveTween.Pause<Tweener>();
		this.setActiveTween.SetAutoKill(false);
		this.setInActiveTween = DOTween.To(() => this.arrowCG.alpha, delegate(float x)
		{
			this.arrowCG.alpha = x;
		}, 0.1f, 0.15f).SetEase(Ease.Linear);
		this.setInActiveTween.Pause<Tweener>();
		this.setInActiveTween.SetAutoKill(false);
		this.mouseEnterTween = DOTween.To(() => this.hoverCG.alpha, delegate(float x)
		{
			this.hoverCG.alpha = x;
		}, 1f, 0.15f).SetEase(Ease.Linear);
		this.mouseEnterTween.Pause<Tweener>();
		this.mouseEnterTween.SetAutoKill(false);
		this.mouseExitTween = DOTween.To(() => this.hoverCG.alpha, delegate(float x)
		{
			this.hoverCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear);
		this.mouseExitTween.Pause<Tweener>();
		this.mouseExitTween.SetAutoKill(false);
		this.clearTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.arrowCG.alpha = 0f;
			this.hoverCG.alpha = 0f;
			this.SetMyParent(this.defaultParent);
		});
		this.clearTween.Pause<Tweener>();
		this.clearTween.SetAutoKill(false);
	}

	public GameObject DefaultBGIMG;

	public GameObject PopArrowIMG;

	public GameObject PopHoverIMG;

	private RectTransform myRT;

	private RectTransform defaultParent;

	private CanvasGroup myCG;

	private CanvasGroup arrowCG;

	private CanvasGroup hoverCG;

	private Vector2 centerPOS = Vector2.zero;

	private Tweener presentSelf;

	private Tweener setActiveTween;

	private Tweener setInActiveTween;

	private Tweener mouseEnterTween;

	private Tweener mouseExitTween;

	private Tweener clearTween;
}
