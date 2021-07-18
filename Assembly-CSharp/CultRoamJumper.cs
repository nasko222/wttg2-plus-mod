using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CultRoamJumper : MonoBehaviour
{
	public void TriggerHammerJump()
	{
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myRoamController.SetMasterLock(true);
		this.myCamera.transform.SetParent(CultFemaleBehaviour.Ins.CameraBone);
		DepthOfField depthOfField = ScriptableObject.CreateInstance<DepthOfField>();
		depthOfField.enabled.Override(true);
		depthOfField.focusDistance.Override(0.7f);
		depthOfField.aperture.Override(25.6f);
		depthOfField.focalLength.Override(56f);
		this.ppVol = PostProcessManager.instance.QuickVolume(this.PPLayerObject.layer, 100f, new PostProcessEffectSettings[]
		{
			depthOfField
		});
		this.ppVol.weight = 0f;
		Vector3 zero = Vector3.zero;
		zero.x = 0f;
		zero.y = -180f;
		zero.z = 0f;
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			CultFemaleBehaviour.Ins.HammerJump();
		});
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, zero, 0.25f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.myCamera.transform.localPosition = x;
		}, Vector3.zero, 0.3f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.ppVol.weight, delegate(float x)
		{
			this.ppVol.weight = x;
		}, 1f, 0.3f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	public void ClearDOF()
	{
		RuntimeUtilities.DestroyVolume(this.ppVol, false, false);
	}

	public void TriggerEndJump()
	{
		this.myRoamController.KillOutOfWay();
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myRoamController.SetMasterLock(true);
	}

	private void Awake()
	{
		CultRoamJumper.Ins = this;
		this.myRoamController = base.GetComponent<roamController>();
		CameraManager.Get(CAMERA_ID.MAIN, out this.myCamera);
	}

	public static CultRoamJumper Ins;

	[SerializeField]
	private GameObject PPLayerObject;

	private roamController myRoamController;

	private Camera myCamera;

	private PostProcessVolume ppVol;
}
