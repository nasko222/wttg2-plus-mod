using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractionHook))]
public class InteractionDisplayHook : MonoBehaviour
{
	private void hoverState()
	{
		if (this.hasLeftClickDisplay)
		{
			bool flag = true;
			if (this.requireLocationCheckForLeftClick && StateManager.PlayerLocation != this.leftClickLocation)
			{
				flag = false;
			}
			if (flag)
			{
				this.leftClickHoverEvents.Invoke();
			}
		}
		if (this.hasRightClickDisplay)
		{
			bool flag2 = true;
			if (this.requireLocationCheckForRightClick && StateManager.PlayerLocation != this.rightClickLocation)
			{
				flag2 = false;
			}
			if (flag2)
			{
				this.rightClickHoverEvents.Invoke();
			}
		}
	}

	private void exitHoverState()
	{
		if (this.hasLeftClickDisplay)
		{
			bool flag = true;
			if (this.requireLocationCheckForLeftClick && StateManager.PlayerLocation != this.leftClickLocation)
			{
				flag = false;
			}
			if (flag)
			{
				this.leftClickExitHoverEvents.Invoke();
			}
		}
		if (this.hasRightClickDisplay)
		{
			bool flag2 = true;
			if (this.requireLocationCheckForRightClick && StateManager.PlayerLocation != this.rightClickLocation)
			{
				flag2 = false;
			}
			if (flag2)
			{
				this.rightClickExitHoverEvents.Invoke();
			}
		}
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.RecvAction += this.hoverState;
		this.myInteractionHook.RecindAction += this.exitHoverState;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.RecvAction -= this.hoverState;
		this.myInteractionHook.RecindAction -= this.exitHoverState;
	}

	[SerializeField]
	private bool hasLeftClickDisplay;

	[SerializeField]
	private bool hasRightClickDisplay;

	[SerializeField]
	private bool requireLocationCheckForLeftClick;

	[SerializeField]
	private bool requireLocationCheckForRightClick;

	[SerializeField]
	private PLAYER_LOCATION leftClickLocation;

	[SerializeField]
	private PLAYER_LOCATION rightClickLocation;

	[SerializeField]
	private UnityEvent leftClickHoverEvents;

	[SerializeField]
	private UnityEvent leftClickExitHoverEvents;

	[SerializeField]
	private UnityEvent rightClickHoverEvents;

	[SerializeField]
	private UnityEvent rightClickExitHoverEvents;

	private InteractionHook myInteractionHook;
}
