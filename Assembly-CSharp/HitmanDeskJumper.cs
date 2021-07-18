using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[RequireComponent(typeof(deskController))]
public class HitmanDeskJumper : MonoBehaviour
{
	public void TriggerJump()
	{
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myDeskController.LockRecovery();
		this.myDeskController.SetMasterLock(true);
		HitmanBehaviour.Ins.Spawn(new Vector3(3.227f, 39.565f, -2.661f), new Vector3(0f, -180f, 0f));
		HitmanBehaviour.Ins.ActivateGunMesh();
		HitmanBehaviour.Ins.TriggerAnim("deskJumpIdle");
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
		DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, -90f, 0f), 0.25f).SetEase(Ease.Linear).SetOptions(true).OnComplete(delegate
		{
			HitmanBehaviour.Ins.TriggerAnim("deskJump");
			MainCameraHook.Ins.TriggerHitManJump();
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit3);
		});
	}

	private void Awake()
	{
		HitmanDeskJumper.Ins = this;
		this.myDeskController = base.GetComponent<deskController>();
	}

	public static HitmanDeskJumper Ins;

	private deskController myDeskController;
}
