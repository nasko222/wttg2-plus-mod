using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[RequireComponent(typeof(deskController))]
public class BombMakerDeskJumper : MonoBehaviour
{
	public void AddComputerJump()
	{
		HitmanComputerJumper.Ins.myComputerController.LeaveEvents.Event += this.TriggerJump;
	}

	public void TriggerJump()
	{
		HitmanComputerJumper.Ins.myComputerController.LeaveEvents.Event -= this.TriggerJump;
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myDeskController.LockRecovery();
		this.myDeskController.SetMasterLock(true);
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(CustomObjectLookUp.BombMakerPCKill);
		gameObject.transform.position = new Vector3(3.3f, 39.675f, -2.794f);
		gameObject.transform.rotation = Quaternion.Euler(new Vector3(0f, -180f, 0f));
		gameObject.GetComponent<BombMakerYoureUseless>().StagePCKill();
		DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, -90f, 0f), 0.25f).SetEase(Ease.Linear).SetOptions(true).OnComplete(delegate
		{
			MainCameraHook.Ins.TriggerHitManJump();
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit1);
			GameManager.TimeSlinger.FireTimer(3.2f, delegate()
			{
				DataManager.ClearGameData();
				MainCameraHook.Ins.ClearARF(2f);
				UIManager.TriggerHardGameOver("YOU DISAPPOINTED THE BOMB MAKER");
			}, 0);
		});
	}

	private void Awake()
	{
		BombMakerDeskJumper.Ins = this;
		this.myDeskController = base.GetComponent<deskController>();
	}

	private void OnDestroy()
	{
		HitmanComputerJumper.Ins.myComputerController.LeaveEvents.Event -= this.TriggerJump;
		BombMakerDeskJumper.Ins = null;
	}

	public static BombMakerDeskJumper Ins;

	private deskController myDeskController;
}
