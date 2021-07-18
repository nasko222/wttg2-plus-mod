using System;
using UnityEngine;

public class HitmanShowerCaughtJump : Jump
{
	protected override void DoStage()
	{
	}

	protected override void DoExecute()
	{
		HitmanBehaviour.Ins.Spawn(new Vector3(5.164f, 39.584f, 1.92f), new Vector3(13.39f, 178.78f, 0f));
		HitmanBehaviour.Ins.WildCardEvents.Event += hideController.Ins.HideTrigger.ForceLeave;
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
		HitmanBehaviour.Ins.TriggerAnim("showerJump");
		hideController.Ins.HideTrigger.LeaveDoom = true;
		HitmanHideJumper.Ins.ShowerCaughtJump();
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.JumpHit3, 0.75f);
	}
}
