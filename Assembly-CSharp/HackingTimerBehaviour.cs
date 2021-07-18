using System;
using UnityEngine;

public class HackingTimerBehaviour : MonoBehaviour
{
	public HackingTimerObject CurrentHackerTimerObject
	{
		get
		{
			return this.curHackTimer;
		}
	}

	public void FireWarmUpTimer(int setWarmUpTime)
	{
		this.warmUpClockOverlayCG.alpha = 1f;
		this.warmUpNumber.FireMe(this.warmUpNumberToStringLookUp[setWarmUpTime]);
		for (int i = 1; i < setWarmUpTime; i++)
		{
			GameManager.TimeSlinger.FireTimer<string>((float)i, new Action<string>(this.warmUpNumber.FireMe), this.warmUpNumberToStringLookUp[setWarmUpTime - i], 0);
		}
		this.hideWarmUpDelay = (float)setWarmUpTime;
		this.hideWarmUpTimeStamp = Time.time;
		this.hideWarmUpClockActive = true;
	}

	public void FireHackingTimer(float setDur, Action SetCallBack)
	{
		this.timerOverlayCG.alpha = 1f;
		if (this.curHackTimer.Active)
		{
			this.curHackTimer.ResetMe(setDur, SetCallBack);
		}
		else
		{
			this.curHackTimer.FireMe(setDur, SetCallBack);
		}
	}

	public void KillHackerTimer()
	{
		this.timerOverlayCG.alpha = 0f;
		if (this.curHackTimer != null)
		{
			this.curHackTimer.ForceKillMe();
		}
	}

	public void setTimerOverLayInactive()
	{
		this.timerOverlayCG.alpha = 0f;
	}

	private void Awake()
	{
		this.warmUpClockOverlayCG = this.WarmUpClockOverlay.GetComponent<CanvasGroup>();
		this.timerOverlayCG = this.TimerOverLay.GetComponent<CanvasGroup>();
		this.warmUpNumber = UnityEngine.Object.Instantiate<GameObject>(this.WarmUpNumberObject, this.WarmUpClockOverlay.GetComponent<RectTransform>()).GetComponent<WarmUpNumberObject>();
		this.curHackTimer = UnityEngine.Object.Instantiate<GameObject>(this.HackerTimerObject, this.TimerOverLay.GetComponent<RectTransform>()).GetComponent<HackingTimerObject>();
	}

	private void Update()
	{
		if (this.hideWarmUpClockActive && Time.time - this.hideWarmUpTimeStamp >= this.hideWarmUpDelay)
		{
			this.hideWarmUpClockActive = false;
			this.warmUpClockOverlayCG.alpha = 0f;
		}
	}

	public GameObject WarmUpClockOverlay;

	public GameObject WarmUpNumberObject;

	public GameObject TimerOverLay;

	public GameObject HackerTimerObject;

	private CanvasGroup warmUpClockOverlayCG;

	private CanvasGroup timerOverlayCG;

	private WarmUpNumberObject warmUpNumber;

	private HackingTimerObject curHackTimer;

	private bool hideWarmUpClockActive;

	private float hideWarmUpTimeStamp;

	private float hideWarmUpDelay;

	private string[] warmUpNumberToStringLookUp = new string[]
	{
		"0",
		"1",
		"2",
		"3",
		"4",
		"5"
	};
}
