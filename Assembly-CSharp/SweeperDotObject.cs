using System;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class SweeperDotObject : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event SweeperDotObject.UpdateHotSpotActions ActivateHotSpot;

	public void BuildMe(int setIndex, int maxIndex)
	{
		this.myIndex = setIndex;
		this.showMeSeq.Restart(true, (float)setIndex * 0.015f);
		if (setIndex == maxIndex)
		{
			this.fireDelay = (float)setIndex * 0.015f;
			this.fireTimeStamp = Time.time;
			this.fireHotSpot = true;
		}
	}

	public void MakeMeHot()
	{
		this.iAmHot = true;
	}

	public void ActivateMyHotSpot(int setIndex)
	{
		if (this.iAmHot)
		{
			this.showHotSpotSeq.Restart(true, (float)setIndex * 0.1f);
		}
	}

	public bool GetAmHot()
	{
		return this.iAmHot;
	}

	public void Destroy()
	{
		this.myCG.alpha = 0f;
		this.myHotSpotCG.alpha = 0f;
		this.iAmHot = false;
		this.myRT.anchoredPosition = Vector2.zero;
		this.fireHotSpot = false;
	}

	private void Awake()
	{
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myHotSpotCG = this.MyHotSpot.GetComponent<CanvasGroup>();
		this.myRT = base.GetComponent<RectTransform>();
		this.showMeSeq = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.2f).SetEase(Ease.Linear).SetDelay(0.015f);
		this.showMeSeq.Pause<Tweener>();
		this.showMeSeq.SetAutoKill(false);
		this.showHotSpotSeq = DOTween.To(() => this.myHotSpotCG.alpha, delegate(float x)
		{
			this.myHotSpotCG.alpha = x;
		}, 1f, 0.1f).SetEase(Ease.Linear).SetDelay(0.1f);
		this.showHotSpotSeq.Pause<Tweener>();
		this.showHotSpotSeq.SetAutoKill(false);
	}

	private void Update()
	{
		if (this.fireHotSpot && Time.time - this.fireTimeStamp >= this.fireDelay)
		{
			this.fireHotSpot = false;
			if (this.ActivateHotSpot != null)
			{
				this.ActivateHotSpot();
			}
		}
	}

	public GameObject MyHotSpot;

	private const float DELAY_TIME = 0.015f;

	private const float FADE_TIME = 0.2f;

	private const float HOT_SPOT_DELAY_TIME = 0.1f;

	private const float HOT_SPOT_FADE_TIME = 0.1f;

	private bool iAmHot;

	private bool fireHotSpot;

	private int myIndex;

	private float fireTimeStamp;

	private float fireDelay;

	private CanvasGroup myCG;

	private CanvasGroup myHotSpotCG;

	private RectTransform myRT;

	private Tweener showMeSeq;

	private Tweener showHotSpotSeq;

	public delegate void UpdateHotSpotActions();
}
