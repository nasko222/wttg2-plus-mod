using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(InteractionHook))]
public class switchToPeepHoleController : MonoBehaviour
{
	private void switchToPeepHoleCon()
	{
		ControllerManager.Get<peepHoleController>(GAME_CONTROLLER.PEEP_HOLE).DoTakeOver();
	}

	private void theDoorOpened()
	{
		this.myInteractionHook.ForceLock = true;
	}

	private void theDoorClosed()
	{
		this.myInteractionHook.ForceLock = false;
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.MainDoorTrigger.DoorOpenEvent.AddListener(new UnityAction(this.theDoorOpened));
		this.MainDoorTrigger.DoorCloseEvent.AddListener(new UnityAction(this.theDoorClosed));
		this.myInteractionHook.LeftClickAction += this.switchToPeepHoleCon;
	}

	private void OnDestroy()
	{
		this.MainDoorTrigger.DoorOpenEvent.RemoveListener(new UnityAction(this.theDoorOpened));
		this.MainDoorTrigger.DoorCloseEvent.RemoveListener(new UnityAction(this.theDoorClosed));
		this.myInteractionHook.LeftClickAction -= this.switchToPeepHoleCon;
	}

	public DoorTrigger MainDoorTrigger;

	private InteractionHook myInteractionHook;
}
