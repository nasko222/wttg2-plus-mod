using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.CrossPlatformInput;

public class peepHoleController : mouseableController
{
	public bool LockOutLeave
	{
		get
		{
			return this.lockOutLeave;
		}
		set
		{
			this.lockOutLeave = value;
		}
	}

	public void DoTakeOver()
	{
		this.TakingOverEvents.Execute();
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).SwitchToPeepHoleController();
	}

	public void TakeOver()
	{
		FlashLightBehaviour.Ins.LockOut();
		this.tmpAE = ScriptableObject.CreateInstance<AutoExposure>();
		this.tmpAE.enabled.Override(true);
		this.tmpAE.minLuminance.Override(-9f);
		this.tmpAE.maxLuminance.Override(-9f);
		this.tmpBloom = ScriptableObject.CreateInstance<Bloom>();
		this.tmpBloom.enabled.Override(true);
		this.tmpBloom.intensity.Override(70f);
		this.tmpPPVol = PostProcessManager.instance.QuickVolume(base.gameObject.layer, 100f, new PostProcessEffectSettings[]
		{
			this.tmpAE,
			this.tmpBloom
		});
		this.tmpPPVol.weight = 1f;
		this.MyCamera.enabled = true;
		this.computerCamera.enabled = false;
		this.mainCamera.enabled = false;
		if (!this.MouseCaptureInit)
		{
			base.Init();
		}
		this.Active = true;
		base.SetMasterLock(false);
		this.MyState = GAME_CONTROLLER_STATE.IDLE;
		StateManager.PlayerState = PLAYER_STATE.PEEPING;
		DOTween.To(() => this.tmpPPVol.weight, delegate(float x)
		{
			this.tmpPPVol.weight = x;
		}, 0f, 0.75f).SetEase(Ease.Linear).OnComplete(delegate
		{
			RuntimeUtilities.DestroyVolume(this.tmpPPVol, false, false);
		});
		this.TookOverEvents.Execute();
	}

	public void ForceOut()
	{
		this.leavePeepMode();
	}

	private void leavePeepMode()
	{
		FlashLightBehaviour.Ins.UnLock();
		this.Active = false;
		base.SetMasterLock(true);
		this.MyState = GAME_CONTROLLER_STATE.LOCKED;
		StateManager.PlayerState = PLAYER_STATE.BUSY;
		this.mainCamera.enabled = true;
		this.computerCamera.enabled = true;
		this.MyCamera.enabled = false;
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).TakeOverFromPeep();
	}

	private void takeInput()
	{
		if (!this.lockControl && !this.lockOutLeave && CrossPlatformInputManager.GetButtonDown("RightClick"))
		{
			this.leavePeepMode();
		}
	}

	protected new void Awake()
	{
		peepHoleController.Ins = this;
		base.Awake();
		ControllerManager.Add(this);
		CameraManager.Get(CAMERA_ID.MAIN, out this.mainCamera);
		CameraManager.Get(CAMERA_ID.COMPUTER, out this.computerCamera);
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

	public static peepHoleController Ins;

	public CustomEvent TakingOverEvents = new CustomEvent(2);

	public CustomEvent TookOverEvents = new CustomEvent(2);

	private Camera computerCamera;

	private Camera mainCamera;

	private PostProcessVolume tmpPPVol;

	private AutoExposure tmpAE;

	private Bloom tmpBloom;

	private bool lockOutLeave;
}
