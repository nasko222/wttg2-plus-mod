using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityStandardAssets.CrossPlatformInput;

public class lobbyComputerController : baseController
{
	public void TakeControl()
	{
		DataManager.LockSave = true;
		this.Active = true;
		base.SetMasterLock(false);
		this.MyState = GAME_CONTROLLER_STATE.IDLE;
		StateManager.PlayerState = PLAYER_STATE.LOBBY_COMPUTER;
		CameraManager.GetCameraHook(this.CameraIControl).SetMyParent(base.transform);
		this.tmpDOF = ScriptableObject.CreateInstance<DepthOfField>();
		this.tmpDOF.enabled.Override(true);
		this.tmpDOF.focusDistance.Override(0.36f);
		this.tmpDOF.aperture.Override(6.1f);
		this.tmpDOF.focalLength.Override(36f);
		this.tmpPPVol = PostProcessManager.instance.QuickVolume(base.gameObject.layer, 100f, new PostProcessEffectSettings[]
		{
			this.tmpDOF
		});
		this.tmpPPVol.weight = 0f;
		GameManager.InteractionManager.LockInteraction();
		GameManager.BehaviourManager.CrossHairBehaviour.HideCrossHairGroup();
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localPosition, delegate(Vector3 x)
		{
			this.MyCamera.transform.localPosition = x;
		}, this.cameraPOS, 0.7f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.MyCamera.transform.localRotation, delegate(Quaternion x)
		{
			this.MyCamera.transform.localRotation = x;
		}, this.cameraROT, 0.4f).SetEase(Ease.Linear));
		sequence.Insert(0f, DOTween.To(() => this.tmpPPVol.weight, delegate(float x)
		{
			this.tmpPPVol.weight = x;
		}, 1f, 0.7f).SetEase(Ease.Linear));
		sequence.Play<Sequence>();
	}

	private void LooseControl()
	{
		this.Active = false;
		base.SetMasterLock(true);
		this.MyState = GAME_CONTROLLER_STATE.LOCKED;
		StateManager.PlayerState = PLAYER_STATE.BUSY;
		GameManager.InteractionManager.UnLockInteraction();
		GameManager.BehaviourManager.CrossHairBehaviour.ShowCrossHairGroup();
		DOTween.To(() => this.tmpPPVol.weight, delegate(float x)
		{
			this.tmpPPVol.weight = x;
		}, 0f, 0.7f).SetEase(Ease.Linear).OnComplete(delegate
		{
			DataManager.LockSave = false;
			RuntimeUtilities.DestroyVolume(this.tmpPPVol, false, false);
		});
		ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM).GlobalTakeOver();
	}

	protected new void Awake()
	{
		base.Awake();
		ControllerManager.Add(this);
	}

	protected new void Start()
	{
		base.Start();
	}

	protected new void Update()
	{
		base.Update();
		if (!this.lockControl && CrossPlatformInputManager.GetButtonDown("RightClick"))
		{
			this.LooseControl();
		}
	}

	protected new void OnDestroy()
	{
		ControllerManager.Remove(this.Controller);
		base.OnDestroy();
	}

	[SerializeField]
	private Vector3 cameraPOS;

	[SerializeField]
	private Vector3 cameraROT;

	private PostProcessVolume tmpPPVol;

	private DepthOfField tmpDOF;
}
