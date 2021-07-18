using System;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(InteractionHook))]
public class GoHideTrigger : MonoBehaviour
{
	public bool LockedOut
	{
		get
		{
			return this.lockedOut;
		}
		set
		{
			this.lockedOut = value;
		}
	}

	public bool LeaveDoom
	{
		get
		{
			return this.leaveDoomActive;
		}
		set
		{
			this.leaveDoomActive = value;
		}
	}

	public void TriggerAction()
	{
		if (this.isHiding)
		{
			this.triggerLeave();
		}
		else
		{
			this.triggerHide();
		}
	}

	public void TriggerHidden()
	{
		this.isHiding = !this.isHiding;
		if (this.isHiding)
		{
			this.myHideController.FullyHidden = true;
		}
	}

	public void ForceLeave()
	{
		this.myHideController.FullyHidden = false;
		this.attemptedLeaveFired = true;
		this.preLeaveEvents.Invoke();
	}

	public void attemptToLeaveHideMode()
	{
		if (this.preLeaveEvents != null)
		{
			this.myHideController.FullyHidden = false;
			this.attemptedLeaveFired = true;
			if (this.leaveDoomActive)
			{
				this.StageLeaveDoomActions.Execute();
				GameManager.TimeSlinger.FireTimer(0.2f, new Action(this.preLeaveEvents.Invoke), 0);
			}
			else
			{
				this.preLeaveEvents.Invoke();
			}
		}
	}

	public void triggerLeave()
	{
		if (this.attemptedLeaveFired)
		{
			if (this.leaveDoomActive)
			{
				this.LeaveDoomActions.Execute();
			}
			else
			{
				this.myHideController.LoseControlToRoam(this.returnRoamROT);
				this.myHideController.PlayerPeakingEvent.Event -= this.playerIsPeaking;
				roamController.Ins.TookControlActions.Event += this.triggerLeaveEvents;
				this.lockInteraction = false;
				this.myInteractionHook.ForceLock = false;
			}
		}
	}

	private void hidePlayer()
	{
		if (!this.lockedOut)
		{
			roamController.Ins.SetMasterLock(true);
			this.myHideController.SetMasterLock(true);
			DataManager.LockSave = true;
			this.lockInteraction = true;
			this.myInteractionHook.ForceLock = true;
			if (this.preHideEvents != null)
			{
				this.preHideEvents.Invoke();
			}
		}
	}

	private void triggerHide()
	{
		this.lockedOut = true;
		this.myHideController.SetMasterLock(true);
		this.myHideController.PutMe(this.hideControllerPOS, this.hideControllerROT);
		this.myHideController.PeakPOS = this.peakLocationPOS;
		this.myHideController.PlayerPeakingEvent.Event += this.playerIsPeaking;
		this.myHideController.TakeOverFromRoam(delegate
		{
			this.hideEvents.Invoke();
			this.myRoamController.PutMe(this.myRoamController.transform.position, this.myRoamController.transform.rotation.eulerAngles, true);
			this.attemptedLeaveFired = false;
			GameManager.TimeSlinger.FireTimer(1f, delegate()
			{
				this.lockedOut = false;
			}, 0);
		}, this);
	}

	private void playerIsPeaking(float PeakAmount)
	{
		this.currentPeakAmount = PeakAmount;
		if (this.peakingEvents != null)
		{
			this.peakingEvents.Invoke(PeakAmount);
		}
	}

	private void triggerLeaveEvents()
	{
		DataManager.LockSave = false;
		this.leaveEvents.Invoke();
		roamController.Ins.TookControlActions.Event -= this.triggerLeaveEvents;
	}

	private void stageMe()
	{
		this.myHideController = ControllerManager.Get<hideController>(GAME_CONTROLLER.HIDE);
		this.myRoamController = ControllerManager.Get<roamController>(GAME_CONTROLLER.ROAM);
		GameManager.StageManager.Stage -= this.stageMe;
	}

	private void Awake()
	{
		this.myInteractionHook = base.GetComponent<InteractionHook>();
		this.myInteractionHook.LeftClickAction += this.hidePlayer;
		GameManager.StageManager.Stage += this.stageMe;
	}

	private void Update()
	{
		if (!this.lockInteraction)
		{
			this.myInteractionHook.ForceLock = !this.activeHotZone.IsHot;
		}
		if (!this.lockedOut && this.isHiding && !this.attemptedLeaveFired && CrossPlatformInputManager.GetButtonDown("RightClick") && this.currentPeakAmount == 0f)
		{
			this.attemptToLeaveHideMode();
		}
	}

	private void OnDestroy()
	{
		this.myInteractionHook.LeftClickAction -= this.hidePlayer;
	}

	public CustomEvent StageLeaveDoomActions = new CustomEvent(2);

	public CustomEvent LeaveDoomActions = new CustomEvent(2);

	[SerializeField]
	private HotZoneTrigger activeHotZone;

	[SerializeField]
	private Vector3 hideControllerPOS;

	[SerializeField]
	private Vector3 hideControllerROT;

	[SerializeField]
	private Vector3 returnRoamROT;

	[SerializeField]
	private Vector3 peakLocationPOS;

	[SerializeField]
	private UnityEvent preHideEvents;

	[SerializeField]
	private UnityEvent hideEvents;

	[SerializeField]
	private UnityEvent preLeaveEvents;

	[SerializeField]
	private UnityEvent leaveEvents;

	[SerializeField]
	private FloatUnityEvent peakingEvents;

	private InteractionHook myInteractionHook;

	private hideController myHideController;

	private roamController myRoamController;

	private bool attemptedLeaveFired;

	private bool isHiding;

	private bool lockInteraction;

	private bool lockedOut;

	private bool leaveDoomActive;

	private float currentPeakAmount;
}
