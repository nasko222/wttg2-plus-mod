using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class JumpRailingTrigger : MonoBehaviour
{
	private void leftClickAction()
	{
		if (this.BalcoyHotZone.IsHot)
		{
			ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).JumpRailingFromBalcoy();
		}
		else if (this.StairWellHotZone.IsHot)
		{
			ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).JumpRailingFromStairWell();
		}
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.leftClickAction;
	}

	private void Update()
	{
		if (!this.BalcoyHotZone.IsHot && !this.StairWellHotZone.IsHot)
		{
			this.myInteractionHook.ForceLock = true;
		}
		else
		{
			this.myInteractionHook.ForceLock = false;
		}
	}

	private void OnDestroy()
	{
		this.myInteractionHook.RightClickAction -= this.leftClickAction;
	}

	public HotZoneTrigger BalcoyHotZone;

	public HotZoneTrigger StairWellHotZone;

	private InteractionHook myInteractionHook;
}
