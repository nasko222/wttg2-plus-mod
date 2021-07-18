using System;
using UnityEngine;

public class HitmanMainDoorJump : Jump
{
	protected override void DoStage()
	{
		GameManager.TimeSlinger.KillTimer(EnemyManager.HitManManager.LockPickTimer);
		HitmanBehaviour.Ins.KillWalk();
		HitmanBehaviour.Ins.ActivateGunMesh();
		HitmanBehaviour.Ins.TriggerAnim("doorJumpIdle");
		HitmanBehaviour.Ins.Spawn(new Vector3(-2.28f, 39.586f, -5.255f), Vector3.zero);
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
		HitmanRoamJumper.Ins.TriggerMainDoorOpenJump();
		GameManager.TimeSlinger.FireTimer(0.5f, delegate()
		{
			MainCameraHook.Ins.TriggerHitManJump();
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit3);
			HitmanBehaviour.Ins.TriggerAnim("doorJump");
		}, 0);
	}

	protected override void DoExecute()
	{
		LookUp.Doors.MainDoor.CancelAutoClose();
	}
}
