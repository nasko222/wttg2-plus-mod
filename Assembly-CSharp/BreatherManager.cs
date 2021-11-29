using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class BreatherManager : MonoBehaviour
{
	public void PlayerSpawnedToDeadDrop()
	{
		DataManager.LockSave = true;
		if (this.firstProductWasPickedUp && this.threatsActive && EnemyManager.State == ENEMY_STATE.IDLE)
		{
			bool flag;
			if (DevTools.Ins != null && DevTools.Ins.forceBreather)
			{
				flag = true;
			}
			else
			{
				int num = UnityEngine.Random.Range(0, 100);
				if (ModsManager.UnlimitedStamina)
				{
					num -= 15;
				}
				if (ModsManager.Nightmare)
				{
					num -= 15;
				}
				switch (this.keyDiscoveryCount)
				{
				case 0:
					flag = (num < 20);
					break;
				case 1:
					flag = (num < 20);
					break;
				case 2:
					flag = (num < 25);
					break;
				case 3:
					flag = (num < 30);
					break;
				case 4:
					flag = (num < 35);
					break;
				case 5:
					flag = (num < 40);
					break;
				case 6:
					flag = (num < 45);
					break;
				case 7:
					flag = (num < 50);
					break;
				case 8:
					flag = (num < 65);
					break;
				default:
					flag = (num < 65);
					break;
				}
			}
			if (flag)
			{
				EnemyManager.State = ENEMY_STATE.BREATHER;
				if (!ModsManager.Nightmare)
				{
					int num2 = UnityEngine.Random.Range(0, this.audioQueSFXs.Length);
					this.audioQueHub.PlaySoundCustomDelay(this.audioQueSFXs[num2], 2f);
				}
				this.breatherIsActive = true;
				this.preLeaveTrigger.SetActive();
				this.pickUpTrigger.SetActive();
				braceController.Ins.PlayerEnteredEvent.Event += this.playerEnteredBraceMode;
				braceController.Ins.PlayerLeftEvent.Event += this.playerLeftBraceMode;
			}
		}
	}

	public void PlayerHitPreLeaveTrigger()
	{
		if (this.breatherIsActive)
		{
			DataManager.ClearGameData();
			PauseManager.LockPause();
			GameManager.InteractionManager.LockInteraction();
			this.spawnToLobbyTrigger.LockOut();
			GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.JumpHit7, 0.2f);
			this.breatherBehaviour.TriggerVoice(BREATHER_VOICE_COMMANDS.LAUGH1);
			this.breatherBehaviour.TriggerExitRush();
		}
	}

	public void PlayerEnteredPickUpLocation()
	{
		if (this.breatherIsActive)
		{
			DataManager.ClearGameData();
			PauseManager.LockPause();
			GameManager.InteractionManager.LockInteraction();
			this.spawnToLobbyTrigger.LockOut();
			if (roamController.Ins.transform.rotation.eulerAngles.y >= 60f && roamController.Ins.transform.rotation.eulerAngles.y <= 130f)
			{
				BreatherRoamJumper.Ins.StagePickUpJump();
				GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.JumpHit7, 0.2f);
				this.breatherBehaviour.TriggerVoice(BREATHER_VOICE_COMMANDS.LAUGH1);
				this.breatherBehaviour.TriggerPickUpRush();
			}
			else
			{
				this.dumpsterTrigger.SetActive();
			}
		}
	}

	public void PlayerEnteredDumpsterTrigger()
	{
		if (this.breatherIsActive)
		{
			this.breatherBehaviour.TriggerDumpsterJump();
			GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.JumpHit7, 0.1f);
			GameManager.TimeSlinger.FireTimer(0.1f, new Action(BreatherRoamJumper.Ins.TriggerDumpsterJump), 0);
		}
	}

	public void PlayerLeftDeadDrop()
	{
		DataManager.LockSave = false;
		if (this.breatherIsActive)
		{
			EnemyManager.State = ENEMY_STATE.IDLE;
		}
		this.breatherIsActive = false;
	}

	public void TriggerWalkToDoor()
	{
		this.breatherBehaviour.TriggerWalkToDoor();
	}

	public void TriggerDoorMech()
	{
		this.doorMechanicActive = true;
		this.currentDoorAttempts = UnityEngine.Random.Range(this.breatherData.DoorAttemptsMin, this.breatherData.DoorAttemptsMax);
		this.openDoorAttemptWindow = UnityEngine.Random.Range(this.breatherData.DoorAttemptWindowMin, this.breatherData.DoorAttemptWindowMax);
		this.openDoorAttemptTimeStamp = Time.time;
		this.openDoorAttemptActive = true;
	}

	public void TriggerSafeDeSpawn()
	{
		EnemyManager.State = ENEMY_STATE.IDLE;
		this.breatherBehaviour.DeSpawn();
		braceController.Ins.PlayerLeftEvent.Event -= this.playerLeftBraceModeTooSoon;
	}

	public void ActivatePeekABooJump()
	{
		this.breatherBehaviour.CapsuleCollider.radius = 0.33f;
		this.breatherBehaviour.NotInMeshEvents.Event += this.triggerPeekABookJump;
		this.breatherBehaviour.InMeshEvents.Event += this.reRollPeekABooJump;
		this.breatherBehaviour.AttemptSpawnBehindPlayer(roamController.Ins.transform, 0.935f);
	}

	private void playerEnteredBraceMode()
	{
		if (!this.playerLeavesBraceDoomActive)
		{
			if (this.braceTimer != null)
			{
				this.braceTimer.UnPause();
			}
			else
			{
				float duration = UnityEngine.Random.Range(this.breatherData.PatrolDelayMin, this.breatherData.PatrolDelayMax);
				GameManager.TimeSlinger.FireHardTimer(out this.braceTimer, duration, new Action(this.triggerDoorPatrol), 0);
			}
		}
	}

	private void playerLeftBraceMode()
	{
		if (this.braceTimer != null)
		{
			this.braceTimer.Pause();
		}
		if (this.playerLeavesBraceDoomActive)
		{
			if (this.doorMechanicActive)
			{
				DOTween.To(() => this.deadDropDoor.transform.localRotation, delegate(Quaternion x)
				{
					this.deadDropDoor.transform.localRotation = x;
				}, Vector3.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true);
				this.openDoorAttemptActive = false;
				this.openDoorActive = false;
			}
			this.stagePeekABooJump();
		}
	}

	private void playerLeftBraceModeTooSoon()
	{
		braceController.Ins.PlayerLeftEvent.Event -= this.playerLeftBraceModeTooSoon;
		this.stagePeekABooJump();
	}

	private void triggerDoorPatrol()
	{
		GameManager.TimeSlinger.KillTimer(this.braceTimer);
		this.braceTimer = null;
		this.playerLeavesBraceDoomActive = true;
		BreatherPatrolBehaviour.Ins.PatrolSpawn();
	}

	private void triggerOpenDoor()
	{
		DOTween.To(() => this.deadDropDoor.transform.localRotation, delegate(Quaternion x)
		{
			this.deadDropDoor.transform.localRotation = x;
		}, new Vector3(0f, -0.5f, 0f), 0.75f).SetEase(Ease.Linear).SetOptions(true);
		GameManager.TimeSlinger.FireTimer(0.55f, delegate()
		{
			this.openDoorWindow = UnityEngine.Random.Range(this.breatherData.OpeningDoorWindowMin, this.breatherData.OpeningDoorWindowMax);
			this.openDoorTimeStamp = Time.time;
			this.openDoorActive = true;
		}, 0);
	}

	private void rollNextAttempt()
	{
		DOTween.To(() => this.deadDropDoor.transform.localRotation, delegate(Quaternion x)
		{
			this.deadDropDoor.transform.localRotation = x;
		}, Vector3.zero, 0.5f).SetEase(Ease.Linear).SetOptions(true);
		this.currentDoorAttempts--;
		if (this.currentDoorAttempts > 0)
		{
			this.openDoorAttemptWindow = UnityEngine.Random.Range(this.breatherData.DoorAttemptWindowMin, this.breatherData.DoorAttemptWindowMax);
			this.openDoorAttemptTimeStamp = Time.time;
			this.openDoorAttemptActive = true;
		}
		else
		{
			this.openDoorAttemptActive = false;
			this.openDoorActive = false;
			this.playerLeavesBraceDoomActive = false;
			this.preLeaveTrigger.SetInActive();
			this.pickUpTrigger.SetInActive();
			this.dumpsterTrigger.SetInActive();
			braceController.Ins.PlayerEnteredEvent.Event -= this.playerEnteredBraceMode;
			braceController.Ins.PlayerLeftEvent.Event -= this.playerLeftBraceMode;
			braceController.Ins.PlayerLeftEvent.Event += this.playerLeftBraceModeTooSoon;
			this.breatherBehaviour.TriggerWalkAwayFromDoor();
		}
	}

	private void triggerDoorDoom()
	{
		DataManager.ClearGameData();
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		DOTween.To(() => this.deadDropDoor.transform.localRotation, delegate(Quaternion x)
		{
			this.deadDropDoor.transform.localRotation = x;
		}, new Vector3(0f, -131f, 0f), 0.3f).SetEase(Ease.Linear).SetOptions(true);
		BreatherBraceJumper.Ins.TriggerDoorJump();
		this.breatherBehaviour.TriggerDoorJump();
		LookUp.Doors.DeadDropDoor.AudioHub.PlaySound(LookUp.SoundLookUp.SlamOpenDoor2);
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit7);
	}

	private void stagePeekABooJump()
	{
		DataManager.ClearGameData();
		this.openDoorAttemptActive = false;
		this.openDoorActive = false;
		this.playerLeavesBraceDoomActive = false;
		this.pickUpTrigger.SetInActive();
		this.dumpsterTrigger.SetInActive();
		braceController.Ins.PlayerLeftEvent.Event -= this.playerLeftBraceMode;
		braceController.Ins.PlayerLeftEvent.Event -= this.playerLeftBraceModeTooSoon;
		this.breatherBehaviour.HardDeSpawn();
		this.peekABooTrigger.SetActive();
	}

	private void triggerPeekABookJump()
	{
		PauseManager.LockPause();
		GameManager.InteractionManager.LockInteraction();
		this.breatherBehaviour.NotInMeshEvents.Event -= this.triggerPeekABookJump;
		this.breatherBehaviour.InMeshEvents.Event -= this.reRollPeekABooJump;
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.JumpHit6, 0.3f);
		this.breatherBehaviour.SpawnBehindPlayer(roamController.Ins.transform, 0.935f);
		this.breatherBehaviour.TriggerPeekABooJump();
		BreatherRoamJumper.Ins.TriggerPeekABooJump();
	}

	private void reRollPeekABooJump()
	{
		this.breatherBehaviour.NotInMeshEvents.Event -= this.triggerPeekABookJump;
		this.breatherBehaviour.InMeshEvents.Event -= this.reRollPeekABooJump;
		GameManager.TimeSlinger.FireTimer(0.2f, new Action(this.ActivatePeekABooJump), 0);
	}

	private void keyWasDiscovered()
	{
		this.keyDiscoveryCount++;
		this.myData.KeysDiscoveredCount = this.keyDiscoveryCount;
		DataManager.Save<BreatherData>(this.myData);
	}

	private void productWasPickedUp()
	{
		GameManager.ManagerSlinger.ProductsManager.ProductWasPickedUp.Event -= this.productWasPickedUp;
		this.firstProductWasPickedUp = true;
		this.myData.ProductWasPickedUp = true;
		DataManager.Save<BreatherData>(this.myData);
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myData = DataManager.Load<BreatherData>(55779);
		if (this.myData == null)
		{
			this.myData = new BreatherData(55779);
			this.myData.KeysDiscoveredCount = 0;
			this.myData.ProductWasPickedUp = false;
		}
		this.keyDiscoveryCount = this.myData.KeysDiscoveredCount;
		this.firstProductWasPickedUp = this.myData.ProductWasPickedUp;
		if (DataManager.LeetMode)
		{
			this.firstProductWasPickedUp = true;
		}
		else if (!this.myData.ProductWasPickedUp)
		{
			GameManager.ManagerSlinger.ProductsManager.ProductWasPickedUp.Event += this.productWasPickedUp;
		}
	}

	private void threatsActivated()
	{
		GameManager.StageManager.ThreatsNowActivated -= this.threatsActivated;
		this.threatsActive = true;
	}

	private void Awake()
	{
		LookUp.SoundLookUp.breatherLaugh = this.audioQueSFXs[0];
		EnemyManager.BreatherManager = this;
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.ThreatsNowActivated += this.threatsActivated;
		GameManager.TheCloud.KeyDiscoveredEvent.Event += this.keyWasDiscovered;
	}

	private void Update()
	{
		if (this.openDoorAttemptActive && Time.time - this.openDoorAttemptTimeStamp >= this.openDoorAttemptWindow)
		{
			this.openDoorAttemptActive = false;
			this.triggerOpenDoor();
		}
		if (this.openDoorActive)
		{
			if (Time.time - this.openDoorTimeStamp >= this.openDoorWindow)
			{
				this.openDoorActive = false;
				this.rollNextAttempt();
			}
			if (!braceController.Ins.BracingDoor)
			{
				this.openDoorActive = false;
				this.triggerDoorDoom();
			}
		}
	}

	private const float BREATHER_Y_OFFSET = 0.935f;

	[SerializeField]
	private BreatherDataDefinition breatherData;

	[SerializeField]
	private Transform deadDropDoor;

	[SerializeField]
	private BreatherBehaviour breatherBehaviour;

	[SerializeField]
	private SpawnToLobbyTrigger spawnToLobbyTrigger;

	[SerializeField]
	private EnemyHotZoneTrigger preLeaveTrigger;

	[SerializeField]
	private EnemyHotZoneTrigger pickUpTrigger;

	[SerializeField]
	private EnemyHotZoneTrigger dumpsterTrigger;

	[SerializeField]
	private EnemyHotZoneTrigger peekABooTrigger;

	[SerializeField]
	private AudioHubObject audioQueHub;

	[SerializeField]
	private AudioFileDefinition[] audioQueSFXs = new AudioFileDefinition[0];

	private BreatherData myData;

	private Timer braceTimer;

	private int currentDoorAttempts;

	private int keyDiscoveryCount;

	private float openDoorAttemptWindow;

	private float openDoorAttemptTimeStamp;

	private float openDoorWindow;

	private float openDoorTimeStamp;

	private bool playerLeavesBraceDoomActive;

	private bool doorMechanicActive;

	private bool openDoorAttemptActive;

	private bool openDoorActive;

	private bool threatsActive;

	private bool breatherIsActive;

	private bool firstProductWasPickedUp;
}
