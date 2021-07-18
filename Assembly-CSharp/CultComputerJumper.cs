using System;
using UnityEngine;

public class CultComputerJumper : MonoBehaviour
{
	public void AddLightsOffJump()
	{
		this.myComputerController.LeaveEvents.Event += this.playerLeftComputerModeLightsOffJump;
	}

	public void playerLeftComputerModeLightsOffJump()
	{
		this.myComputerController.LeaveEvents.Event -= this.playerLeftComputerModeLightsOffJump;
		EnemyManager.CultManager.StageDeskJump();
		CultDeskJumper.Ins.TriggerLightsOffJump();
	}

	private void Awake()
	{
		CultComputerJumper.Ins = this;
		this.myComputerController = base.GetComponent<computerController>();
	}

	private void OnDestroy()
	{
	}

	public static CultComputerJumper Ins;

	private computerController myComputerController;
}
