using System;
using UnityEngine;

public class HitmanFloor3Jump : Jump
{
	protected override void DoStage()
	{
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
		HitmanBehaviour.Ins.ActivateGunMesh();
		HitmanBehaviour.Ins.TriggerAnim("hallDoorJumpIdle");
		HitmanBehaviour.Ins.Spawn(new Vector3(26.749f, 11.325f, -6.315f), new Vector3(0f, -93.07f, 0f));
		HitmanRoamJumper.Ins.TriggerHallWayDoorJump();
		GameManager.TimeSlinger.FireTimer(0.4f, delegate()
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit3);
			HitmanBehaviour.Ins.TriggerAnim("hallDoorJump");
		}, 0);
	}

	protected override void DoExecute()
	{
		LookUp.Doors.Door8.CancelAutoClose();
	}
}
