using System;
using System.Collections.Generic;

public class tweenSlinger
{
	public tweenSlinger()
	{
		GameManager.PauseManager.GamePaused += this.playerHasPaused;
		GameManager.PauseManager.GameUnPaused += this.playerHasUnPaused;
		this.tweenPool = new PooledStack<DOSTween>(() => new DOSTween(this), 5);
	}

	~tweenSlinger()
	{
		GameManager.PauseManager.GamePaused -= this.playerHasPaused;
		GameManager.PauseManager.GameUnPaused -= this.playerHasUnPaused;
	}

	public void FireDOSTweenLiner(float setFromValue, float setToValue, float setDuration, Action<float> setCallBackAction)
	{
		DOSTween dostween = this.tweenPool.Pop();
		dostween.Build(setFromValue, setToValue, setDuration, setCallBackAction, false);
		this.tweens.Add(dostween);
	}

	public void FireDOSTweenLiner(float setFromValue, float setToValue, float setDuration, Action<float> setCallBackAction, bool ignoreFreezeTime = false)
	{
		DOSTween dostween = this.tweenPool.Pop();
		dostween.Build(setFromValue, setToValue, setDuration, setCallBackAction, ignoreFreezeTime);
		this.tweens.Add(dostween);
	}

	public void FireDOSTweenLiner(float setFromValue, float setToValue, float setDuration, Action<float> setCallBackAction, Action<float> setDoneCallBackAction)
	{
		DOSTween dostween = this.tweenPool.Pop();
		dostween.Build(setFromValue, setToValue, setDuration, setCallBackAction, setDoneCallBackAction, false);
		this.tweens.Add(dostween);
	}

	public void FireDOSTweenLiner(float setFromValue, float setToValue, float setDuration, Action<float> setCallBackAction, Action<float> setDoneCallBackAction, bool ignoreFreezeTime = false)
	{
		DOSTween dostween = this.tweenPool.Pop();
		dostween.Build(setFromValue, setToValue, setDuration, setCallBackAction, setDoneCallBackAction, ignoreFreezeTime);
		this.tweens.Add(dostween);
	}

	public DOSTween PlayDOSTweenLiner(float setFromValue, float setToValue, float setDuration, Action<float> setCallBackAction)
	{
		DOSTween dostween = new DOSTween(this);
		dostween.HardTween = true;
		dostween.Build(setFromValue, setToValue, setDuration, setCallBackAction, false);
		this.tweens.Add(dostween);
		return dostween;
	}

	public DOSTween PlayDOSTweenLiner(float setFromValue, float setToValue, float setDuration, Action<float> setCallBackAction, bool ignoreFreezeTime = false)
	{
		DOSTween dostween = new DOSTween(this);
		dostween.HardTween = true;
		dostween.Build(setFromValue, setToValue, setDuration, setCallBackAction, ignoreFreezeTime);
		this.tweens.Add(dostween);
		return dostween;
	}

	public DOSTween PlayDOSTweenLiner(float setFromValue, float setToValue, float setDuration, Action<float> setCallBackAction, Action<float> setDoneCallBackAction)
	{
		DOSTween dostween = new DOSTween(this);
		dostween.HardTween = true;
		dostween.Build(setFromValue, setToValue, setDuration, setCallBackAction, setDoneCallBackAction, false);
		this.tweens.Add(dostween);
		return dostween;
	}

	public DOSTween PlayDOSTweenLiner(float setFromValue, float setToValue, float setDuration, Action<float> setCallBackAction, Action<float> setDoneCallBackAction, bool ignoreFreezeTime = false)
	{
		DOSTween dostween = new DOSTween(this);
		dostween.HardTween = true;
		dostween.Build(setFromValue, setToValue, setDuration, setCallBackAction, setDoneCallBackAction, ignoreFreezeTime);
		this.tweens.Add(dostween);
		return dostween;
	}

	public void FireDOSTweenStep(float setFromValue, float setToValue, float setDelayPerUnit, float setMaxDuration, Action<float> setCallBackAction)
	{
		DOSTween dostween = new DOSTween(this);
		dostween.HardTween = true;
		dostween.Build(setFromValue, setToValue, setDelayPerUnit, setMaxDuration, setCallBackAction, false);
		this.tweens.Add(dostween);
	}

	public void FireDOSTweenStep(float setFromValue, float setToValue, float setDelayPerUnit, float setMaxDuration, Action<float> setCallBackAction, bool ignoreFreezeTime = false)
	{
		DOSTween dostween = this.tweenPool.Pop();
		dostween.Build(setFromValue, setToValue, setDelayPerUnit, setMaxDuration, setCallBackAction, ignoreFreezeTime);
		this.tweens.Add(dostween);
	}

	public void FireDOSTweenStep(float setFromValue, float setToValue, float setDelayPerUnit, float setMaxDuration, Action<float> setCallBackAction, Action<float> setDoneCallBackAction)
	{
		DOSTween dostween = this.tweenPool.Pop();
		dostween.Build(setFromValue, setToValue, setDelayPerUnit, setMaxDuration, setCallBackAction, setDoneCallBackAction, false);
		this.tweens.Add(dostween);
	}

	public void FireDOSTweenStep(float setFromValue, float setToValue, float setDelayPerUnit, float setMaxDuration, Action<float> setCallBackAction, Action<float> setDoneCallBackAction, bool ignoreFreezeTime = false)
	{
		DOSTween dostween = this.tweenPool.Pop();
		dostween.Build(setFromValue, setToValue, setDelayPerUnit, setMaxDuration, setCallBackAction, setDoneCallBackAction, ignoreFreezeTime);
		this.tweens.Add(dostween);
	}

	public DOSTween PlayDOSTweenStep(float setFromValue, float setToValue, float setDelayPerUnit, float setMaxDuration, Action<float> setCallBackAction)
	{
		DOSTween dostween = new DOSTween(this);
		dostween.HardTween = true;
		dostween.Build(setFromValue, setToValue, setDelayPerUnit, setMaxDuration, setCallBackAction, false);
		this.tweens.Add(dostween);
		return dostween;
	}

	public DOSTween PlayDOSTweenStep(float setFromValue, float setToValue, float setDelayPerUnit, float setMaxDuration, Action<float> setCallBackAction, bool ignoreFreezeTime = false)
	{
		DOSTween dostween = new DOSTween(this);
		dostween.HardTween = true;
		dostween.Build(setFromValue, setToValue, setDelayPerUnit, setMaxDuration, setCallBackAction, ignoreFreezeTime);
		this.tweens.Add(dostween);
		return dostween;
	}

	public DOSTween PlayDOSTweenStep(float setFromValue, float setToValue, float setDelayPerUnit, float setMaxDuration, Action<float> setCallBackAction, Action<float> setDoneCallBackAction)
	{
		DOSTween dostween = new DOSTween(this);
		dostween.HardTween = true;
		dostween.Build(setFromValue, setToValue, setDelayPerUnit, setMaxDuration, setCallBackAction, setDoneCallBackAction, false);
		this.tweens.Add(dostween);
		return dostween;
	}

	public DOSTween PlayDOSTweenStep(float setFromValue, float setToValue, float setDelayPerUnit, float setMaxDuration, Action<float> setCallBackAction, Action<float> setDoneCallBackAction, bool ignoreFreezeTime = false)
	{
		DOSTween dostween = new DOSTween(this);
		dostween.HardTween = true;
		dostween.Build(setFromValue, setToValue, setDelayPerUnit, setMaxDuration, setCallBackAction, setDoneCallBackAction, ignoreFreezeTime);
		this.tweens.Add(dostween);
		return dostween;
	}

	public void KillAllTweens()
	{
		foreach (DOSTween dostween in this.tweens)
		{
			dostween.killMe();
			if (!dostween.HardTween)
			{
				this.tweenPool.Push(dostween);
			}
		}
		this.tweens.Clear();
	}

	public void KillTween(DOSTween theTween)
	{
		if (this.tweens.Remove(theTween))
		{
			theTween.killMe();
			if (!theTween.HardTween)
			{
				this.tweenPool.Push(theTween);
			}
			theTween = null;
		}
	}

	public void Update()
	{
		if (!this.timeFroze)
		{
			this.Tick.Execute();
		}
		else
		{
			this.FrozeTick.Execute();
		}
	}

	private void playerHasPaused()
	{
		this.timeFroze = true;
	}

	private void playerHasUnPaused()
	{
		this.timeFroze = false;
	}

	public CustomEvent Tick = new CustomEvent(10);

	public CustomEvent FrozeTick = new CustomEvent(10);

	private PooledStack<DOSTween> tweenPool;

	private HashSet<DOSTween> tweens = new HashSet<DOSTween>();

	private bool timeFroze;
}
