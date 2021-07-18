using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class PowerSwitchTrigger : MonoBehaviour
{
	private void switchPowerOn()
	{
		EnvironmentManager.PowerBehaviour.SwitchPowerOn();
	}

	private void powerWentOn()
	{
		this.myInteractionHook.ForceLock = true;
	}

	private void powerWentOff()
	{
		this.myInteractionHook.ForceLock = false;
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.switchPowerOn;
	}

	private void Start()
	{
		this.myInteractionHook.ForceLock = true;
		EnvironmentManager.PowerBehaviour.PowerOnEvent.Event += this.powerWentOn;
		EnvironmentManager.PowerBehaviour.PowerOffEvent.Event += this.powerWentOff;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.switchPowerOn;
	}

	private InteractionHook myInteractionHook;
}
