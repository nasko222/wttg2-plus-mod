using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class CultDeskJumper : MonoBehaviour
{
	public void TriggerLightsOffJump()
	{
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myDeskController.LockRecovery();
		this.myDeskController.SetMasterLock(true);
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			this.myCamera.transform.SetParent(CultFemaleBehaviour.Ins.CameraBone);
			this.myCamera.transform.localPosition = Vector3.zero;
			EnemyManager.CultManager.TriggerDeskJump();
		});
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(0f, -90f, 0f), 0.25f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Play<Sequence>();
	}

	private void Awake()
	{
		CultDeskJumper.Ins = this;
		this.myDeskController = base.GetComponent<deskController>();
		CameraManager.Get(CAMERA_ID.MAIN, out this.myCamera);
	}

	public static CultDeskJumper Ins;

	private deskController myDeskController;

	private Camera myCamera;
}
