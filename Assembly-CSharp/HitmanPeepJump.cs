using System;
using UnityEngine;

public class HitmanPeepJump : Jump
{
	protected override void DoStage()
	{
		HitmanBehaviour.Ins.KillWalk();
		HitmanBehaviour.Ins.TriggerAnim("peepJumpIdle");
		HitmanBehaviour.Ins.Spawn(new Vector3(-2.304f, 39.588f, -6.225f), new Vector3(-0.2f, 0f, 0f));
		HitmanBehaviour.Ins.ActivateGunMesh();
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
	}

	protected override void DoExecute()
	{
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		GameManager.TimeSlinger.FireTimer(0.75f, delegate()
		{
			HitmanBehaviour.Ins.TriggerAnim("peepJump");
		}, 0);
		MainCameraHook.Ins.TriggerHitManJump();
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit3);
	}
}
