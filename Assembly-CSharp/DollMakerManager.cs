using System;
using System.Collections.Generic;
using UnityEngine;

public class DollMakerManager : MonoBehaviour
{
	public void ReleaseTheDollMaker()
	{
		if (!this.dollMakerReleased)
		{
			GameManager.StageManager.ManuallyActivateThreats();
			this.dollMakerReleased = true;
			this.myDollMakerData.IsReleased = true;
			DataManager.Save<DollMakerData>(this.myDollMakerData);
			this.generateReleaseWindow(false);
		}
	}

	public void DoorPowerTrip()
	{
		EnvironmentManager.PowerBehaviour.LockedOut = false;
		EnvironmentManager.PowerBehaviour.ForcePowerOff();
		EnvironmentManager.PowerBehaviour.PowerOnEvent.Event += this.playerTurnedPowerBackOn;
		EnvironmentManager.PowerBehaviour.LockedOut = true;
		GameManager.AudioSlinger.KillSound(this.dollMakerMusic);
		LookUp.Doors.MainDoor.EnableDoor();
		LookUp.Doors.BalconyDoor.EnableDoor();
		LookUp.Doors.BathroomDoor.EnableDoor();
		PauseManager.UnLockPause();
		this.dollMakerDoorHelperLight.enabled = false;
		this.dollMakerBehaviour.DeSpawn();
		peepHoleController.Ins.LockOutLeave = false;
		peepHoleController.Ins.ForceOut();
	}

	public void PlayerHitWarningTrigger()
	{
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.dollMakerBehaviour.StageSpeech();
		DollMakerRoamJumper.Ins.TriggerSpeechJump();
	}

	public void ClearWarningTrigger()
	{
		PauseManager.UnLockPause();
		GameManager.InteractionManager.UnLockInteraction();
		DataManager.LockSave = false;
		EnemyManager.State = ENEMY_STATE.IDLE;
		LookUp.Doors.MainDoor.DisableDoor();
		this.theMaker.MarkerWasPickedUp.Event += this.markerWasPickedUpForTheFirstTime;
		this.theMaker.SpawnMeTo(this.markerDefaultSpawnPOS, this.markerDefaultSpawnROT);
		this.activateMarkerTime();
		GameManager.TimeSlinger.FireTimer(2f, delegate()
		{
			MainCameraHook.Ins.ResetARF();
		}, 0);
	}

	public void MarkerWasPlaced(int UnitNumber, Vector3 SpawnPOS, Vector3 SpawnROT)
	{
		this.activeUnitNumber = UnitNumber;
		this.theMaker.SpawnMeTo(SpawnPOS, SpawnROT);
		this.myDollMakerData.ActiveUnitNumber = UnitNumber;
		DataManager.Save<DollMakerData>(this.myDollMakerData);
		for (int i = 0; i < this.markerTriggers.Length; i++)
		{
			this.markerTriggers[i].DeActivate();
		}
	}

	private void triggerDollMakerSpawn()
	{
		if (EnemyManager.State == ENEMY_STATE.IDLE && EnvironmentManager.PowerState == POWER_STATE.ON)
		{
			DataManager.LockSave = true;
			EnemyManager.State = ENEMY_STATE.DOLL_MAKER;
			EnvironmentManager.PowerBehaviour.LockedOut = true;
			if (StateManager.PlayerState == PLAYER_STATE.COMPUTER)
			{
				this.stageDoorSpawn();
			}
			else
			{
				computerController.Ins.EnterEvents.Event += this.stageDoorSpawn;
			}
		}
		else
		{
			this.delayWindow = 30f;
			this.delayTimeStamp = Time.time;
			this.delayActive = true;
		}
	}

	private void stageDoorSpawn()
	{
		computerController.Ins.EnterEvents.Event -= this.stageDoorSpawn;
		this.dollMakerDoorHelperLight.enabled = true;
		this.dollMakerBehaviour.StageDoorSpawn();
		GameManager.AudioSlinger.MuffleAudioHub(AUDIO_HUB.MANIKIN_HUB, 0.25f, 0f);
		GameManager.AudioSlinger.PlaySound(this.dollMakerMusic);
		peepHoleController.Ins.TakingOverEvents.Event += this.playerIsEnteringPeepHole;
		peepHoleController.Ins.TookOverEvents.Event += this.playerEnteredPeepHole;
		LookUp.Doors.MainDoor.DisableDoor();
		LookUp.Doors.BalconyDoor.DisableDoor();
		LookUp.Doors.BathroomDoor.DisableDoor();
		this.dollMakerBehaviour.ManikinTransform.localPosition = new Vector3(-0.002f, 0f, 0f);
		GameManager.TimeSlinger.FireHardTimer(out this.forcePowerTripTimer, this.myData.ForcePowerTripTime, new Action(this.forcePowerTrip), 0);
	}

	private void generateReleaseWindow()
	{
		this.delayWindow = UnityEngine.Random.Range(this.myData.DelayTimeMin, this.myData.DelayTimeMax);
		this.delayTimeStamp = Time.time;
		this.delayActive = true;
	}

	private void forcePowerTrip()
	{
		peepHoleController.Ins.LockOutLeave = false;
		peepHoleController.Ins.TakingOverEvents.Event -= this.playerIsEnteringPeepHole;
		peepHoleController.Ins.TookOverEvents.Event -= this.playerEnteredPeepHole;
		EnvironmentManager.PowerBehaviour.LockedOut = false;
		EnvironmentManager.PowerBehaviour.ForcePowerOff();
		EnvironmentManager.PowerBehaviour.PowerOnEvent.Event += this.playerTurnedPowerBackOn;
		EnvironmentManager.PowerBehaviour.LockedOut = true;
		GameManager.AudioSlinger.KillSound(this.dollMakerMusic);
		LookUp.Doors.MainDoor.EnableDoor();
		LookUp.Doors.BalconyDoor.EnableDoor();
		LookUp.Doors.BathroomDoor.EnableDoor();
		this.dollMakerDoorHelperLight.enabled = false;
		this.dollMakerBehaviour.DeSpawn();
		this.dollMakerBehaviour.TriggerAnim("ExitDollGrabIdle");
	}

	private void playerTurnedPowerBackOn()
	{
		EnvironmentManager.PowerBehaviour.PowerOnEvent.Event -= this.playerTurnedPowerBackOn;
		EnvironmentManager.PowerBehaviour.ResetPowerTripTime();
		EnvironmentManager.PowerBehaviour.LockedOut = false;
		this.warningTrigger.SetActive();
	}

	private void markerWasPickedUpForTheFirstTime()
	{
		this.myDollMakerData.IsActivated = true;
		DataManager.Save<DollMakerData>(this.myDollMakerData);
		this.theMaker.MarkerWasPickedUp.Event -= this.markerWasPickedUpForTheFirstTime;
		LookUp.Doors.MainDoor.EnableDoor();
	}

	private void activateMarkerTime()
	{
		this.markerWindow = UnityEngine.Random.Range(this.myData.MarkerCoolTimeMin, this.myData.MarkerCoolTimeMax);
		if (ModsManager.Nightmare)
		{
			this.markerWindow *= 0.5f;
		}
		this.markerTimeStamp = Time.time;
		this.markerActive = true;
	}

	private void triggerMarkerTimesUp()
	{
		StateManager.PlayerLocationChangeEvents.Event -= this.triggerMarkerTimesUp;
		StateManager.PlayerStateChangeEvents.Event -= this.triggerMarkerTimesUp;
		if ((EnemyManager.State != ENEMY_STATE.IDLE || EnvironmentManager.PowerState != POWER_STATE.ON || StateManager.BeingHacked) && (EnemyManager.State != ENEMY_STATE.DOLL_MAKER || EnvironmentManager.PowerState != POWER_STATE.ON || StateManager.BeingHacked))
		{
			this.markerWindow = 60f;
			this.markerTimeStamp = Time.time;
			this.markerActive = true;
			return;
		}
		if (StateManager.PlayerState == PLAYER_STATE.BUSY)
		{
			StateManager.PlayerStateChangeEvents.Event += this.triggerMarkerTimesUp;
			return;
		}
		if (StateManager.PlayerLocation == PLAYER_LOCATION.UNKNOWN)
		{
			StateManager.PlayerLocationChangeEvents.Event += this.triggerMarkerTimesUp;
			return;
		}
		EnemyManager.State = ENEMY_STATE.DOLL_MAKER;
		if (GameManager.ManagerSlinger.TenantTrackManager.CheckIfFemaleTenant(this.activeUnitNumber))
		{
			this.nextMarkerCheck();
			return;
		}
		if (GameManager.ManagerSlinger.TenantTrackManager.CheckLucas(this.activeUnitNumber))
		{
			this.myDollMakerData.IsSatisfied = true;
			EnemyManager.State = ENEMY_STATE.IDLE;
			DollMakerManager.Lucassed = true;
			return;
		}
		EnvironmentManager.PowerBehaviour.LockedOut = true;
		DataManager.LockSave = true;
		DataManager.ClearGameData();
		if (StateManager.PlayerState == PLAYER_STATE.DESK || StateManager.PlayerState == PLAYER_STATE.COMPUTER)
		{
			if (StateManager.PlayerState == PLAYER_STATE.COMPUTER)
			{
				this.dollMakerBehaviour.SpawnBehindDesk();
				computerController.Ins.LeaveEvents.Event += this.triggerComputerJump;
				return;
			}
			GameManager.InteractionManager.LockInteraction();
			this.dollMakerBehaviour.SpawnBehindDesk();
			this.triggerComputerJump();
			DollMakerDeskJumper.Ins.TriggerDeskJump();
			return;
		}
		else
		{
			if (StateManager.PlayerLocation != PLAYER_LOCATION.DEAD_DROP && StateManager.PlayerLocation != PLAYER_LOCATION.DEAD_DROP_ROOM)
			{
				this.dollMakerBehaviour.TriggerAnim("triggerUniJumpIdle");
				this.dollMakerBehaviour.NotInMeshEvents.Event += this.triggerBehindPlayerJump;
				this.dollMakerBehaviour.InMeshEvents.Event += this.reRollBehindPlayerJump;
				this.dollMakerBehaviour.AttemptSpawnBehindPlayer(roamController.Ins.transform, 0.93429f);
				return;
			}
			StateManager.PlayerLocationChangeEvents.Event += this.triggerMarkerTimesUp;
			return;
		}
	}

	private void triggerBehindPlayerJump()
	{
		this.dollMakerBehaviour.NotInMeshEvents.Event -= this.triggerBehindPlayerJump;
		this.dollMakerBehaviour.InMeshEvents.Event -= this.reRollBehindPlayerJump;
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.JumpHit5, 0.2f);
		this.dollMakerBehaviour.SpawnBehindPlayer(roamController.Ins.transform, 0.93429f);
		DollMakerRoamJumper.Ins.TriggerUniJump();
		this.dollMakerBehaviour.TriggerUniJump();
		GameManager.TimeSlinger.FireTimer(6.5f, delegate()
		{
			MainCameraHook.Ins.ClearARF(1f);
			UIManager.TriggerGameOver("YOU DISAPPOINTED THE DOLL MAKER");
		}, 0);
	}

	private void reRollBehindPlayerJump()
	{
		this.dollMakerBehaviour.NotInMeshEvents.Event -= this.triggerBehindPlayerJump;
		this.dollMakerBehaviour.InMeshEvents.Event -= this.reRollBehindPlayerJump;
		GameManager.TimeSlinger.FireTimer(0.2f, new Action(this.triggerMarkerTimesUp), 0);
	}

	private void triggerComputerJump()
	{
		ComputerChairObject.Ins.Hide();
		computerController.Ins.LeaveEvents.Event -= this.triggerComputerJump;
		DollMakerDeskJumper.Ins.TriggerDeskJump();
		GameManager.TimeSlinger.FireTimer(0.1f, delegate()
		{
			GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit5);
			this.dollMakerBehaviour.TriggerDeskJump();
		}, 0);
		GameManager.TimeSlinger.FireTimer(5.75f, delegate()
		{
			UIManager.TriggerGameOver("YOU DISAPPOINTED THE DOLL MAKER");
		}, 0);
	}

	private void nextMarkerCheck()
	{
		if ((StateManager.PlayerLocation == PLAYER_LOCATION.MAIN_ROON || StateManager.PlayerLocation == PLAYER_LOCATION.BATH_ROOM) && this.activeUnitNumber >= 600 && !this.myDollMakerData.PlayedHelpMeSound)
		{
			EnvironmentManager.PowerBehaviour.PowerOutageHub.PlaySound(this.helpMeSFX);
			this.myDollMakerData.PlayedHelpMeSound = true;
		}
		EnemyManager.State = ENEMY_STATE.IDLE;
		DollMakerData dollMakerData = this.myDollMakerData;
		int currentVictims = dollMakerData.CurrentVictims;
		dollMakerData.CurrentVictims = currentVictims + 1;
		this.myDollMakerData.UsedUnitNumbers.Add(this.activeUnitNumber);
		this.PriceUnit = this.activeUnitNumber;
		for (int i = 0; i < this.markerTriggers.Length; i++)
		{
			if (this.markerTriggers[i].UnitNumber == this.activeUnitNumber)
			{
				this.markerTriggers[i].LockOut();
				i = this.markerTriggers.Length;
			}
		}
		this.activeUnitNumber = 0;
		this.myDollMakerData.ActiveUnitNumber = 0;
		if (this.myDollMakerData.CurrentVictims < this.myData.TargetVictimCount || ModsManager.Nightmare)
		{
			float duration = UnityEngine.Random.Range(this.myData.MarkerResetTimeMin / (ModsManager.Nightmare ? 4f : 2f), this.myData.MarkerResetTimeMax / (ModsManager.Nightmare ? 4f : 2f));
			GameManager.TimeSlinger.FireTimer(duration, new Action(this.rePlaceMarker), 0);
			CurrencyManager.AddCurrency(GameManager.ManagerSlinger.TenantTrackManager.CheckDollMakerPrice(this.PriceUnit));
		}
		else
		{
			if (SteamSlinger.Ins != null)
			{
				SteamSlinger.Ins.UnlockSteamAchievement(STEAM_ACHIEVEMENT.DOLLMAKERPET);
			}
			CurrencyManager.AddCurrency(ModsManager.EasyModeActive ? 100f : 200f);
			this.myDollMakerData.IsSatisfied = true;
		}
		DataManager.Save<DollMakerData>(this.myDollMakerData);
	}

	private void rePlaceMarker()
	{
		StateManager.PlayerStateChangeEvents.Event -= this.rePlaceMarker;
		if (StateManager.PlayerState != PLAYER_STATE.COMPUTER)
		{
			StateManager.PlayerStateChangeEvents.Event += this.rePlaceMarker;
			return;
		}
		LookUp.Doors.MainDoor.AudioHub.PlaySound(this.makerQueSFX);
		LookUp.Doors.MainDoor.DisableDoor();
		this.theMaker.MarkerWasPickedUp.Event += this.markerWasPickedUpForTheFirstTime;
		this.theMaker.SpawnMeTo(new Vector3(-2.5611f, 40.6517f, -5.0414f), Vector3.zero);
		this.activateMarkerTime();
	}

	private void playerIsEnteringPeepHole()
	{
		GameManager.TimeSlinger.KillTimer(this.forcePowerTripTimer);
		peepHoleController.Ins.TakingOverEvents.Event -= this.playerIsEnteringPeepHole;
		GameManager.AudioSlinger.UnMuffleAudioHub(AUDIO_HUB.MANIKIN_HUB, 0.75f);
	}

	private void playerEnteredPeepHole()
	{
		peepHoleController.Ins.TookOverEvents.Event -= this.playerEnteredPeepHole;
		PauseManager.LockPause();
		peepHoleController.Ins.LockOutLeave = true;
		GameManager.TimeSlinger.FireTimer(2f, delegate()
		{
			this.dollMakerBehaviour.TriggerAnim("triggerDollGrab");
		}, 0);
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myDollMakerData = DataManager.Load<DollMakerData>(this.myID);
		if (this.myDollMakerData == null)
		{
			this.myDollMakerData = new DollMakerData(this.myID);
			this.myDollMakerData.IsReleased = false;
			this.myDollMakerData.IsActivated = false;
			this.myDollMakerData.CurrentVictims = 0;
			this.myDollMakerData.IsSatisfied = false;
			this.myDollMakerData.UsedUnitNumbers = new List<int>();
			this.myDollMakerData.ActiveUnitNumber = 0;
			this.myDollMakerData.PlayedHelpMeSound = false;
		}
	}

	private void threatsActivated()
	{
		GameManager.StageManager.ThreatsNowActivated -= this.threatsActivated;
		if (!this.myDollMakerData.IsSatisfied)
		{
			this.dollMakerReleased = this.myDollMakerData.IsReleased;
			this.dollMakerActivated = this.myDollMakerData.IsActivated;
			this.activeUnitNumber = this.myDollMakerData.ActiveUnitNumber;
			if (this.dollMakerReleased && !this.dollMakerActivated)
			{
				this.generateReleaseWindow();
			}
			else if (this.dollMakerReleased && this.dollMakerActivated)
			{
				for (int i = 0; i < this.myDollMakerData.UsedUnitNumbers.Count; i++)
				{
					for (int j = 0; j < this.markerTriggers.Length; j++)
					{
						if (this.myDollMakerData.UsedUnitNumbers[i] == this.markerTriggers[j].UnitNumber)
						{
							this.markerTriggers[j].LockOut();
							j = this.markerTriggers.Length;
						}
					}
				}
				if (this.activeUnitNumber > 0)
				{
					for (int k = 0; k < this.markerTriggers.Length; k++)
					{
						this.markerTriggers[k].DeActivate();
					}
				}
				else
				{
					UIInventoryManager.ShowDollMakerMarker();
					for (int l = 0; l < this.markerTriggers.Length; l++)
					{
						this.markerTriggers[l].Activate();
					}
				}
				this.activateMarkerTime();
			}
		}
	}

	private void markerWasPickedUp()
	{
		this.activeUnitNumber = 0;
		for (int i = 0; i < this.markerTriggers.Length; i++)
		{
			this.markerTriggers[i].Activate();
		}
	}

	private void Awake()
	{
		EnemyManager.DollMakerManager = this;
		this.myID = base.transform.position.GetHashCode();
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.ThreatsNowActivated += this.threatsActivated;
		this.theMaker.MarkerWasPickedUp.Event += this.markerWasPickedUp;
	}

	private void Update()
	{
		if (this.delayActive && Time.time - this.delayTimeStamp >= this.delayWindow)
		{
			this.delayActive = false;
			if (!this.forced)
			{
				this.triggerDollMakerSpawn();
			}
		}
		if (this.markerActive && Time.time - this.markerTimeStamp >= this.markerWindow)
		{
			this.markerActive = false;
			this.triggerMarkerTimesUp();
		}
	}

	private void OnDestroy()
	{
		this.theMaker.MarkerWasPickedUp.Event -= this.markerWasPickedUp;
	}

	private void generateReleaseWindow(bool instantly)
	{
		if (instantly)
		{
			this.delayWindow = (float)UnityEngine.Random.Range(5, 10);
		}
		else
		{
			this.delayWindow = UnityEngine.Random.Range(this.myData.DelayTimeMin, this.myData.DelayTimeMax);
		}
		this.delayTimeStamp = Time.time;
		this.delayActive = true;
	}

	public void ReleaseTheDollMaker(bool instantly)
	{
		if (!this.dollMakerReleased)
		{
			GameManager.StageManager.ManuallyActivateThreats();
			this.dollMakerReleased = true;
			this.myDollMakerData.IsReleased = true;
			DataManager.Save<DollMakerData>(this.myDollMakerData);
			this.generateReleaseWindow(true);
		}
	}

	public void ForceMarker()
	{
		if (this.forced)
		{
			return;
		}
		this.dollMakerActivated = true;
		this.forced = true;
		StateManager.PlayerStateChangeEvents.Event -= this.rePlaceMarker;
		if (StateManager.PlayerState == PLAYER_STATE.COMPUTER)
		{
			LookUp.Doors.MainDoor.AudioHub.PlaySound(this.makerQueSFX);
			LookUp.Doors.MainDoor.DisableDoor();
			this.theMaker.MarkerWasPickedUp.Event += this.markerWasPickedUpForTheFirstTime;
			this.theMaker.SpawnMeTo(new Vector3(-2.5611f, 40.6517f, -5.0414f), Vector3.zero);
			this.activateMarkerTime();
			return;
		}
		StateManager.PlayerStateChangeEvents.Event += this.rePlaceMarker;
	}

	private void activateQuickMarkerTime()
	{
		this.markerWindow = UnityEngine.Random.Range(this.myData.MarkerCoolTimeMin / 2f, this.myData.MarkerCoolTimeMax / 2f);
		this.markerTimeStamp = Time.time;
		this.markerActive = true;
	}

	public void ThrowAllTenants()
	{
		if (ModsManager.Nightmare)
		{
			GameManager.ManagerSlinger.TenantTrackManager.UnLockSystem();
			return;
		}
		for (int i = 0; i < GameManager.ManagerSlinger.TenantTrackManager.TenantDatas.Length; i++)
		{
			if (GameManager.ManagerSlinger.TenantTrackManager.TenantDatas[i].tenantUnit != 0)
			{
				GameManager.ManagerSlinger.TextDocManager.CreateTextDoc(GameManager.ManagerSlinger.TenantTrackManager.TenantDatas[i].tenantUnit.ToString(), string.Concat(new object[]
				{
					GameManager.ManagerSlinger.TenantTrackManager.TenantDatas[i].tenantName,
					Environment.NewLine,
					Environment.NewLine,
					"Age: ",
					GameManager.ManagerSlinger.TenantTrackManager.TenantDatas[i].tenantAge,
					Environment.NewLine,
					Environment.NewLine,
					GameManager.ManagerSlinger.TenantTrackManager.TenantDatas[i].tenantNotes
				}));
			}
		}
	}

	public string MarkerDebug
	{
		get
		{
			if (this.markerWindow - (Time.time - this.markerTimeStamp) > 0f)
			{
				return ((int)(this.markerWindow - (Time.time - this.markerTimeStamp))).ToString();
			}
			return 0.ToString();
		}
	}

	private const float DOLLMAKER_Y_OFFSET = 0.93429f;

	[SerializeField]
	private DollMakerDataDefinition myData;

	[SerializeField]
	private DollMakerBehaviour dollMakerBehaviour;

	[SerializeField]
	private Light dollMakerDoorHelperLight;

	[SerializeField]
	private AudioFileDefinition dollMakerMusic;

	[SerializeField]
	private AudioFileDefinition helpMeSFX;

	[SerializeField]
	private EnemyHotZoneTrigger warningTrigger;

	[SerializeField]
	private DollMakerMarkerBehaviour theMaker;

	[SerializeField]
	private Vector3 markerDefaultSpawnPOS = Vector3.zero;

	[SerializeField]
	private Vector3 markerDefaultSpawnROT = Vector3.zero;

	[SerializeField]
	private AudioFileDefinition makerQueSFX;

	[SerializeField]
	private DollMakerMarkerTrigger[] markerTriggers = new DollMakerMarkerTrigger[0];

	private float delayTimeStamp;

	private float delayWindow;

	private float markerWindow;

	private float markerTimeStamp;

	private bool dollMakerReleased;

	private bool dollMakerActivated;

	private bool delayActive;

	private bool markerActive;

	private int myID;

	private int activeUnitNumber;

	private DollMakerData myDollMakerData;

	private Timer forcePowerTripTimer;

	private int PriceUnit;

	private bool forced;

	public static bool Lucassed;
}
