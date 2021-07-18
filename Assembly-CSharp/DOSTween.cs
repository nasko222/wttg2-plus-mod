using System;
using UnityEngine;

public class DOSTween
{
	public DOSTween(tweenSlinger curTweenSlinger)
	{
		this.myTweenSlinger = curTweenSlinger;
		curTweenSlinger.Tick.Event += this.Tick;
		curTweenSlinger.FrozeTick.Event += this.FreezeTick;
	}

	~DOSTween()
	{
		if (!this.HardTween)
		{
			this.myTweenSlinger.Tick.Event -= this.Tick;
			this.myTweenSlinger.FrozeTick.Event -= this.FreezeTick;
		}
	}

	public void Build(float setFromValue, float setToValue, float setDuration, Action<float> setCallBackAction, bool setIgnoreFreezeTime = false)
	{
		this.fromValue = setFromValue;
		this.toValue = setToValue;
		this.tweenDuration = setDuration;
		this.ignoreFreezeTime = setIgnoreFreezeTime;
		this.active = true;
		this.callBackAction = setCallBackAction;
		this.startTimeStamp = Time.time;
		this.linerTweenActive = true;
	}

	public void Build(float setFromValue, float setToValue, float setDuration, Action<float> setCallBackAction, Action<float> setDoneCallBackAction, bool setIgnoreFreezeTime = false)
	{
		this.Build(setFromValue, setToValue, setDuration, setCallBackAction, setIgnoreFreezeTime);
		this.doneCallBackAction = setDoneCallBackAction;
	}

	public void Build(float setFromValue, float setToValue, float setDelayPerUnit, float setMaxDuration, Action<float> setCallBackAction, bool setIgnoreFreezeTime = false)
	{
		this.fromValue = setFromValue;
		this.toValue = setToValue;
		this.delayPerUnit = setDelayPerUnit;
		this.maxTweenDuration = setMaxDuration;
		this.ignoreFreezeTime = setIgnoreFreezeTime;
		this.tweenDuration = Mathf.Min(Mathf.Abs(this.toValue - this.fromValue) * this.delayPerUnit, this.maxTweenDuration);
		this.startTimeStamp = (this.stepTimeStamp = Time.time);
		if (this.tempValue == this.toValue)
		{
			this.tempValue = this.fromValue;
		}
		this.active = true;
		this.callBackAction = setCallBackAction;
		this.stepTweenActive = true;
	}

	public void Build(float setFromValue, float setToValue, float setDelayPerUnit, float setMaxDuration, Action<float> setCallBackAction, Action<float> setDoneCallBackAction, bool setIgnoreFreezeTime = false)
	{
		this.Build(setFromValue, setToValue, setDelayPerUnit, setMaxDuration, setCallBackAction, setIgnoreFreezeTime);
		this.doneCallBackAction = setDoneCallBackAction;
	}

	public void Pause()
	{
		this.FreezeTick();
		this.active = false;
	}

	public void UnPause()
	{
		this.active = true;
	}

	public void killMe()
	{
		this.startTimeStamp = 0f;
		this.stepTimeStamp = 0f;
		this.toValue = 0f;
		this.fromValue = 0f;
		this.tempValue = 0f;
		this.delayPerUnit = 0f;
		this.tweenDuration = 0f;
		this.maxTweenDuration = 0f;
		this.freezeTimeStamp = 0f;
		this.freeAddTime = 0f;
		this.callBackAction = null;
		this.doneCallBackAction = null;
		this.linerTweenActive = false;
		this.stepTweenActive = false;
		this.ignoreFreezeTime = false;
		this.wasFrozen = false;
		this.active = false;
		if (this.HardTween)
		{
			this.myTweenSlinger.Tick.Event -= this.Tick;
			this.myTweenSlinger.FrozeTick.Event -= this.FreezeTick;
		}
	}

	public void Tick()
	{
		if (this.active)
		{
			if (this.wasFrozen)
			{
				this.wasFrozen = false;
				this.freeAddTime += Time.time - this.freezeTimeStamp;
			}
			if (this.linerTweenActive)
			{
				if (Time.time - this.freeAddTime - this.startTimeStamp < this.tweenDuration)
				{
					this.tempValue = Mathf.Lerp(this.fromValue, this.toValue, (Time.time - this.freeAddTime - this.startTimeStamp) / this.tweenDuration);
					this.callBackAction(this.tempValue);
				}
				else
				{
					this.tempValue = this.toValue;
					this.callBackAction(this.tempValue);
					if (this.doneCallBackAction != null)
					{
						this.doneCallBackAction(this.tempValue);
					}
					else
					{
						this.myTweenSlinger.KillTween(this);
					}
				}
			}
			else if (this.stepTweenActive)
			{
				if (Time.time - this.freeAddTime - this.stepTimeStamp >= this.delayPerUnit && this.tempValue != this.toValue)
				{
					this.stepTimeStamp = Time.time;
					this.tempValue = this.fromValue + Mathf.Lerp(0f, this.toValue - this.fromValue, (Time.time - this.freeAddTime - this.startTimeStamp) / this.tweenDuration);
					this.callBackAction(this.tempValue);
				}
				else if (this.tempValue == this.toValue)
				{
					if (this.doneCallBackAction != null)
					{
						this.doneCallBackAction(this.tempValue);
					}
					else
					{
						this.myTweenSlinger.KillTween(this);
					}
				}
			}
		}
	}

	public void FreezeTick()
	{
		if (this.active)
		{
			if (!this.ignoreFreezeTime)
			{
				if (!this.wasFrozen)
				{
					this.wasFrozen = true;
					this.freezeTimeStamp = Time.time;
				}
			}
			else
			{
				this.Tick();
			}
		}
	}

	public bool HardTween;

	private Action<float> callBackAction;

	private Action<float> doneCallBackAction;

	private tweenSlinger myTweenSlinger;

	private float startTimeStamp;

	private float stepTimeStamp;

	private float toValue;

	private float fromValue;

	private float tempValue;

	private float delayPerUnit;

	private float tweenDuration;

	private float maxTweenDuration;

	private float freezeTimeStamp;

	private float freeAddTime;

	private bool linerTweenActive;

	private bool stepTweenActive;

	private bool ignoreFreezeTime;

	private bool wasFrozen;

	private bool active;
}
