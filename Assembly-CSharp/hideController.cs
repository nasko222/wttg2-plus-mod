using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class hideController : mouseableController
{
	public bool FullyHidden
	{
		get
		{
			return this.fullyHidden;
		}
		set
		{
			this.fullyHidden = value;
		}
	}

	public Vector3 PeakPOS
	{
		get
		{
			return this.peakPOS;
		}
		set
		{
			this.peakPOS = value;
		}
	}

	public GoHideTrigger HideTrigger
	{
		get
		{
			return this.currentHideTrigger;
		}
	}

	public void TakeOverFromRoam(Action ReturnAction, GoHideTrigger SetHideTrigger)
	{
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).LoseControl();
		this.currentHideTrigger = SetHideTrigger;
		if (this.MouseCaptureInit)
		{
			this.MyMouseCapture.setCameraTargetRot(0f);
			this.MyMouseCapture.setRotatingObjectTargetRot(this.defaultCameraHolderRotation);
			this.MyMouseCapture.setRotatingObjectRotation(this.defaultCameraHolderRotation);
		}
		CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(this.MouseRotatingObject.transform);
		Sequence sequence = DOTween.Sequence().OnComplete(delegate
		{
			if (OptionDataHook.Ins.Options.Mic)
			{
				MicMeterHook.Ins.PresentMicGroup(0.5f);
			}
			this.takeControl();
			ReturnAction();
		});
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, Vector3.zero, 0.75f).SetEase(Ease.OutBack));
		sequence.Insert(0.65f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, Vector3.zero, 0.35f).SetEase(Ease.Linear).SetOptions(true));
		sequence.Play<Sequence>();
	}

	public void LoseControlToRoam()
	{
		this.loseControl();
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).ReturnMe();
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).GlobalTakeOver();
	}

	public void LoseControlToRoam(Vector3 CustomROT)
	{
		this.loseControl();
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).ReturnMe(CustomROT);
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).GlobalTakeOver();
	}

	public void PutMe(Vector3 SetPOS, Vector3 SetROT)
	{
		base.transform.position = SetPOS;
		base.transform.rotation = Quaternion.Euler(SetROT);
	}

	private void takeControl()
	{
		if (!this.MouseCaptureInit)
		{
			base.Init();
		}
		this.Active = true;
		base.SetMasterLock(false);
		this.MyState = GAME_CONTROLLER_STATE.IDLE;
		StateManager.PlayerState = PLAYER_STATE.HIDING;
	}

	private void loseControl()
	{
		if (OptionDataHook.Ins.Options.Mic)
		{
			MicMeterHook.Ins.DismissMicGroup(0.5f);
		}
		this.Active = false;
		base.SetMasterLock(true);
		this.MyState = GAME_CONTROLLER_STATE.LOCKED;
		StateManager.PlayerState = PLAYER_STATE.BUSY;
		this.currentHideTrigger = null;
	}

	private void takeInput()
	{
		float num = (!this.fullyHidden) ? 0f : CrossPlatformInputManager.GetAxis("LeftClickWeighted");
		if (this.lockControl)
		{
			num = 0f;
		}
		if (this.FullyHidden)
		{
			this.PlayerPeakingEvent.Execute(num);
		}
		this.MouseRotatingObject.transform.localPosition = Vector3.Lerp(this.defaultCamreaHolderPOS, this.peakPOS, num);
	}

	protected new void Awake()
	{
		base.Awake();
		hideController.Ins = this;
		ControllerManager.Add(this);
		this.defaultCamreaHolderPOS = this.MouseRotatingObject.transform.localPosition;
	}

	protected new void Start()
	{
		base.Start();
	}

	protected new void Update()
	{
		base.Update();
		this.takeInput();
	}

	protected new void OnDestroy()
	{
		ControllerManager.Remove(this.Controller);
		base.OnDestroy();
	}

	public static hideController Ins;

	public CustomEvent<float> PlayerPeakingEvent = new CustomEvent<float>(4);

	[SerializeField]
	private Vector3 defaultCameraHolderRotation;

	private Vector3 defaultCamreaHolderPOS;

	private Vector3 peakPOS;

	private bool fullyHidden;

	private GoHideTrigger currentHideTrigger;
}
