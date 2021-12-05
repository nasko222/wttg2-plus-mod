using System;
using UnityEngine;

public class BombMakerFloor1Jump : Jump
{
	protected override void DoStage()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CustomObjectLookUp.BombMakerHallwayJump, Vector3.zero, Quaternion.identity);
		gameObject.transform.position = new Vector3(25.95f, 0.188f, -6.31f);
		gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, -90f, 0f));
		HitmanRoamJumper.Ins.TriggerHallWayDoorJump();
		BombMakerBehindJump BMBH = gameObject.GetComponent<BombMakerBehindJump>();
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.JumpHit1, 0.3f);
		GameManager.TimeSlinger.FireTimer(0.5f, delegate()
		{
			BMBH.ElbowRot();
		}, 0);
		GameManager.TimeSlinger.FireTimer(0.8f, delegate()
		{
			BMBH.gunRecoil();
			HitmanBehaviour.Ins.GunFlashBombMaker();
		}, 0);
		GameManager.TimeSlinger.FireTimer(1.2f, new Action(this.GunFlashGameOver), 0);
		GameManager.TimeSlinger.FireTimer(1.25f, delegate()
		{
			UnityEngine.Object.Destroy(gameObject);
		}, 0);
	}

	protected override void DoExecute()
	{
		LookUp.Doors.Door1.CancelAutoClose();
	}

	public void GunFlashGameOver()
	{
		DataManager.ClearGameData();
		MainCameraHook.Ins.ClearARF(2f);
		UIManager.TriggerHardGameOver("YOU DISAPPOINTED THE BOMB MAKER");
	}
}
