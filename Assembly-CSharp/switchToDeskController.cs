using System;
using UnityEngine;

public class switchToDeskController : MonoBehaviour
{
	private void switchToDeskCon()
	{
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).LoseControl();
		ControllerManager.Get<deskController>(GAME_CONTROLLER.DESK).TakeOverFromRoam();
		ComputerChairObject.Ins.SetToInUsePosition();
	}

	private void Awake()
	{
		this.myBoxCollider = base.GetComponent<BoxCollider>();
		base.GetComponent<InteractionHook>().LeftClickAction += this.switchToDeskCon;
	}

	private void OnDestroy()
	{
		base.GetComponent<InteractionHook>().LeftClickAction -= this.switchToDeskCon;
	}

	private BoxCollider myBoxCollider;
}
