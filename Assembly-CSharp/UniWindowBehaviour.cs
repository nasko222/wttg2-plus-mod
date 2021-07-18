using System;
using UnityEngine.Events;

public class UniWindowBehaviour : WindowBehaviour
{
	protected override void OnClose()
	{
		if (this.CloseEvents != null)
		{
			this.CloseEvents.Invoke();
		}
	}

	protected override void OnLaunch()
	{
		if (this.LaunchEvents != null)
		{
			this.LaunchEvents.Invoke();
		}
	}

	protected override void OnMax()
	{
		if (this.MaxEvents != null)
		{
			this.MaxEvents.Invoke();
		}
	}

	protected override void OnMin()
	{
		if (this.MinEvents != null)
		{
			this.MinEvents.Invoke();
		}
	}

	protected override void OnResized()
	{
		if (this.ResizedEvents != null)
		{
			this.ResizedEvents.Invoke();
		}
	}

	protected override void OnUnMax()
	{
		if (this.UnMaxEvents != null)
		{
			this.UnMaxEvents.Invoke();
		}
	}

	protected override void OnUnMin()
	{
		if (this.UnMinEvents != null)
		{
			this.UnMinEvents.Invoke();
		}
	}

	public UnityEvent CloseEvents;

	public UnityEvent LaunchEvents;

	public UnityEvent MaxEvents;

	public UnityEvent UnMaxEvents;

	public UnityEvent MinEvents;

	public UnityEvent UnMinEvents;

	public UnityEvent ResizedEvents;
}
