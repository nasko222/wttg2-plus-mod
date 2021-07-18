using System;
using UnityEngine;

public class HitmanWardrobeCaughtJump : Jump
{
	protected override void DoStage()
	{
	}

	protected override void DoExecute()
	{
		HitmanBehaviour.Ins.TriggerAnim("wardrobeJumpIdle");
		HitmanBehaviour.Ins.Spawn(new Vector3(-3.227f, 39.213f, 1.642f), new Vector3(4.82f, 3.19f, 0f));
		HitmanBehaviour.Ins.WildCardEvents.Event += hideController.Ins.HideTrigger.ForceLeave;
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
		HitmanBehaviour.Ins.TriggerAnim("wardrobeJump");
		hideController.Ins.HideTrigger.LeaveDoom = true;
		MainCameraHook.Ins.TriggerHitManJump();
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.JumpHit3, 0.75f);
	}
}
