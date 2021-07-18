using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class switchToBraceController : MonoBehaviour
{
	private void enterBraceMode()
	{
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).LoseControl();
		ControllerManager.Get<braceController>(GAME_CONTROLLER.BRACE).TakeOverFromRoam();
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.RightClickAction += this.enterBraceMode;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.RightClickAction -= this.enterBraceMode;
	}

	private InteractionHook myInteractionHook;
}
