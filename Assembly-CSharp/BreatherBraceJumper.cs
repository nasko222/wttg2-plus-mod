using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

[RequireComponent(typeof(braceController))]
public class BreatherBraceJumper : MonoBehaviour
{
	public void TriggerDoorJump()
	{
		this.myBraceController.SetMasterLock(true);
		MainCameraHook.Ins.AddHeadHit(0f);
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

	private void Awake()
	{
		BreatherBraceJumper.Ins = this;
		this.myBraceController = base.GetComponent<braceController>();
		CameraManager.Get(CAMERA_ID.MAIN, out this.myCamera);
	}

	public static BreatherBraceJumper Ins;

	[SerializeField]
	private GameObject PPLayerObject;

	private braceController myBraceController;

	private Camera myCamera;
}
