using System;
using System.Collections.Generic;

public class timeSlinger
{
	public timeSlinger(bool FromGameManager = true)
	{
		if (FromGameManager)
		{
			GameManager.PauseManager.GamePaused += this.playerHasPaused;
			GameManager.PauseManager.GameUnPaused += this.playerHasUnPaused;
			this.isFromGameManager = false;
		}
		this.timerPool = new PooledStack<Timer>(() => new Timer(), 10);
	}

	~timeSlinger()
	{
		if (this.isFromGameManager)
		{
			GameManager.PauseManager.GamePaused -= this.playerHasPaused;
			GameManager.PauseManager.GameUnPaused -= this.playerHasUnPaused;
		}
	}

	public void FireTimer(float duration, Action callBack, int loopCount = 0)
	{
		Timer timer = this.timerPool.Pop();
		timer.Build(duration, loopCount);
		timer.SetAction(callBack);
		timer.Fire();
		this.timers.Add(timer);
	}

	public void FireHardTimer(out Timer returnTimer, float duration, Action callBack, int loopCount = 0)
	{
		returnTimer = new Timer();
		returnTimer.HardTimer = true;
		returnTimer.Build(duration, loopCount);
		returnTimer.SetAction(callBack);
		returnTimer.Fire();
		this.timers.Add(returnTimer);
	}

	public void FireTimer<A>(float duration, Action<A> callBack, A callBackValue, int loopCount = 0)
	{
		Timer timer = this.timerPool.Pop();
		timer.Build(duration, loopCount);
		timer.SetAction<A>(callBack, callBackValue);
		timer.Fire();
		this.timers.Add(timer);
	}

	public void FireHardTimer<A>(out Timer returnTimer, float duration, Action<A> callBack, A callBackValue, int loopCount = 0)
	{
		returnTimer = new Timer();
		returnTimer.HardTimer = true;
		returnTimer.Build(duration, loopCount);
		returnTimer.SetAction<A>(callBack, callBackValue);
		returnTimer.Fire();
		this.timers.Add(returnTimer);
	}

	public void FireTimer<A, B>(float duration, Action<A, B> callBack, A aCallBackValue, B bCallBackValue, int loopCount = 0)
	{
		Timer timer = this.timerPool.Pop();
		timer.Build(duration, loopCount);
		timer.SetAction<A, B>(callBack, aCallBackValue, bCallBackValue);
		timer.Fire();
		this.timers.Add(timer);
	}

	public void FireHardTimer<A, B>(out Timer returnTimer, float duration, Action<A, B> callBack, A aCallBackValue, B bCallBackValue, int loopCount = 0)
	{
		returnTimer = new Timer();
		returnTimer.HardTimer = true;
		returnTimer.Build(duration, loopCount);
		returnTimer.SetAction<A, B>(callBack, aCallBackValue, bCallBackValue);
		returnTimer.Fire();
		this.timers.Add(returnTimer);
	}

	public void FireTimer<A, B, C>(float duration, Action<A, B, C> callBack, A aCallBackValue, B bCallBackValue, C cCallBackValue, int loopCount = 0)
	{
		Timer timer = this.timerPool.Pop();
		timer.Build(duration, loopCount);
		timer.SetAction<A, B, C>(callBack, aCallBackValue, bCallBackValue, cCallBackValue);
		timer.Fire();
		this.timers.Add(timer);
	}

	public void FireHardTimer<A, B, C>(out Timer returnTimer, float duration, Action<A, B, C> callBack, A aCallBackValue, B bCallBackValue, C cCallBackValue, int loopCount = 0)
	{
		returnTimer = new Timer();
		returnTimer.HardTimer = true;
		returnTimer.Build(duration, loopCount);
		returnTimer.SetAction<A, B, C>(callBack, aCallBackValue, bCallBackValue, cCallBackValue);
		returnTimer.Fire();
		this.timers.Add(returnTimer);
	}

	public void FireTimer<A, B, C, D>(float duration, Action<A, B, C, D> callBack, A aCallBackValue, B bCallBackValue, C cCallBackValue, D dCallBackValue, int loopCount = 0)
	{
		Timer timer = this.timerPool.Pop();
		timer.Build(duration, loopCount);
		timer.SetAction<A, B, C, D>(callBack, aCallBackValue, bCallBackValue, cCallBackValue, dCallBackValue);
		timer.Fire();
		this.timers.Add(timer);
	}

	public void FireHardTimer<A, B, C, D>(out Timer returnTimer, float duration, Action<A, B, C, D> callBack, A aCallBackValue, B bCallBackValue, C cCallBackValue, D dCallBackValue, int loopCount = 0)
	{
		returnTimer = new Timer();
		returnTimer.HardTimer = true;
		returnTimer.Build(duration, loopCount);
		returnTimer.SetAction<A, B, C, D>(callBack, aCallBackValue, bCallBackValue, cCallBackValue, dCallBackValue);
		returnTimer.Fire();
		this.timers.Add(returnTimer);
	}

	public void KillTimer(Timer timerToKill)
	{
		if (timerToKill != null)
		{
			timerToKill.KillMe();
			this.timers.Remove(timerToKill);
			if (!timerToKill.HardTimer)
			{
				this.timerPool.Push(timerToKill);
			}
			timerToKill = null;
		}
	}

	public void Update()
	{
		if (!this.freezeTime)
		{
			this.Tick.Execute();
		}
		else
		{
			this.FreezeTick.Execute();
		}
	}

	private void playerHasPaused()
	{
		this.freezeTime = true;
	}

	private void playerHasUnPaused()
	{
		this.freezeTime = false;
	}

	public CustomEvent Tick = new CustomEvent(10);

	public CustomEvent FreezeTick = new CustomEvent(10);

	private PooledStack<Timer> timerPool;

	private HashSet<Timer> timers = new HashSet<Timer>();

	private bool freezeTime;

	private bool isFromGameManager;
}
