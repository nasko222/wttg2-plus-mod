using System;
using UnityEngine;
using UnityEngine.Events;

public class HitmanPatrolBehaviour : MonoBehaviour
{
	public void Patrol()
	{
		PLAYER_LOCATION playerLocation = StateManager.PlayerLocation;
		if (playerLocation != PLAYER_LOCATION.MAIN_ROON)
		{
			if (playerLocation == PLAYER_LOCATION.BATH_ROOM)
			{
				this.mainRoomStageHunt();
			}
		}
		else
		{
			this.mainRoomStageHunt();
		}
	}

	public void PickNextPatrol()
	{
		this.patrolCount--;
		int num = UnityEngine.Random.Range(1, 4);
		if (num != 1)
		{
			if (num != 2)
			{
				if (num == 3)
				{
					HitmanBehaviour.Ins.TriggerAnim("lookIdle3");
				}
			}
			else
			{
				HitmanBehaviour.Ins.TriggerAnim("lookIdle2");
			}
		}
		else
		{
			HitmanBehaviour.Ins.TriggerAnim("lookIdle1");
		}
		if (this.patrolCount > 0)
		{
			GameManager.TimeSlinger.FireHardTimer(out this.patrolTimer, 8f, delegate()
			{
				HitmanBehaviour.Ins.TriggerAnim("idle");
				this.pickPatrolPoint();
			}, 0);
		}
		else
		{
			GameManager.TimeSlinger.FireHardTimer(out this.patrolTimer, 8f, delegate()
			{
				HitmanBehaviour.Ins.TriggerAnim("idle");
				this.patrolOut();
			}, 0);
		}
	}

	public void ReachedPatrolExitPoint()
	{
		this.checkIfPlayerIsPeaking = false;
		this.checkMicCheck = false;
		hideController.Ins.PlayerPeakingEvent.Event -= this.playerIsPeaking;
		if (OptionDataHook.Ins.Options.Mic)
		{
			GameManager.BehaviourManager.PlayerAudioBehaviour.CurrentPlayersLoudLevel.Event -= this.playerVoiceLevel;
		}
		hideController.Ins.HideTrigger.LeaveDoom = false;
		hideController.Ins.HideTrigger.StageLeaveDoomActions.Event -= this.stageLeftHidingJump;
		hideController.Ins.HideTrigger.LeaveDoomActions.Event -= this.leftHidingJump;
		PLAYER_LOCATION playerLocation = StateManager.PlayerLocation;
		if (playerLocation != PLAYER_LOCATION.MAIN_ROON)
		{
			if (playerLocation == PLAYER_LOCATION.BATH_ROOM)
			{
				HitmanBehaviour.Ins.LeaveMainRoom();
			}
		}
		else
		{
			HitmanBehaviour.Ins.LeaveMainRoom();
		}
	}

	private void mainRoomStageHunt()
	{
		int num = 0;
		hideController.Ins.HideTrigger.LockedOut = true;
		hideController.Ins.HideTrigger.LeaveDoom = true;
		hideController.Ins.HideTrigger.StageLeaveDoomActions.Event += this.stageLeftHidingJump;
		hideController.Ins.HideTrigger.LeaveDoomActions.Event += this.leftHidingJump;
		if (InventoryManager.GetProductCount(HARDWARE_PRODUCTS.POLICE_SCANNER) > 0 && PoliceScannerBehaviour.Ins != null && PoliceScannerBehaviour.Ins.IsOn)
		{
			num++;
		}
		if (RouterBehaviour.Ins.Owned && RouterBehaviour.Ins.RouterIsActive)
		{
			num++;
		}
		if (ComputerPowerHook.Ins.PowerOn)
		{
			GameManager.BehaviourManager.NotesBehaviour.ClearNotes();
			double currentCurrency = (double)CurrencyManager.CurrentCurrency;
			float num2 = UnityEngine.Random.Range(0.5f, 0.9f);
			CurrencyManager.RemoveCurrency((float)Math.Round(currentCurrency * (double)num2, 3));
			num++;
		}
		for (int i = 0; i < this.lightsToCheck.Length; i++)
		{
			if (this.lightsToCheck[i].LightsAreOn)
			{
				num++;
			}
		}
		if (LookUp.Doors.MainDoor.SetDistanceLODs != null)
		{
			for (int j = 0; j < LookUp.Doors.MainDoor.SetDistanceLODs.Length; j++)
			{
				LookUp.Doors.MainDoor.SetDistanceLODs[j].OverwriteCulling = true;
			}
		}
		this.patrolCount = Mathf.Max(3, num * 2);
		HitmanBehaviour.Ins.SplineMove.PathIsCompleted += this.mainRoomHunt;
		HitmanBehaviour.Ins.EnterMainRoom();
	}

	private void mainRoomHunt()
	{
		hideController.Ins.PlayerPeakingEvent.Event += this.playerIsPeaking;
		this.peekCount = 0;
		this.peakingTimeStamp = Time.time;
		this.checkIfPlayerIsPeaking = true;
		if (OptionDataHook.Ins.Options.Mic)
		{
			GameManager.BehaviourManager.PlayerAudioBehaviour.CurrentPlayersLoudLevel.Event += this.playerVoiceLevel;
			this.micCheckCount = 0;
			this.micLoudHitCount = 0;
			this.micCheckTimeStamp = Time.time;
			this.checkMicCheck = true;
		}
		hideController.Ins.HideTrigger.LockedOut = false;
		HitmanBehaviour.Ins.SplineMove.PathIsCompleted -= this.mainRoomHunt;
		LookUp.Doors.MainDoor.ForceDoorClose();
		if (FlashLightBehaviour.Ins.LightOn)
		{
			this.stageCaughtDoom();
		}
		else
		{
			FlashLightBehaviour.Ins.FlashLightWentOn.Event += this.flashLightWasTriggered;
		}
		this.pickPatrolPoint();
	}

	private void playerIsPeaking(float SetValue)
	{
		this.playerPeaking = (SetValue >= 0.5f);
	}

	private void playerIsPeakingDoom(float SetValue)
	{
		if (SetValue <= 0.65f)
		{
			hideController.Ins.PlayerPeakingEvent.Event -= this.playerIsPeakingDoom;
			this.triggerCaughtDoom();
		}
	}

	private void playerVoiceLevel(float LoudLevel)
	{
		if (LoudLevel >= 0.65f)
		{
			this.micLoudHitCount++;
		}
	}

	private void pickPatrolPoint()
	{
		PLAYER_LOCATION playerLocation = StateManager.PlayerLocation;
		if (playerLocation != PLAYER_LOCATION.MAIN_ROON)
		{
			if (playerLocation == PLAYER_LOCATION.BATH_ROOM)
			{
				int num = UnityEngine.Random.Range(1, this.bathRoomPatrolPoints.Length);
				PatrolPointDefinition patrolPointDefinition = this.bathRoomPatrolPoints[num];
				HitmanBehaviour.Ins.PatrolTo(patrolPointDefinition);
				this.bathRoomPatrolPoints[num] = this.bathRoomPatrolPoints[0];
				this.bathRoomPatrolPoints[0] = patrolPointDefinition;
			}
		}
		else
		{
			int num = UnityEngine.Random.Range(1, this.mainRoomPatrolPoints.Length);
			PatrolPointDefinition patrolPointDefinition = this.mainRoomPatrolPoints[num];
			HitmanBehaviour.Ins.PatrolTo(patrolPointDefinition);
			this.mainRoomPatrolPoints[num] = this.mainRoomPatrolPoints[0];
			this.mainRoomPatrolPoints[0] = patrolPointDefinition;
		}
	}

	private void patrolOut()
	{
		if (!this.lockOutExit)
		{
			PLAYER_LOCATION playerLocation = StateManager.PlayerLocation;
			if (playerLocation != PLAYER_LOCATION.MAIN_ROON)
			{
				if (playerLocation == PLAYER_LOCATION.BATH_ROOM)
				{
					HitmanBehaviour.Ins.PatrolTo(this.mainRoomExitPatrolPoint);
				}
			}
			else
			{
				HitmanBehaviour.Ins.PatrolTo(this.mainRoomExitPatrolPoint);
			}
		}
		FlashLightBehaviour.Ins.FlashLightWentOn.Event -= this.flashLightWasTriggered;
	}

	private void stageLeftHidingJump()
	{
		hideController.Ins.PlayerPeakingEvent.Event -= this.playerIsPeaking;
		GameManager.BehaviourManager.PlayerAudioBehaviour.CurrentPlayersLoudLevel.Event -= this.playerVoiceLevel;
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		if (OptionDataHook.Ins.Options.Mic)
		{
			MicMeterHook.Ins.DismissMicGroup(0.25f);
		}
		GameManager.TimeSlinger.KillTimer(this.patrolTimer);
		HitmanBehaviour.Ins.KillPatrol();
		PLAYER_LOCATION playerLocation = StateManager.PlayerLocation;
		if (playerLocation != PLAYER_LOCATION.MAIN_ROON)
		{
			if (playerLocation == PLAYER_LOCATION.BATH_ROOM)
			{
				if (HitmanBehaviour.Ins.InBathRoom)
				{
					this.showerJump.Stage();
				}
				else
				{
					PauseManager.UnLockPause();
					GameManager.InteractionManager.UnLockInteraction();
					hideController.Ins.HideTrigger.LockedOut = false;
					hideController.Ins.HideTrigger.LeaveDoom = false;
					this.bathroomJump.Stage();
					LookUp.Doors.BathroomDoor.DoorOpenEvent.AddListener(new UnityAction(this.bathroomJump.Execute));
					LookUp.Doors.BathroomDoor.LockOutAutoClose = true;
				}
			}
		}
		else
		{
			this.wardrobeJump.Stage();
		}
	}

	private void leftHidingJump()
	{
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event += EnemyManager.HitManManager.GunFlashGameOver;
		hideController.Ins.HideTrigger.StageLeaveDoomActions.Event -= this.stageLeftHidingJump;
		hideController.Ins.HideTrigger.LeaveDoomActions.Event -= this.leftHidingJump;
		PLAYER_LOCATION playerLocation = StateManager.PlayerLocation;
		if (playerLocation != PLAYER_LOCATION.MAIN_ROON)
		{
			if (playerLocation == PLAYER_LOCATION.BATH_ROOM)
			{
				if (HitmanBehaviour.Ins.InBathRoom)
				{
					this.showerJump.Execute();
				}
			}
		}
		else
		{
			this.wardrobeJump.Execute();
		}
	}

	private void stageCaughtDoom()
	{
		this.lockOutExit = true;
		if (OptionDataHook.Ins.Options.Mic)
		{
			GameManager.BehaviourManager.PlayerAudioBehaviour.CurrentPlayersLoudLevel.Event -= this.playerVoiceLevel;
		}
		hideController.Ins.PlayerPeakingEvent.Event -= this.playerIsPeaking;
		hideController.Ins.PlayerPeakingEvent.Event += this.playerIsPeakingDoom;
		hideController.Ins.HideTrigger.LockedOut = true;
		hideController.Ins.HideTrigger.LeaveDoom = false;
	}

	private void triggerCaughtDoom()
	{
		DataManager.LockSave = true;
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		GameManager.TimeSlinger.KillTimer(this.patrolTimer);
		HitmanBehaviour.Ins.KillPatrol();
		hideController.Ins.HideTrigger.LockedOut = true;
		hideController.Ins.HideTrigger.LeaveDoom = false;
		hideController.Ins.HideTrigger.StageLeaveDoomActions.Event -= this.stageLeftHidingJump;
		hideController.Ins.HideTrigger.LeaveDoomActions.Event -= this.leftHidingJump;
		PLAYER_LOCATION playerLocation = StateManager.PlayerLocation;
		if (playerLocation != PLAYER_LOCATION.MAIN_ROON)
		{
			if (playerLocation == PLAYER_LOCATION.BATH_ROOM)
			{
				if (HitmanBehaviour.Ins.InBathRoom)
				{
					this.showerCaughtJump.Execute();
				}
				else
				{
					PauseManager.UnLockPause();
					GameManager.InteractionManager.UnLockInteraction();
					hideController.Ins.HideTrigger.LockedOut = false;
					hideController.Ins.HideTrigger.LeaveDoom = false;
					this.bathroomJump.Stage();
					LookUp.Doors.BathroomDoor.DoorOpenEvent.AddListener(new UnityAction(this.bathroomJump.Execute));
					GameManager.TimeSlinger.FireTimer(5f, new Action(LookUp.Doors.MainDoor.ForceOpenDoor), 0);
					GameManager.TimeSlinger.FireTimer(8f, new Action(LookUp.Doors.MainDoor.ForceDoorClose), 0);
				}
			}
		}
		else
		{
			this.wardrobeCaughtJump.Execute();
		}
	}

	private void flashLightWasTriggered(bool IsOn)
	{
		if (IsOn)
		{
			FlashLightBehaviour.Ins.FlashLightWentOn.Event -= this.flashLightWasTriggered;
			this.stageCaughtDoom();
		}
	}

	private void Awake()
	{
		HitmanPatrolBehaviour.Ins = this;
	}

	private void Update()
	{
		if (this.checkIfPlayerIsPeaking && Time.time - this.peakingTimeStamp >= EnemyManager.HitManManager.Data.CheckPeakingInterval)
		{
			this.peakingTimeStamp = Time.time;
			if (this.playerPeaking)
			{
				this.peekCount++;
			}
			if (this.peekCount >= EnemyManager.HitManManager.Data.MaxPeakCount)
			{
				this.checkIfPlayerIsPeaking = false;
				this.checkMicCheck = false;
				this.stageCaughtDoom();
			}
		}
		if (this.checkMicCheck && Time.time - this.micCheckTimeStamp >= EnemyManager.HitManManager.Data.MicCheckInterval)
		{
			this.micCheckTimeStamp = Time.time;
			if (this.micLoudHitCount >= EnemyManager.HitManManager.Data.AddMicCheckThreshold)
			{
				this.micCheckCount++;
			}
			if (this.micCheckCount >= EnemyManager.HitManManager.Data.MaxMicCheck)
			{
				this.checkIfPlayerIsPeaking = false;
				this.checkMicCheck = false;
				this.stageCaughtDoom();
			}
			this.micLoudHitCount = 0;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		for (int i = 0; i < this.mainRoomPatrolPoints.Length; i++)
		{
			Gizmos.DrawWireCube(this.mainRoomPatrolPoints[i].Position, new Vector3(0.2f, 0.2f, 0.2f));
		}
		Gizmos.color = Color.blue;
		for (int j = 0; j < this.bathRoomPatrolPoints.Length; j++)
		{
			Gizmos.DrawWireCube(this.bathRoomPatrolPoints[j].Position, new Vector3(0.2f, 0.2f, 0.2f));
		}
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(this.mainRoomExitPatrolPoint.Position, new Vector3(0.2f, 0.2f, 0.2f));
	}

	public static HitmanPatrolBehaviour Ins;

	[SerializeField]
	private LightTrigger[] lightsToCheck = new LightTrigger[0];

	[SerializeField]
	private PatrolPointDefinition mainRoomExitPatrolPoint;

	[SerializeField]
	private PatrolPointDefinition[] mainRoomPatrolPoints = new PatrolPointDefinition[0];

	[SerializeField]
	private PatrolPointDefinition[] bathRoomPatrolPoints = new PatrolPointDefinition[0];

	private float peakingTimeStamp;

	private float micCheckTimeStamp;

	private bool playerPeaking;

	private bool checkIfPlayerIsPeaking;

	private bool checkMicCheck;

	private bool lockOutExit;

	private int patrolCount;

	private int peekCount;

	private int micCheckCount;

	private int micLoudHitCount;

	private Timer patrolTimer;

	private Timer lockHideControllerTimer;

	private HitmanWardrobeJump wardrobeJump = new HitmanWardrobeJump();

	private HitmanWardrobeCaughtJump wardrobeCaughtJump = new HitmanWardrobeCaughtJump();

	private HitmanShowerJump showerJump = new HitmanShowerJump();

	private HitmanBathroomJump bathroomJump = new HitmanBathroomJump();

	private HitmanShowerCaughtJump showerCaughtJump = new HitmanShowerCaughtJump();
}
