using System;
using UnityEngine;

public class HitmanShowerJump : Jump
{
	protected override void DoStage()
	{
		HitmanBehaviour.Ins.ActivateGunMesh();
		HitmanBehaviour.Ins.TriggerAnim("showerJumpShortIdle");
		HitmanBehaviour.Ins.Spawn(new Vector3(5.67f, 39.583f, 2.2f), new Vector3(7.03f, -180f, 0f));
		HitmanHideJumper.Ins.ShowerJump();
	}

	protected override void DoExecute()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit3);
		HitmanBehaviour.Ins.TriggerAnim("showerJumpShort");
	}
}
