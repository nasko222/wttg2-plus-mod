using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class deskController : mouseableController
{
	public void LoseControl()
	{
		this.Active = false;
		base.SetMasterLock(true);
		this.MyState = GAME_CONTROLLER_STATE.LOCKED;
		StateManager.PlayerState = PLAYER_STATE.BUSY;
	}

	public void TakeControl()
	{
		if (!this.MouseCaptureInit)
		{
			base.Init();
		}
		this.Active = true;
		base.SetMasterLock(false);
		this.MyState = GAME_CONTROLLER_STATE.IDLE;
		StateManager.PlayerState = PLAYER_STATE.DESK;
	}

	public void TakeOverFromRoam()
	{
		if (this.MouseCaptureInit)
		{
			this.MyMouseCapture.setCameraTargetRot(0f);
			this.MyMouseCapture.setRotatingObjectTargetRot(this.DefaultCameraHolderRotation);
			this.MyMouseCapture.setRotatingObjectRotation(this.DefaultCameraHolderRotation);
		}
		CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(this.MouseRotatingObject.transform);
		Sequence sequence = DOTween.Sequence().OnComplete(new TweenCallback(this.TakeControl));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, Vector3.zero, 0.75f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, Vector3.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Play<Sequence>();
	}

	public void TakeOverFromStart()
	{
		if (this.MouseCaptureInit)
		{
			this.MyMouseCapture.setCameraTargetRot(0f);
			this.MyMouseCapture.setRotatingObjectTargetRot(this.DefaultCameraHolderRotation);
			this.MyMouseCapture.setRotatingObjectRotation(this.DefaultCameraHolderRotation);
		}
		CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(this.MouseRotatingObject.transform);
		Sequence sequence = DOTween.Sequence().OnComplete(new TweenCallback(this.TakeControl));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, Vector3.zero, 1f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, Vector3.zero, 0.75f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Play<Sequence>();
	}

	public void SwitchToComputerController()
	{
		this.LoseControl();
		GameManager.BehaviourManager.CrossHairBehaviour.HideCrossHairGroup();
		GameManager.AudioSlinger.UnMuffleAudioHub(AUDIO_HUB.COMPUTER_HUB, 0.3f);
		Sequence sequence = DOTween.Sequence().OnComplete(new TweenCallback(ControllerManager.Get<computerController>(GAME_CONTROLLER.COMPUTER).TakeControl));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, Vector3.zero, 0.3f).SetEase(Ease.OutSine).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.MouseRotatingObject.transform.localRotation, delegate(Quaternion x)
		{
			this.MouseRotatingObject.transform.localRotation = x;
		}, new Vector3(0f, 90f, 0f), 0.3f).SetEase(Ease.OutSine).SetOptions(true));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, new Vector3(-0.03f, -0.13f, 0.014f), 0.3f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.fieldOfView, delegate(float x)
		{
			this.MyCamera.fieldOfView = x;
		}, 30.9f, 0.3f).SetEase(Ease.InSine));
		sequence.Play<Sequence>();
	}

	public void LeaveComputerMode()
	{
		GameManager.AudioSlinger.MuffleAudioHub(AUDIO_HUB.COMPUTER_HUB, 0.6f, 0.3f);
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			if (!this.lockOutFromRecovery)
			{
				this.TakeControl();
				this.MyMouseCapture.setCameraTargetRot(0f);
				this.MyMouseCapture.setRotatingObjectTargetRot(this.DefaultCameraHolderRotation);
				this.MyMouseCapture.setRotatingObjectRotation(this.DefaultCameraHolderRotation);
				GameManager.InteractionManager.UnLockInteraction();
				GameManager.BehaviourManager.CrossHairBehaviour.ShowCrossHairGroup();
			}
		});
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, Vector3.zero, 0.3f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.fieldOfView, delegate(float x)
		{
			this.MyCamera.fieldOfView = x;
		}, 60f, 0.3f).SetEase(Ease.InSine));
		sequence.Play<Sequence>();
	}

	public void LockRecovery()
	{
		this.lockOutFromRecovery = true;
	}

	public void TakeOverMainCamera()
	{
		CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(this.MouseRotatingObject.transform);
	}

	private void getInput()
	{
		if (!this.lockControl && CrossPlatformInputManager.GetButtonDown("RightClick"))
		{
			this.switchToRoamController();
		}
	}

	private void switchToRoamController()
	{
		this.LoseControl();
		ComputerChairObject.Ins.SetToNotInUsePosition();
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).GlobalTakeOver();
	}

	private void postBaseStage()
	{
		this.PostStage.Event -= this.postBaseStage;
		if (this.Active)
		{
			CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(this.MouseRotatingObject.transform);
			if (!DataManager.ContinuedGame)
			{
				this.MyCamera.transform.localPosition = Vector3.zero;
				this.MyCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
			}
		}
	}

	private void postBaseLive()
	{
		this.PostLive.Event -= this.postBaseLive;
		if (this.Active)
		{
			this.TakeControl();
		}
	}

	protected new void Awake()
	{
		deskController.Ins = this;
		base.Awake();
		ControllerManager.Add(this);
		this.PostStage.Event += this.postBaseStage;
		this.PostLive.Event += this.postBaseLive;
	}

	protected new void Start()
	{
		base.Start();
	}

	protected new void Update()
	{
		base.Update();
		this.getInput();
	}

	protected new void OnDestroy()
	{
		ControllerManager.Remove(this.Controller);
		base.OnDestroy();
	}

	public void UnLockRecovery()
	{
		this.lockOutFromRecovery = false;
	}

	public static deskController Ins;

	public float MaxLookLeft = -70f;

	public float MaxLookRight = 70f;

	public Vector3 DefaultCameraHolderRotation;

	private Vector3 horzVector = Vector3.zero;

	private bool lockOutFromRecovery;
}
