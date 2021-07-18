using System;
using UnityEngine;

public class HitmanLobbyComputerJump : Jump
{
	protected override void DoStage()
	{
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
		HitmanBehaviour.Ins.ActivateGunMesh();
		HitmanBehaviour.Ins.TriggerAnim("hallDoorJumpIdle");
		HitmanBehaviour.Ins.Spawn(new Vector3(3.63f, 0f, -21.47f), new Vector3(0.2f, 176.9f, 0f));
	}

	protected override void DoExecute()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit3);
		HitmanBehaviour.Ins.TriggerAnim("hallDoorJump");
	}
}
