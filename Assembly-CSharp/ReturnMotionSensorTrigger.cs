using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class ReturnMotionSensorTrigger : MonoBehaviour
{
	private void hoverAction()
	{
		this.motionSensorObject.transform.position = GameManager.ManagerSlinger.MotionSensorManager.CurrentMotionSensorSpawnLocation;
		this.motionSensorObject.transform.rotation = Quaternion.Euler(GameManager.ManagerSlinger.MotionSensorManager.CurrentMotionSensorSpawRotat);
		this.motionSensorObjectMeshRenderer.enabled = true;
	}

	private void exitAction()
	{
		this.motionSensorObjectMeshRenderer.enabled = false;
	}

	private void leftClickAction()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.ItemPutDown1);
		UIInventoryManager.HideMotionSensor();
		this.motionSensorObjectMeshRenderer.enabled = false;
		GameManager.ManagerSlinger.MotionSensorManager.ReturnMotionSensor();
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.motionSensorObjectMeshRenderer = this.motionSensorObject.GetComponent<MeshRenderer>();
		this.myInteractionHook.RecvAction += this.hoverAction;
		this.myInteractionHook.RecindAction += this.exitAction;
		this.myInteractionHook.LeftClickAction += this.leftClickAction;
	}

	private void Update()
	{
		if (StateManager.PlayerState == PLAYER_STATE.MOTION_SENSOR_PLACEMENT)
		{
			this.myInteractionHook.ForceLock = false;
		}
		else
		{
			this.myInteractionHook.ForceLock = true;
		}
	}

	private void OnDestroy()
	{
		this.myInteractionHook.RecvAction -= this.hoverAction;
		this.myInteractionHook.RecindAction -= this.exitAction;
		this.myInteractionHook.LeftClickAction -= this.leftClickAction;
	}

	[SerializeField]
	private GameObject motionSensorObject;

	private MeshRenderer motionSensorObjectMeshRenderer;

	private InteractionHook myInteractionHook;
}
