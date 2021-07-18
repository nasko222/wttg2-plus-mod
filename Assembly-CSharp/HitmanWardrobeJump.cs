using System;
using UnityEngine;

public class HitmanWardrobeJump : Jump
{
	protected override void DoStage()
	{
		HitmanBehaviour.Ins.ActivateGunMesh();
		HitmanBehaviour.Ins.TriggerAnim("wardrobeShortJumpIdle");
		HitmanBehaviour.Ins.Spawn(new Vector3(-3.2f, 39.266f, 0.93f), new Vector3(8.5f, 3.18f, 0f));
		HitmanHideJumper.Ins.WardrobeJump();
	}

	protected override void DoExecute()
	{
		MainCameraHook.Ins.TriggerHitManJump();
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit3);
		HitmanBehaviour.Ins.TriggerAnim("wardrobeShortJump");
	}
}
