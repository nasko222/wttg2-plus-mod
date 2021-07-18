using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class switchToComputerController : MonoBehaviour
{
	public void Lock()
	{
		this.myInteractionHook.ForceLock = true;
	}

	public void UnLock()
	{
		this.myInteractionHook.ForceLock = false;
	}

	private void switchToComputerCon()
	{
		ControllerManager.Get<deskController>(GAME_CONTROLLER.DESK).SwitchToComputerController();
	}

	private void Awake()
	{
		switchToComputerController.Ins = this;
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.switchToComputerCon;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.switchToComputerCon;
	}

	public static switchToComputerController Ins;

	private InteractionHook myInteractionHook;
}
