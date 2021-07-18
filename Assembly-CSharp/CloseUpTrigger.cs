using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(InteractionHook))]
public class CloseUpTrigger : MonoBehaviour
{
	private void goClose()
	{
		if (!this.interactLock)
		{
			this.interactLock = true;
			SteamSlinger.Ins.InspectLoreDoc(this.CameraTargetPOS.GetHashCode());
			this.lastCameraPOS = this.myCamera.transform.localPosition;
			this.lastCameraROT = this.myCamera.transform.localRotation.eulerAngles;
			this.lastCameraParent = this.myCameraHook.CurrentParent;
			if (this.disableDOF)
			{
				this.tmpDOF = ScriptableObject.CreateInstance<DepthOfField>();
				this.tmpDOF.enabled.Override(false);
				this.tmpPPVol = PostProcessManager.instance.QuickVolume(this.PPObject.layer, 100f, new PostProcessEffectSettings[]
				{
					this.tmpDOF
				});
				this.tmpPPVol.weight = 1f;
			}
			ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).SetMasterLock(true);
			GameManager.InteractionManager.LockInteraction();
			GameManager.BehaviourManager.CrossHairBehaviour.HideCrossHairGroup();
			this.myCamera.transform.SetParent(base.transform);
			DOTween.To(() => this.myCamera.transform.rotation, delegate(Quaternion x)
			{
				this.myCamera.transform.rotation = x;
			}, this.CameraTargetROT, 0.5f).SetEase(Ease.Linear).SetOptions(true);
			DOTween.To(() => this.myCamera.transform.position, delegate(Vector3 x)
			{
				this.myCamera.transform.position = x;
			}, this.CameraTargetPOS, 0.75f).SetEase(Ease.Linear).OnComplete(delegate
			{
				this.interactLock = false;
				this.isCloseUp = true;
			});
		}
	}

	private void leaveClose()
	{
		if (!this.interactLock)
		{
			this.isCloseUp = false;
			this.interactLock = true;
			this.myCamera.transform.SetParent(this.lastCameraParent);
			DOTween.To(() => this.myCamera.transform.localPosition, delegate(Vector3 x)
			{
				this.myCamera.transform.localPosition = x;
			}, roamController.Ins.DefaultCameraPOS, 0.75f).SetEase(Ease.Linear).OnComplete(delegate
			{
				if (this.disableDOF)
				{
					RuntimeUtilities.DestroyVolume(this.tmpPPVol, false, false);
				}
				GameManager.InteractionManager.UnLockInteraction();
				GameManager.BehaviourManager.CrossHairBehaviour.ShowCrossHairGroup();
				ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).SetMasterLock(false);
				GameManager.TimeSlinger.FireTimer(1f, delegate()
				{
					this.interactLock = false;
				}, 0);
			});
			DOTween.To(() => this.myCamera.transform.localRotation, delegate(Quaternion x)
			{
				this.myCamera.transform.localRotation = x;
			}, this.lastCameraROT, 0.5f).SetEase(Ease.Linear).SetOptions(true);
		}
	}

	private void stageMe()
	{
		CameraManager.Get(this.cameraIControl, out this.myCamera);
		this.myCameraHook = CameraManager.GetCameraHook(this.cameraIControl);
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.goClose;
		GameManager.StageManager.Stage += this.stageMe;
		SteamSlinger.Ins.AddLoreDoc(this.CameraTargetPOS.GetHashCode());
	}

	private void Update()
	{
		if (this.isCloseUp && CrossPlatformInputManager.GetButtonDown("RightClick"))
		{
			this.leaveClose();
		}
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.goClose;
	}

	[SerializeField]
	private CAMERA_ID cameraIControl;

	[SerializeField]
	private Vector3 CameraTargetPOS;

	[SerializeField]
	private Vector3 CameraTargetROT;

	[SerializeField]
	private bool disableDOF;

	[SerializeField]
	private GameObject PPObject;

	private Camera myCamera;

	private CameraHook myCameraHook;

	private InteractionHook myInteractionHook;

	private Vector3 lastCameraPOS = Vector3.zero;

	private Vector3 lastCameraROT = Vector3.zero;

	private Transform lastCameraParent;

	private PostProcessVolume tmpPPVol;

	private DepthOfField tmpDOF;

	private bool isCloseUp;

	private bool interactLock;
}
