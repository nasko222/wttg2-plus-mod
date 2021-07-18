using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(roamController))]
public class HitmanRoamJumper : MonoBehaviour
{
	public void AddLobbyComputerJump()
	{
		this.myRoamController.TookControlActions.Event += this.triggerLobbyComputerJump;
	}

	public void ClearPPVol()
	{
		if (this.ppVol != null)
		{
			RuntimeUtilities.DestroyVolume(this.ppVol, false, false);
		}
	}

	public void TriggerBathroomJump()
	{
		LookUp.Doors.BathroomDoor.CancelAutoClose();
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myRoamController.SetMasterLock(true);
		DepthOfField depthOfField = ScriptableObject.CreateInstance<DepthOfField>();
		depthOfField.enabled.Override(true);
		depthOfField.focusDistance.Override(0.23f);
		depthOfField.aperture.Override(25.4f);
		depthOfField.focalLength.Override(28f);
		this.ppVol = PostProcessManager.instance.QuickVolume(this.PPLayerObject.layer, 100f, new PostProcessEffectSettings[]
		{
			depthOfField
		});
		this.ppVol.weight = 0f;
		DOTween.To(() => this.ppVol.weight, delegate(float x)
		{
			this.ppVol.weight = x;
		}, 1f, 2f).SetEase(Ease.Linear);
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, new Vector3(5.244362f, 40.52543f, 1.948186f), 0.75f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, -90f, 0f), 0.5f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Play<Sequence>();
	}

	public void TriggerMainDoorOpenJump()
	{
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myRoamController.SetMasterLock(true);
		GameManager.TimeSlinger.FireTimer(0.5f, delegate()
		{
			this.myRoamController.KillOutOfWay();
			Sequence sequence = DOTween.Sequence();
			sequence.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
			{
				base.transform.position = x;
			}, new Vector3(-2.185477f, 40.52925f, -3.811371f), 0.3f).SetEase(Ease.Linear));
			sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.myCamera.transform.localRotation = x;
			}, Vector3.zero, 0.3f).SetEase(Ease.Linear).SetOptions(true));
			sequence.Play<Sequence>();
		}, 0);
	}

	public void TriggerMainDoorOpendJump()
	{
		this.myRoamController.LockFromDoorRecovry();
		DepthOfField depthOfField = ScriptableObject.CreateInstance<DepthOfField>();
		depthOfField.enabled.Override(true);
		depthOfField.focusDistance.Override(0.23f);
		depthOfField.aperture.Override(25.4f);
		depthOfField.focalLength.Override(28f);
		this.ppVol = PostProcessManager.instance.QuickVolume(this.PPLayerObject.layer, 100f, new PostProcessEffectSettings[]
		{
			depthOfField
		});
		this.ppVol.weight = 0f;
		DOTween.To(() => this.ppVol.weight, delegate(float x)
		{
			this.ppVol.weight = x;
		}, 1f, 2f).SetEase(Ease.Linear);
	}

	public void TriggerMainDoorOutSideJump()
	{
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myRoamController.SetMasterLock(true);
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, new Vector3(-2.253f, base.transform.position.y, -5.411f), 0.5f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, Vector3.zero, 0.4f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Play<Sequence>();
	}

	public void TriggerHallWayDoorJump()
	{
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myRoamController.SetMasterLock(true);
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, new Vector3(24.727f, base.transform.position.y, -6.284f), 0.5f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, 90f, 0f), 0.4f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Play<Sequence>();
	}

	public void TriggerStairWayDoorJump()
	{
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myRoamController.SetMasterLock(true);
		this.myRoamController.LockFromDoorRecovry();
	}

	private void triggerLobbyComputerJump()
	{
		this.myRoamController.TookControlActions.Event -= this.triggerLobbyComputerJump;
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.myRoamController.SetMasterLock(true);
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			EnemyManager.HitManManager.ExecuteLobbyComputerJump();
		});
		sequence.Insert(0f, DOTween.To(() => base.transform.position, delegate(Vector3 x)
		{
			base.transform.position = x;
		}, new Vector3(3.587919f, base.transform.position.y, -22.74409f), 0.3f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => base.transform.rotation, delegate(Quaternion x)
		{
			base.transform.rotation = x;
		}, new Vector3(0f, 0f, 0f), 0.25f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.myCamera.transform.localRotation = x;
		}, Vector3.zero, 0.3f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Play<Sequence>();
	}

	private void Awake()
	{
		HitmanRoamJumper.Ins = this;
		this.myRoamController = base.GetComponent<roamController>();
		CameraManager.Get(CAMERA_ID.MAIN, out this.myCamera);
	}

	public static HitmanRoamJumper Ins;

	[SerializeField]
	private GameObject PPLayerObject;

	private roamController myRoamController;

	private Camera myCamera;

	private PostProcessVolume ppVol;
}
