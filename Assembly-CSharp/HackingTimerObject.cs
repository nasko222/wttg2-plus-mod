using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class HackingTimerObject : MonoBehaviour
{
	public Timer HackerTimeTimer
	{
		get
		{
			return this.hackerTimeTimer;
		}
	}

	public void FireMe(float setDur, Action SetCallBack)
	{
		this.myCG.alpha = 1f;
		this.Active = true;
		this.myCallBack = SetCallBack;
		GameManager.AudioSlinger.PlaySound(this.PresentSFX);
		this.myRT.anchoredPosition = new Vector2(0f, -(base.GetComponent<RectTransform>().sizeDelta.y / 2f + 5f));
		this.FillIMG.fillAmount = 1f;
		this.hackerTimerTween = GameManager.TweenSlinger.PlayDOSTweenLiner(1f, 0f, setDur, this.updateFillAction);
		GameManager.TimeSlinger.FireHardTimer(out this.hackerTimeTimer, setDur, new Action(this.timesUp), 0);
		float duration = Mathf.Max(setDur * 0.65f, setDur - 7.5f);
		GameManager.TimeSlinger.FireHardTimer(out this.panicTickTimer, duration, new Action(this.ActivateTick), 0);
	}

	public void ResetMe(float SetDur, Action SetCallBack)
	{
		this.tickActive = false;
		GameManager.TweenSlinger.KillTween(this.hackerTimerTween);
		GameManager.TimeSlinger.KillTimer(this.hackerTimeTimer);
		GameManager.TimeSlinger.KillTimer(this.panicTickTimer);
		this.FillIMG.fillAmount = 1f;
		this.hackerTimerTween = GameManager.TweenSlinger.PlayDOSTweenLiner(1f, 0f, SetDur, this.updateFillAction);
		GameManager.TimeSlinger.FireHardTimer(out this.hackerTimeTimer, SetDur, new Action(this.timesUp), 0);
		float duration = Mathf.Max(SetDur * 0.65f, SetDur - 7.5f);
		GameManager.TimeSlinger.FireHardTimer(out this.panicTickTimer, duration, new Action(this.ActivateTick), 0);
	}

	public void ForceKillMe()
	{
		this.Active = false;
		this.tickActive = false;
		this.isPaused = false;
		GameManager.TweenSlinger.KillTween(this.hackerTimerTween);
		GameManager.TimeSlinger.KillTimer(this.hackerTimeTimer);
		GameManager.TimeSlinger.KillTimer(this.panicTickTimer);
		this.hackerTimerTween = null;
		this.hackerTimeTimer = null;
		this.panicTickTimer = null;
		this.killMeTween.Restart(true, -1f);
	}

	public void Pause()
	{
		if (this.hackerTimeTimer != null)
		{
			this.isPaused = true;
			this.hackerTimerTween.Pause();
			this.hackerTimeTimer.Pause();
			this.panicTickTimer.Pause();
		}
	}

	public void UnPause()
	{
		if (this.hackerTimeTimer != null)
		{
			this.isPaused = false;
			this.hackerTimeTimer.UnPause();
			this.hackerTimerTween.UnPause();
			this.panicTickTimer.UnPause();
		}
	}

	public float GetTimeLeft()
	{
		return this.hackerTimeTimer.TimeLeft;
	}

	private void timesUp()
	{
		if (this.myCallBack != null)
		{
			this.myCallBack();
		}
		this.ForceKillMe();
	}

	private void ActivateTick()
	{
		this.tickTimeStamp = Time.time;
		this.tickActive = true;
	}

	private void updateFill(float setValue)
	{
		this.FillIMG.fillAmount = setValue;
	}

	private void Awake()
	{
		this.myCG = base.GetComponent<CanvasGroup>();
		this.myRT = base.GetComponent<RectTransform>();
		this.killMeTween = DOTween.To(() => this.myCG.alpha, delegate(float x)
		{
			this.myCG.alpha = x;
		}, 0f, 0.25f).OnComplete(delegate
		{
			GameManager.HackerManager.HackingTimer.setTimerOverLayInactive();
		});
		this.killMeTween.Pause<Tweener>();
		this.killMeTween.SetAutoKill(false);
		this.updateFillAction = new Action<float>(this.updateFill);
	}

	private void Update()
	{
		if (this.tickActive && !this.isPaused && Time.time - this.tickTimeStamp >= 0.1f)
		{
			this.tickTimeStamp = Time.time;
			GameManager.AudioSlinger.PlaySound(this.TickSFX);
		}
	}

	public Image FillIMG;

	public bool Active;

	public AudioFileDefinition PresentSFX;

	public AudioFileDefinition TickSFX;

	private DOSTween hackerTimerTween;

	private Timer hackerTimeTimer;

	private Timer panicTickTimer;

	private Action myCallBack;

	private Action<float> updateFillAction;

	private CanvasGroup myCG;

	private RectTransform myRT;

	private bool tickActive;

	private bool isPaused;

	private float tickTimeStamp;

	private Tweener killMeTween;
}
