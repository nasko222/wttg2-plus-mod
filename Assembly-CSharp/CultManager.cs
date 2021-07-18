using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CultManager : MonoBehaviour
{
	public void StageSpawn(Definition SpawnData)
	{
		this.spawnData = (CultSpawnDefinition)SpawnData;
	}

	public void IsPlayerSeen(bool Seen)
	{
		if (this.spawnData.SeePlayer && Seen && this.closeJumpActive)
		{
			this.closeJumpActive = false;
			this.triggerCloseJump();
		}
	}

	public void StageCloseJump()
	{
		DataManager.LockSave = true;
		DataManager.ClearGameData();
		if (this.spawnData.SeePlayer)
		{
			this.closeJumpActive = true;
		}
		else
		{
			this.triggerCloseJump();
		}
	}

	public void StageDeskJump()
	{
		DataManager.LockSave = true;
		DataManager.ClearGameData();
		this.cultMale.StageDeskJump();
		this.cultFemale.StageDeskJump();
	}

	public void TriggerDeskJump()
	{
		ComputerChairObject.Ins.SetToNotInUsePosition();
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit4);
		this.cultMale.TriggerDeskJump();
		this.cultFemale.TriggerDeskJump();
		GameManager.TimeSlinger.FireTimer(5f, delegate()
		{
			MainCameraHook.Ins.ClearARF(0.45f);
		}, 0);
		GameManager.TimeSlinger.FireTimer(5.4f, delegate()
		{
			UIManager.TriggerGameOver("KILLED");
		}, 0);
	}

	public void DeSpawn()
	{
		EnemyManager.State = ENEMY_STATE.IDLE;
		EnvironmentManager.PowerBehaviour.LockedOut = false;
		DataManager.LockSave = false;
		this.normalSpawnFireWindowActive = false;
		this.generateNormalSpawnWindow();
	}

	public void StageEndJump()
	{
		this.cultMale.StageEndJump();
		CultMaleCamHelper.Ins.StageEndJump();
		LookUp.Doors.Door1.DoorWasOpenedEvent.AddListener(new UnityAction(this.endDoorWasOpened));
	}

	public void attemptSpawn()
	{
		StateManager.PlayerLocationChangeEvents.Event -= this.attemptSpawn;
		if (EnemyManager.State != ENEMY_STATE.IDLE || EnvironmentManager.PowerState != POWER_STATE.ON)
		{
			this.normalSpawnFireWindow /= 2f;
			this.normalSpawnTimeStamp = Time.time;
			this.normalSpawnFireWindowActive = true;
			return;
		}
		if (StateManager.PlayerLocation == PLAYER_LOCATION.UNKNOWN)
		{
			StateManager.PlayerLocationChangeEvents.Event += this.attemptSpawn;
			return;
		}
		StateManager.PlayerLocationChangeEvents.Event -= this.attemptSpawn;
		if (this.normalSpawnLookUp.ContainsKey(StateManager.PlayerLocation))
		{
			EnemyManager.State = ENEMY_STATE.CULT;
			EnvironmentManager.PowerBehaviour.LockedOut = true;
			DataManager.LockSave = true;
			List<CultSpawnDefinition> list = this.normalSpawnLookUp[StateManager.PlayerLocation];
			int index = UnityEngine.Random.Range(0, list.Count);
			list[index].InvokeSpawnEvent();
			return;
		}
		StateManager.PlayerLocationChangeEvents.Event += this.attemptSpawn;
	}

	private void triggerCloseJump()
	{
		this.cultFemale.ValidSpawnLocationEvent.Event += this.performCloseJump;
		this.cultFemale.InValidSpawnLocationEvent.Event += this.reRollCloseJump;
		this.cultFemale.AttemptSpawnBehindPlayer();
	}

	private void reRollCloseJump()
	{
		this.cultFemale.ValidSpawnLocationEvent.Event -= this.performCloseJump;
		this.cultFemale.InValidSpawnLocationEvent.Event -= this.reRollCloseJump;
		GameManager.TimeSlinger.FireTimer(0.2f, new Action(this.triggerCloseJump), 0);
	}

	private void performCloseJump()
	{
		DataManager.LockSave = true;
		DataManager.ClearGameData();
		this.cultFemale.ValidSpawnLocationEvent.Event -= this.performCloseJump;
		this.cultFemale.InValidSpawnLocationEvent.Event -= this.reRollCloseJump;
		CultRoamJumper.Ins.TriggerHammerJump();
		GameManager.AudioSlinger.PlaySoundWithCustomDelay(LookUp.SoundLookUp.JumpHit4, 0.25f);
		GameManager.TimeSlinger.FireTimer(4.7f, delegate()
		{
			MainCameraHook.Ins.ClearARF(0.45f);
		}, 0);
		GameManager.TimeSlinger.FireTimer(5f, delegate()
		{
			UIManager.TriggerGameOver("KILLED");
		}, 0);
		GameManager.TimeSlinger.FireTimer(6f, delegate()
		{
			CultRoamJumper.Ins.ClearDOF();
		}, 0);
	}

	private void powerWentOff()
	{
		if (this.powerOffAttackModeActivated && EnemyManager.State == ENEMY_STATE.IDLE && this.darkSpawnLookUp.ContainsKey(StateManager.PlayerLocation))
		{
			int num = UnityEngine.Random.Range(0, 10);
			bool flag;
			switch (this.keyDiscoveryCount)
			{
			case 4:
				flag = (num < 5);
				break;
			case 5:
				flag = (num < 6);
				break;
			case 6:
				flag = (num < 6);
				break;
			case 7:
				flag = (num < 8);
				break;
			case 8:
				flag = (num < 9);
				break;
			default:
				flag = (num < 3);
				break;
			}
			if (flag)
			{
				EnemyManager.State = ENEMY_STATE.CULT;
				EnvironmentManager.PowerBehaviour.LockedOut = true;
				DataManager.LockSave = true;
				List<CultSpawnDefinition> list = this.darkSpawnLookUp[StateManager.PlayerLocation];
				int index = UnityEngine.Random.Range(0, list.Count);
				list[index].InvokeSpawnEvent();
			}
		}
	}

	private void forceActivateNormalSpawns()
	{
		this.normalSpawnActivated = true;
		this.generateNormalSpawnWindow();
	}

	private void generateNormalSpawnWindow()
	{
		this.normalSpawnFireWindow = UnityEngine.Random.Range(this.data.NormalSpawnFireWindowMin, this.data.NormalSpawnFireWindowMax);
		this.normalSpawnTimeStamp = Time.time;
		this.normalSpawnFireWindowActive = true;
	}

	private void endDoorWasOpened()
	{
		GameManager.AudioSlinger.PlaySound(LookUp.SoundLookUp.JumpHit4);
		LookUp.Doors.Door1.DoorWasOpenedEvent.RemoveListener(new UnityAction(this.endDoorWasOpened));
		LookUp.Doors.Door1.CancelAutoClose();
		CultRoamJumper.Ins.TriggerEndJump();
		CultMaleCamHelper.Ins.TriggerEndJump();
		this.cultMale.TriggerAnim("triggerEndJump");
	}

	private void lightsWentOn()
	{
		if (this.lightsOffModeActivated)
		{
			int num = 0;
			for (int i = 0; i < this.lightsToCheck.Length; i++)
			{
				if (!this.lightsToCheck[i].LightsAreOn)
				{
					num++;
				}
			}
			if (num < 5)
			{
				GameManager.TimeSlinger.KillTimer(this.lightCheckTimer);
				this.lightCheckTimer = null;
			}
		}
	}

	private void lightsWentOff()
	{
		if (this.lightsOffModeActivated)
		{
			int num = 0;
			for (int i = 0; i < this.lightsToCheck.Length; i++)
			{
				if (!this.lightsToCheck[i].LightsAreOn)
				{
					num++;
				}
			}
			if (num >= 5 && this.lightCheckTimer == null)
			{
				GameManager.TimeSlinger.FireHardTimer(out this.lightCheckTimer, this.data.LightCheckTimerLength, new Action(this.attemptLightJump), 0);
			}
		}
	}

	private void attemptLightJump()
	{
		if (this.lightsOffModeActivated)
		{
			if (EnemyManager.State == ENEMY_STATE.IDLE && EnvironmentManager.PowerState == POWER_STATE.ON)
			{
				int num = 0;
				for (int i = 0; i < this.lightsToCheck.Length; i++)
				{
					if (!this.lightsToCheck[i].LightsAreOn)
					{
						num++;
					}
				}
				if (num >= 5)
				{
					EnemyManager.State = ENEMY_STATE.CULT;
					DataManager.LockSave = true;
					EnvironmentManager.PowerBehaviour.LockedOut = true;
					CultComputerJumper.Ins.AddLightsOffJump();
					return;
				}
			}
			else
			{
				GameManager.TimeSlinger.FireHardTimer(out this.lightCheckTimer, 120f, new Action(this.attemptLightJump), 0);
			}
		}
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myCultData = DataManager.Load<CultData>(99);
		if (this.myCultData == null)
		{
			this.myCultData = new CultData(99);
			this.myCultData.KeysDiscoveredCount = 0;
			this.myCultData.NormalSpawnActivated = false;
			this.myCultData.PowerOffAttackActivated = false;
			this.myCultData.LightsOffAttackActivated = false;
		}
		this.keyDiscoveryCount = this.myCultData.KeysDiscoveredCount;
	}

	private void threatsActivated()
	{
		GameManager.StageManager.ThreatsNowActivated -= this.threatsActivated;
		this.normalSpawnActivated = this.myCultData.NormalSpawnActivated;
		if (this.normalSpawnActivated)
		{
			this.generateNormalSpawnWindow();
		}
		else
		{
			GameManager.TimeSlinger.FireHardTimer(out this.forceNormalSpawnTimer, this.data.TimeRequredForNormalSpawn, new Action(this.forceActivateNormalSpawns), 0);
		}
		this.powerOffAttackModeActivated = this.myCultData.PowerOffAttackActivated;
		if (this.powerOffAttackModeActivated)
		{
			EnvironmentManager.PowerBehaviour.PowerOffEvent.Event += this.powerWentOff;
		}
		this.lightsOffModeActivated = this.myCultData.LightsOffAttackActivated;
	}

	private void keyWasDiscovered()
	{
		this.keyDiscoveryCount++;
		if (!this.normalSpawnActivated && this.keyDiscoveryCount >= this.data.KeysRequiredForNormalSpawn)
		{
			GameManager.TimeSlinger.KillTimer(this.forceNormalSpawnTimer);
			this.normalSpawnActivated = true;
			this.generateNormalSpawnWindow();
			this.myCultData.NormalSpawnActivated = true;
		}
		if (!this.powerOffAttackModeActivated && this.keyDiscoveryCount >= this.data.KeysRequiredForPowerSpawn)
		{
			EnvironmentManager.PowerBehaviour.PowerOffEvent.Event += this.powerWentOff;
			this.powerOffAttackModeActivated = true;
			this.myCultData.PowerOffAttackActivated = true;
		}
		if (!this.lightsOffModeActivated && this.keyDiscoveryCount >= this.data.KeysRequiredForLightTrigger)
		{
			this.lightsOffModeActivated = true;
			this.myCultData.LightsOffAttackActivated = true;
		}
		this.myCultData.KeysDiscoveredCount = this.keyDiscoveryCount;
		DataManager.Save<CultData>(this.myCultData);
	}

	private void Awake()
	{
		EnemyManager.CultManager = this;
		GameManager.StageManager.Stage += this.stageMe;
		GameManager.StageManager.ThreatsNowActivated += this.threatsActivated;
		GameManager.TheCloud.KeyDiscoveredEvent.Event += this.keyWasDiscovered;
		for (int i = 0; i < this.normalSpawns.Length; i++)
		{
			if (!this.normalSpawnLookUp.ContainsKey(this.normalSpawns[i].Location))
			{
				this.normalSpawnLookUp.Add(this.normalSpawns[i].Location, new List<CultSpawnDefinition>());
			}
			this.normalSpawnLookUp[this.normalSpawns[i].Location].Add(this.normalSpawns[i]);
		}
		for (int j = 0; j < this.darkSpawns.Length; j++)
		{
			if (!this.darkSpawnLookUp.ContainsKey(this.darkSpawns[j].Location))
			{
				this.darkSpawnLookUp.Add(this.darkSpawns[j].Location, new List<CultSpawnDefinition>());
			}
			this.darkSpawnLookUp[this.darkSpawns[j].Location].Add(this.darkSpawns[j]);
		}
		for (int k = 0; k < this.lightsToCheck.Length; k++)
		{
			this.lightsToCheck[k].LightsWentOnEvent.Event += this.lightsWentOn;
			this.lightsToCheck[k].LightsWentOffEvent.Event += this.lightsWentOff;
		}
	}

	private void Update()
	{
		if (this.normalSpawnFireWindowActive && Time.time - this.normalSpawnTimeStamp >= this.normalSpawnFireWindow)
		{
			this.normalSpawnFireWindowActive = false;
			this.attemptSpawn();
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		for (int i = 0; i < this.normalSpawns.Length; i++)
		{
			if (this.normalSpawns[i] != null)
			{
				Gizmos.DrawWireMesh(this.debugMesh, this.normalSpawns[i].Position, Quaternion.Euler(this.normalSpawns[i].Rotation));
			}
		}
		Gizmos.color = Color.red;
		for (int j = 0; j < this.darkSpawns.Length; j++)
		{
			if (this.darkSpawns[j] != null)
			{
				Gizmos.DrawWireMesh(this.debugMesh, this.darkSpawns[j].Position, Quaternion.Euler(this.darkSpawns[j].Rotation));
			}
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < this.lightsToCheck.Length; i++)
		{
			this.lightsToCheck[i].LightsWentOnEvent.Event -= this.lightsWentOn;
			this.lightsToCheck[i].LightsWentOffEvent.Event -= this.lightsWentOff;
		}
	}

	[SerializeField]
	private CultDataDefinition data;

	[SerializeField]
	private CultMaleBehaviour cultMale;

	[SerializeField]
	private CultFemaleBehaviour cultFemale;

	[SerializeField]
	private Mesh debugMesh;

	[SerializeField]
	private LightTrigger[] lightsToCheck = new LightTrigger[0];

	[SerializeField]
	private CultSpawnDefinition[] normalSpawns = new CultSpawnDefinition[0];

	[SerializeField]
	private CultSpawnDefinition[] darkSpawns = new CultSpawnDefinition[0];

	private Dictionary<PLAYER_LOCATION, List<CultSpawnDefinition>> normalSpawnLookUp = new Dictionary<PLAYER_LOCATION, List<CultSpawnDefinition>>();

	private Dictionary<PLAYER_LOCATION, List<CultSpawnDefinition>> darkSpawnLookUp = new Dictionary<PLAYER_LOCATION, List<CultSpawnDefinition>>();

	private CultSpawnDefinition spawnData;

	private CultLooker cultLooker;

	private CultData myCultData;

	private bool normalSpawnActivated;

	private bool closeJumpActive;

	private bool powerOffAttackModeActivated;

	private bool lightsOffModeActivated;

	private bool normalSpawnFireWindowActive;

	private float normalSpawnFireWindow;

	private float normalSpawnTimeStamp;

	private int keyDiscoveryCount;

	private Timer lightCheckTimer;

	private Timer forceNormalSpawnTimer;
}
