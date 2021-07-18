using System;
using UnityEngine;

[RequireComponent(typeof(computerController))]
public class HitmanComputerJumper : MonoBehaviour
{
	public void AddDelayComputerJump()
	{
		this.myComputerController.EnterEvents.Event += this.playerEntersComputerModeDelayTrigger;
		this.myComputerController.LeaveEvents.Event += this.playerLeftComputerModeDelayTrigger;
	}

	public void AddComputerJump()
	{
		this.myComputerController.LeaveEvents.Event += this.playerLeftComputerModeTrigger;
	}

	private void playerEntersComputerModeDelayTrigger()
	{
		this.delayTriggerTimeStamp = Time.time;
	}

	private void playerLeftComputerModeDelayTrigger()
	{
		if (Time.time - this.delayTriggerTimeStamp >= this.delayTriggerJumpTime)
		{
			this.myComputerController.EnterEvents.Event -= this.playerEntersComputerModeDelayTrigger;
			this.myComputerController.LeaveEvents.Event -= this.playerLeftComputerModeDelayTrigger;
			HitmanDeskJumper.Ins.TriggerJump();
		}
	}

	private void playerLeftComputerModeTrigger()
	{
		this.myComputerController.LeaveEvents.Event -= this.playerLeftComputerModeTrigger;
		HitmanDeskJumper.Ins.TriggerJump();
	}

	private void Awake()
	{
		HitmanComputerJumper.Ins = this;
		this.myComputerController = base.GetComponent<computerController>();
	}

	private void OnDestroy()
	{
		this.myComputerController.EnterEvents.Event -= this.playerEntersComputerModeDelayTrigger;
		this.myComputerController.LeaveEvents.Event -= this.playerLeftComputerModeDelayTrigger;
		this.myComputerController.LeaveEvents.Event -= this.playerLeftComputerModeTrigger;
	}

	public static HitmanComputerJumper Ins;

	[SerializeField]
	private float delayTriggerJumpTime = 10f;

	private computerController myComputerController;

	private float delayTriggerTimeStamp;
}
