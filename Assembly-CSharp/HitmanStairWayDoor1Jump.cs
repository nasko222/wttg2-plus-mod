using System;
using UnityEngine;

public class HitmanStairWayDoor1Jump : Jump
{
	protected override void DoStage()
	{
		HitmanBehaviour.Ins.ActivateGunMesh();
		HitmanBehaviour.Ins.TriggerAnim("doorJumpShortIdle");
		HitmanBehaviour.Ins.Spawn(new Vector3(21.79f, 0.021f, -6.2f), new Vector3(2f, 91.5f, 0f));
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
	}

	protected override void DoExecute()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit3);
		HitmanRoamJumper.Ins.TriggerStairWayDoorJump();
		LookUp.Doors.Door8.CancelAutoClose();
		HitmanBehaviour.Ins.TriggerAnim("doorJumpShort");
	}
}
