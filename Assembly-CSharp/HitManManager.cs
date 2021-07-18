using System;
using UnityEngine;
using UnityEngine.Events;

public class HitManManager : MonoBehaviour
{
	public Timer LockPickTimer
	{
		get
		{
			return this.lockPickTimer;
		}
	}

	public HitmanDataDefinition Data
	{
		get
		{
			return this.data;
		}
	}

	public void SpawnHitman(HitmanSpawnDefinition SpawnData)
	{
		HitmanBehaviour.Ins.SpawnData = SpawnData;
		this.spawn();
	}

	public void GunFlashGameOver()
	{
		DataManager.ClearGameData();
		MainCameraHook.Ins.ClearARF(2f);
		HitmanBehaviour.Ins.GunFlashDoneEvents.Event -= this.GunFlashGameOver;
		UIManager.TriggerHardGameOver("ASSASSINATED");
		HitmanRoamJumper.Ins.ClearPPVol();
	}

	public void ProxyHitEnd()
	{
		if (!this.proxyHitSpawn)
		{
			this.failSafeTrigger.FailSafe();
		}
	}

	public void DeSpawn()
	{
		EnemyManager.State = ENEMY_STATE.IDLE;
		EnvironmentManager.PowerBehaviour.LockedOut = false;
		DataManager.LockSave = false;
		this.generateFireWindow();
	}

	public void ExecuteLobbyComputerJump()
	{
		this.lobbyComputerJump.Execute();
	}

	private void stageHitmanPatrol()
	{
		if (EnemyManager.State == ENEMY_STATE.IDLE)
		{
			if (EnvironmentManager.PowerState == POWER_STATE.ON)
			{
				if (!StateManager.BeingHacked)
				{
					EnemyManager.State = ENEMY_STATE.HITMAN;
					EnvironmentManager.PowerBehaviour.LockedOut = true;
					DataManager.LockSave = true;
					this.triggerHitmanPatrol();
				}
				else
				{
					this.fireWindow = 30f;
					this.fireWindowActive = true;
				}
			}
			else
			{
				this.fireWindow *= 0.35f;
				this.fireWindowActive = true;
			}
		}
		else
		{
			this.fireWindow /= 2f;
			this.fireWindowActive = true;
		}
	}

	private void triggerHitmanPatrol()
	{
		this.proxyHitSpawn = false;
		this.BeginPatrolEvents.Invoke();
	}

	private void spawn()
	{
		this.proxyHitSpawn = true;
		if (!LookUp.Doors.MainDoor.DoingSomething)
		{
			if (StateManager.PlayerState != PLAYER_STATE.BUSY)
			{
				StateManager.PlayerStateChangeEvents.Event -= this.spawn;
				if (StateManager.PlayerLocation != PLAYER_LOCATION.UNKNOWN)
				{
					StateManager.PlayerLocationChangeEvents.Event -= this.spawn;
					if (StateManager.PlayerLocation == PLAYER_LOCATION.BATH_ROOM || StateManager.PlayerLocation == PLAYER_LOCATION.OUTSIDE || StateManager.PlayerLocation == PLAYER_LOCATION.MAIN_ROON)
					{
						LookUp.Doors.MainDoor.DoorOpenEvent.AddListener(new UnityAction(this.mainDoorJump.Stage));
						LookUp.Doors.MainDoor.DoorWasOpenedEvent.AddListener(new UnityAction(this.mainDoorJump.Execute));
						HitmanBehaviour.Ins.ReachedEndPath.Event += this.mainDoorSpawnAction;
						HitmanBehaviour.Ins.Spawn(null);
					}
					else
					{
						GameManager.TimeSlinger.FireTimer(this.data.NotInMainRoomLeeyWayTime, new Action(this.subSpawnCheck), 0);
					}
				}
				else
				{
					StateManager.PlayerLocationChangeEvents.Event += this.spawn;
				}
			}
			else
			{
				StateManager.PlayerStateChangeEvents.Event += this.spawn;
			}
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.spawn), 0);
		}
	}

	private void subSpawnCheck()
	{
		if (!LookUp.Doors.MainDoor.DoingSomething)
		{
			if (StateManager.PlayerState != PLAYER_STATE.BUSY)
			{
				StateManager.PlayerStateChangeEvents.Event -= this.spawn;
				if (StateManager.PlayerLocation != PLAYER_LOCATION.UNKNOWN)
				{
					StateManager.PlayerLocationChangeEvents.Event -= this.spawn;
					if (StateManager.PlayerLocation == PLAYER_LOCATION.BATH_ROOM || StateManager.PlayerLocation == PLAYER_LOCATION.OUTSIDE || StateManager.PlayerLocation == PLAYER_LOCATION.MAIN_ROON)
					{
						LookUp.Doors.MainDoor.DoorOpenEvent.AddListener(new UnityAction(this.mainDoorJump.Stage));
						LookUp.Doors.MainDoor.DoorWasOpenedEvent.AddListener(new UnityAction(this.mainDoorJump.Execute));
						HitmanBehaviour.Ins.ReachedEndPath.Event += this.mainDoorSpawnAction;
						HitmanBehaviour.Ins.Spawn(null);
					}
					else
					{
						this.outSideMainRoomAction();
					}
				}
				else
				{
					StateManager.PlayerLocationChangeEvents.Event += this.spawn;
				}
			}
			else
			{
				StateManager.PlayerStateChangeEvents.Event += this.spawn;
			}
		}
		else
		{
			GameManager.TimeSlinger.FireTimer(0.5f, new Action(this.spawn), 0);
		}
	}

	private void mainDoorSpawnAction()
	{
		HitmanBehaviour.Ins.ReachedEndPath.Event -= this.mainDoorSpawnAction;
		GameManager.TimeSlinger.FireTimer(1f, delegate()
		{
			if (LookUp.Doors.MainDoor.Locked)
			{
				LookUp.Doors.MainDoor.AudioHub.PlaySound(LookUp.SoundLookUp.DoorKnobSFX);
				if (ModsManager.EasyModeActive)
				{
					LookUp.Doors.MainDoor.AudioHub.PlaySoundCustomDelay(LookUp.SoundLookUp.DoorKnobSFX, 1f);
					LookUp.Doors.MainDoor.AudioHub.PlaySoundCustomDelay(LookUp.SoundLookUp.DoorKnobSFX, 2f);
				}
				HitmanBehaviour.Ins.TriggerAnim("lockPick");
				GameManager.TimeSlinger.FireHardTimer(out this.lockPickTimer, this.data.LockPickingTime, new Action(this.mainRoomAttackAction), 0);
				return;
			}
			this.mainRoomAttackAction();
		}, 0);
	}

	private void mainRoomAttackAction()
	{
		if (StateManager.PlayerState != PLAYER_STATE.BUSY)
		{
			HitmanBehaviour.Ins.TriggerAnim("idle");
			StateManager.PlayerStateChangeEvents.Event -= this.mainRoomAttackAction;
			if (StateManager.PlayerState == PLAYER_STATE.HIDING)
			{
				LookUp.Doors.MainDoor.DoorOpenEvent.RemoveListener(new UnityAction(this.mainDoorJump.Stage));
				LookUp.Doors.MainDoor.DoorWasOpenedEvent.RemoveListener(new UnityAction(this.mainDoorJump.Execute));
				HitmanPatrolBehaviour.Ins.Patrol();
			}
			else if (StateManager.PlayerLocation == PLAYER_LOCATION.BATH_ROOM && !LookUp.Doors.BathroomDoor.DoingSomething)
			{
				this.bathroomJump.Stage();
				LookUp.Doors.BathroomDoor.DoorOpenEvent.AddListener(new UnityAction(this.bathroomJump.Execute));
			}
			else if (StateManager.PlayerState == PLAYER_STATE.COMPUTER)
			{
				HitmanComputerJumper.Ins.AddComputerJump();
			}
			else
			{
				if (StateManager.PlayerState == PLAYER_STATE.PEEPING)
				{
					HitmanBehaviour.Ins.WalkAwayFromMainDoor();
				}
				HitmanPeepJumper.Ins.AddPeepJump();
				LookUp.Doors.BathroomDoor.DoorWasClosedEvent.AddListener(new UnityAction(this.checkBathroomDoorJump));
				HitmanComputerJumper.Ins.AddDelayComputerJump();
			}
		}
		else
		{
			StateManager.PlayerStateChangeEvents.Event += this.mainRoomAttackAction;
		}
	}

	private void checkBathroomDoorJump()
	{
		if (StateManager.PlayerLocation == PLAYER_LOCATION.BATH_ROOM)
		{
			LookUp.Doors.BathroomDoor.DoorWasClosedEvent.RemoveListener(new UnityAction(this.checkBathroomDoorJump));
			this.bathroomJump.Stage();
			LookUp.Doors.BathroomDoor.DoorOpenEvent.AddListener(new UnityAction(this.bathroomJump.Execute));
		}
	}

	private void generateFireWindow()
	{
		this.fireWindow = UnityEngine.Random.Range(this.data.FireWindowMin, this.data.FireWindowMax);
		if (DataManager.LeetMode)
		{
			this.fireWindow *= 0.4f;
		}
		this.fireTimeStamp = Time.time;
		this.fireWindowActive = true;
	}

	private void outSideMainRoomAction()
	{
		LookUp.Doors.MainDoor.DoorOpenEvent.AddListener(new UnityAction(this.mainDoorOutsideJump.Stage));
		LookUp.Doors.MainDoor.DoorWasOpenedEvent.AddListener(new UnityAction(this.mainDoorOutsideJump.Execute));
		if (StateManager.PlayerState == PLAYER_STATE.LOBBY_COMPUTER)
		{
			this.lobbyComputerJump.Stage();
			HitmanRoamJumper.Ins.AddLobbyComputerJump();
		}
		else
		{
			switch (StateManager.PlayerLocation)
			{
			case PLAYER_LOCATION.HALL_WAY10:
				LookUp.Doors.Door10.DoorOpenEvent.AddListener(new UnityAction(this.floor10Jump.Stage));
				LookUp.Doors.Door10.DoorWasOpenedEvent.AddListener(new UnityAction(this.floor10Jump.Execute));
				break;
			case PLAYER_LOCATION.HALL_WAY8:
				LookUp.Doors.Door8.DoorOpenEvent.AddListener(new UnityAction(this.floor8Jump.Stage));
				LookUp.Doors.Door8.DoorWasOpenedEvent.AddListener(new UnityAction(this.floor8Jump.Execute));
				break;
			case PLAYER_LOCATION.HALL_WAY6:
				LookUp.Doors.Door6.DoorOpenEvent.AddListener(new UnityAction(this.floor6Jump.Stage));
				LookUp.Doors.Door6.DoorWasOpenedEvent.AddListener(new UnityAction(this.floor6Jump.Execute));
				break;
			case PLAYER_LOCATION.HALL_WAY5:
				LookUp.Doors.Door5.DoorOpenEvent.AddListener(new UnityAction(this.floor5Jump.Stage));
				LookUp.Doors.Door5.DoorWasOpenedEvent.AddListener(new UnityAction(this.floor5Jump.Execute));
				break;
			case PLAYER_LOCATION.HALL_WAY3:
				LookUp.Doors.Door3.DoorOpenEvent.AddListener(new UnityAction(this.floor3Jump.Stage));
				LookUp.Doors.Door3.DoorWasOpenedEvent.AddListener(new UnityAction(this.floor3Jump.Execute));
				break;
			case PLAYER_LOCATION.HALL_WAY1:
				LookUp.Doors.Door1.DoorOpenEvent.AddListener(new UnityAction(this.floor1Jump.Stage));
				LookUp.Doors.Door1.DoorWasOpenedEvent.AddListener(new UnityAction(this.floor1Jump.Execute));
				break;
			case PLAYER_LOCATION.STAIR_WAY:
				LookUp.Doors.Door1.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor1Jump.Stage));
				LookUp.Doors.Door1.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor1Jump.Execute));
				LookUp.Doors.Door3.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor3Jump.Stage));
				LookUp.Doors.Door3.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor3Jump.Execute));
				LookUp.Doors.Door5.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor5Jump.Stage));
				LookUp.Doors.Door5.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor5Jump.Execute));
				LookUp.Doors.Door6.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor6Jump.Stage));
				LookUp.Doors.Door6.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor6Jump.Execute));
				LookUp.Doors.Door8.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor8Jump.Stage));
				LookUp.Doors.Door8.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor8Jump.Execute));
				LookUp.Doors.Door10.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor10Jump.Stage));
				LookUp.Doors.Door10.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor10Jump.Execute));
				break;
			case PLAYER_LOCATION.MAINTENANCE_ROOM:
				LookUp.Doors.Door1.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor1Jump.Stage));
				LookUp.Doors.Door1.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor1Jump.Execute));
				LookUp.Doors.Door3.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor3Jump.Stage));
				LookUp.Doors.Door3.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor3Jump.Execute));
				LookUp.Doors.Door5.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor5Jump.Stage));
				LookUp.Doors.Door5.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor5Jump.Execute));
				LookUp.Doors.Door6.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor6Jump.Stage));
				LookUp.Doors.Door6.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor6Jump.Execute));
				LookUp.Doors.Door8.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor8Jump.Stage));
				LookUp.Doors.Door8.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor8Jump.Execute));
				LookUp.Doors.Door10.DoorOpenEvent.AddListener(new UnityAction(this.stairwayDoor10Jump.Stage));
				LookUp.Doors.Door10.DoorWasOpenedEvent.AddListener(new UnityAction(this.stairwayDoor10Jump.Execute));
				break;
			case PLAYER_LOCATION.LOBBY:
				LookUp.Doors.Door1.DoorOpenEvent.AddListener(new UnityAction(this.floor1Jump.Stage));
				LookUp.Doors.Door1.DoorWasOpenedEvent.AddListener(new UnityAction(this.floor1Jump.Execute));
				break;
			case PLAYER_LOCATION.DEAD_DROP:
				LookUp.Doors.Door1.DoorOpenEvent.AddListener(new UnityAction(this.floor1Jump.Stage));
				LookUp.Doors.Door1.DoorWasOpenedEvent.AddListener(new UnityAction(this.floor1Jump.Execute));
				break;
			case PLAYER_LOCATION.LOBBY_COMPUTER:
				LookUp.Doors.Door1.DoorOpenEvent.AddListener(new UnityAction(this.floor1Jump.Stage));
				LookUp.Doors.Door1.DoorWasOpenedEvent.AddListener(new UnityAction(this.floor1Jump.Execute));
				break;
			case PLAYER_LOCATION.DEAD_DROP_ROOM:
				LookUp.Doors.Door1.DoorOpenEvent.AddListener(new UnityAction(this.floor1Jump.Stage));
				LookUp.Doors.Door1.DoorWasOpenedEvent.AddListener(new UnityAction(this.floor1Jump.Execute));
				break;
			}
		}
	}

	private void saveMyData()
	{
		DataManager.Save<HitManData>(this.myHitmanData);
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myHitmanData = DataManager.Load<HitManData>(7);
		if (this.myHitmanData == null)
		{
			this.myHitmanData = new HitManData(7);
			this.myHitmanData.IsActivated = false;
			this.myHitmanData.KeysDiscoveredCount = 0;
			this.myHitmanData.TimeLeftOnWindow = 0f;
		}
		this.keyDiscoverCount = this.myHitmanData.KeysDiscoveredCount;
		base.InvokeRepeating("saveMyData", 0f, 30f);
	}

	private void threatsActivated()
	{
		GameManager.StageManager.ThreatsNowActivated -= this.threatsActivated;
		if (this.myHitmanData.IsActivated)
		{
			this.hitmanActivated = true;
			if (this.myHitmanData.TimeLeftOnWindow >= 10f)
			{
				this.fireTimeStamp = Time.time;
				this.fireWindow = this.myHitmanData.TimeLeftOnWindow;
				this.fireWindowActive = true;
			}
			else
			{
				this.generateFireWindow();
			}
		}
	}

	private void keyWasDiscovered()
	{
		if (!this.hitmanActivated)
		{
			this.keyDiscoverCount++;
			this.myHitmanData.KeysDiscoveredCount++;
			if (this.keyDiscoverCount >= this.data.KeysRequiredToTrigger)
			{
				this.hitmanActivated = true;
				this.myHitmanData.IsActivated = true;
				this.generateFireWindow();
			}
			DataManager.Save<HitManData>(this.myHitmanData);
		}
	}

	private void Awake()
	{
		EnemyManager.HitManManager = this;
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.ThreatsNowActivated += this.threatsActivated;
		GameManager.TheCloud.KeyDiscoveredEvent.Event += this.keyWasDiscovered;
	}

	private void Update()
	{
		if (this.fireWindowActive)
		{
			this.myHitmanData.TimeLeftOnWindow = this.fireWindow - (Time.time - this.fireTimeStamp);
			if (Time.time - this.fireTimeStamp >= this.fireWindow)
			{
				this.fireWindowActive = false;
				this.stageHitmanPatrol();
			}
		}
	}

	private void OnDestroy()
	{
		base.CancelInvoke("saveMyData");
	}

	public void ReleaseTheHitman()
	{
		if (!this.hitmanActivated)
		{
			this.hitmanActivated = true;
			this.myHitmanData.IsActivated = true;
			this.generateFireWindow();
		}
	}

	[SerializeField]
	private HitmanDataDefinition data;

	public UnityEvent BeginPatrolEvents = new UnityEvent();

	[SerializeField]
	private HitmanSpawnTrigger failSafeTrigger;

	[SerializeField]
	private EnemyHotZoneTrigger[] hotZoneTriggers = new EnemyHotZoneTrigger[0];

	private int keyDiscoverCount;

	private float fireTimeStamp;

	private float fireWindow;

	private bool fireWindowActive;

	private bool proxyHitSpawn;

	private bool hitmanActivated;

	private Timer lockPickTimer;

	private HitManData myHitmanData;

	private HitmanMainDoorJump mainDoorJump = new HitmanMainDoorJump();

	private HitmanBathroomJump bathroomJump = new HitmanBathroomJump();

	private HitmanStairWayDoor1Jump stairwayDoor1Jump = new HitmanStairWayDoor1Jump();

	private HitmanStairWayDoor3Jump stairwayDoor3Jump = new HitmanStairWayDoor3Jump();

	private HitmanStairWayDoor5Jump stairwayDoor5Jump = new HitmanStairWayDoor5Jump();

	private HitmanStairWayDoor6Jump stairwayDoor6Jump = new HitmanStairWayDoor6Jump();

	private HitmanStairWayDoor8Jump stairwayDoor8Jump = new HitmanStairWayDoor8Jump();

	private HitmanStairWayDoor10Jump stairwayDoor10Jump = new HitmanStairWayDoor10Jump();

	private HitmanFloor10Jump floor10Jump = new HitmanFloor10Jump();

	private HitmanFloor8Jump floor8Jump = new HitmanFloor8Jump();

	private HitmanFloor6Jump floor6Jump = new HitmanFloor6Jump();

	private HitmanFloor5Jump floor5Jump = new HitmanFloor5Jump();

	private HitmanFloor3Jump floor3Jump = new HitmanFloor3Jump();

	private HitmanFloor1Jump floor1Jump = new HitmanFloor1Jump();

	private HitmanMainDoorOutsideJump mainDoorOutsideJump = new HitmanMainDoorOutsideJump();

	private HitmanLobbyComputerJump lobbyComputerJump = new HitmanLobbyComputerJump();
}
