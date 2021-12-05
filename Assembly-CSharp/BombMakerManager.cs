using System;
using UnityEngine;

public class BombMakerManager : MonoBehaviour
{
	public void ReleaseTheBombMaker()
	{
		if (!this.bombMakerReleased)
		{
			this.SulphurTaken = 0;
			GameManager.StageManager.ManuallyActivateThreats();
			this.bombMakerReleased = true;
			this.activateSulphurTime();
		}
	}

	private void triggerBombMakerKill()
	{
		if (EnemyManager.State != ENEMY_STATE.IDLE || EnvironmentManager.PowerState != POWER_STATE.ON || StateManager.PlayerLocation == PLAYER_LOCATION.UNKNOWN)
		{
			this.sulphurWindow = 40f;
			this.sulphurTimeStamp = Time.time;
			this.sulphurActive = true;
			return;
		}
		EnvironmentManager.PowerBehaviour.LockedOut = true;
		DataManager.LockSave = true;
		DataManager.ClearGameData();
		EnemyManager.State = ENEMY_STATE.BOMB_MAKER;
		if (StateManager.PlayerLocation == PLAYER_LOCATION.MAIN_ROON || StateManager.PlayerLocation == PLAYER_LOCATION.BATH_ROOM || StateManager.PlayerLocation == PLAYER_LOCATION.OUTSIDE)
		{
			Debug.Log("pc jumpscare foo");
			Debug.Log("hallway 8 jumpscare foo");
			return;
		}
		BombMakerBehaviour.Ins.StageBombMakerOutsideKill();
	}

	private void activateSulphurTime()
	{
		this.sulphurWindow = UnityEngine.Random.Range(this.bmData.SulphurCoolTimeMin, this.bmData.SulphurCoolTimeMax);
		if (ModsManager.Nightmare)
		{
			this.sulphurWindow *= 0.6f;
		}
		this.sulphurTimeStamp = Time.time;
		this.sulphurActive = true;
	}

	private void triggerSulphurTimesUp()
	{
		if ((EnemyManager.State != ENEMY_STATE.IDLE || EnvironmentManager.PowerState != POWER_STATE.ON) && (EnemyManager.State != ENEMY_STATE.BOMB_MAKER || EnvironmentManager.PowerState != POWER_STATE.ON))
		{
			this.sulphurWindow = 40f;
			this.sulphurTimeStamp = Time.time;
			this.sulphurActive = true;
			return;
		}
		EnemyManager.State = ENEMY_STATE.BOMB_MAKER;
		if (SulphurInventory.SulphurAmount <= 0)
		{
			this.triggerBombMakerKill();
			return;
		}
		EnemyManager.State = ENEMY_STATE.IDLE;
		SulphurInventory.RemoveSulphur(1);
		Debug.Log("laugh foo");
		CurrencyManager.AddCurrency(ModsManager.EasyModeActive ? 20f : 65f);
		if (this.SulphurTaken < 5)
		{
			this.SulphurTaken++;
			this.activateSulphurTime();
		}
	}

	private void Awake()
	{
		EnemyManager.BombMakerManager = this;
		this.myID = base.transform.position.GetHashCode();
	}

	private void Update()
	{
		if (this.sulphurActive && Time.time - this.sulphurTimeStamp >= this.sulphurWindow)
		{
			this.sulphurActive = false;
			this.triggerSulphurTimesUp();
		}
	}

	private void OnDestroy()
	{
		EnemyManager.BombMakerManager = null;
		Debug.Log("[BombMaker Mod] Unloaded successfully");
	}

	private float sulphurTimeStamp;

	private float sulphurWindow;

	private bool bombMakerReleased;

	private bool bombMakerActivated;

	private bool sulphurActive;

	private int myID;

	private bool forced;

	[HideInInspector]
	public BombMakerBehaviour bombMakerBehaviour;

	[HideInInspector]
	public BombMakerDataDefinition bmData;

	[HideInInspector]
	public int SulphurTaken;
}
