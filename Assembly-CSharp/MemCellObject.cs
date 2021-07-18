using System;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class MemCellObject : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event MemCellObject.MemCellActions IWasActivated;

	public void BuildMe(int setIndex, int setMaxIndex)
	{
		this.myIndex = setIndex;
		this.maxIndex = setMaxIndex;
		this.MemInactiveCellIMGCG.alpha = 0.25f;
	}

	public void ActivateInactiveCellIMG()
	{
		this.activateInactiveCellTween.Restart(true, -1f);
	}

	public void ActivateActiveCellIMG()
	{
		this.hideInactiveCellTween.Restart(true, -1f);
		if (this.myIndex != this.maxIndex - 1)
		{
			this.showActiveCellTween1.Restart(true, -1f);
		}
		else
		{
			this.showActiveCellTween2.Restart(true, -1f);
		}
	}

	public void ResetMe()
	{
		this.MemInactiveCellIMGCG.alpha = 1f;
		this.MemActiveCellIMGCG.alpha = 0f;
		this.MemInactiveLineImage.fillAmount = 0f;
	}

	private void ActivateConLine()
	{
		this.activateConLineTween.Restart(true, -1f);
	}

	private void TriggerIWasActivated()
	{
		if (this.IWasActivated != null)
		{
			this.IWasActivated();
		}
	}

	private void Awake()
	{
		this.MemInactiveCellIMGCG = this.MemInactiveCellIMG.GetComponent<CanvasGroup>();
		this.MemActiveCellIMGCG = this.MemActiveCellIMG.GetComponent<CanvasGroup>();
		this.MemInactiveLineImage = this.MemInactiveLineIMG.GetComponent<Image>();
		this.activateInactiveCellTween = DOTween.To(() => this.MemInactiveCellIMGCG.alpha, delegate(float x)
		{
			this.MemInactiveCellIMGCG.alpha = x;
		}, 1f, 0.25f).SetEase(Ease.Linear);
		this.activateInactiveCellTween.Pause<Tweener>();
		this.activateInactiveCellTween.SetAutoKill(false);
		this.hideInactiveCellTween = DOTween.To(() => this.MemInactiveCellIMGCG.alpha, delegate(float x)
		{
			this.MemInactiveCellIMGCG.alpha = x;
		}, 0f, 0.25f).SetEase(Ease.Linear);
		this.hideInactiveCellTween.Pause<Tweener>();
		this.hideInactiveCellTween.SetAutoKill(false);
		this.showActiveCellTween1 = DOTween.To(() => this.MemActiveCellIMGCG.alpha, delegate(float x)
		{
			this.MemActiveCellIMGCG.alpha = x;
		}, 1f, 0.3f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.ActivateConLine));
		this.showActiveCellTween1.Pause<Tweener>();
		this.showActiveCellTween1.SetAutoKill(false);
		this.showActiveCellTween2 = DOTween.To(() => this.MemActiveCellIMGCG.alpha, delegate(float x)
		{
			this.MemActiveCellIMGCG.alpha = x;
		}, 1f, 0.3f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.TriggerIWasActivated));
		this.showActiveCellTween2.Pause<Tweener>();
		this.showActiveCellTween2.SetAutoKill(false);
		this.activateConLineTween = DOTween.To(() => this.MemInactiveLineImage.fillAmount, delegate(float x)
		{
			this.MemInactiveLineImage.fillAmount = x;
		}, 1f, 0.2f).SetEase(Ease.Linear).OnComplete(new TweenCallback(this.TriggerIWasActivated));
		this.activateConLineTween.Pause<Tweener>();
		this.activateConLineTween.SetAutoKill(false);
	}

	public GameObject MemInactiveLineIMG;

	public GameObject MemInactiveCellIMG;

	public GameObject MemActiveLineIMG;

	public GameObject MemActiveCellIMG;

	private const float INACTIVE_CELL_IMG_FADE_TIME = 0.25f;

	private const float ACTIVE_CELL_IMG_FADE_TIME = 0.3f;

	private const float ACTIVE_CON_LINE_FILL_TIME = 0.2f;

	private int myIndex;

	private int maxIndex;

	private CanvasGroup MemInactiveCellIMGCG;

	private CanvasGroup MemActiveCellIMGCG;

	private Image MemInactiveLineImage;

	private Tweener activateInactiveCellTween;

	private Tweener hideInactiveCellTween;

	private Tweener showActiveCellTween1;

	private Tweener showActiveCellTween2;

	private Tweener activateConLineTween;

	public delegate void MemCellActions();
}
