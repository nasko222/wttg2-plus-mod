using System;
using UnityEngine;

public class Timer
{
	public Timer()
	{
		this.tickAction = new Action(this.Tick);
		this.freezeAction = new Action(this.FreezeTick);
	}

	public Timer(float timerDuration, int setLoopCount = 0)
	{
		this.duration = timerDuration;
		this.startTimeStamp = Time.time;
		this.loopCount = setLoopCount;
		this.tickAction = new Action(this.Tick);
		this.freezeAction = new Action(this.FreezeTick);
	}

	public float TimeLeft
	{
		get
		{
			return this.timeLeft;
		}
	}

	public void Build(float timerDuration, int setLoopCount = 0)
	{
		this.duration = timerDuration;
		this.startTimeStamp = Time.time;
		this.loopCount = setLoopCount;
	}

	public void SetAction(Action setCallBack)
	{
		this.myCallBack = new ActionStorage(setCallBack);
	}

	public void SetAction<A>(Action<A> setCallBack, A ASetValue)
	{
		this.myCallBack = new ActionStorage<A>(setCallBack, ASetValue);
	}

	public void SetAction<A, B>(Action<A, B> setCallBack, A ASetValue, B BSetValue)
	{
		this.myCallBack = new ActionStorage<A, B>(setCallBack, ASetValue, BSetValue);
	}

	public void SetAction<A, B, C>(Action<A, B, C> setCallBack, A ASetValue, B BSetValue, C CSetValue)
	{
		this.myCallBack = new ActionStorage<A, B, C>(setCallBack, ASetValue, BSetValue, CSetValue);
	}

	public void SetAction<A, B, C, D>(Action<A, B, C, D> setCallBack, A ASetValue, B BSetValue, C CSetValue, D DSetValue)
	{
		this.myCallBack = new ActionStorage<A, B, C, D>(setCallBack, ASetValue, BSetValue, CSetValue, DSetValue);
	}

	public void AddLoopCallBack(Action setCallBack)
	{
		this.loopCallBack = new ActionStorage(setCallBack);
	}

	public void AddLoopCallBack<A>(Action<A> setCallBack, A ASetValue)
	{
		this.loopCallBack = new ActionStorage<A>(setCallBack, ASetValue);
	}

	public void AddLoopCallBack<A, B>(Action<A, B> setCallBack, A ASetValue, B BSetValue)
	{
		this.loopCallBack = new ActionStorage<A, B>(setCallBack, ASetValue, BSetValue);
	}

	public void AddLoopCallBack<A, B, C>(Action<A, B, C> setCallBack, A ASetValue, B BSetValue, C CSetValue)
	{
		this.loopCallBack = new ActionStorage<A, B, C>(setCallBack, ASetValue, BSetValue, CSetValue);
	}

	public void AddLoopCallBack<A, B, C, D>(Action<A, B, C, D> setCallBack, A ASetValue, B BSetValue, C CSetValue, D DSetValue)
	{
		this.loopCallBack = new ActionStorage<A, B, C, D>(setCallBack, ASetValue, BSetValue, CSetValue, DSetValue);
	}

	public void Fire()
	{
		this.loopCount--;
		this.freezeTimeStamp = 0f;
		this.wasFrozen = false;
		this.freezeAddTime = 0f;
		GameManager.TimeSlinger.Tick.Event += this.tickAction;
		GameManager.TimeSlinger.FreezeTick.Event += this.freezeAction;
		this.amActive = true;
	}

	public void KillMe()
	{
		GameManager.TimeSlinger.Tick.Event -= this.tickAction;
		GameManager.TimeSlinger.FreezeTick.Event -= this.freezeAction;
		this.myCallBack = null;
		this.loopCallBack = null;
		this.amActive = false;
		this.wasFrozen = false;
		this.loopCount = 0;
		this.freezeTimeStamp = 0f;
		this.freezeAddTime = 0f;
		this.duration = 0f;
		this.startTimeStamp = 0f;
		this.timeLeft = 0f;
	}

	public void Pause()
	{
		this.FreezeTick();
		this.amActive = false;
	}

	public void UnPause()
	{
		this.amActive = true;
	}

	private void TriggerCallBack()
	{
		this.myCallBack.Fire();
		if (this.loopCount > 0)
		{
			this.loopCount--;
			this.freezeTimeStamp = 0f;
			this.freezeAddTime = 0f;
			this.startTimeStamp = Time.time;
			this.amActive = true;
		}
		else
		{
			if (this.loopCallBack != null)
			{
				this.loopCallBack.Fire();
			}
			GameManager.TimeSlinger.KillTimer(this);
		}
	}

	private void Tick()
	{
		if (this.amActive)
		{
			if (this.wasFrozen)
			{
				this.wasFrozen = false;
				this.freezeAddTime += Time.time - this.freezeTimeStamp;
			}
			this.timeLeft = this.duration - (Time.time - this.freezeAddTime - this.startTimeStamp);
			if (Time.time - this.freezeAddTime - this.startTimeStamp >= this.duration)
			{
				this.amActive = false;
				this.TriggerCallBack();
			}
		}
	}

	private void FreezeTick()
	{
		if (this.amActive && !this.wasFrozen)
		{
			this.wasFrozen = true;
			this.freezeTimeStamp = Time.time;
		}
	}

	~Timer()
	{
		this.KillMe();
	}

	public bool HardTimer;

	private ActionSlinger myCallBack;

	private ActionSlinger loopCallBack;

	private bool wasFrozen;

	private bool amActive;

	private int loopCount;

	private float freezeTimeStamp;

	private float freezeAddTime;

	private float duration;

	private float startTimeStamp;

	private float timeLeft;

	private Action tickAction;

	private Action freezeAction;
}
