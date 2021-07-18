using System;
using UnityEngine;

public class HitmanMainDoorOutsideJump : Jump
{
	protected override void DoStage()
	{
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
		HitmanBehaviour.Ins.ActivateGunMesh();
		HitmanBehaviour.Ins.TriggerAnim("hallDoorJumpIdle");
		HitmanBehaviour.Ins.Spawn(new Vector3(-2.282f, 39.587f, -3.191f), new Vector3(0f, 175.95f, 0f));
		HitmanRoamJumper.Ins.TriggerMainDoorOutSideJump();
		GameManager.TimeSlinger.FireTimer(0.4f, delegate()
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit3);
			HitmanBehaviour.Ins.TriggerAnim("hallDoorJump");
		}, 0);
	}

	protected override void DoExecute()
	{
		LookUp.Doors.MainDoor.CancelAutoClose();
	}
}
