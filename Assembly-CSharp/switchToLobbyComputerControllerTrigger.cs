using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class switchToLobbyComputerControllerTrigger : MonoBehaviour
{
	private void switchToLobbyComputerController()
	{
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).LoseControl();
		ControllerManager.Get<lobbyComputerController>(GAME_CONTROLLER.LOBBY_COMPUTER).TakeControl();
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.switchToLobbyComputerController;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.switchToLobbyComputerController;
	}

	private InteractionHook myInteractionHook;
}
