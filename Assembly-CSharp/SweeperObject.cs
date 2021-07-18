using System;
using System.Collections.Generic;
using System.Diagnostics;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class SweeperObject : MonoBehaviour
{
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event SweeperObject.VoidActions IWasDismissed;

	public void WarmMe(int myIndex, SweeperObjectDefinition SetDef)
	{
		this.myDeff = SetDef;
		if (myIndex == 0)
		{
			this.warmTween.Restart(false, -1f);
			this.BuildMe();
		}
		else
		{
			this.warmTween.Restart(true, (float)myIndex * 0.2f);
			this.buildMeDelay = (float)myIndex * 0.2f;
			this.buildMeTimeStamp = Time.time;
			this.activateBuildMe = true;
		}
	}

	public void BuildMe()
	{
		Vector2 sizeDelta = new Vector2((float)this.myDeff.NumOfDotsPerSweeper * 11f - 4f, base.GetComponent<RectTransform>().sizeDelta.y);
		Vector2 zero = Vector2.zero;
		int num = UnityEngine.Random.Range(this.myDeff.HotZoneDotSizeMin, this.myDeff.HotZoneDotSizeMax);
		int num2 = UnityEngine.Random.Range(0, this.myDeff.NumOfDotsPerSweeper - num);
		this.myRT.sizeDelta = sizeDelta;
		this.dotHolderRT.sizeDelta = sizeDelta;
		this.hotZoneStartIndex = num2;
		this.hotZoneEndIndex = num2 + (num - 1);
		for (int i = 0; i < this.myDeff.NumOfDotsPerSweeper; i++)
		{
			SweeperDotObject sweeperDotObject = this.sweeperDotObjectPool.Pop();
			sweeperDotObject.ActivateHotSpot += this.activateTheHotSpotsAction;
			sweeperDotObject.BuildMe(i, this.myDeff.NumOfDotsPerSweeper - 1);
			if (i >= num2 && i <= num2 + (num - 1))
			{
				sweeperDotObject.MakeMeHot();
			}
			sweeperDotObject.GetComponent<RectTransform>().anchoredPosition = zero;
			zero.x += 11f;
			this.curDotObjects.Add(sweeperDotObject);
		}
		this.hotBarWidth = (float)UnityEngine.Random.Range(this.myDeff.HotBarWidthMin, this.myDeff.HotBarWidthMax);
		this.hotBarRT.sizeDelta = new Vector2(this.hotBarWidth, this.hotBarRT.sizeDelta.y);
		this.hotBarRT.anchoredPosition = Vector2.zero;
		this.curHotBarPOS = Vector2.zero;
		this.hotBarMaxX = this.myRT.sizeDelta.x - this.hotBarWidth;
		this.scrollSpeed = UnityEngine.Random.Range(this.myDeff.ScrollSpeedMin, this.myDeff.ScrollSpeedMax);
	}

	public void FireMe()
	{
		this.hotBarCG.alpha = 1f;
		this.fullShowMeTween.Restart(true, -1f);
		this.scrollIsActive = true;
		this.sweeperHotBarScroll = GameManager.TweenSlinger.PlayDOSTweenLiner(0f, this.hotBarMaxX, this.scrollSpeed, this.updateHotBarPOSAction, this.updateHotBarDirAction);
	}

	public void PlayerHasDecided()
	{
		this.fullShowMeTween.Pause<Tweener>();
		this.scrollIsActive = false;
		GameManager.TweenSlinger.KillTween(this.sweeperHotBarScroll);
		this.sweeperHotBarScroll = null;
		float num = (float)this.hotZoneStartIndex * 11f;
		float num2 = (float)this.hotZoneEndIndex * 11f;
		if (this.hotBarRT.anchoredPosition.x >= num && this.hotBarRT.anchoredPosition.x + this.hotBarRT.sizeDelta.x <= num2)
		{
			this.MyScore = 5;
		}
		else
		{
			int num3;
			if (this.hotBarRT.anchoredPosition.x + this.hotBarRT.sizeDelta.x < num)
			{
				num3 = Mathf.FloorToInt((num - (this.hotBarRT.anchoredPosition.x + this.hotBarRT.sizeDelta.x)) / 11f);
			}
			else
			{
				num3 = Mathf.FloorToInt((this.hotBarRT.anchoredPosition.x + this.hotBarRT.sizeDelta.x - num2) / 11f);
			}
			num3 = ((num3 >= 0) ? num3 : 0);
			if (num3 <= 4)
			{
				this.MyScore = 5 - num3;
			}
			else
			{
				this.MyScore = 0;
			}
		}
		if (this.MyScore == 5)
		{
			GameManager.AudioSlinger.PlaySound(this.Aced);
		}
		else if (this.MyScore < 5 && this.MyScore > 0)
		{
			GameManager.AudioSlinger.PlaySound(this.Scored);
		}
		else if (this.MyScore == 0)
		{
			GameManager.AudioSlinger.PlaySound(this.Failed);
		}
		for (int i = 0; i < this.curDotObjects.Count; i++)
		{
			this.curDotObjects[i].ActivateHotSpot -= this.activateTheHotSpotsAction;
		}
		this.DismissMe();
	}

	public void MoveMeUp()
	{
		DOTween.To(() => this.myRT.anchoredPosition, delegate(Vector2 x)
		{
			this.myRT.anchoredPosition = x;
		}, new Vector2(0f, this.myRT.sizeDelta.y + 10f), 0.15f).SetRelative(true).SetDelay(0.15f);
	}

	public void ForceEnd()
	{
		this.scrollIsActive = false;
		GameManager.TweenSlinger.KillTween(this.sweeperHotBarScroll);
		this.sweeperHotBarScroll = null;
		for (int i = 0; i < this.curDotObjects.Count; i++)
		{
			this.curDotObjects[i].ActivateHotSpot -= this.activateTheHotSpotsAction;
			this.curDotObjects[i].Destroy();
			this.sweeperDotObjectPool.Push(this.curDotObjects[i]);
		}
		this.fadeMeOutTween.Restart(true, -1f);
		this.scaleMeDownTween.Restart(true, -1f);
	}

	public void DestroyMe()
	{
		for (int i = 0; i < this.curDotObjects.Count; i++)
		{
			this.curDotObjects[i].ActivateHotSpot -= this.activateTheHotSpotsAction;
			this.curDotObjects[i].Destroy();
			this.sweeperDotObjectPool.Push(this.curDotObjects[i]);
		}
		this.scrollIsActive = false;
		GameManager.TweenSlinger.KillTween(this.sweeperHotBarScroll);
		this.sweeperHotBarScroll = null;
		this.curDotObjects.Clear();
		this.myRT.anchoredPosition = Vector2.zero;
		this.myRT.localScale = Vector3.one;
		this.myCG.alpha = 0f;
		this.hotBarCG.alpha = 0f;
	}

	private void updateHotBarPOS(float setX)
	{
		if (this.scrollIsActive)
		{
			this.curHotBarPOS.x = setX;
			this.hotBarRT.anchoredPosition = this.curHotBarPOS;
		}
	}

	private void updateHotBarDir(float setX)
	{
		GameManager.TweenSlinger.KillTween(this.sweeperHotBarScroll);
		if (this.scrollIsActive)
		{
			this.updateHotBarPOS(setX);
			if (setX >= this.hotBarMaxX)
			{
				GameManager.AudioSlinger.PlaySound(this.Ping);
				this.sweeperHotBarScroll = GameManager.TweenSlinger.PlayDOSTweenLiner(this.hotBarMaxX, 0f, this.scrollSpeed, this.updateHotBarPOSAction, this.updateHotBarDirAction);
			}
			else if (setX == 0f)
			{
				GameManager.AudioSlinger.PlaySound(this.Pong);
				this.sweeperHotBarScroll = GameManager.TweenSlinger.PlayDOSTweenLiner(0f, this.hotBarMaxX, this.scrollSpeed, this.updateHotBarPOSAction, this.updateHotBarDirAction);
			}
		}
	}

	private void UpdateActiveIndex(int setIndex)
	{
		this.activeIndex = setIndex;
	}

	private void ActivateTheHotSpots()
	{
		int num = 0;
		for (int i = 0; i < this.curDotObjects.Count; i++)
		{
			if (this.curDotObjects[i].GetAmHot())
			{
				this.curDotObjects[i].ActivateMyHotSpot(num);
				num++;
			}
		}
	}

	private void DismissMe()
	{
		this.fadeMeOutTween.Restart(true, -1f);
		if (this.MyScore == 5)
		{
			this.scaleMeUpTween.Restart(true, -1f);
		}
		else
		{
			this.scaleMeDownTween.Restart(true, -1f);
		}
		if (this.IWasDismissed != null)
		{
			this.IWasDismissed();
		}
	}

	private void Awake()
	{
		this.myCG = base.GetComponent<CanvasGroup>();
		this.hotBarCG = this.HotBar.GetComponent<CanvasGroup>();
		this.myRT = base.GetComponent<RectTransform>();
		this.dotHolderRT = this.DotHolder.GetComponent<RectTransform>();
		this.hotBarRT = this.HotBar.GetComponent<RectTransform>();
		this.warmTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0.25f, 0.2f).SetEase(Ease.Linear);
		this.warmTween.Pause<Tweener>();
		this.warmTween.SetAutoKill(false);
		this.fullShowMeTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 1f, 0.2f).SetEase(Ease.InSine);
		this.fullShowMeTween.Pause<Tweener>();
		this.fullShowMeTween.SetAutoKill(false);
		this.fadeMeOutTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.15f).SetEase(Ease.Linear);
		this.fadeMeOutTween.Pause<Tweener>();
		this.fadeMeOutTween.SetAutoKill(false);
		this.scaleMeDownTween = DOTween.To(() => this.myRT.localScale, delegate(Vector3 x)
		{
			this.myRT.localScale = x;
		}, Vector3.zero, 0.15f).SetEase(Ease.OutCirc).OnComplete(delegate
		{
			this.myRT.localScale = Vector3.one;
		});
		this.scaleMeDownTween.Pause<Tweener>();
		this.scaleMeDownTween.SetAutoKill(false);
		this.scaleMeUpTween = DOTween.To(() => this.myRT.localScale, delegate(Vector3 x)
		{
			this.myRT.localScale = x;
		}, new Vector3(1.1f, 1.1f, 1f), 0.15f).SetEase(Ease.OutCirc);
		this.scaleMeUpTween.Pause<Tweener>();
		this.scaleMeUpTween.SetAutoKill(false);
		this.activateTheHotSpotsAction = new SweeperDotObject.UpdateHotSpotActions(this.ActivateTheHotSpots);
		this.updateHotBarPOSAction = new Action<float>(this.updateHotBarPOS);
		this.updateHotBarDirAction = new Action<float>(this.updateHotBarDir);
		this.sweeperDotObjectPool = new PooledStack<SweeperDotObject>(() => UnityEngine.Object.Instantiate<GameObject>(this.DotObject, this.dotHolderRT).GetComponent<SweeperDotObject>(), 30);
	}

	private void Update()
	{
		if (this.activateBuildMe && Time.time - this.buildMeTimeStamp >= this.buildMeDelay)
		{
			this.activateBuildMe = false;
			this.BuildMe();
		}
	}

	public GameObject DotObject;

	public GameObject DotHolder;

	public GameObject HotBar;

	public int MyScore;

	public AudioFileDefinition Ping;

	public AudioFileDefinition Pong;

	public AudioFileDefinition Aced;

	public AudioFileDefinition Scored;

	public AudioFileDefinition Failed;

	private const float SLIDER_SPACING = 10f;

	private const float FADE_DELAY_TIME = 0.2f;

	private const float DOT_WIDTH_SIZE = 11f;

	private const float DOT_SPACING_BUFFER = 4f;

	private const float DISMISS_TIME = 0.15f;

	private SweeperObjectDefinition myDeff;

	private PooledStack<SweeperDotObject> sweeperDotObjectPool;

	private List<SweeperDotObject> curDotObjects = new List<SweeperDotObject>(30);

	private CanvasGroup myCG;

	private CanvasGroup hotBarCG;

	private RectTransform myRT;

	private RectTransform dotHolderRT;

	private RectTransform hotBarRT;

	private Vector2 curHotBarPOS = Vector2.zero;

	private bool scrollIsActive;

	private bool activateBuildMe;

	private int hotZoneStartIndex;

	private int hotZoneEndIndex;

	private int activeIndex;

	private float hotBarWidth;

	private float hotBarMaxX;

	private float scrollSpeed;

	private float buildMeDelay;

	private float buildMeTimeStamp;

	private DOSTween sweeperHotBarScroll;

	private Tweener warmTween;

	private Tweener fullShowMeTween;

	private Tweener fadeMeOutTween;

	private Tweener scaleMeDownTween;

	private Tweener scaleMeUpTween;

	private Tweener moveMeUpTween;

	private SweeperDotObject.UpdateHotSpotActions activateTheHotSpotsAction;

	private Action<float> updateHotBarPOSAction;

	private Action<float> updateHotBarDirAction;

	public delegate void VoidActions();
}
