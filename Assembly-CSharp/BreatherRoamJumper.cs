using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BreatherRoamJumper : MonoBehaviour
{
	public void StagePickUpJump()
	{
		GameManager.TimeSlinger.FireTimer(0.35f, delegate()
		{
			this.myRoamController.SetMasterLock(true);
			DOTween.To(() => this.myRoamController.transform.rotation, delegate(Quaternion x)
			{
				this.myRoamController.transform.rotation = x;
			}, new Vector3(0f, 270f, 0f), 0.35f).SetEase(Ease.Linear).SetOptions(true).OnComplete(delegate
			{
				roamController.Ins.MyMouseCapture.setRotatingObjectTargetRot(new Vector3(0f, 270f, 0f));
				this.myRoamController.SetMasterLock(false);
			});
		}, 0);
	}

	public void TriggerExitRushJump()
	{
		this.myRoamController.SetMasterLock(true);
		this.myCamera.transform.SetParent(BreatherBehaviour.Ins.HelperBone);
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, Vector3.zero, 0.5f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 0.5f).SetOptions(true).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void TriggerDumpsterJump()
	{
		this.myRoamController.SetMasterLock(true);
		this.myCamera.transform.SetParent(BreatherBehaviour.Ins.HelperBone);
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, Vector3.zero, 0.5f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 0.1f).SetOptions(true).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void TriggerPeekABooJump()
	{
		this.myRoamController.SetMasterLock(true);
		this.myCamera.transform.SetParent(BreatherBehaviour.Ins.HelperBone);
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, Vector3.zero, 0.4f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 0.25f).SetOptions(true).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	private void Awake()
	{
		BreatherRoamJumper.Ins = this;
		this.myRoamController = base.GetComponent<roamController>();
		CameraManager.Get(CAMERA_ID.MAIN, out this.myCamera);
	}

	public static BreatherRoamJumper Ins;

	[SerializeField]
	private GameObject PPLayerObject;

	private roamController myRoamController;

	private Camera myCamera;

	private PostProcessVolume ppVol;
}
