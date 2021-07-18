using System;
using UnityEngine;

public class HitmanBathroomJump : Jump
{
	protected override void DoStage()
	{
		HitmanBehaviour.Ins.Spawn(new Vector3(3.609f, 39.586f, 2.042f), new Vector3(0f, 90f, 0f));
		HitmanBehaviour.Ins.ActivateGunMesh();
		HitmanBehaviour.Ins.TriggerAnim("doorJumpIdle");
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
	}

	protected override void DoExecute()
	{
		HitmanRoamJumper.Ins.TriggerBathroomJump();
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit3);
		HitmanBehaviour.Ins.TriggerAnim("doorJump");
	}
}
