using System;
using UnityEngine;

[RequireComponent(typeof(InteractionHook))]
public class PowerComputerTrigger : MonoBehaviour
{
	public void Lock()
	{
		this.myInteractionHook.ForceLock = true;
	}

	public void UnLock()
	{
		this.myInteractionHook.ForceLock = false;
	}

	private void powerOnComputer()
	{
		ComputerPowerHook.Ins.PowerComputer();
	}

	private void powerWentOut()
	{
		this.myInteractionHook.ForceLock = true;
	}

	private void powerWentOn()
	{
		if (!ComputerPowerHook.Ins.PowerOn)
		{
			this.myInteractionHook.ForceLock = false;
		}
	}

	private void Awake()
	{
		PowerComputerTrigger.Ins = this;
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.powerOnComputer;
	}

	private void Start()
	{
		EnvironmentManager.PowerBehaviour.PowerOffEvent.Event += this.powerWentOut;
		EnvironmentManager.PowerBehaviour.PowerOnEvent.Event += this.powerWentOn;
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.powerOnComputer;
	}

	public static PowerComputerTrigger Ins;

	private InteractionHook myInteractionHook;
}
