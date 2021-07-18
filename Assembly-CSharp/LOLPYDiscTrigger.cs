using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class LOLPYDiscTrigger : MonoBehaviour
{
	private void leftClickAction()
	{
		this.myInteractionHook.ForceLock = true;
		GameManager.ManagerSlinger.LOLPYDiscManager.LOLPYDiscBeh.InsertMe();
	}

	private void theDiscWasPickedUp()
	{
		this.myInteractionHook.ForceLock = false;
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myInteractionHook.LeftClickAction += this.leftClickAction;
		this.myInteractionHook.ForceLock = true;
		GameManager.ManagerSlinger.LOLPYDiscManager.DiscWasPickedUp += this.theDiscWasPickedUp;
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.leftClickAction;
	}

	private InteractionHook myInteractionHook;
}
