using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class DollMakerDeskJumper : MonoBehaviour
{
	public void TriggerDeskJump()
	{
		this.myDeskController.LockRecovery();
		this.myDeskController.SetMasterLock(true);
		GameManager.TimeSlinger.FireTimer(0.1f, delegate()
		{
			this.myCamera.transform.SetParent(DollMakerBehaviour.Ins.HelperBone);
			Sequence sequence = DOTween.Sequence();
			sequence.Insert(0.1f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, Vector3.zero, 0.5f).SetEase(Ease.Linear));
			sequence.Insert(0.1f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.myCamera.transform.localRotation = x;
			}, Vector3.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Play<Sequence>();
		}, 0);
	}

	private void Awake()
	{
		DollMakerDeskJumper.Ins = this;
		this.myDeskController = base.GetComponent<deskController>();
		CameraManager.Get(CAMERA_ID.MAIN, out this.myCamera);
	}

	public static DollMakerDeskJumper Ins;

	private deskController myDeskController;

	private Camera myCamera;
}
