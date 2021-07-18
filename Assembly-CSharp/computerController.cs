using System;
using UnityStandardAssets.CrossPlatformInput;

public class computerController : baseController
{
	public void LoseControl()
	{
		this.Active = false;
		base.SetMasterLock(true);
		this.MyState = GAME_CONTROLLER_STATE.LOCKED;
		StateManager.PlayerState = PLAYER_STATE.BUSY;
		this.myComputerCameraManager.BecomeSlave();
		GameManager.ManagerSlinger.CursorManager.SetOverwrite(false);
		GameManager.ManagerSlinger.CursorManager.SwitchToDefaultCursor();
	}

	public void TakeControl()
	{
		this.EnterEvents.Execute();
		FlashLightBehaviour.Ins.LockOut();
		this.Active = true;
		base.SetMasterLock(false);
		this.MyState = GAME_CONTROLLER_STATE.IDLE;
		StateManager.PlayerState = PLAYER_STATE.COMPUTER;
		this.myComputerCameraManager.BecomeMaster();
		LookUp.DesktopUI.DesktopGraphicRaycaster.enabled = true;
		GameManager.InteractionManager.LockInteraction();
		GameManager.BehaviourManager.CrossHairBehaviour.HideCrossHairGroup();
		GameManager.ManagerSlinger.CursorManager.SwitchToCustomCursor();
		GameManager.ManagerSlinger.CursorManager.SetOverwrite(true);
	}

	public void LeaveMe()
	{
		GameManager.TimeSlinger.FireTimer(0.3f, delegate()
		{
			FlashLightBehaviour.Ins.UnLock();
			this.LeaveEvents.Execute();
		}, 0);
		LookUp.DesktopUI.DesktopGraphicRaycaster.enabled = false;
		this.LoseControl();
		ControllerManager.Get<deskController>(GAME_CONTROLLER.DESK).LeaveComputerMode();
	}

	private void postBaseStage()
	{
		this.PostStage.Event -= this.postBaseStage;
		this.myComputerCameraManager = ComputerCameraManager.Ins;
		if (this.Active)
		{
			ControllerManager.Get<deskController>(GAME_CONTROLLER.DESK).TakeOverMainCamera();
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
		base.Awake();
		ControllerManager.Add(this);
		computerController.Ins = this;
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
		if (!this.lockControl && CrossPlatformInputManager.GetButtonDown("RightClick"))
		{
			this.LeaveMe();
		}
	}

	protected new void OnDestroy()
	{
		ControllerManager.Remove(this.Controller);
		base.OnDestroy();
	}

	public static computerController Ins;

	public CustomEvent EnterEvents = new CustomEvent(3);

	public CustomEvent LeaveEvents = new CustomEvent(3);

	private ComputerCameraManager myComputerCameraManager;
}
